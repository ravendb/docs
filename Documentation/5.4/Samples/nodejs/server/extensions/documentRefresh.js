import { DocumentStore, ConfigureRefreshOperation } from "ravendb";

const documentStore = new DocumentStore();

async function refresh()
{
    const session = store.openSession();

    //region configure_refresh
    // Enable document refreshing and set the refresh interval to 5 minutes:
    // =====================================================================
    
    // Define the refresh configuration object 
    const refreshConfiguration = {
        disabled: false,           // Enable refreshing
        refreshFrequencyInSec: 300 // Set interval to 5 minutes
    };

    // Define the configure refresh operation, pass the configuration to set
    const configureRefreshOp = new ConfigureRefreshOperation(refreshConfiguration);

    // Execute the operation by passing it to maintenance.send
    await documentStore.maintenance.send(configureRefreshOp);
    //endregion

    //region refresh_example
    // Setting a document to refresh after 1 hour:
    // ==========================================+ 

    // Load a document
    const session = documentStore.openSession();
    const employee = await session.load("employees/1-A");

    // Get the metadata of the document
    const metadata = session.advanced.getMetadataFor(employee);

    // Set the "@refresh" metadata property with the refresh date in UTC format
    const refreshAt = new Date(new Date().getTime() + (60_000 * 60))
    metadata["@refresh"] = refreshAt.toISOString();

    // Save the document
    await session.saveChanges();
    //endregion
}

{
    //region syntax_1
    const configureRefreshOp = new ConfigureRefreshOperation(refreshConfiguration);
    //endregion

    //region syntax_2
    // The refreshConfiguration object
    {
        disabled,
        refreshFrequencyInSec
    }
    //endregion
}
