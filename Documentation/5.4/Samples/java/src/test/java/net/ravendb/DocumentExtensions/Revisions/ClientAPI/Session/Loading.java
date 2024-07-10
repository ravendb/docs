package net.ravendb.ClientApi.Session.Revisions;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.json.MetadataAsDictionary;

import java.util.List;
import java.util.Map;

public class Loading {
    private interface IFoo {
        //region syntax_1
        <T> List<T> getFor(Class<T> clazz, String id);

        <T> List<T> getFor(Class<T> clazz, String id, int start);

        <T> List<T> getFor(Class<T> clazz, String id, int start, int pageSize);
        //endregion

        //region syntax_2
        List<MetadataAsDictionary> getMetadataFor(String id);

        List<MetadataAsDictionary> getMetadataFor(String id, int start);

        List<MetadataAsDictionary> getMetadataFor(String id, int start, int pageSize);
        //endregion

        //region syntax_3
        <T> T get(Class<T> clazz, String changeVector);

        <T> Map<String, T> get(Class<T> clazz, String[] changeVectors);
        //endregion
    }

    private static class Order {

    }

    public void samples() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region example_1_sync
                List<Order> orderRevisions = session
                    .advanced()
                    .revisions()
                    .getFor(Order.class, "orders/1-A", 0, 10);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region example_2_sync
                List<MetadataAsDictionary> orderRevisionsMetadata = session
                    .advanced()
                    .revisions()
                    .getMetadataFor("orders/1-A", 0, 10);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                String orderRevisionChangeVector = null;
                //region example_3_sync
                Order orderRevision = session
                    .advanced()
                    .revisions()
                    .get(Order.class, orderRevisionChangeVector);
                //endregion
            }
        }
    }
}
