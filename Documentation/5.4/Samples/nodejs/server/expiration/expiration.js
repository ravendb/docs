import { DocumentStore, AbstractCsharpIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();

async function expiration() {    
    {
        const session = store.openSession();

        //region expiration_1
        // Define the expiration configuration object 
        const expirationConfiguration: ExpirationConfiguration = {
            deleteFrequencyInSec: 50,
            disabled: false
        };
        
        // Define the configure expiration operation,
        // pass the configuration to set
        const configureExpirationOp = new ConfigureExpirationOperation(expirationConfiguration);

        // Execute the operation by passing it to maintenance.send
        await store.maintenance.send(configureExpirationOp);
        //endregion

        //region expiration_2
        // Setting a new document to expire after 5 minutes:
        // =================================================

        // Create a new document
        const newEmployee = new Employee();
        newEmployee.lastName = "Smith";

        const session = documentStore.openSession();
        await session.store(newEmployee);

        // Get the metadata of the document
        const metadata = session.advanced.getMetadataFor(newEmployee);

        // Set the '@expires' metadata property with the expiration date in UTC format
        const expiresAt = new Date(new Date().getTime() + (5 * 60_000))
        metadata[CONSTANTS.Documents.Metadata.EXPIRES] = expiresAt.toISOString();

        // Save the new document to the database
        await session.saveChanges();
        //endregion
    }
}
