package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Distinct {

    //region distinct_3_1
    public static class Order_Countries extends AbstractIndexCreationTask {
        public static class Result {
            private String country;

            public String getCountry() {
                return country;
            }

            public void setCountry(String country) {
                this.country = country;
            }
        }

        public Order_Countries() {
            map = "docs.Orders.Select(o => new {" +
                "    Country = o.ShipTo.Country" +
                "})";

            reduce = "results.GroupBy(r => r.Country).Select(g => new {" +
                "    Country = g.Key" +
                "})";
        }
    }
    //endregion

    private static class Order {

    }

    public Distinct() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region distinct_1_1
                // returns sorted list of countries w/o duplicates
                List<String> countries = session
                    .query(Order.class)
                    .orderBy("shipTo.country")
                    .selectFields(String.class, "shipTo.country")
                    .distinct()
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region distinct_2_1
                int numberOfCountries = session
                    .query(Order.class)
                    .selectFields(String.class, "shipTo.country")
                    .distinct()
                    .count();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region distinct_3_2
                int numberOfCountries = session
                    .query(Order_Countries.Result.class, Order_Countries.class)
                    .count();
                //endregion
            }
        }
    }
}
