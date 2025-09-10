import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { BaseButtonPageComponent } from './base-button.component';
import { BaseButtonModule } from '@indusoft/base-button';
import { BaseModule } from '@indusoft/angular-base';

@NgModule({
    declarations: [BaseButtonPageComponent],
    imports: [
        BrowserModule,
        BaseButtonModule,
        BaseModule
    ],
    providers: [],
    bootstrap: [BaseButtonPageComponent]
})
export class BaseButtonExampleModule { }
