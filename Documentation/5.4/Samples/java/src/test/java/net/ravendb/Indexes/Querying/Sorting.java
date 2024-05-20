package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.queries.spatial.PointField;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.OrderingType;

import java.util.List;

public class Sorting {

    //region sorting_5_6
    public static class Employee_ByFirstName extends AbstractIndexCreationTask {
        public Employee_ByFirstName() {
            map = "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName" +
                "})";

            store("FirstName", FieldStorage.YES);
        }
    }
    //endregion

    //region sorting_1_4
    public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {
        public Products_ByUnitsInStock() {
            map = "docs.Products.Select(product => new {" +
                "    UnitsInStock = product.UnitsInStock" +
                "})";
        }
    }
    //endregion

    //region sorting_1_5
    public static class Products_ByUnitsInStockAndName extends AbstractIndexCreationTask {
        public Products_ByUnitsInStockAndName() {
            map = "docs.Products.Select(product => new {" +
                "    UnitsInStock = product.UnitsInStock" +
                "    Name = product.Name" +
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
                "    Name = product.Name," +
                "    NameForSorting = product.Name" +
                "})";

            index("Name", FieldIndexing.SEARCH);
        }
    }
    //endregion

    //region sorting_9_3
    public static class Events_ByCoordinates extends AbstractIndexCreationTask {
        public Events_ByCoordinates() {
            map = "docs.Events.Select(e => new {" +
                "   Coordinates = this.CreateSpatialField(e.Latitude, e.Longitude)" +
                "})";
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
                    .whereGreaterThan("UnitsInStock", 10)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_2_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .orderByDescending("UnitsInStock")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_8_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .orderByDescending("UnitsInStock", OrderingType.STRING)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_3_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .randomOrdering()
                    .whereGreaterThan("UnitsInStock", 10)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_4_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .orderByScore()
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_4_3
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStockAndName.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .orderBy("UnitsInStock")
                    .orderByScore()
                    .orderByDescending("Name")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_6_1
                List<Product> results = session
                    .query(Product.class, Products_ByName_Search.class)
                    .search("Name", "Louisiana")
                    .orderByDescending("NameForSorting")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_7_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .orderBy("Name", OrderingType.ALPHA_NUMERIC)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_9_1
                List<Events> results = session
                    .query(Events.class, Events_ByCoordinates.class)
                    .spatial("Coordinates", criteria -> criteria.withinRadius(500, 30, 30))
                    .orderByDistance(new PointField("Latitude", "Longitude"), 32.1234, 23.4321)
                    .toList();
                //endregion
            }
        }
    }

    public static class Events {

    }
}
