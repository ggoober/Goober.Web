import { Component } from "@angular/core";
import {
    DateValueFormatter,
    FilterTypes,
    GridViewOptions,
    GridViewRowSelectionModes,
    GridViewSelectionChangedArgs,
    ICellRendererParams,
    IColumnDefinition,
    IValueFormatterParams
} from "@indusoft/grid-view";
import { CellEditorByTypeComponent, GridCellContentType, InputByTypeCellParams } from "@indusoft/cell-editor-by-type";
import { IElement } from "@indusoft/multiselect";
import { BehaviorSubject } from "rxjs";
import { Observable } from "rxjs";

@Component({
    selector: 'grid-view-editor',
    templateUrl: 'grid-view-app.component.html',
    styleUrls: ['grid-view-app.component.scss']
})
export class GridViewAppComponent {

    public options: GridViewOptions = {
        suppressRowHoverHighlight: false,
        columnHoverHighlight: true,
        paginationEnabled: true,
        rowSelectionMode: GridViewRowSelectionModes.Multiple,
        textOverrideModel: {
            filterPlaceholder: 'Hello...',
            datetimeFilterPlaceholder: 'Select...',
            applyFilterButtonTitle: 'Применить',
            resetFilterButtonTitle: 'Отменить',
            equalsFilterTypeTitle: '='
        },
        alwaysShowHorizontalScroll: false,
        rowClassRules: {
            test: (params) => params.data.brand === 'Toyota' || params.data.brand === 'Porsche'
        },
        getRowStyle: () => { return { color: 'darkblue' } }
    };
    selectedRow: any;

    onGridViewRowSelectionChanged($event: GridViewSelectionChangedArgs) {
        this.selectedRow = this.rowData.find(s => s === $event.selectedRows[0]);
    }

    public listChooser: Function = function (params: ICellRendererParams): Observable<IElement[]> {
        const condition = params.rowIndex % 2;
        switch (condition === 0) {
            case true:
                return this.comboBoxList1;
            case false:
                return this.comboBoxList2;
        }
    }

    public inputByType = {
        inputByTypeComp: CellEditorByTypeComponent
    };

    public updList() {
        this.comboBoxList1.next([...this.comboBoxList1.value, ...this.comboBoxList2.value]);
    }

    public comboBoxList1: BehaviorSubject<IElement[]> = new BehaviorSubject<IElement[]>([
        { value: 1, title: 'Элемент 1' },
        { value: 2, title: 'Элемент 2' },
        { value: 3, title: 'Элемент 3' },
        { value: 4, title: 'Элемент 4' }
    ]);

    public comboBoxList2: BehaviorSubject<IElement[]> =  new BehaviorSubject<IElement[]>([
        { value: 1, title: '1 Элемент - 2 лист' },
        { value: 2, title: '2 Элемент - 2 лист' },
        { value: 3, title: '3 Элемент - 2 лист' },
        { value: 4, title: '4 Элемент - 2 лист' }
    ]);

    public simpleColumnDefs: IColumnDefinition[] = [
        {
            field: 'brand',
            headerName: 'Строка',
            filter: {
                filterType: FilterTypes.Text,
                showFloatingFilter: false
            },
            cellRenderer: CellEditorByTypeComponent,
            cellRendererParams: params => this.typeChooserFunc(params, params.colDef.headerName)
        },
        {
            field: 'model',
            headerName: 'Строка др.',
            filter: {
                filterType: FilterTypes.Text,
                showFloatingFilter: false
            },
            cellRenderer: CellEditorByTypeComponent,
            cellRendererParams: params => this.typeChooserFunc(params, params.colDef.headerName)
        },
        {
            field: 'price',
            headerName: 'Число',
            filter: {
                filterType: FilterTypes.Number,
                showFloatingFilter: false
            },
            cellRenderer: CellEditorByTypeComponent,
            cellRendererParams: params => this.typeChooserFunc(params, params.colDef.headerName)
        },
        {
            field: 'isValid',
            headerName: 'Булево',
            cellRenderer: CellEditorByTypeComponent,
            cellRendererParams: params => this.typeChooserFunc(params, params.colDef.headerName)
        },
        {
            field: 'creationDate',
            headerName: 'Дата',
            filter: {
                filterType: FilterTypes.Date,
                showFloatingFilter: false
            },
            valueFormatter: (params: IValueFormatterParams) => {
                if (params.data && params.data[params.colDef.field])
                    return DateValueFormatter.format(params.data[params.colDef.field], 'dd.MM.YYYY HH:mm:ss')
            },
            cellRenderer: CellEditorByTypeComponent,
            cellRendererParams: params => this.typeChooserFunc(params, params.colDef.headerName)
        },
        {
            field: 'dictionary',
            headerName: 'Словарь',
            valueFormatter: (params: IValueFormatterParams) => {
                if (params.data && params.data[params.colDef.field])
                    return params.data[params.colDef.field].title;
            },
            cellRenderer: CellEditorByTypeComponent,
            cellRendererParams: params => this.typeChooserFunc(params, params.colDef.headerName)
        }
    ];

