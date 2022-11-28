import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

{
    //region syntax
    session.advanced.defer(...commands);
    //endregion
}

async function defer() {
    //region defer_1
    const session = documentStore.openSession();
    
    // Define a patchRequest object for the PatchCommandData used in the 'defer' below
    const patchRequest = new PatchRequest();
    patchRequest.script = "this.Supplier = 'suppliers/2-A';";

    // 'defer' is available in the session's advanced methods 
    session.advanced.defer(

        // Define commands to be executed:
        // i.e. Put a new document
        new PutCommandDataBase("products/999-A", null, null, {
            "Name": "My Product",
            "Supplier": "suppliers/1-A"
            "@metadata": { "@collection": "Products" }
        }),
        
        // Patch document
        new PatchCommandData("products/999-A", null, patchRequest, null),

        // Force a revision to be created
        new ForceRevisionCommandData("products/999-A"),

        // Delete a document
        new DeleteCommandData("products/1-A", null)
    );

    // All deferred commands will be sent to the server upon calling SaveChanges
    await session.saveChanges();
    
    }
    //endregion
}
