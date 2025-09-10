import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ConfirmDialogModule } from '@indusoft/confirm-dialog';
import { BaseModule } from '@indusoft/angular-base';
import { ConfirmDialogPageComponent } from './confirm-dialog.component';

@NgModule({
    declarations: [ConfirmDialogPageComponent],
    imports: [BrowserModule, BaseModule, ConfirmDialogModule],
    providers: [],
    bootstrap: [ConfirmDialogPageComponent]
})
export class ConfirmDialogExampleModule { }
