///<binding BeforeBuild='build' Clean='clean' />
"use strict"

// Единственное, что стоит менять в этом файле. Если true - то все неминифицировано, можно красиво и спокойно отлаживать, false - все упаковано, минифицировано, ток для продакшна
const debug = true;
const version = "0.2";

/**  ================================================================================
  *  ========================= Gulp Builder By Stalis v0.2 ==========================
  *  ================================================================================
  *
  *  Этот gulpfile.js создан для упрощения работы со сборкой js-файлов
  *
  *  Если ты читаешь эту надпись, то знай, что **ВСЕ**, что тебе, скорее всего, необходимо, можно сделать в `package.json` и `.hermesrc`
  *  Навряд ли кому-то хочется изменять что-то в скрипте сборщика, но советую подумать раза три, прежде, чем сюда лезть :)
  *
  *  ==================== Getting Started
  *  ========== JS
  *  ==== Добавление JS-библиотеки
  *  Для добавления JS-библиотеки в `vendor.js` достаточно просто установить его через менеджер пакетов NPM или Yarn, et cetera
  *  В целом, неважно чем, главное, чтобы пакет библиотеки был прописан в секции `dependencies` в `package.json`
  *  Секция `devDependencies` при этом не трогается, туда можно класть что угодно
  *
  *  В случае, если необходимо добавить библиотеку не из NPM лучше всего ее нагуглить и поставить через менеджер пакетов :)
  *  
  *  Но на крайняк, можно в файле `.hermesrc` прописать алиас для библиотеки в формате:
  *  ```json:
  *  <...>
  *      "aliases": {
  *          "myLib": "./wwwroot/lib/myLib.js"
  *      }
  *  <...>
  *  
  *  Тогда использование библиотеки `myLib` становится таким же, как и сторонних библиотек из NPM, о чем читай дальше
  *  
  *  Также бывают случаи, как у `vue.js` - по умолчанию подключается файл `vue.runtime.common.js`, который не умеет в DOM-шаблоны, которые мы используем
  *  В таком случае можно прописать алиас относительно `node_modules`:
  *  ```json
  *     "aliases": {
  *         "vue": "vue/dist/vue.common.js"
  *     }
  *  ```
  *
  *  ==== Структура JS-проекта
  *  Внутри wwwroot есть две директории: `src/js` и `dist/js`. Собственно, `src/js` для исходников, `dist/js` для собранных файлов
  *  В папке `src/js` есть файл `common.js`, который участвует в сборке `vendor.js`, Туда можно положить какие-нибудь инициализации объектов для всего приложения
  *
  *  При сборке в каждом подкаталоге исходников ищется `index.js`, если его нет, то папка пропускается для сборки
  *  Если же он находится, тогда этот файл используется для сборки `bundle.js`. Все зависимости от других файлов и библиотек резолвятся автоматически
  *  При резолве учитываются библиотеки из `vendor.js`
  *  
  *  Структура каталогов исходников переносится в `dist/js`, но НЕ РЕКУРСИВНО, то есть:
  *
  *  src/js/                                    dist/js/
  *   |-- common.js                              |-- vendor.js
  *   |-- login/                                 |-- login/
  *   |    |-- index.js                =====\         |-- bundle.js
  *   |    |-- components/             ======> 
  *   |         |-- passwordInput.js   =====/
  *   |-- plugins/
  *        |-- bootstrap-vue.js
  *        |-- vue-async-computed.js
  *  
  *  ==== Использование библиотек в JS
  *  ВСЕ библиотеки импортятся явно внутри каждого JS-файла.
  *
  *  ```index.js
  *  // так (ES2015 style)
  *  import axios from "axios";
  *  // или так (CommonJS style)
  *  const axios = require("axios");
  *  ```
  *
  *  При внедрении скрипта в разметку(чего я делать не советую) используется только второй вариант
  *  НЕ НАДО ВНЕДРЯТЬ ЛИБЫ РУКАМИ ЧЕРЕЗ <script> ПОЖАЛУЙСТА! УВАЖАЙТЕ ДРУГИХ РАЗРАБОВ!
  *
  *  ========== CSS
  *  ==== Добавление CSS-файлов
  *  CSS-файлы в проекте собираются похожим на JS образом, за единственным исключением - из установленных библиотек они не подтягиваются автоматически
  *  Если необходимо добавить css-файл в `vendor.css`, его ОБЯЗАТЕЛЬНО нужно вписать в `.hermesrc`:
  *  ```json
  *  <...>
  *      "vendorCss": [
  *          "path/to/css/style.css"
  *      ]
  *  <...>
  *
  *  ```
  *  
  *  В случае, когда нужен локальный css, можно использовать структуру, как у js-файлов, желательно с такими же именами каталогов(УВАЖАЙТЕ, МАТЬ ВАШУ, ДРУГИХ РАЗРАБОТЧИКОВ!)
  *  Сss-файлы в этих каталогах так же будут соединяться в один
  *
  *  ========== Gulp Task'и
  *  ==== build:*:vendor
  *  Собирает vendor-файлы с общими библиотеками
  *
  *  ==== build:*:apps
  *  Собирает бандлы указанного типа, с учетом структуры каталогов
  *
  *  ==== build:*
  *  Запускает сборку vendor-файлов и бандлов
  *
  *  ==== clean:*
  *  Запускает очистку папки dist в указанных каталогах
  *
  *  ==== watch:*
  *  Запускает слежение за файлами определенного типа. Не работает при изменении библиотек. При изменении файлов автоматически запускается соответствующая задача `build:*:apps`
  *
  *  ==== build
  *  Запускает полную сборку файлов проекта
  *
  *  ==================== Правила вежливости и чистоты
  *  ========== Глобальные переменные
  *  Вообще, за использование глобальных переменных бьют по рукам, но в нашем случае это еще хоть как-то +/- оправдано Razor'ом(Вот эти вот всякие штуки после `@` в .cshtml)
  *  Но все равно: НЕ СРИТЕ ПЕРЕМЕННЫМИ В ГЛОБАЛЬНУЮ ОБЛАСТЬ ВИДИМОСТИ!!!!!
  *
  *  Если необходимо сделать глобальную переменную с данными из Razor'а, то создайте в теге <script> на страничке объект с понятным названием и закидывайте все туда
  *  Лучше иметь по 3-4 вложенный объекта, в которых все уложено, нежели посередине скрипта увидеть непонятное название переменно и искать, откуда она взялась
  *  
  *  Внутри скриптов лучше использовать globalThis для доступа к этим переменным, чтобы явно указать, что они из глобальной области видимости
  *
  *  ========== Разделение кода
  *  ==== Внутри модуля
  *  Файлы бесплатные!!! Можете сделать их хоть тысячи, никто не запрещает, главное, чтобы код можно было спокойно и удобно читать, а каждый модуль отвечал за свою задачу
  *  Если речь идет о модулях для Vue.js - рекомендую использовать [компоненты](https://tfs.hermesrussia.ru/DefaultCollection/Core-Repositories/_wiki/wikis/Core-Repositories.wiki?wikiVersion=GBwikiMaster&pagePath=%2F%D0%91%D0%B8%D0%B1%D0%BB%D0%B8%D0%BE%D1%82%D0%B5%D0%BA%D0%B8%2FVue.js%20%D0%BA%D0%BE%D0%BC%D0%BF%D0%BE%D0%BD%D0%B5%D0%BD%D1%82%D1%8B)
  *  JS-части компонентов можно писать в отдельных файлах следуя структуре проекта, тогда они будут участвовать в сборке всего общего JS-файла и их даже можно будет отлаживать
  *
  *  ==== Общий код
  *  Так как сборщик игнорирует сборку кода в папках без index.js, но все равно его резолвит, то можно использовать такие папки как общие модули
  *  То есть если в проекте необходимы какие-то самописные функции, классы и тд в нескольких модулях, их необязательно внедрять в vendor.js
  *  Файл common.js специально сделан один, чтобы в него не получилось бы красиво запихнуть вообще все
  *  
  *  Где могут понадобится общие модули? К примеру подключение плагинов для Vue.js
  *  К примеру так выглядит файл с подключением библиотеки BootstrapVue и SVG-иконок из Bootstrap:
  *  
  *  ```bootstrap-vue.js
  *  import Vue from 'vue'
  *
  *  import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
  *
  *  Vue.use(BootstrapVue)
  *  Vue.use(IconsPlugin)
  *  ```
  *  
  *  Положив этот файл, к примеру, в папку `src/js/plugins` мы сможем использовать этот файл в любом модуле проекта
  *  При этом, если в этой папке не окажется файла index.js он не будет собран в отдельный модуль
  *  
  *  Подключение плагина тогда будет выглядеть вот так:
  *  ```common.js или index.js
  *  <...>
  *  import "./plugins/bootstrap-vue" // не нужно указывать расширение и не нужна
  *  <...>
  *  ```
  *  
  *  Таким образом мы можем сделать папки, где будут хранится различного рода подключаемые модули и использовать их только в необходимых метсах
  *  Тем самым разгружая браузер на тех страницах, где эти модули не нужны и оставляя себе больше пространства для работы
  *  
  *  ==================== Примечания
  *  ========== 1. Обновления
  *  Вверху указана версия скрипта, постараюсь обновлять ее параллельно с его изменениями, чтобы не было потом проблем
  *  Обратной совместимости не будет, т.е. проект, который собирается с новой версией скрипта, очень навряд ли будет собираться с более старой
  *  
  *  
  *  
  */

