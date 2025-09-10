import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { ChipsInputPageModule } from "./app/chipsInput.module";


platformBrowserDynamic()
    .bootstrapModule(ChipsInputPageModule)
    .catch((err) => console.error(err));