    rowData = [
        {
            'brand': 'Toyota',
            model: 'Celica',
            price: 35000,
            isValid: true,
            creationDate: new Date(),
            dictionary: this.comboBoxList1.value[0],
            children: [{
                'brand': 'Toyota - inside',
                model: 'AAA',
                price: 777,
                creationDate: new Date(),
                children: [{
                    'brand': 'DEEEPER',
                    model: 'DOWN THE RABBIT HOLE',
                    price: 666,
                    creationDate: new Date(),
                },
                {
                    'brand': 'HELL YEAH',
                    model: 'CAASDL;ASD',
                    price: 1234653735,
                    creationDate: new Date(),
                }]
            }]
        },
        {
            'brand': 'Ford',
            model: 'Mondeo',
            price: 32000,
            isValid: false,
            creationDate: new Date(),
            dictionary: this.comboBoxList2.value[0]
        },
        {
            'brand': 'Porsche',
            model: 'Boxter',
            price: 72000,
            isValid: true,
            creationDate: new Date(),
            dictionary: this.comboBoxList1.value[0]
        },
        {
            'brand': 'Nissan',
            model: 'X-Trail',
            price: 40000,
            isValid: false,
            creationDate: new Date(),
            dictionary: this.comboBoxList2.value[0]
        },
        {
            'brand': 'Lada',
            model: 'Kalina',
            price: 999999,
            isValid: true,
            creationDate: new Date(),
            dictionary: this.comboBoxList1.value[0]
        },
        {
            'brand': 'McLaren',
            model: '720s',
            price: 40,
            isValid: false,
            creationDate: new Date()
        },
        {
            'brand': 'Nissan',
            model: 'GT-R',
            price: 12,
            isValid: true,
            creationDate: new Date(),
            children: [{
                'brand': 'Nissan - inside',
                model: 'Privet1',
                price: 982891,
                creationDate: new Date(),
                children: [{
                    'brand': 'New dawn fades',
                    model: '1',
                    price: 123,
                    creationDate: new Date(),
                },
                {
                    'brand': 'Ups',
                    model: '3',
                    price: 1234653735,
                    creationDate: new Date(),
                },
                {
                    'brand': 'Ups321',
                    model: '2',
                    price: 1234653735,
                    creationDate: new Date(),
                }]
            },
            {
                'brand': 'XYZ',
                model: 'hELLO-WORLD',
                price: 982891,
                creationDate: new Date(),
                children: [{
                    'brand': 'X',
                    model: '9',
                    price: 123,
                    creationDate: new Date(),
                },
                {
                    'brand': 'Z',
                    model: '5',
                    price: 1234653735,
                    creationDate: new Date(),
                },
                {
                    'brand': 'Y',
                    model: '8',
                    price: 1234653735,
                    creationDate: new Date(),
                }]
            }]
        },
        {
            'brand': 'Mini',
            model: 'Countryman',
            price: 15000,
            isValid: false,
            creationDate: new Date()
        },
        {
            'brand': 'Lotus',
            model: 'Elise',
            price: 50000,
            isValid: true,
            creationDate: new Date()
        },
        {
            'brand': "17.07.2022/90д",
            model: 'E',
            price: 50000,
            isValid: true,
            creationDate: new Date(),
            dictionary: this.comboBoxList2.value[0]
        }
    ];


    private typeChooserFunc(params: any, headerName: string): InputByTypeCellParams {
        const condition = headerName;
        const listCondition = params.rowIndex % 2;
        switch (condition) {
            case 'Строка':
                return { cellType: GridCellContentType.String } as InputByTypeCellParams;
            case 'Число': {
                const result: InputByTypeCellParams = {
                    cellType: GridCellContentType.Numeric,
                    extraData: {
                        minValue: 5,
                        maxValue: 50000
                    }
                }
                return result;
            }
            case 'Булево': {
                const result: InputByTypeCellParams = {
                    cellType: GridCellContentType.Boolean
                }
                return result;
            }
            case 'Дата': {
                const result: InputByTypeCellParams = {
                    cellType: GridCellContentType.Date,
                    extraData: {
                        footerText: 'Привет, мир'
                    }
                }
                return result;
            }
            case 'Словарь': {
                const result: InputByTypeCellParams = {
                    cellType: GridCellContentType.List,
                    extraData: {
                        list: listCondition ? this.comboBoxList2 : this.comboBoxList1
                    }
                }
                return result;
            }
            case 'Флот':
                return { cellType: GridCellContentType.Numeric } as InputByTypeCellParams;
            default:
                return { cellType: GridCellContentType.String } as InputByTypeCellParams;
        }
    }

    public consoleLogger(event: any): void {
        console.log(event);
    }
}