console.log(`================================================================================`);
console.log(`=========================== Gulp Builder By Stalis v${version} ==========================`);
console.log(`================================================================================`);

const fs = require('fs');
const path = require('path').posix;
const resolve = require('resolve');

const gulp = require('gulp');
const nop = require('gulp-nop');
const concat = require('gulp-concat');
const cleanCSS = require('gulp-clean-css');
const merge = require('merge-stream');
const source = require('vinyl-source-stream');
const rimraf = require('rimraf');

const browserify = require('browserify');
const babelify = require('babelify');
const uglifyify = require('uglifyify');

const gulpsrc = JSON.parse(fs.readFileSync('.gulpsrc'));
const packageJson = JSON.parse(fs.readFileSync('./package.json'));
const packageDependencies = Object.keys(packageJson.dependencies)                           // get dependencies names
    .concat(Object.keys(gulpsrc.aliases))                    // merge aliases
    .filter((item, pos, arr) => arr.indexOf(item) === pos);   // de-duplication

const vendorCss = gulpsrc.vendorCss;

const paths = {
    webRoot: './wwwroot/',
    nodeRoot: './node_modules/',
};

paths.js = {
    root: path.join(paths.webRoot, 'dist', 'js'),
    src: path.join(paths.webRoot, 'src', 'js'),
};

