import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ScrollUpModule } from '@indusoft/scroll-up';
import { BaseModule } from '@indusoft/angular-base';
import { AppComponent } from './scroll-up.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
      BrowserModule,
      BaseModule,
    ScrollUpModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
