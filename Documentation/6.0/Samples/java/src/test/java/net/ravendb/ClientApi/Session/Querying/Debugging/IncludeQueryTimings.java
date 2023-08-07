package net.ravendb.ClientApi.Session.Debugging;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.timings.QueryTimings;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.primitives.Reference;
import net.ravendb.northwind.Product;

import java.util.List;
import java.util.Map;

public class IncludeQueryTimings {

    public interface IFoo<T> {
        //region timing_1
        IDocumentQueryCustomization timings(Reference<QueryTimings> timings);
        //endregion
    }

    public void explain() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region timing_2
                Reference<QueryTimings> timingsRef = new Reference<>();
                List<Product> resultsWithTimings = session.advanced().documentQuery(Product.class)
                    .timings(timingsRef)
                    .search("Name", "Syrup")
                    .toList();

                Map<String, QueryTimings> timingsMap = timingsRef.value.getTimings();
                //endregion
            }
        }
    }
}
