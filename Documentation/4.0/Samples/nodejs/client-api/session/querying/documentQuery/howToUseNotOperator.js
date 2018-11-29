import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function examples() {
    {
        //region not_1
        // load all entities from 'Employees' collection
        // where firstName NOT equals 'Robert'
        const employees = await session.advanced
            .documentQuery({ collection: "Employees" })
            .not()
            .whereEquals("FirstName", "Robert")
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
            .whereEquals("FirstName", "Robert")
            .andAlso()
            .whereEquals("LastName", "King")
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
            .whereNotEquals("FirstName", "Robert")
            .andAlso()
            .whereNotEquals("LastName", "King")
            .all();
        //endregion
    }
}