paths.css = {
    root: path.join(paths.webRoot, 'dist', 'css'),
    src: path.join(paths.webRoot, 'src', 'css'),
};

const entryJsName = 'index.js';
const commonJsName = 'common.js';
const vendorJsName = 'vendor.js';
const bundleJsName = 'bundle.js';

const entryCssName = 'index.css';
const commonCssName = 'common.css';
const vendorCssName = 'vendor.css';
const bundleCssName = 'style.css';

const browserify_config = {
    debug: debug,
    transform: [
        babelify.configure({
            presets: ['@babel/preset-env'],
            sourceMapsAbsolute: debug,
        }),
        debug ? undefined : 'uglifyify',
    ],
};

function getFolders(dir) {
    try {
        return fs.readdirSync(dir)
            .filter(function (file) {
                return fs.statSync(path.join(dir, file)).isDirectory();
            });
    } catch (err) {
        if (err.code === 'ENOENT') {
            console.error(`No directory: ${dir}`)
        } else {
            console.error(err);
        }
        return null;
    }
}

function resolveLib(lib) {
    let toResolve = lib;

    if (!!gulpsrc.aliases) {
        if (!!gulpsrc.aliases[lib]) {
            toResolve = gulpsrc.aliases[lib];
        }
    }

    return resolve.sync(toResolve);
}

gulp.task('build:js:vendor', function () {
    let b = browserify(browserify_config);

    let commonJs = path.join(paths.js.src, commonJsName);
    if (fs.existsSync(commonJs)) {
        b.add(commonJs);
    }

    packageDependencies.forEach(lib => b.require(resolveLib(lib), { expose: lib, transform: false }));

    return b.bundle()
        .pipe(source(vendorJsName))
        .pipe(gulp.dest(paths.js.root));
});

gulp.task('build:js:apps', function () {
    let folders = getFolders(paths.js.src);
    if (folders === null) {
        return gulp.src('.').pipe(nop());
    }

    let tasks = folders.map(folder => {
        let srcJsPath = path.join(paths.js.src, folder, entryJsName);
        if (fs.existsSync(srcJsPath) === false) {
            return gulp.src('.').pipe(nop());
        }

        let destFolderPath = path.join(paths.js.root, folder);

        let b = browserify(srcJsPath, browserify_config);
        b.external(packageDependencies);

        return b.bundle()
            .pipe(source(bundleJsName))
            .pipe(gulp.dest(destFolderPath));
    });

    return merge(tasks);
});

gulp.task('build:js', gulp.parallel(['build:js:vendor', 'build:js:apps']));

gulp.task('build:css:vendor', function () {
    return gulp.src(vendorCss.map(p => resolve.sync(p)))
        .pipe(concat(vendorCssName))
        .pipe(cleanCSS({ debug: debug }))
        .pipe(gulp.dest(paths.css.root));
});

gulp.task('build:css:apps', function () {
    let folders = getFolders(paths.css.src);
    if (folders === null) {
        return gulp.src('.').pipe(nop());
    }

    let tasks = folders.map(folder => {

        let srcCssPath = path.join(paths.css.src, folder, entryCssName);
        if (fs.existsSync(srcCssPath) === false) {
            return gulp.src('.').pipe(nop());
        }

        let destFolderPath = path.join(paths.css.root, folder);

        return gulp.src(srcCssPath)
            .pipe(concat(bundleCssName))
            .pipe(cleanCSS({ debug: debug }))
            .pipe(gulp.dest(destFolderPath));
    })
    return merge(tasks);
})

gulp.task('build:css', gulp.parallel(['build:css:vendor', 'build:css:apps']))

gulp.task('watch:js', function () {
    gulp.watch(path.join(paths.js.src, '**', '*.js'), gulp.series('build:js:apps'));
});

gulp.task('watch:css', function () {
    gulp.watch(path.join(paths.css.src, '**', '*.css'), gulp.series(['build:css:apps']));
});

gulp.task('clean:js', function (cb) {
    rimraf(paths.js.root, cb);
});

gulp.task('clean:css', function (cb) {
    rimraf(paths.css.root, cb);
});

gulp.task('build', gulp.parallel(['build:js', 'build:css']));
gulp.task('clean', gulp.parallel(['clean:js', 'clean:css']));