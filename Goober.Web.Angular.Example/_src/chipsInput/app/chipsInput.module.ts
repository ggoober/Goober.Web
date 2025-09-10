import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BaseModule } from '@indusoft/angular-base';
import { ShipsInputPageComponent } from './chipsInput.component';
import { ChipsInputModule } from '@indusoft/chips-input'

@NgModule({
    declarations: [ ShipsInputPageComponent ],
    imports: [BrowserModule, BaseModule, BrowserAnimationsModule, ChipsInputModule ],
    providers: [],
    bootstrap: [ ShipsInputPageComponent ]
})
export class ChipsInputPageModule {}

