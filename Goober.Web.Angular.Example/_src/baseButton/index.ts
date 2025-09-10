import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { BaseButtonExampleModule } from "./app/base-button.module";


platformBrowserDynamic()
    .bootstrapModule(BaseButtonExampleModule)
    .catch((err) => console.error(err));
