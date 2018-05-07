package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.OrderingType;

import java.util.List;

public class Sorting {

    //region sorting_5_6
    public static class Employee_ByFirstName extends AbstractIndexCreationTask {
        public Employee_ByFirstName() {
            map = "docs.Employees.Select(employee => new {" +
                "    firstName = employee.firstName" +
                "})";

            store("firstName", FieldStorage.YES);
        }
    }
    //endregion

    //region sorting_1_4
    public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {
        public Products_ByUnitsInStock() {
            map = "docs.Products.Select(product => new {" +
                "    unitsInStock = product.unitsInStock" +
                "})";
        }
    }
    //endregion

    //region sorting_6_4
    public static class Products_ByName_Search extends AbstractIndexCreationTask {
        public static class Result {
            private String name;
            private String nameForSorting;

            public String getName() {
                return name;
            }

            public void setName(String name) {
                this.name = name;
            }

            public String getNameForSorting() {
                return nameForSorting;
            }

            public void setNameForSorting(String nameForSorting) {
                this.nameForSorting = nameForSorting;
            }
        }

        public Products_ByName_Search() {
            map = "docs.Products.Select(product => new {" +
                "    name = product.name," +
                "    nameForSorting = product.name" +
                "})";

            index("name", FieldIndexing.SEARCH);
        }
    }
    //endregion

    public static class Products_ByName extends AbstractIndexCreationTask {

    }

    private static class Product {

    }

    public Sorting() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_1_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("unitsInStock", 10)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_2_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("unitsInStock", 10)
                    .orderByDescending("unitsInStock")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_8_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("unitsInStock", 10)
                    .orderByDescending("unitsInStock", OrderingType.STRING)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_3_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .randomOrdering()
                    .whereGreaterThan("unitsInStock", 10)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_4_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("unitsInStock", 10)
                    .orderByScore()
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_6_1
                List<Product> results = session
                    .query(Product.class, Products_ByName_Search.class)
                    .search("name", "Louisiana")
                    .orderByDescending("nameForSorting")
                    .toList();

                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_7_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("unitsInStock", 10)
                    .orderBy("name", OrderingType.ALPHA_NUMERIC)
                    .toList();
                //endregion
            }
        }
    }
}
