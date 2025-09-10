import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { GridViewAppModule } from './app/grid-view-app.module';


platformBrowserDynamic()
    .bootstrapModule(GridViewAppModule)
    .catch((err) => console.error(err));
