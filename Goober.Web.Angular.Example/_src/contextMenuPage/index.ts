import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/context-menu.module';



platformBrowserDynamic()
    .bootstrapModule(AppModule)
    .catch((err) => console.error(err));
