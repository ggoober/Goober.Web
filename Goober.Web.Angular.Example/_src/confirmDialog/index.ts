import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { ConfirmDialogExampleModule } from "./app/confirm-dialog.module";


platformBrowserDynamic()
    .bootstrapModule(ConfirmDialogExampleModule)
    .catch((err) => console.error(err));
