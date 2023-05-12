import { 
    DocumentStore,
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

async function example() {
    
    const session = store.openSession();
    {
        //region indexes_2
        const employees = await session.query({ indexName: "Employees/ByFirstName" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region indexes_4
        const employees = await session.advanced
            .rawQuery("from index 'Employees/ByFirstName' where FirstName = 'Robert'")
            .all();
        //endregion
    }
}

