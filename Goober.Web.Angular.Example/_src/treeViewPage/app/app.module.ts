import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { BaseModule } from "@indusoft/angular-base"
import { TreeViewModule } from "@indusoft/tree-view"

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BaseModule, TreeViewModule, BrowserAnimationsModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
