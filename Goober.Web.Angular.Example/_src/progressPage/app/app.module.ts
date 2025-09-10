import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { ProgressSpinnerModule } from "@indusoft/progress-spinner"

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BaseModule, ProgressSpinnerModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
