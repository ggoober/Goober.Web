import { Component } from "@angular/core";


import { ChipAddedEventArgs, ChipRemovedEventArgs, ChipClickedEventArgs } from '@indusoft/chips-input';

@Component({
    selector: 'chips-input-page',
    templateUrl: 'chipsInput.component.html',
    styleUrls: ['chipsInput.component.scss']
})

export class ShipsInputPageComponent {
    title = 'ChipsInput';

    public values: string[] = [
        "One",
        "Two"
    ];
    public objectValues: ISomeData[] = [
        { id: 1, name: "Obj One" },
        { id: 2, name: "Obj Two" }
    ];
    public disableDragDrop: boolean = false;

    public onChipAdded(event: ChipAddedEventArgs): void {
        console.log('onChipAdded ' + JSON.stringify(event.chip));
        console.log('evene.chip---->', typeof event.chip)
        console.log('values ' + JSON.stringify(this.values));
        console.log('evene.values---->', this.values)
    }

    public onChipRemoved(event: ChipRemovedEventArgs): void {
        console.log('onChipRemoved ' + JSON.stringify(event.chip));
        console.log('values ' + JSON.stringify(this.values));
    }

    public onChipClicked(event: ChipClickedEventArgs): void {
        console.log('onChipClicked ' + JSON.stringify(event.chip));
        console.log('values ' + JSON.stringify(this.values));
    }

    public onChipAddedSimple(event: ChipAddedEventArgs): void {
        console.log('onChipAdded' + JSON.stringify(event.chip));
    }
    public onChipRemovedSimple(event: ChipRemovedEventArgs): void {
        console.log('onChipRemoved' + JSON.stringify(event.chip));
    }

    public onChipClickedSimple(event: ChipClickedEventArgs): void {
        console.log('onChipClicked' + JSON.stringify(event.chip));
    }


    public toggleDragDrop(): void {
        this.disableDragDrop = !this.disableDragDrop;
    }


    public onChipAddedObj(event: ChipAddedEventArgs): void {
        console.log('onChipAddedObj' + JSON.stringify(event.chip));

        const ids = this.objectValues.map(ov => { return ov.id });
        console.log('ids', ids)
        const next = Math.max(...ids) + 1;
        this.objectValues = [... this.objectValues, { id: next, name: event.chip }];

        console.log('valuesObj' + JSON.stringify(this.objectValues));
    }
        
    public onChipRemovedObj(event: ChipRemovedEventArgs): void {
        console.log('onChipRemovedObj' + JSON.stringify(event.chip));

        const notRemoved = this.objectValues.filter(ov => ov.name !== event.chip.name);
        this.objectValues = [...notRemoved];

        console.log('valuesObj' + JSON.stringify(this.objectValues));
    }

    public onChipClickedObj(event: ChipClickedEventArgs): void {
        console.log('onChipClickedObj' + JSON.stringify(event.chip));
        console.log('valuesObj' + JSON.stringify(this.objectValues));
    }
    
};

export interface ISomeData {
    id: number;
    name: string;
}

