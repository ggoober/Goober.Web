import { Component } from '@angular/core';
import { ViewFormatType, SelectionModeType } from '@indusoft/date-time-picker';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'DateTimePicker';

    ViewFormatType = ViewFormatType;
    SelectionModeType = SelectionModeType;



  }    


