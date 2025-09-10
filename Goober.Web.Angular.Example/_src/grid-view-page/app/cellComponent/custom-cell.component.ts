import { Component } from '@angular/core';
import { ICellRendererAngularComp, ICellRendererParams } from '@indusoft/grid-view';

@Component({
    templateUrl: './custom-cell.component.html',
    styles: ['button { height: var(--min-line-height); width: 180px; overflow: hidden;  justify-content: flex-start;}'],
    selector: 'custom-cell'
})
export class CustomCellComponent implements ICellRendererAngularComp {
    params: ICellRendererParams;
    refresh(params: ICellRendererParams): boolean {
        /** Можно обновлять даные ячеек */
        params.value = this.params.value
        return true
    }
    agInit(params: ICellRendererParams): void {
        /** Записываем текущие данные ячейки */
        this.params = params;
    }

    public showRowData() {
        window.alert(this.params.value)
    }

}
