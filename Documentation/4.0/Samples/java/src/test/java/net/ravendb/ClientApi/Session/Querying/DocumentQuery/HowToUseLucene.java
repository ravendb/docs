package net.ravendb.ClientApi.Session.Querying.DocumentQuery;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

public class HowToUseLucene {

    private interface IFoo<T> {
        //region lucene_1
        IDocumentQuery<T> whereLucene(String fieldName, String whereClause);
        //endregion
    }

    public HowToUseLucene() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region lucene_2
                session
                    .advanced()
                    .documentQuery(Company.class)
                    .whereLucene("Name", "bistro")
                    .toList();
                //endregion
            }
        }
    }

    private static class Company {
    }
}
