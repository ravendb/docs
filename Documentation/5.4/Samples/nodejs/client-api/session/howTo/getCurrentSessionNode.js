import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region current_session_node_1
    session.advanced.getCurrentSessionNode();
    //endregion
}

{
    /*
    //region current_session_node_2
    ServerNode {
        url: "https://localhost:8080",
        database: "Database",
        clusterTag: "A",
        serverRole: "Member" // Role can be one of: "None", "Promotable", "Member", "Rehab" 
    }
    //endregion
    */
}

async function examples() {
    {
        //region current_session_node_3
        const serverNode = session.advanced.getCurrentSessionNode();
        console.log(serverNode.url);
        //endregion
    }
}
