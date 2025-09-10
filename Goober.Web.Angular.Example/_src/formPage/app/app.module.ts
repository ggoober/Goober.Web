import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { OverlayWindowModule } from '@indusoft/overlay-window';
import { OverlayComponent } from './overlay/overlay.component';
import { TabFormModule } from '@indusoft/tab-form'

@NgModule({
    declarations: [AppComponent, OverlayComponent],
    imports: [BrowserModule, BaseModule, OverlayWindowModule, TabFormModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}

