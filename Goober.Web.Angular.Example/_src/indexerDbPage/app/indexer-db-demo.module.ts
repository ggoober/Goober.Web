import { LOCALE_ID, NgModule } from "@angular/core";
import { registerLocaleData } from "@angular/common";
import localeRu from '@angular/common/locales/ru';

import { BrowserModule } from "@angular/platform-browser";
import { BaseModule } from "@indusoft/angular-base";
import { BaseServicesModule } from "@indusoft/angular-base-services";
import { GridViewModule } from "@indusoft/grid-view";
import { IndexerDbDemoComponent } from "./indexer-db-demo.component";
import { PeopleRepository } from "./repositories/people-repository";


registerLocaleData(localeRu);

@NgModule({
    declarations: [IndexerDbDemoComponent],
    imports: [
        BrowserModule,
        BaseModule,
        GridViewModule,
        BaseServicesModule,
    ],
    providers: [{ provide: LOCALE_ID, useValue: 'ru' }, PeopleRepository],
    bootstrap: [IndexerDbDemoComponent]
})
export class IndexerDbDemoModule {

}
