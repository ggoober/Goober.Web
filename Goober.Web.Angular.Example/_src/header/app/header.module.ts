import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { BaseModule } from "@indusoft/angular-base"

import { HeaderComponent } from './header.component';

@NgModule({
    declarations: [HeaderComponent],
    imports: [BrowserModule, BrowserAnimationsModule, BaseModule],
    providers: [],
    bootstrap: [HeaderComponent]
})
export class HeaderModule {

}
