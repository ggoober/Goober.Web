import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css', './app.component.scss']
})
export class AppComponent {
    public settings = "/settings/home_settings.json"
    title = 'TestA1';
}
