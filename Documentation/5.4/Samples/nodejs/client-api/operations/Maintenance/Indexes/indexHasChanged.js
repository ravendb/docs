import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getStats() {
    {
        //region index_has_changed
        // Some index definition
        const indexDefinition = new IndexDefinition();
        indexDefinition.name = "UsersByName";
        indexDefinition.maps = new Set([ `from user in docs.Users select new { user.Name }` ]);

        // Define the has-changed operation, pass the index definition
        const indexHasChangedOp = new IndexHasChangedOperation(indexDefinition);

        // Execute the operation by passing it to maintenance.send
        const indexHasChanged = await documentStore.maintenance.send(indexHasChangedOp);

        // Return values:
        // false: The definition of the index passed is the SAME as the one deployed on the server  
        // true:  The definition of the index passed is DIFFERENT than the one deployed on the server
        //        Or - index does not exist
        //endregion
    }
}

{
    //region syntax
    const indexHasChangedOp = new IndexHasChangedOperation(definition);
    //endregion

}
