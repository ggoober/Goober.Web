import { IEntityId } from "@indusoft/indexed-db-base";

export class PeopleEntity implements IEntityId {
    public id: number;
    public name: string;
    public email: string;
    public country: string;
    public age: number;
}
