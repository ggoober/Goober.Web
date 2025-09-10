import { Component } from "@angular/core";
import { ConfirmDialogSettings, ConfirmDialogService, ConfirmDialogButtonSettings, DialogResultType } from '@indusoft/confirm-dialog';


@Component({
    selector: 'confirm-dialog-page',
    templateUrl: 'confirm-dialog.component.html',
    styleUrls: ['confirm-dialog.component.scss']
})

export class ConfirmDialogPageComponent {
    title = 'Confirm-dialog';
    constructor(private confirmDialogService: ConfirmDialogService) { }
    
    public changeTitle = false


    public onClick(): void {
        const settings = new ConfirmDialogSettings(
            'Confirm dialog demo',
            `Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1, 
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            ext1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1, Body text1,Body text1 Body text1 Body text1 Body text1Body text1 Body text1 vBody text1Body text1Body text1Bodytext1Body text1, Body text1Body text1,Body text1
            Body text1`,

            new ConfirmDialogButtonSettings(
                'Ok',
                'fa-check'
            ),
            new ConfirmDialogButtonSettings(
                'Cancel',
                'fa-times'
            )
        );

        this.confirmDialogService.showDialog(settings)
            .then((strk: string) => {
                if (DialogResultType.Ok === strk) {
                    this.changeTitle = !this.changeTitle
                }
            })
            .catch((error) => console.log(error)
        )
    }
};
