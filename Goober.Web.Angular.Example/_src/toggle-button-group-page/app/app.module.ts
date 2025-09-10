import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base";
import { ToggleButtonGroupModule } from '@indusoft/toggle-button-group';

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BaseModule, ToggleButtonGroupModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
