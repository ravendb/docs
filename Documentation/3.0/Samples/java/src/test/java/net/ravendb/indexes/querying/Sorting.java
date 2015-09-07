package net.ravendb.indexes.querying;

import com.google.common.collect.ImmutableMap;
import com.mysema.query.annotations.QueryEntity;
import net.ravendb.abstractions.data.Constants;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.data.SortedField;
import net.ravendb.abstractions.indexing.FieldIndexing;
import net.ravendb.abstractions.indexing.FieldStorage;
import net.ravendb.abstractions.indexing.SortOptions;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QEmployee;
import net.ravendb.samples.northwind.QProduct;

import java.util.List;
import java.util.UUID;

public class Sorting {

    //region sorting_5_6
    public static class Employee_ByFirstName extends AbstractIndexCreationTask {

        public Employee_ByFirstName() {
            QEmployee e = QEmployee.employee;
            map = "from employee in docs.employees select new  { employee.FirstName } ";
            store(e.firstName, FieldStorage.YES);
        }
    }
    //endregion

    //region sorting_6_4
    public static class Products_ByName_Search extends AbstractIndexCreationTask {

        @QueryEntity public static class Result {

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
            QProduct p = QProduct.product;
            map = "from product in docs.products select new  { Name = product.Name, NameForSorting = product.Name } ";
            indexes.put(p.name, FieldIndexing.ANALYZED);
        }
    }
    //endregion

    //region sorting_1_4
    public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {

        public Products_ByUnitsInStock() {
            map = " from product in docs.Products " +
                " select new                    " +
                "   {                           " +
                "       product.UnitsInStock    " +
                "   };";

            QProduct p = QProduct.product;
            sort(p.unitsInStock, SortOptions.INT);
        }
    }
    //endregion

    public static class Products_ByName extends AbstractIndexCreationTask {

        public Products_ByName() {
            map = " from product in docs.Products " +
                " select new                    " +
                "   {                           " +
                "       product.Name    " +
                "   };";
        }
    }

    @SuppressWarnings({"unused", "boxing"}) public Sorting() throws Exception {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_1_1
                QProduct p = QProduct.product;
                List<Product> results = session.query(Product.class, Products_ByUnitsInStock.class)
                    .where(p.unitsInStock.gt(10)).toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_1_2
                QProduct p = QProduct.product;
                List<Product> results = session.advanced().documentQuery(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan(p.unitsInStock, 10).toList();
                //endregion
            }

            //region sorting_1_3
            QueryResult result = store.getDatabaseCommands()
                .query("Products/ByUnitsInStock", new IndexQuery("UnitsInStock_Range:{Ix10 TO NULL}"));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_2_1
                QProduct p = QProduct.product;
                List<Product> results = session.query(Product.class, Products_ByUnitsInStock.class)
                    .where(p.unitsInStock.gt(10)).orderBy(p.unitsInStock.desc()).toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_2_2
                QProduct p = QProduct.product;
                List<Product> results = session.advanced().documentQuery(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan(p.unitsInStock, 10).orderByDescending(p.unitsInStock).toList();
                //endregion
            }

            //region sorting_2_3
            IndexQuery query = new IndexQuery();
            query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
            query.setSortedFields(new SortedField[] {new SortedField("-UnitsInStock_Range")});
            QueryResult result = store.getDatabaseCommands().query("Products/ByUnitsInStock", query);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_3_1
                QProduct p = QProduct.product;
                List<Product> results = session.query(Product.class, Products_ByUnitsInStock.class)
                    .customize(new DocumentQueryCustomizationFactory().randomOrdering()).where(p.unitsInStock.gt(10))
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_3_2
                QProduct p = QProduct.product;
                List<Product> results = session.advanced().documentQuery(Product.class, Products_ByUnitsInStock.class)
                    .randomOrdering().whereGreaterThan(p.unitsInStock, 10).toList();
                //endregion
            }

            //region sorting_3_3
            IndexQuery query = new IndexQuery();
            query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
            query.setSortedFields(
                new SortedField[] {new SortedField(Constants.RANDOM_FIELD_NAME + ";" + UUID.randomUUID())});
            QueryResult result = store.getDatabaseCommands().query("Products/ByUnitsInStock", query);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_4_1
                QProduct p = QProduct.product;
                List<Product> results = session.query(Product.class, Products_ByUnitsInStock.class)
                    .where(p.unitsInStock.gt(10)).orderByScore().toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_4_2
                QProduct p = QProduct.product;
                List<Product> results = session.advanced().documentQuery(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan(p.unitsInStock, 10).orderByScore().toList();
                //endregion
            }

            //region sorting_4_3
            IndexQuery query = new IndexQuery();
            query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
            query.setSortedFields(new SortedField[] {new SortedField(Constants.TEMPORARY_SCORE_VALUE)});
            QueryResult result = store.getDatabaseCommands().query("Products/ByUnitsInStock", query);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_5_3
                List<Employee> results = session.query(Employee.class, Employee_ByFirstName.class).customize(
                    new DocumentQueryCustomizationFactory()
                        .customSortUsing("AssemblyQualifiedName", true)).addTransformerParameter("len", 1).toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region sorting_5_4
                List<Employee> results = session.advanced().documentQuery(Employee.class, Employee_ByFirstName.class)
                    .customSortUsing("SortByNumberOfCharactersFromEnd", true)
                    .setTransformerParameters(ImmutableMap.of("len", RavenJToken.fromObject(1))).toList();
            }

            //region sorting_5_5
            IndexQuery indexQuery = new IndexQuery();
            SortedField sortedField = new SortedField(Constants.CUSTOM_SORT_FIELD_NAME +
                "-" + // "-" - descending, "" - ascending
                ";" +
                "SorterFullName");
            indexQuery.setSortedFields(new SortedField[] {sortedField});
            indexQuery.setTransformerParameters(ImmutableMap.of("len", RavenJToken.fromObject(1)));

            store.getDatabaseCommands().query("Employees/ByFirstName", indexQuery);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region sorting_6_1
                QSorting_Products_ByName_Search_Result p = QSorting_Products_ByName_Search_Result.result;
                List<Products_ByName_Search.Result> results = session
                    .query(Products_ByName_Search.Result.class, Products_ByName_Search.class)
                    .search(p.name, "Louisiana").orderBy(p.nameForSorting.desc()).toList();
                //endregion
            }
            try (IDocumentSession session = store.openSession()) {
                //region sorting_6_2
                List<Product> results = session.advanced().documentQuery(Product.class, Products_ByName_Search.class)
                    .search("Name", "Louisiana").orderByDescending("NameForSorting").toList();
                //endregion
            }

            //region sorting_6_3
            IndexQuery indexQuery = new IndexQuery();
            indexQuery.setQuery("Name:Louisiana*");
            indexQuery.setSortedFields(new SortedField[] {new SortedField("-NameForSorting")});

            store.getDatabaseCommands().query("Products/ByName/Search", indexQuery);
            //endregion
        }
    }
}
