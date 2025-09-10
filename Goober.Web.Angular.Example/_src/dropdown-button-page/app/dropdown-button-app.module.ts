import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { BaseModule } from "@indusoft/angular-base";
import { DropdownButtonModule } from "@indusoft/dropdown-button";

import { DropDownButtonAppComponent } from "./dropdown-button-app.component";


@NgModule({
    declarations: [DropDownButtonAppComponent],
    imports: [BrowserModule, BaseModule, BrowserAnimationsModule, DropdownButtonModule],
    providers: [],
    bootstrap: [DropDownButtonAppComponent]
})
export class DropdownButtonAppModule {

}
