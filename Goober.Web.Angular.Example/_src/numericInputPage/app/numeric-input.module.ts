import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './numeric-input.component';
import { BaseModule } from '@indusoft/angular-base';
import { NumericInputModule } from '@indusoft/numeric-input';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BaseModule,
    NumericInputModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
