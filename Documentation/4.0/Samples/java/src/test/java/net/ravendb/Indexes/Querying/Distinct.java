package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Distinct {

    //region distinct_3_1
    public static class Employees_ByCountry extends AbstractIndexCreationTask {

    public Employees_ByCountry() {

        // The map phase indexes the country listed in each employee document
        // CountryCount is assigned with 1, which will be aggregated in the reduce phase
        map = "docs.Employees.Select(employee => new { " +
              "    Country = employee.Address.Country, " +
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
                // returns sorted list of countries w/o duplicates
                List<String> countries = session
                    .query(Order.class)
                    .orderBy("ShipTo.Country")
                    .selectFields(String.class, "ShipTo.Country")
                    .distinct()
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region distinct_2_1
                // results will contain the number of unique countries
                int numberOfCountries = session
                    .query(Order.class)
                    .selectFields(String.class, "ShipTo.Country")
                    .distinct()
                    .count();
                //endregion
            }


                //region distinct_3_2
                try (IDocumentSession session = DocumentStoreHolder.store.openSession()) {
                    Employees_ByCountry.Result queryResult = session.query(Employees_ByCountry.Result.class, Employees_ByCountry.class)
                        .whereEquals("Country", country)
                        .firstOrDefault();

                    int numberOfEmployeesFromCountry = queryResult != null ? queryResult.getCountryCount() : 0;
                }
                //endregion
            }
        }
    }
}
