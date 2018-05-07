package net.ravendb.ClientApi.Cluster;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class HowClientApiIntegratesWithReplicationAndCluster {
    private static class User {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    public HowClientApiIntegratesWithReplicationAndCluster() {

        //region InitializationSample
        try (IDocumentStore store = new DocumentStore(new String[]{
            "http://[node A url]",
            "http://[node B url]",
            "http://[node C url]"
        }, "TestDB")) {


            store.initialize();

            // the rest of ClientAPI code
        }
        //endregion

        try (IDocumentStore store = new DocumentStore()) {
            //region WriteAssuranceSample
            try (IDocumentSession session = store.openSession()) {
                User user = new User();
                user.setName("John Dow");

                session.store(user);

                //make sure that the comitted data is replicated to 2 nodes
                //before returning from the saveChanges() call.
                //TODO: session.advanced()
                 //TODO:   .waitForReplicationAfterSaveChanges()

            }
            //endregion
        }

    }
}
