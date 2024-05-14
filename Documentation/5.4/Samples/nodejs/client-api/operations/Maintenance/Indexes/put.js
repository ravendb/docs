import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getStats() {
    {
        //region put_1
        // Create an index definition
        const indexDefinition = new IndexDefinition();

        // Name is mandatory, can use any string
        indexDefinition.name = "OrdersByTotal";

        // Define the index map functions, string format
        // A single string for a map-index, multiple strings for a multi-map-index
        indexDefinition.maps = new Set([`
            // Define the collection that will be indexed:
            from order in docs.Orders

                // Define the index-entry:
                select new 
                {
                    // Define the index-fields within each index-entry:
                    Employee = order.Employee,
                    Company = order.Company,
                    Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                }`
        ]);
        
         // indexDefinition.reduce = ...

        // Can provide other index definitions available on the IndexDefinition class
        // Override the default values, e.g.:
        indexDefinition.deploymentMode = "Rolling";
        indexDefinition.priority = "High";
        indexDefinition.configuration = {
            "Indexing.IndexMissingFieldsAsNull": "true"
        };
        // See all available properties in syntax below
        
        // Define the put indexes operation, pass the index definition
        // Note: multiple index definitions can be passed, see syntax below
        const putIndexesOp = new PutIndexesOperation(indexDefinition);

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(putIndexesOp);
        //endregion
    }
    {
        //region put_1_JS
        // Create an index definition
        const indexDefinition = new IndexDefinition();

        // Name is mandatory, can use any string
        indexDefinition.name = "OrdersByTotal";

        // Define the index map functions, string format
        // A single string for a map-index, multiple strings for a multi-map-index
        indexDefinition.maps = new Set([`
            map('Orders', function(order) {
                return {
                    Employee: order.Employee,
                    Company: order.Company,
                    Total: order.Lines.reduce(function(sum, l) {
                        return sum + (l.Quantity * l.PricePerUnit) * (1 - l.Discount);
                    }, 0)
                };
            });`
        ]);

        // indexDefinition.reduce = ...

        // Can provide other index definitions available on the IndexDefinition class
        // Override the default values, e.g.:
        indexDefinition.deploymentMode = "Rolling";
        indexDefinition.priority = "High";
        indexDefinition.configuration = {
            "Indexing.IndexMissingFieldsAsNull": "true"
        };
        // See all available properties in syntax below

        // Define the put indexes operation, pass the index definition
        // Note: multiple index definitions can be passed, see syntax below
        const putIndexesOp = new PutIndexesOperation(indexDefinition);

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(putIndexesOp);
        //endregion
    }
}

{
    //region syntax
    const putIndexesOperation = new PutIndexesOperation(indexesToAdd);
    //endregion
}
