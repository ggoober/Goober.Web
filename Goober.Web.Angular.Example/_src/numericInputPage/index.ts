import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from "./app/numeric-input.module";


platformBrowserDynamic()
    .bootstrapModule(AppModule)
    .catch((err) => console.error(err));
