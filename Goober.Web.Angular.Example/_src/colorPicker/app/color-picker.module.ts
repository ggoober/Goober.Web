import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { ColorPickerPageComponent } from './color-picker.component';
import { ColorPickerModule } from '@indusoft/color-picker';
import { BaseModule } from '@indusoft/angular-base';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [ColorPickerPageComponent],
    imports: [
        BrowserModule,
        ColorPickerModule,
        BaseModule,
        FormsModule,
        ReactiveFormsModule
    ],
    providers: [],
    bootstrap: [ColorPickerPageComponent]
})

export class ColorPickerExampleModule { }
