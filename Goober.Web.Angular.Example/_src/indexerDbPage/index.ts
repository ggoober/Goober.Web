import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { IndexerDbDemoModule } from './app/indexer-db-demo.module';

platformBrowserDynamic()
    .bootstrapModule(IndexerDbDemoModule)
    .catch((err) => console.error(err));
