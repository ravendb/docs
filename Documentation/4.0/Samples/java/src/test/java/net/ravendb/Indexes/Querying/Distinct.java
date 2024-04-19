package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Distinct {

    //region distinct_3_1
    public static class Orders_ByShipToCountry extends AbstractIndexCreationTask {

    public Orders_ByShipToCountry() {

        // The map phase indexes the country listed in each order document
        // CountryCount is assigned with 1, which will be aggregated in the reduce phase
        map = "docs.Orders.Select(order => new { " +
              "    Country = order.ShipTo.Country, " +
              "    CountryCount = 1 " +
              "})";

        // The reduce phase will group the Country results and aggregate the CountryCount
        reduce = "results.GroupBy(result => result.Country).Select(g => new { " +
                 "    Country = g.Key, " +
                 "    CountryCount = Enumerable.Sum(g, x => x.CountryCount) " +
                 "})";
    }

    public static class Result {
        private String country;
        private int countryCount;

        public String getCountry() {
            return country;
        }

        public void setCountry(String country) {
            this.country = country;
        }

        public int getCountryCount() {
            return countryCount;
        }

        public void setCountryCount(int countryCount) {
            this.countryCount = countryCount;
        }
    }
}
    //endregion

    private static class Order {

    }

    public Distinct() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region distinct_1_1
                // Get a sorted list without duplicates:
                // =====================================
                
                List<String> countries = session
                    .query(Order.class)
                    .orderBy("ShipTo.Country")
                    .selectFields(String.class, "ShipTo.Country")
                     // Call 'distinct' to remove duplicates from results
                     // Items wil be compared based on field 'Country' that is specified in the above 'selectFields' 
                    .distinct()
                    .toList();
                    
                // Running this on the Northwind sample data
                // will result in a sorted list of 21 countries w/o duplicates.
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region distinct_2_1
                // Count the number of unique countries:
                // =====================================
                
                int numberOfCountries = session
                    .query(Order.class)
                    .selectFields(String.class, "ShipTo.Country")
                    .distinct()
                    .count();
                    
                // Running this on the Northwind sample data,
                // will result in 21, which is the number of unique countries.
                //endregion
            }


                //region distinct_3_2
                // Query the map-reduce index defined above
                try (IDocumentSession session = DocumentStoreHolder.store.openSession()) {
                    Orders_ByShipToCountry.Result queryResult = session
                        .query(Orders_ByShipToCountry.Result.class, Orders_ByShipToCountry.class)
                        .toList();

                    // The resulting list contains all index-entry items where each entry represents a country. 
                    // The size of the list corresponds to the number of unique countries.
                    int numberOfUniqueCountries = queryResult.length;
                }
                //endregion
            }
        }
    }
}
