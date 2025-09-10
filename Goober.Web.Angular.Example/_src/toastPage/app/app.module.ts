import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { ToastModule } from "@indusoft/toast"

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BaseModule, ToastModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
