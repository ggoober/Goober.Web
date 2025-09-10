import { registerLocaleData } from "@angular/common";
import { LOCALE_ID, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import localeRu from '@angular/common/locales/ru';

import { BaseModule } from "@indusoft/angular-base";
import { BaseServicesModule } from "@indusoft/angular-base-services";
import { GridViewModule } from "@indusoft/grid-view";
import { GridViewAppComponent } from "./grid-view-app.component";
import { ExampleDataService } from "./services/pagination-example/example-data.service";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations"

registerLocaleData(localeRu);

@NgModule({
    declarations: [GridViewAppComponent],
    imports: [BrowserModule, BaseModule, BrowserAnimationsModule, GridViewModule, BaseServicesModule],
    providers: [{ provide: LOCALE_ID, useValue: 'ru' },ExampleDataService],
    bootstrap: [GridViewAppComponent]
})
export class GridViewAppModule {

}
