import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { ExternaJsModule } from './app/external-js.module';



platformBrowserDynamic()
    .bootstrapModule(ExternaJsModule)
    .catch((err) => console.error(err));
