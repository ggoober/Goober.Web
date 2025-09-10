import { Component, NgZone, OnInit } from "@angular/core";
import { ExternalJsBase, ExternalJsLoaderService } from "@indusoft/angular-base-services";
import { ExampleService } from "./services/example.service";


@Component({
    selector: 'external-js',
    templateUrl: 'external-js.component.html',
    styleUrls: ['external-js.component.css']
})
export class ExternalJsComponent extends ExternalJsBase {

    startValue: string = "start";
    endValue: string = "end";

    constructor(public ngZone: NgZone,
        public service: ExampleService,
        public jsLoader: ExternalJsLoaderService) {

        super(ngZone, jsLoader);
        this.init(this, "/assets/js/externalFunctionsBase.js");
    }


    

    public callHelloFunc(): void {
        //externalFunctions.prototype.hello("Indusoft");
        const extFuncHello = this.externalFunctions.prototype["hello"];
        extFuncHello("Indusoft");
    }

    public callComponentFunc(): void {
        const extFunc = this.externalFunctions.prototype["callMyMethod"];
        extFunc(this);
    }

    public callComponentFuncWithoutRef(): void {
        const extFunc = this.externalFunctions.prototype["callMyMethodWithoutRef"];
        extFunc();
    }

    public callChangeFunc(): void {
        const extFunc = this.externalFunctions.prototype["changeValues"];
        extFunc();
    }

    public callComponentService(): void {
        const extFunc = this.externalFunctions.prototype["callComponentService"];
        extFunc();
    }

    public testMethodWithoutRef() {
        console.log("Hello from test without ref method!");
    }

    public testMethod(): void {
        console.log("Hello from test method!");
    }
}
