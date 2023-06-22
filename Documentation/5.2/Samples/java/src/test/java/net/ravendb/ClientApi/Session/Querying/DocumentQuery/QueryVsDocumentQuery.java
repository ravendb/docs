package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.session.IDocumentSession;

public class QueryVsDocumentQuery {

    private class Order {

    }

    private static class  Orders_ByShipToAndLines extends AbstractIndexCreationTask {

    }

    public QueryVsDocumentQuery() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region query_1a
                session.query(Order.class)
                //endregion
                ;

                //region query_1b
                session.advanced().documentQuery(Order.class)
                //endregion
                ;

                //region query_2a
                session.query(Order.class, Orders_ByShipToAndLines.class)
                //endregion
                ;

                //region query_2b
                session.advanced().documentQuery(Order.class, Orders_ByShipToAndLines.class)
                //endregion
                ;

                //region query_3a
                session.query(Order.class, Query.index("Orders/ByShipToAndLines"))
                //endregion
                ;

                //region query_3b
                session.advanced().documentQuery(Order.class, "Orders/ByShipToAndLines", null, false);
                //endregion

                //region query_4a
                session.query(Order.class, Query.collection("orders"))
                //endregion
                ;

                //region query_4b
                session.advanced().documentQuery(Order.class, null, "orders", false);
                //endregion
            }
        }


    }
}
