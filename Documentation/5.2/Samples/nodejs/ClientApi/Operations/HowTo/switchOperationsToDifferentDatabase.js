import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function addDatabaseNode() {
    {
        //region for_database_1
        // Define default database on the store
        const documentStore = new DocumentStore("yourServerURL", "DefaultDB");
        documentStore.initialize();

        // Use 'forDatabase', get operation executor for another database
         const opExecutor = documentStore.operations.forDatabase("AnotherDB");

        // Send the operation, e.g. 'GetRevisionsOperation' will be executed on "AnotherDB"
        const revisionsInAnotherDB =
            await opExecutor.send(new GetRevisionsOperation("Orders/1-A"));

        // Without 'forDatabase', the operation is executed "DefaultDB"
        const revisionsInDefaultDB =
           await documentStore.operations.send(new GetRevisionsOperation("Company/1-A"));
        //endregion
    }
    {
        //region for_database_2
        // Define default database on the store
        const documentStore = new DocumentStore("yourServerURL", "DefaultDB");
        documentStore.initialize();

        // Use 'forDatabase', get maintenance operation executor for another database
        const opExecutor = documentStore.maintenance.forDatabase("AnotherDB");

        // Send the maintenance operation, e.g. get database stats for "AnotherDB"
        const statsForAnotherDB =
            await opExecutor.send(new GetStatisticsOperation());

        // Without 'forDatabase', the stats are retrieved for "DefaultDB"
        const statsForDefaultDB =
            await documentStore.maintenance.send(new GetStatisticsOperation());
        //endregion
    }
}

{
    //region syntax_1
    store.operations.forDatabase(databaseName);
    //endregion
}
{
    //region syntax_2
    store.maintenance.forDatabase(databaseName);
    //endregion
}
