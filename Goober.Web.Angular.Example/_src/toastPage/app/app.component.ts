import { Component } from '@angular/core';
import { ToastService, ToastSettings, ToastType } from '@indusoft/toast';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'Toast';

    constructor(
        private toastService: ToastService) {

    }

    public success(): void {
        this.toastService.show(new ToastSettings(ToastType.Success, "Заголовок уведомления", "Текст уведомления об успешном завершении действия"));
    }

    public info(): void {
        this.toastService.show(new ToastSettings(ToastType.Information, "Заголовок уведомления", "Текст информационного уведомления"));
    }

    public warning(): void {
        this.toastService.show(new ToastSettings(ToastType.Warning, "Заголовок уведомления", "Текст предупреждающего уведомления"));
    }

    public error(): void {
        this.toastService.show(new ToastSettings(ToastType.Error, "Заголовок уведомления", "Текст уведомления об ошибке"));
    }
}
