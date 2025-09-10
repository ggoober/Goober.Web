import { Component } from '@angular/core';
import { ProgressSpinnerService } from '@indusoft/progress-spinner';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'ProgresS';

    constructor(private spinnerService: ProgressSpinnerService) {
    }

    public spinner(): void {
        this.spinnerService.show({});
        setTimeout(() => {
            this.spinnerService.close();
        }, 3000);
    }
}
