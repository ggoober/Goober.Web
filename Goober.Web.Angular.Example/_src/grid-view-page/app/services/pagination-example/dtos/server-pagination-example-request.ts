
import { FilterModelDto } from "@indusoft/grid-view";
import { SortModelDto } from "@indusoft/grid-view";


export class ServerPaginationExampleRequest {
    public pageNumber: number;
    public pageSize: number;
    public sortModel: SortModelDto;
    public filterModel: FilterModelDto;

    constructor(pageNumber: number, pageSize: number, filterModel: FilterModelDto, sortModel: SortModelDto) {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.filterModel = filterModel;
        this.sortModel = sortModel;
    }
}
