import { Component } from '@angular/core';

@Component({
    selector: 'base-button-page',
    templateUrl: './base-button.component.html',
    styleUrls: ['./base-button.component.scss']
})
export class BaseButtonPageComponent {
    title = 'base-button-component';

    public alertFunc(data: string): void {
        window.alert(data);
    }
}
