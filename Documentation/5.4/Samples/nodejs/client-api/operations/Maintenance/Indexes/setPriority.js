import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function setPriority() {
    {
        //region set_priority_single
        // Define the set priority operation
        // Pass index name & priority
        const setPriorityOp = new SetIndexesPriorityOperation("Orders/Totals", "High");

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if index does not exist
        await store.maintenance.send(setPriorityOp);
        //endregion

    }
    {
        //region set_priority_multiple
        // Define the index list and the new priority:
        const parameters = {
            indexNames: ["Orders/Totals", "Orders/ByCompany"],
            priority: "Low"
        }

        // Define the set priority operation, pass the parameters
        const setPriorityOp = new SetIndexesPriorityOperation(parameters);

        // Execute the operation by passing it to maintenance.send
        // An exception will be thrown if any of the specified indexes do not exist
        await store.maintenance.send(setPriorityOp);
        //endregion
    }
}

{
    //region syntax_1
    // Available overloads:
    const setPriorityOp = new SetIndexesPriorityOperation(indexName, priority);
    const setPriorityOp = new SetIndexesPriorityOperation(parameters);
    //endregion

    //region syntax_2
    // parameters object
    {
        indexNames, // string[], list of index names
        priority    // Priority to set
    }
    //endregion
}
