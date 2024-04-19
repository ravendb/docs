package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.northwind.Company;

public class HowToUseFuzzy {

    private interface IFoo<T> {
        //region fuzzy_1
        IDocumentQuery<T> fuzzy(double var1);
        //endregion
    }

    public void sample1(IDocumentSession session) {
        //region fuzzy_2
        session.advanced().documentQuery(Company.class)
            .whereEquals("Name", "Ernts Hnadel")
            .fuzzy(0.5)
            .toList();
        //endregion
    }
}
