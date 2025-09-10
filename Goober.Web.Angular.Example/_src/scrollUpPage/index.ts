import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from "./app/scroll-up.module";


platformBrowserDynamic()
    .bootstrapModule(AppModule)
    .catch((err) => console.error(err));
