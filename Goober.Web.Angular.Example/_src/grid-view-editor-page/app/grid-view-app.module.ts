import { CommonModule, registerLocaleData } from "@angular/common";
import { LOCALE_ID, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import localeRu from '@angular/common/locales/ru';

import { BaseModule } from "@indusoft/angular-base";
import { BaseServicesModule } from "@indusoft/angular-base-services";
import { GridViewModule } from "@indusoft/grid-view";
import { GridViewAppComponent } from "./grid-view-app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations"
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DateTimePickerModule } from "@indusoft/date-time-picker";
import { NumericInputModule } from "@indusoft/numeric-input";
import { MultiselectModule } from "@indusoft/multiselect";
import { CellEditorByTypeModule } from "@indusoft/cell-editor-by-type";

registerLocaleData(localeRu);

@NgModule({
    declarations: [GridViewAppComponent],
    imports: [BrowserModule,
        CommonModule,
        BaseModule,
        BrowserAnimationsModule,
        GridViewModule,
        BaseServicesModule,
        FormsModule,
        ReactiveFormsModule,
        DateTimePickerModule,
        NumericInputModule,
        MultiselectModule,
        CellEditorByTypeModule
    ],
    providers: [{ provide: LOCALE_ID, useValue: 'ru' }],
    bootstrap: [GridViewAppComponent]
})
export class GridViewAppModule {

}
