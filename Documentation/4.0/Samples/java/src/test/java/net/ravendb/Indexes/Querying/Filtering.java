package net.ravendb.Indexes.Querying;

import com.sun.org.apache.xpath.internal.operations.Or;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.lang.reflect.Array;
import java.util.Arrays;
import java.util.List;

public class Filtering {

    private class Employee {
    }

    private class Product {
    }

    private class Order {
    }

    private class BlogPost {
    }

    //region filtering_0_4
    public class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            map = "docs.Employees.Select(employee => new {" +
                "    firstName = employee.firstName," +
                "    lastName = employee.lastName" +
                "})";
        }
    }
    //endregion

    //region filtering_1_4
    public class Products_ByUnitsInStock extends AbstractIndexCreationTask {
        public Products_ByUnitsInStock() {
            map = "docs.Products.Select(product => new {" +
                "        unitsInStock = product.unitsInStock" +
                "    })";
        }
    }
    //endregion

    //region filtering_7_4
    public static class Orders_ByTotalPrice extends AbstractIndexCreationTask {
        public static class Result {
            private double totalPrice;

            public double getTotalPrice() {
                return totalPrice;
            }

            public void setTotalPrice(double totalPrice) {
                this.totalPrice = totalPrice;
            }
        }
        public Orders_ByTotalPrice() {
            map = "docs.Orders.Select(order => new {" +
                "    totalPrice = Enumerable.Sum(order.lines, x => ((decimal)((((decimal) x.quantity) * x.pricePerUnit) * (1M - x.discount))))" +
                "})";
        }
    }
    //endregion

    //region filtering_2_4
    public class Order_ByOrderLinesCount extends AbstractIndexCreationTask {
        public Order_ByOrderLinesCount() {
            map = "docs.Orders.Select(order => new {" +
                "    lines_count = order.lines.Count" +
                "})";
        }
    }
    //endregion

    //region filtering_3_4
    public class Order_ByOrderLines_ProductName extends AbstractIndexCreationTask {
        public Order_ByOrderLines_ProductName() {
            map = "docs.Orders.Select(order => new {" +
                "    lines_productName = order.lines.Select(x => x.productName)" +
                "})";
        }
    }
    //endregion

    //region filtering_5_4
    public class BlogPosts_ByTags extends AbstractIndexCreationTask {
        public BlogPosts_ByTags() {
            map = "docs.BlogPosts.Select(post => new {" +
                "    tags = post.tags" +
                "})";
        }
    }
    //endregion

    public Filtering() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region filtering_0_1
                List<Employee> results = session
                    .query(Employee.class, Employees_ByFirstAndLastName.class) // query 'Employees/ByFirstAndLastName' index
                    .whereEquals("firstName", "Robert") // filtering predicates
                    .andAlso()   // by default OR is between each condition
                    .whereEquals("lastName", "King") // materialize query by sending it to server for processing
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_1_1
                List<Product> results = session
                    .query(Product.class, Products_ByUnitsInStock.class) // query 'Products/ByUnitsInStock' index
                    .whereGreaterThan("unitsInStock", 50) // filtering predicates
                    .toList(); // materialize query by sending it to server for processing

                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_7_1
                List<Order> results = session
                    .query(Orders_ByTotalPrice.Result.class, Orders_ByTotalPrice.class)
                    .whereGreaterThan("totalPrice", 50)
                    .ofType(Order.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_2_1
                List<Order> results = session
                    .query(Order.class, Order_ByOrderLinesCount.class) // query 'Order/ByOrderLinesCount' index
                    .whereGreaterThan("lines_count", 50) // filtering predicates
                    .toList();   // materialize query by sending it to server for processing
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_3_1
                session
                    .query(Order.class, Order_ByOrderLines_ProductName.class) // query 'Order/ByOrderLines/ProductName' index
                    .whereEquals("lines_productName", "Teatime Chocolate Biscuits") // filtering predicates
                    .toList(); // materialize query by sending it to server for processing
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_4_1
                List<Employee> results = session
                    .query(Employee.class, Employees_ByFirstAndLastName.class) // query 'Employees/ByFirstAndLastName' index
                    .whereIn("firstName", Arrays.asList("Robert", "Nancy")) // filtering predicates
                    .toList();// materialize query by sending it to server for processing
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_5_1
                List<BlogPost> results = session
                    .query(BlogPost.class, BlogPosts_ByTags.class)  // query 'BlogPosts/ByTags' index
                    .containsAny("tags", Arrays.asList("Development", "Research")) // filtering predicates
                    .toList(); // materialize query by sending it to server for processing
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_6_1
                List<BlogPost> results = session
                    .query(BlogPost.class, BlogPosts_ByTags.class) // query 'BlogPosts/ByTags' index
                    .containsAll("tags", Arrays.asList("Development", "Research")) // filtering predicates
                    .toList(); // materialize query by sending it to server for processing
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_8_1
                // return all products which name starts with 'ch'
                List<Product> results = session
                    .query(Product.class)
                    .whereStartsWith("name", "ch")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_9_1
                // return all products which name ends with 'ra'
                List<Product> results = session
                    .query(Product.class)
                    .whereEndsWith("name", "ra")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region filtering_10_1
                // return all orders that were shipped to 'Albuquerque'
                List<Order> results = session
                    .query(Order.class)
                    .whereEquals("shipTo_city", "Albuquerque")
                    .toList();
                //endregion
            }
        }
    }
}
