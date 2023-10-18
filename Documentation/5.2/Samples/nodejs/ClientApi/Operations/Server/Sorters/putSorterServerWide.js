import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function putSorterServerWide() {
    
        //region put_sorter
        // Create the sorter definition object
        const sorterDefinition = {
            // The sorter name must be the same as the sorter's class name in your code
            name: "MySorter",
            // The code must be compilable and include all necessary using statements (C# code)
            code: "<code of custom sorter>"
        };
        
        // Define the put sorters operation, pass the sorter definition
        const putSortersServerWideOp = new PutServerWideSortersOperation(sorterDefinition);

        // Execute the operation by passing it to maintenance.server.send
        await documentStore.maintenance.server.send(putSortersServerWideOp );
        //endregion
}

{
    //region syntax_1
    const putSortersServerWideOp = new PutServerWideSortersOperation(sortersToAdd);
    //endregion
}

{
    //region syntax_2
    // The sorter definition object 
    {
        name: string;
        code: string;
    }
    //endregion
}
