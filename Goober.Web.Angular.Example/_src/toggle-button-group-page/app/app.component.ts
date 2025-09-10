import { Component } from '@angular/core';


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    title = 'toggle-button-group';

    public state1: string = '';
    public state2: string = 'State1';
    public state3: string = '';
    public style1: boolean = false;
    public style2: boolean = false;
    public style3: boolean = false;

    public app_toggle1(a: any) {
        this.state1 = a;
    }

    public app_toggle2(a: any) {
        this.state2 = a;
    }

    public app_toggle3(a: any) {
        this.state3 = a;
    }

    public app_toggle4(a: any) {
        switch (a) {
            case 'Bold':
                this.style1 = !this.style1;
                break;
            case 'Italic':
                this.style2 = !this.style2;
                break;
            case 'Underline':
                this.style3 = !this.style3;
                break;
        }
    }
}
