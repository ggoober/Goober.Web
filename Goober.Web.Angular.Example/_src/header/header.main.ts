import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { HeaderModule } from './app/header.module';

document.addEventListener('DOMContentLoaded', () => {
    platformBrowserDynamic().bootstrapModule(HeaderModule)
        .catch(err => console.error(err));
});

