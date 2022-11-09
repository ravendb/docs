package net.ravendb.ClientApi.Session.Debugging;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.explanation.Explanations;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.primitives.Reference;
import net.ravendb.northwind.Product;

import java.util.List;

public class IncludeExplanations {

    public interface IFoo<T> {
        //region explain_1
        IDocumentQuery<T> includeExplanations(Reference<Explanations> explanations);
        //endregion
    }

    public void explain() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region explain_2
                Reference<Explanations> explanationRef = new Reference<>();
                List<Product> syrups = session.advanced().documentQuery(Product.class)
                    .includeExplanations(explanationRef)
                    .search("Name", "Syrup")
                    .toList();

                String[] scoreDetails = explanationRef.value.getExplanations(syrups.get(0).getId());
                //endregion
            }
        }
    }
}
