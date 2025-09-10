import { Component, Input } from "@angular/core";

@Component({
    selector: 'accordion-page',
    templateUrl: 'accordion.component.html',
    styleUrls: ['accordion.component.scss']
})

export class AccordionComponent {
    title = 'Accordion';
    @Input()
    public collapsing: boolean = false;

    public showItem: boolean = false;

    public test(): void {
        window.alert('Клик по кнопке заголовка')
    }
    public toggle() {
        console.log('hello')
    }
    public tests(event: any): void {
        console.log(event)
    }
    
};

