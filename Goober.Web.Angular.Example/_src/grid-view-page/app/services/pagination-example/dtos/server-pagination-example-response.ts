import { ServerPaginationExampleDto } from "./server-pagination-example-dto";

export class ServerPaginationExampleResponse {
    public total: number;
    public dtos: ServerPaginationExampleDto[] = [];
}
