import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

let documentType, indexName, collectionName, isMapReduce;

const documentStore = new DocumentStore();
const session = documentStore.openSession();

class Employee {
}

class MyCustomIndex extends AbstractIndexCreationTask { }

function docQueryIface() {
    //region document_query_1
    session.advanced.documentQuery(documentType);

    session.advanced.documentQuery({
        indexName,
        collectionName,
        isMapReduce,
        documentType
    });
    //endregion
}

async function docQueryExamples() {
    {
        //region document_query_2
        // load all entities from 'Employees' collection
        const employees = await session.advanced
            .documentQuery(Employee)
            .all();

        // same as
        const employees2 = await session.advanced
            .documentQuery({ collection: "Employees" })
            .all();
        //endregion
    }

    {
        //region document_query_3
        // load all entities from 'Employees' collection
        // where firstName equals 'Robert'
        const employees = await session.advanced
            .documentQuery({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region document_query_4
        // load all entities from 'Employees' collection
        // where firstName equals 'Robert'
        // using 'My/Custom/Index'
        const employees = await session.advanced
            .documentQuery({ indexName: "My/Custom/Index" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }
}
