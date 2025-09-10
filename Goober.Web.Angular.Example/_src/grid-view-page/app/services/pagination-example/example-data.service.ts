import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ConsoleLoggerService, HttpServiceBase, OperationError } from "@indusoft/angular-base-services";
import { catchError, Observable } from "rxjs";

import { ServerPaginationExampleRequest } from "./dtos/server-pagination-example-request";
import { ServerPaginationExampleResponse } from "./dtos/server-pagination-example-response";

@Injectable()
export class ExampleDataService extends HttpServiceBase {
    protected serviceUrl: string;

    private static readonly SERVICE_PATH = "api/server-pagination-example/";

    constructor(public http: HttpClient) {
        super(http);

        this.serviceUrl = `${window.location.origin}/${ExampleDataService.SERVICE_PATH}`;
    }

    public getData(request: ServerPaginationExampleRequest): Observable<ServerPaginationExampleResponse | OperationError> {
        const url = `${this.serviceUrl}get`;

        return this.http.post<ServerPaginationExampleResponse>(url, request)
            .pipe(
                catchError(this.handleError(
                    "ExampleDataService.getData",
                    OperationError.Message("Произошла ошибка при получении пейджированных данных.")
                ))
            );
    }
}
