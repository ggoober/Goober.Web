import { Injectable } from "@angular/core";
import { IndexedDbBaseRepository } from "@indusoft/indexed-db-base";
import { PeoplesDb } from "../db/peoples-db";
import { PeopleEntity } from "../entities/people-entity";

@Injectable()
export class PeopleRepository extends IndexedDbBaseRepository<PeopleEntity, PeoplesDb> {
    protected TABLE: string = "peoples";
    protected db: PeoplesDb;

    constructor() {
        super();
        this.db = new PeoplesDb();
    }
}

