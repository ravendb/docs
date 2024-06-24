import { 
    DocumentStore,
    AbstractIndexCreationTask,
    IndexDefinition,
    PutIndexesOperation
} from "ravendb";

const store = new DocumentStore();

    //region indexes_1
    class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        constructor() {
            super();
            this.map =  "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    LastName = employee.LastName" +
                "})";
        }
    }
    //endregion

    class Employee { }

    async function whatAreIndexes() {
        
            //region indexes_2
            // save index on server
            const employeesIndex = new Employees_ByFirstAndLastName();
            await employeesIndex.execute(store);
            //endregion

            {
                const session = store.openSession();
                //region indexes_3
                const results = await session
                    .query({ indexName: employeesIndex.getIndexName()  })
                    .whereEquals("FirstName", "Robert")
                    .all();
                //endregion
            }
 
    }

