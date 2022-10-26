import { DocumentStore } from "ravendb";

const store = new DocumentStore();

async function openClusterWideSession() {

    //region open_cluster_session
    const session = store.openSession({
        // Set mode to be cluster-wide
        transactionMode: "ClusterWide"

        // Session will be single-node when either:
        //   * Mode is not specified
        //   * Explicitly set to SingleNode
    });
    //endregion
}
