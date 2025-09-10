import { Component } from "@angular/core";
import { IUserData } from "@indusoft/angular-base";

@Component({
    selector: 'header',
    templateUrl: 'header.component.html',
    styleUrls: ['header.component.css']
})
export class HeaderComponent {
    public settings: string = "/settings/base_settings.json";
    public userData: IUserData = { id: 0, login: "sam", name: "sam", role: 'admin' }
}
