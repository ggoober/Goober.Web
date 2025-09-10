import { Component } from '@angular/core';
import { OverlayWindowRef } from '@indusoft/overlay-window';

@Component({
  selector: 'app-overlay',
  templateUrl: './overlay.component.html',
  styleUrls: ['./overlay.component.css']
})
export class OverlayComponent {

    public data: string | undefined;

    constructor(private ref: OverlayWindowRef) {
        this.data = ref.settings.data; // получение данных
    }

    public closeOverlay(value: string): void {
        this.ref.close(value); // закрытие окна
    }
}
