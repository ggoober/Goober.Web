import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { BaseModule } from "@indusoft/angular-base";
import { BaseServicesModule } from "@indusoft/angular-base-services";
import { ExternalJsComponent } from "./external-js.component";
import { ExampleService } from "./services/example.service";

@NgModule({
    declarations: [ExternalJsComponent],
    imports: [BrowserModule, BaseModule, BaseServicesModule],
    providers: [ExampleService],
    bootstrap: [ExternalJsComponent]
})
export class ExternaJsModule {
}
