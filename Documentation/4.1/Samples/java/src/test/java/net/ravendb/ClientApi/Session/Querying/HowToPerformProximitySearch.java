package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

public class HowToPerformProximitySearch {

    private interface IFoo<T> {
        //region proximity_1
        IDocumentQuery<T> proximity(int proximity);
        //endregion
    }

    public void sample1(IDocumentSession session) {
        //region proximity_2
        session
            .advanced()
            .documentQuery(Fox.class)
            .search("name", "quick fox")
            .proximity(2)
            .toList();
        //endregion
    }

    public static class Fox {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
