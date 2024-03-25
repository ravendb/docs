package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class HowToUseIntersect {



    private interface IFoo<T> {
        //region intersect_1
        IDocumentQuery<T> intersect();
        //endregion
    }

    public HowToUseIntersect() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region intersect_2
                // return all T-shirts that are manufactured by 'Raven'
                // and contain both 'Small Blue' and 'Large Gray' types
                List<TShirt> tShirts = session
                    .query(TShirt.class, TShirts_ByManufacturerColorSizeAndReleaseYear.class)
                    .whereEquals("manufacturer", "Raven")
                    .intersect()
                    .whereEquals("color", "Blue")
                    .andAlso()
                    .whereEquals("size", "Small")
                    .intersect()
                    .whereEquals("color", "Gray")
                    .andAlso()
                    .whereEquals("size", "Large")
                    .toList();
                //endregion
            }
        }
    }

    private static class TShirt {

    }

    private static class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask {

    }
}
