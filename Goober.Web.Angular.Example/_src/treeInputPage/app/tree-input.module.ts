import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BaseModule } from '@indusoft/angular-base';

import { TreeInputModule } from '@indusoft/tree-input';

import { AppComponent } from './tree-input.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    TreeInputModule,
    BaseModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
