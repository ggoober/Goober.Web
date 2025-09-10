import { Component, OnInit } from "@angular/core";
import { AssetsLoaderService, ConsoleLoggerService, OperationError } from "@indusoft/angular-base-services";
import { ColumnDefinitionsParser, DateValueFormatter, FilterModelDto, FilterTypes, GridViewFilterChangedArgs, GridViewInitialisedArgs, GridViewOptions, GridViewSortChangedArgs, IColumnDefinition, IValueFormatterParams, PaginationParametersChangedArgs, SortModelDto } from "@indusoft/grid-view";
import { ServerPaginationExampleRequest } from "./services/pagination-example/dtos/server-pagination-example-request";
import { ExampleDataService } from "./services/pagination-example/example-data.service";
import { CustomCellComponent } from "./cellComponent/custom-cell.component"

@Component({
    selector: 'grid-view-app',
    templateUrl: 'grid-view-app.component.html',
    styleUrls: ['grid-view-app.component.scss']
})
export class GridViewAppComponent implements OnInit {

    public totalRows: number;
    public pageSizeList: number[] = [10, 20, 25, 50];
    public pageSize: number = 10;
    public pageNumber: number = 1;
    public filterModel: FilterModelDto;
    public sortModel: SortModelDto;
    public pageSize2: number = 10;
    public pageNumber2: number = 1;
    public filterModel2: FilterModelDto;
    public sortModel2: SortModelDto;

    /** Список дополнительных компонентов грида
        селектор (как его будет вызывать грид) : компонент
    */
    public myComponents = {
        customCellButton: CustomCellComponent
    }

    public loading: boolean = false;

    public gridOptions: GridViewOptions = {
        suppressRowHoverHighlight: false,
        columnHoverHighlight: true,
        paginationEnabled: true,
        textOverrideModel: {
            filterPlaceholder: "",
            datetimeFilterPlaceholder: "",
            applyFilterButtonTitle: "Применить",
            resetFilterButtonTitle: "Отмена",
            equalsFilterTypeTitle: "Равно",
            notEqualFilterTypeTitle: "Не равно",
            lessThanFilterTypeTitle: "Меньше",
            greaterThanFilterTypeTitle: "Больше",
            lessThanOrEqualFilterTypeTitle: "Меньше или равно",
            greaterThanOrEqualFilterTypeTitle: "Больше или равно",
            inRangeFilterTypeTitle: "В диапазоне",
            containsFilterTypeTitle: "Содержит",
            notContainsFilterTypeTitle: "Не содержит",
            startsWithFilterTypeTitle: "Начинается с",
            endsWithFilterTypeTitle: "Заканчивается на",
            andConditionTitle: "И",
            orConditionTitle: "ИЛИ",
            loadingTitle: "Загрузка..",
            noRowsToShowTitle: "Ничего не найдено"
        },
        rowClassRules: { "selectedRow": params => params.data.number <= 300 && params.data.number > 0 && !params.data.isValid}
    }

    columnDefinitions: IColumnDefinition[] = [
        {
            field: 'id',
            hidden: true
        },
        {
            field: 'name',
            headerName: 'Имя',
            filter: {
                filterType: FilterTypes.Text,
                showFloatingFilter: true
            },
            headerCheckboxSelectionEnable: true,
            checkboxSelectionEnable: true
        },
        {
            field: 'number',
            headerName: 'Цена',
            filter: {
                filterType: FilterTypes.Number,
                showFloatingFilter: true
            },
            valueFormat: "00.00"
        },
        {
            field: 'isValid',
            headerName: 'Валидно',
            filter: {
                filterType: FilterTypes.Boolean,
                showFloatingFilter: true
            }
        },
        {
            field: 'creationDate',
            headerName: 'Дата',
            valueFormatter: (params: IValueFormatterParams) => {
                const formatted = DateValueFormatter.format(params.value, 'dd.MM.YYYY HH:mm:ss', 'ru');
                return formatted;
            },
            filter: {
                filterType: FilterTypes.Date,
                valuesComparator: (filterLocalDateAtMidnight: Date, cellValue: any) => {

                    if (cellValue === null || cellValue === undefined)
                        return 0;

                    const cellValueDate = DateValueFormatter.parseDateFromString(cellValue);

                    if (cellValueDate < filterLocalDateAtMidnight) {
                        return -1;
                    } else if (cellValueDate > filterLocalDateAtMidnight) {
                        return 1;
                    }
                    return 0;
                },
                showFloatingFilter: true
            }
        }
    ];

    firstColumn: IColumnDefinition = {
        field: "name",
        headerName: "Имя",
        filter: {
            filterType: FilterTypes.Text,
            showFloatingFilter: true
        },
        headerCheckboxSelectionEnable: true,
        checkboxSelectionEnable: true,
        valueFormatter: "'Префикс_' + data.name"
    }

    thirdColumn: IColumnDefinition = {
        field: "isValid",
        headerName: "Валидно",
        filter: {
            filterType: 4
        },
        cellClassRules: {
            "selected": params => params.value
        }
    }

    columnDefinitionsDynamic: IColumnDefinition[] = [];

    rowData: any[] = [];

    constructor(private dataService: ExampleDataService,
        private assetsLoader: AssetsLoaderService,
        private logger: ConsoleLoggerService,
        private columnParser: ColumnDefinitionsParser) {

    }

    public ngOnInit(): void {
        this.assetsLoader.getAssetContent("dynamic-grid-columns.json")
            .subscribe((result) => {
                if (result instanceof OperationError) {
                    return;
                }

                if (Array.isArray(result) == false) {
                    this.logger.writeError('Bad file format.');
                    return;
                }
                const columns = <[]>JSON.parse(JSON.stringify(result));
                this.columnDefinitionsDynamic = this.columnParser.parse(columns);
            });
    }

    public onGridViewInitialised(event: GridViewInitialisedArgs): void {
        this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
    }

    public onPaginationChanged(event: PaginationParametersChangedArgs): void {
        this.pageNumber = event.page;
        this.pageSize = event.pageSize;

        this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
    }

    public onGridViewFilterChanged(event: GridViewFilterChangedArgs): void {
        this.filterModel = event.filterModel;
        this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
    }

    public onGridViewSortChanged(event: GridViewSortChangedArgs): void {
        this.sortModel = event.sortModel;
        this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
    }

    private refreshData(pageNumber: number, pageSize: number, filterModel?: FilterModelDto, sortModel?: SortModelDto): void {
        const request = new ServerPaginationExampleRequest(pageNumber, pageSize, filterModel, sortModel);

        this.dataService.getData(request)
            .subscribe(response => {
                if (response instanceof OperationError) {
                    return;
                }

                this.rowData = response.dtos;
                this.totalRows = response.total;

            });
    }
}
