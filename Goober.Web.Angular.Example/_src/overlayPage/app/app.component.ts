import { Component } from '@angular/core';
import { OverlayWindowPosition, OverlayWindowService, OverlayWindowSettings } from "@indusoft/overlay-window";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'Overlay';

    constructor(private overlayService: OverlayWindowService) {

    }

    public clickOverlay(): void {
        const settings = new OverlayWindowSettings(undefined, undefined, undefined, true, true, OverlayWindowPosition.Center);
        const ref = this.overlayService.showWindow("Текст внутри overlay", settings);

        ref.onClose().subscribe(result => {
            console.log(result);
        })
    }
}
