import { Component } from '@angular/core';

@Component({
  selector: 'switch-page',
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.scss']
})
export class AppComponent {
  public checked1: boolean = false;
  public checked2: boolean = true;
  public checked3: boolean = true;
  public checked4: boolean = false;
}
