import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function addDatabaseNode() {
    {
        //region for_node_1
        // Default node access can be defined on the store        
        const documentStore = new DocumentStore(["serverUrl_1", "serverUrl_2", "..."], "DefaultDB");
        
        // For example:
        // With readBalanceBehavior set to: 'FastestNode':
        // Client READ requests will address the fastest node
        // Client WRITE requests will address the preferred node
        documentStore.conventions.readBalanceBehavior = "FastestNode";
        documentStore.initialize();
        
        // Use 'forNode' to override the default node configuration 
        // Get a server operation executor for a specific node
        const serverOpExecutor = await documentStore.maintenance.server.forNode("C");

        // The maintenance.server operation will be executed on the specified node 'C'
        const dbNames = await serverOpExecutor.send(new GetDatabaseNamesOperation(0, 25));
        //endregion
    }
}

{
    //region syntax_1
    await store.maintenance.server.forNode(nodeTag);
    //endregion
}
