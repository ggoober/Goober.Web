import { Component } from '@angular/core';
import { OverlayWindowPosition, OverlayWindowService, OverlayWindowSettings } from "@indusoft/overlay-window";
import { OverlayComponent } from './overlay/overlay.component';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'Form';
    public tabs = [
        { for: 'tab1', selected: false, name: 'Tab-item1' },
        { for: 'tab2', selected: false, name: 'Tab-item2' },
        { for: 'tab3', selected: true, name: 'Tab-item3' },
        { for: 'tab4', selected: false, name: 'Tab-item4' },
        { for: 'tab5', selected: false, name: 'Tab-item5' },
        { for: 'tab6', selected: false, name: 'Tab-item6' },
        { for: 'tab7', selected: false, name: 'Tab-item7' },
        { for: 'tab8', selected: false, name: 'Tab-item8' },
        { for: 'tab9', selected: false, name: 'Tab-item9' }
    ]
    constructor(private overlayService: OverlayWindowService) { }
    public openOverlay(): void {
        const settings = new OverlayWindowSettings(undefined, undefined, undefined, false, true, OverlayWindowPosition.Center);
        const ref = this.overlayService.showWindow(OverlayComponent, settings);

        ref.onClose().subscribe(result => {
            console.log(result);
        })
    };

    public clickOverlay(): void {
        const settings = new OverlayWindowSettings(undefined, undefined, undefined, false, true, OverlayWindowPosition.Center);
        const ref = this.overlayService.showWindow(OverlayComponent, settings);

        ref.onClose().subscribe(result => {
            console.log(result);
        })
    }
}
