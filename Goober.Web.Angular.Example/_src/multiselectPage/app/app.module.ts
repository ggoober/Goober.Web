import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { MultiselectModule } from "@indusoft/multiselect"

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BaseModule, MultiselectModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
