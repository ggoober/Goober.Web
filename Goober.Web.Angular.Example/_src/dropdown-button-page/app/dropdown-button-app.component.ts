import { Component } from "@angular/core";
import { ClickHandlerFuncParam, DropdownButtonItem } from "@indusoft/dropdown-button";
import { TestDataModel } from "./models/test-data-model";

@Component({
    selector: 'dropdown-button-app',
    templateUrl: 'dropdown-button-app.component.html',
    styleUrls: ['dropdown-button-app.component.scss']
})
export class DropDownButtonAppComponent {

    public dropdownItems: DropdownButtonItem<TestDataModel>[] = [
        {
            id: "1",
            title: "Item 1",
            iconClass: "fas fa-window-maximize",
            clickHandler: (param: ClickHandlerFuncParam) => {
                console.log(param.item);
            },
            data: {
                id: 1,
                name: "TestName1",
                date: new Date()
            }
        },
        {
            id: "2",
            title: "Item 2",
            iconClass: "pi pi-times",
            clickHandler: (param: ClickHandlerFuncParam) => {
                console.log(param);
            }
        },
        {
            id: "3",
            title: "Item 3",
            iconClass: "pi pi-info",
            clickHandler: (param: ClickHandlerFuncParam) => {
                console.log(param);
            }
        }
    ];
}
