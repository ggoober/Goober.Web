
import { Dexie, Table } from "@indusoft/indexed-db-base";
import { PeopleEntity } from "../entities/people-entity";

export class PeoplesDb extends Dexie {

    public peoples: Table<PeopleEntity, number>;

    constructor() {
        super("Peoples");
        this.version(1).stores(
            {
                peoples: '++id, name, email, country, age'
            });
    }
}
