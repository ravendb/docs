import { AbstractJavaScriptIndexCreationTask, DocumentStore} from "ravendb";
const documentStore = new DocumentStore();

{
    //region indexes_1
    class Orders_ByTotal extends AbstractJavaScriptIndexCreationTask {
        /// ...
    }
    //endregion
}
{
    //region indexes_2
    class Orders_ByTotal extends AbstractJavaScriptIndexCreationTask {
        constructor() {
            super();
            // ...
            
            // Set an indexing configuration value for this index:
            this.configuration = {
                "Indexing.MapTimeoutInSec": "30",
            }
        }
    }
    //endregion
}

async function deployIndexes() {
    //region indexes_3
    // Call 'execute' directly on the index instance
    await new Orders_ByTotal().execute(documentStore);
    //endregion

    //region indexes_4
    // Call 'executeIndex' on your store object
    await documentStore.executeIndex(new Orders_ByTotal());
    //endregion

    //region indexes_5
    const indexesToDeploy = [new Orders_ByTotal(), new Employees_ByLastName()];
    // Call 'executeIndexes' on your store object
    await documentStore.executeIndexes(indexesToDeploy);
    //endregion

    //region indexes_6
    const indexesToDeploy = [new Orders_ByTotal(), new Employees_ByLastName()];
    // Call the static method 'createIndexes' on the IndexCreation class
    await IndexCreation.createIndexes(indexesToDeploy, documentStore);
    //endregion
}

//region indexes_7
// Define a static-index:
// ======================
class Orders_ByTotal extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        
        this.map("Orders", order => {
            return {
                Employee: order.Employee,
                Company: order.Company,
                Total: order.Lines.reduce((sum, line) =>
                       sum + (line.Quantity * line.PricePerUnit) * (1 - line.Discount), 0)
            }
        });

        // Customize the index configuration
        this.deploymentMode = "Rolling";
        this.configuration["Indexing.MapTimeoutInSec"] = "30";
        this.indexes.add(x => x.Company, "Search");
        // ...
    }
}

async function main() {
    const documentStore = new DocumentStore("http://localhost:8080", "Northwind");
    documentStore.initialize();

    // Deploy the index:
    // =================
    const ordersByTotalIndex = new Orders_ByTotal();
    await ordersByTotalIndex.execute(documentStore);
    
    const session = documentStore.openSession()
    
    // Query the index:
    // ================
    const myIndexName = ordersByTotalIndex.getIndexName();
    
    const orders = await session
        .query({ indexName: myIndexName })
        .whereGreaterThan("Total", 100)
        .all();    
}
//endregion

{
    const session = store.openSession();
    
    //region indexes_8
    const employees = await session
        .query({ collection: 'employees' })
        .whereEquals("FirstName", "Robert")
        .whereEquals("LastName", "King")
        .all();
    //endregion
}

{
    //region syntax
    // Call this method directly on the index instance
    execute(store);
    execute(store, conventions)>;
    execute(store, conventions, database);
    
    // Call these methods on the store object
    executeIndex(index);
    executeIndex(index, database);
    executeIndexes(indexes);
    executeIndexes(indexes, database);
    
    // Call these methods on the IndexCreation class
    createIndexes(indexes, store);
    createIndexes(indexes, store, conventions);
    //endregion
}
