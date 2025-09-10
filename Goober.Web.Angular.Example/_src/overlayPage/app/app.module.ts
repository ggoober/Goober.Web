import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { OverlayWindowModule } from "@indusoft/overlay-window"

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BaseModule, OverlayWindowModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
