import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { DropdownButtonAppModule } from './app/dropdown-button-app.module';



platformBrowserDynamic()
    .bootstrapModule(DropdownButtonAppModule)
    .catch((err) => console.error(err));
