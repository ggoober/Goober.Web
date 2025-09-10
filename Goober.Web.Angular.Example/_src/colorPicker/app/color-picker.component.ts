import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
    selector: 'color-picker-page',
    templateUrl: './color-picker.component.html',
    styleUrls: ['./color-picker.component.scss']
})

export class ColorPickerPageComponent {
    title = 'color-picker';
    public color1: string = "#9bb26c";
    public color2: string = "#BED630";
    public color3: string = "#FFFFFF";
    public color4: string = "#92C732";
    
    constructor(private appFormBuilder: FormBuilder) { }

    public appFormGroup: FormGroup = this.appFormBuilder.group({
        element: ["#0CB14B"]
    });

    public test(event: any): void {
        console.log(event)
    }
}

