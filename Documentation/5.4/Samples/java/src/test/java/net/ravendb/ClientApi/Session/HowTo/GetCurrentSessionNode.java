package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.http.ServerNode;

public class GetCurrentSessionNode {

    private interface IFoo {
        //region current_session_node_1
        ServerNode getCurrentSessionNode();
        //endregion
    }

    private static class Foo {
        //region current_session_node_2
        public class ServerNode {
            private String url;
            private String database;
            private String clusterTag;
            private Role serverRole;

            // getters and setters
        }

        public enum Role {
            NONE,
            PROMOTABLE,
            MEMBER,
            REHAB
        }
        //endregion
    }

    public void examples() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region current_session_node_3
                ServerNode serverNode = session.advanced().getCurrentSessionNode();
                System.out.println(serverNode.getUrl());
                //endregion
            }
        }
    }
}
