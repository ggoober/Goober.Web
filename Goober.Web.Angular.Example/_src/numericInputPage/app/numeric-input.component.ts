import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
    selector: 'numeric-input-page',
    templateUrl: './numeric-input.component.html',
    styleUrls: ['./numeric-input.component.scss']
})
export class AppComponent {

  private initValue11: number = 1;

  private initValue12: number[] = [18, 99];

  private initValue21: string = "2";
  private initValue22: string = "[1,13]";
  private initValue23: string = "1,23";

  private initValue24: string[] = ['2','43'];
 
  public appFormGroup: FormGroup = this.appFormBuilder.group({
    numericInput: [this.initValue21],
    numericInput2: [this.initValue24]
  });

  public defaultValue: number = 100;
  public integerValue!: number;
  public nonNegativeValue!: number;
  public nonExpValue!: number;
  public minMaxValue: number = 2;
  public rangeValue: number[] = [100,10000];
  public disabledValue: number = 1917;
  public readonlyValue: number = 2022;

  constructor(private appFormBuilder: FormBuilder) { }
}
