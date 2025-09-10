import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { ColorPickerExampleModule } from "./app/color-picker.module";


platformBrowserDynamic()
    .bootstrapModule(ColorPickerExampleModule)
    .catch((err) => console.error(err));
