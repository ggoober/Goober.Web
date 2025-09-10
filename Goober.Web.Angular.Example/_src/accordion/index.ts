import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AccordionExampleModule } from "./app/accordion.module";


platformBrowserDynamic()
    .bootstrapModule(AccordionExampleModule)
    .catch((err) => console.error(err));
