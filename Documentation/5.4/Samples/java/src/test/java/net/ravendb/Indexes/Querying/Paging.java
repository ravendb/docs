package net.ravendb.Indexes.Querying;

import net.ravendb.ClientApi.Operations.Statistics;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.QueryStatistics;
import net.ravendb.client.primitives.Reference;

import java.util.List;

public class Paging {

    //region paging_0_4
    public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {
        public Products_ByUnitsInStock() {
            map = "docs.Products.Select(product => new {" +
                "    UnitsInStock = product.UnitsInStock" +
                "})";
        }
    }
    //endregion

    //region paging_6_0
    public static class Orders_ByOrderLines_ProductName extends AbstractIndexCreationTask {
        public Orders_ByOrderLines_ProductName() {
            map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new {" +
                "    Product = line.ProductName " +
                "})";
        }
    }
    //endregion

    private static class Order_ByOrderLines_ProductName extends AbstractIndexCreationTask {

    }

    private static class Product {

    }

    private static class Order {

    }

    public Paging() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region paging_0_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region paging_2_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .skip(20) // skip 2 pages worth of products
                    .take(10) // take up to 10 products
                    .toList(); // execute query
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region paging_3_1
                Reference<QueryStatistics> stats = new Reference<QueryStatistics>();

                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class)
                    .statistics(stats)
                    .whereGreaterThan("UnitsInStock", 10)
                    .skip(20)
                    .take(10)
                    .toList();

                int totalResults = stats.value.getTotalResults();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region paging_4_1
                List<Product> results;
                int pageNumber = 0;
                int pageSize = 10;
                int skippedResults = 0;
                Reference<QueryStatistics> stats = new Reference<>();

                do {

                    results = session
                        .query(Product.class, Products_ByUnitsInStock.class)
                        .statistics(stats)
                        .skip((pageNumber * pageSize) + skippedResults)
                        .take(pageSize)
                        .whereGreaterThan("UnitsInStock", 10)
                        .distinct()
                        .toList();

                    skippedResults += stats.value.getSkippedResults();
                    pageNumber++;
                } while (results.size() > 0);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region paging_6_1
                List<Order> results;
                int pageNumber = 0;
                int pageSize = 50;
                int skippedResults = 0;
                Reference<QueryStatistics> stats = new Reference<>();

                do {
                    results = session
                        .query(Order.class, Order_ByOrderLines_ProductName.class)
                        .statistics(stats)
                        .skip((pageNumber * pageSize) + skippedResults)
                        .take(pageSize)
                        .toList();

                    skippedResults += stats.value.getSkippedResults();
                    pageNumber++;
                } while (results.size() > 0);
                //endregion
            }
        }

    }
}
