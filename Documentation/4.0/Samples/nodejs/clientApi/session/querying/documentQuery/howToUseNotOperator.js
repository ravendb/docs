import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region not_1
    // load all entities from 'Employees' collection
    // where firstName NOT equals 'Robert'
    const employees = await session.advanced
        .documentQuery({ collection: "Employees" })
        .not()
        .whereEquals("firstName", "Robert")
        .all();
    //endregion
}

{
    //region not_2
    // load all entities from 'Employees' collection
    // where firstName NOT equals 'Robert'
    // and lastName NOT equals 'King'
    const employees = await session.advanced
        .documentQuery({ collection: "Employees" })
        .not()
        .openSubclause()
        .whereEquals("firstName", "Robert")
        .andAlso()
        .whereEquals("lastName", "King")
        .closeSubclause()
        .all();
    //endregion
}

{
    //region not_3
    // load all entities from 'Employees' collection
    // where firstName NOT equals 'Robert'
    // and lastName NOT equals 'King'
    // identical to 'Example II' but 'whereNotEquals' is used
    const employees = await session.advanced
        .documentQuery({ collection: "Employees" })
        .whereNotEquals("firstName", "Robert")
        .andAlso()
        .whereNotEquals("lastName", "King")
        .all();
    //endregion
}
