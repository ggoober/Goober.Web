import { Component } from "@angular/core";
import { EquationTypes, FilterModelDto, FilterTypes, GridViewFilterChangedArgs, GridViewInitialisedArgs, GridViewOptions, GridViewSelectionChangedArgs, GridViewSortChangedArgs, IColumnDefinition, Operators, PaginationParametersChangedArgs, SortModelDto } from "@indusoft/grid-view";
import { FilterCombineOperator, RepositoryEquationType, RepositoryFilter, RepositoryFilterCondition, RepositoryFilterType, RepositorySort } from "@indusoft/indexed-db-base";
import { PeopleEntity } from "./entities/people-entity";
import { PeopleRepository } from "./repositories/people-repository";

@Component({
    selector: 'indexer-db-demo',
    templateUrl: 'indexer-db-demo.component.html',
    styleUrls: ['indexer-db-demo.component.scss']
})
export class IndexerDbDemoComponent {

    public totalRows: number;
    public pageSizeList: number[] = [10, 20, 25, 50];
    public pageSize: number = 10;
    public pageNumber: number = 1;
    public filterModel: FilterModelDto;
    public sortModel: SortModelDto;

    public loading: boolean = false;

    public gridOptions: GridViewOptions = {
        suppressRowHoverHighlight: false,
        columnHoverHighlight: true,
        paginationEnabled: true
    }

    public columnDefinitions: IColumnDefinition[] = [
        {
            field: 'id',
            headerName: 'Id',
            filter: {
                filterType: FilterTypes.Number,
                showFloatingFilter: true
            },
        },
        {
            field: 'name',
            headerName: 'Name',
            filter: {
                filterType: FilterTypes.Text,
                showFloatingFilter: true
            }
        },
        {
            field: 'email',
            headerName: 'Email',
            filter: {
                filterType: FilterTypes.Text,
                showFloatingFilter: true
            }
        },
        {
            field: 'country',
            headerName: 'Country',
            filter: {
                filterType: FilterTypes.Text,
                showFloatingFilter: true
            }
        },
        {
            field: 'age',
            headerName: 'Age',
            filter: {
                filterType: FilterTypes.Number,
                showFloatingFilter: true
            },
            valueFormat: "00.00"
        }
    ];

    public rowData: any[] = [];
    public selectedRow: any;

    constructor(private repository: PeopleRepository) { }

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

    public gridViewRowSelectionChanged(event: GridViewSelectionChangedArgs): void {
        this.selectedRow = event.selectedRows[0];
    }

    public onAddClicked(): void {
        const random = this.randomNumber(1, 100);

        const people = new PeopleEntity();
        people.name = "Name" + random;
        people.email = "email" + random + "@somewere.com";
        people.country = "Country" + random;
        people.age = random;

        this.repository.insert(people)
            .subscribe(result => {
                this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
            });
    }

    public onUpdateClicked(): void {
        const people = <PeopleEntity>this.selectedRow;

        people.name = people.name + "_Updated";

        this.repository.update(people)
            .subscribe(result => {
                this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
            })
    }

    public onClearClicked(): void {
        this.repository.clearAll()
            .subscribe(result => {
                this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
            })
    }

    public onDeleteClicked(): void {
        const people = <PeopleEntity>this.selectedRow;

        this.repository.delete(people)
            .subscribe(result => {
                this.refreshData(this.pageNumber, this.pageSize, this.filterModel, this.sortModel);
            })
    }

    public onDeleteDbClicked(): void {
        this.repository.deleteDb()
            .subscribe(result => {
                if (result) {
                    this.rowData = [];
                }
            })
    }

    private refreshData(pageNumber: number, pageSize: number, filterModel?: FilterModelDto, sortModel?: SortModelDto): void {
        const filters = this.mapFilters(filterModel);
        const sorts = this.mapSorts(sortModel);

        this.repository.getPaged(pageNumber, pageSize, filters, sorts)
            .subscribe(result => {
                this.rowData = result.data;
                this.totalRows = result.count;
            });
    }

    private mapFilters(filterModel?: FilterModelDto): RepositoryFilter[] {
        if (!filterModel || !filterModel.filters || filterModel.filters.length === 0)
            return [];

        const filters = filterModel.filters.map(filter => {
            const filterType: FilterTypes = filter.filterType as FilterTypes;
            const repoFilterType = this.mapFilterType(filterType);
            return new RepositoryFilter(filter.fieldName,
                repoFilterType,
                filter.conditions.map(condition => {
                    return new RepositoryFilterCondition(this.mapConditionEquationType(condition.type), condition.filter, condition.filterTo);
                }),
                filter.operator ? this.mapOperator(filter.operator) : null
            );
        });

        return filters;
    }

    private mapFilterType(type: FilterTypes): RepositoryFilterType {
        const sourceFieldName = FilterTypes[type];
        const targetFieldName = sourceFieldName as keyof typeof RepositoryFilterType;
        return RepositoryFilterType[targetFieldName];
    }

    private mapConditionEquationType(type: EquationTypes): RepositoryEquationType {
        return RepositoryEquationType[EquationTypes[type] as keyof typeof RepositoryEquationType];
    }

    private mapOperator(operator: Operators): FilterCombineOperator {
        return FilterCombineOperator[Operators[operator] as keyof typeof FilterCombineOperator];
    }

    private mapSorts(sortModel?: SortModelDto): RepositorySort[] {
        if (!sortModel || !sortModel.sorts || sortModel.sorts.length === 0)
            return [];
    }

    private randomNumber(min: number, max: number): number {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
}
