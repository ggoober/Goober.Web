import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AccordionModule } from '@indusoft/accordion';
import { BaseModule } from '@indusoft/angular-base';
import { AccordionComponent } from './accordion.component';

@NgModule({
    declarations: [AccordionComponent],
    imports: [BrowserModule, BaseModule, BrowserAnimationsModule, AccordionModule],
    providers: [],
    bootstrap: [AccordionComponent]
})
export class AccordionExampleModule { }
