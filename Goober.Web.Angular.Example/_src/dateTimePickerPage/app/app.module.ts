import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { DateTimePickerModule } from "@indusoft/date-time-picker"
import { BrowserAnimationsModule } from "@angular/platform-browser/animations"

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BrowserAnimationsModule ,BaseModule, DateTimePickerModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
