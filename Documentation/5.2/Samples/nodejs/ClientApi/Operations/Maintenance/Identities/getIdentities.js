import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getIdentities() {
    {
        //region get_identities
        // Create a document with an identity ID:
        // ======================================

        const session = documentStore.openSession();        
        const company = new Company();
        company.name = "RavenDB";

        // Request the server to generate an identity ID for the new document. Pass:
        //   * The entity to store
        //   * The collection name with a pipe (|) postfix 
        await session.store(company, "companies|");

        // If this is the first identity created for this collection,
        // and if the identity value was not customized
        // then a document with an identity ID "companies/1" will be created
        await session.saveChanges();
        
        // Get identities information:
        // ===========================

        // Define the get identities operation
        const getIdentitiesOp = new GetIdentitiesOperation();
        
        // Execute the operation by passing it to maintenance.send
        const identities = await store.maintenance.send(getIdentitiesOp);

        // Results
        const latestIdentityValue = identities["companies|"]; // => value will be 1
        //endregion
    }
}

{
    //region syntax
    const getIdentitiesOp = new GetIdentitiesOperation();
    //endregion
}
