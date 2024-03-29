import {
    AbstractCsharpIndexCreationTask,
    DocumentStore,
    IndexDefinition, IndexDefinitionBuilder,
    PutIndexesOperation
} from "ravendb";
import {IndexCreation} from "ravendb/dist/Documents/Indexes/IndexCreation";

const store = new DocumentStore();

class Order { }

class Employee { }

//region indexes_8
class Orders_Totals extends AbstractCsharpIndexCreationTask {

    constructor() {
        super();
        this.map = "docs.Orders.Select(order => new { " +
            "    Employee = order.Employee, " +
            "    Company = order.Company, " +
            "    Total = Enumerable.Sum(order.Lines, l => ((decimal)((((decimal) l.Quantity) * l.PricePerUnit) * (1M - l.Discount)))) " +
            "})";
    }

}

async function main() {
    store.initialize();

    const ordersTotalsIndex = new Orders_Totals();
    await ordersTotalsIndex.execute(store);

    {
        const session = store.openSession();
        const orders = await session
            .query({ indexName: ordersTotalsIndex.getIndexName() })
            .whereGreaterThan("Total", 100)
            .ofType(Order)
            .all();
    }
}
//endregion

{
    //region indexes_1
    class Orders_Totals extends AbstractCsharpIndexCreationTask {
        /// ...
    }
    //endregion


    let indexConfig;
    //region indexes_9
    class Orders_Totals extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            // ...
            this.configuration = indexConfig;
        }

    }
    //endregion
}



async function creating() {
    //region indexes_2
    // deploy index to database defined in `DocumentStore.getDatabase` method
    // using default DocumentStore `conventions`
    await new Orders_Totals().execute(store);
    //endregion

    //region indexes_3
    // deploy index to `Northwind` database
    // using default DocumentStore `conventions`
    await new Orders_Totals().execute(store, store.conventions, "Northwind");
    //endregion

    //region indexes_4
    await IndexCreation.createIndexes([new Orders_Totals()], store);
    //endregion

    //region indexes_5
    const indexDefinition = new IndexDefinition();
    indexDefinition.name = "Orders/Totals";
    indexDefinition.maps = new Set([
        "from order in docs.Orders " +
        " select new " +
        " { " +
        "    order.Employee, " +
        "    order.Company, " +
        "    Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount)) " +
        "}" 
    ]);

    await store.maintenance.send(new PutIndexesOperation(indexDefinition));
    //endregion

    //region indexes_6
    let builder = new IndexDefinitionBuilder();
    builder.map = `docs.Order.select(order => new {    
                                Employee = order.Employee,    
                                Company = order.Company,
                                Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount)
                            })`;

    await store.maintenance.send(new PutIndexesOperation(builder.toIndexDefinition(store.conventions)));

    //endregion
    {
        const session = store.openSession();
        //region indexes_7
        const employees = await session
            .query(Employee)
            .whereEquals("FirstName", "Robert")
            .andAlso()
            .whereEquals("LastName", "King")
            .all();
        //endregion
    }

}

