package net.ravendb.indexes.querying;

import com.mysema.query.annotations.QueryEntity;
import com.mysema.query.types.Order;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.BlogPost;
import net.ravendb.samples.QBlogPost;
import net.ravendb.samples.northwind.*;
import org.junit.Test;

import java.util.Arrays;
import java.util.List;


public class Filtering {

  @Test
  public void testCreateIndexes() throws Exception {
    try (IDocumentStore store = new DocumentStore("http://localhost:8080", "sample").initialize()) {
      store.executeIndex(new Employees_ByFirstAndLastName());
      store.executeIndex(new Products_ByUnitsInStock());
      store.executeIndex(new Order_ByOrderLinesCount());
      store.executeIndex(new Order_ByOrderLines_ProductName());
      store.executeIndex(new BlogPosts_ByTags());
    }
  }

  public static class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
  {
    //region filtering_0_4
    public Employees_ByFirstAndLastName() {
      map =
       " from employee in docs.Employees " +
       " select new                              " +
       " {                                       " +
       "     FirstName = employee.FirstName,     " +
       "     LastName = employee.LastName        " +
       " };                                      ";
    }
    //endregion
  }

  //region filtering_1_4
  public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {
    public Products_ByUnitsInStock() {
      map =
       " from product in docs.Products " +
       " select new                           " +
       "   {                                  " +
       "       product.UnitsInStock           " +
       "   };                                 ";
    }
  }
  //endregion

  //region filtering_7_4
  public static class Orders_ByTotalPrice extends AbstractIndexCreationTask {
    @QueryEntity
    public static class Result {
      public double totalPrice;
    }

    public Orders_ByTotalPrice() {
      map = "from order in docs.orders select new { TotalPrice = order.Lines.Sum(x => (x.Quantity * x.PricePerUnit) * (1 - x.Discount)) }";
    }
  }
  //endregion

  //region filtering_2_4
  public static class Order_ByOrderLinesCount extends AbstractIndexCreationTask {
    public Order_ByOrderLinesCount() {
      map =
        " from order in docs.Orders     " +
        " select new                         " +
        " {                                  " +
        "   Lines_Count = order.Lines.Count  " +
        " };";
    }
  }
  //endregion

  //region filtering_3_4
  public static class Order_ByOrderLines_ProductName extends AbstractIndexCreationTask {
    public Order_ByOrderLines_ProductName() {
      map =
       " from order in docs.Orders                                  " +
       " select new                                                      " +
       " {                                                               " +
       "     Lines_ProductName = order.Lines.Select(x => x.ProductName)  " +
       " }; ";
    }
  }
  //endregion


  //region filtering_5_4
  public static class BlogPosts_ByTags extends AbstractIndexCreationTask {
    public BlogPosts_ByTags() {
      map =
       " from post in docs.Posts " +
       " select new                  " +
       "  {                          " +
       "      post.Tags              " +
       "  };";
    }
  }
  //endregion

  @SuppressWarnings({"unused", "boxing"})
  public Filtering() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_0_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class, Employees_ByFirstAndLastName.class)   //query 'Employees/ByFirstAndLastName' index
          .where(e.firstName.eq("Robert").and(e.lastName.eq("King")))  // filtering predicates
          .toList();     // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_0_2
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .advanced()
          .documentQuery(Employee.class, Employees_ByFirstAndLastName.class) // query 'Employees/ByFirstAndLastName' index
          .whereEquals(e.firstName, "Robert") // filtering predicates
          .andAlso()                       // by default OR is between each condition
          .whereEquals(e.lastName, "King") // filtering predicates
          .toList();                     // materialize query by sending it to server for processing
        //endregion
      }

      //region filtering_0_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("Employees/ByFirstAndLastName",
          new IndexQuery("FirstName:Robert AND LastName:King"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_1_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class) // query 'Products/ByUnitsInStock' index
          .where(p.unitsInStock.gt(10)) // filtering predicates
          .toList();  // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_1_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class) // query 'Products/ByUnitsInStock' index
          .whereGreaterThan(p.unitsInStock, 50) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      //region filtering_1_3
      store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock",
          new IndexQuery("UnitsInStock_Range:{Ix50 TO NULL}"));
      //endregion
    }


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_7_1
        QFiltering_Orders_ByTotalPrice_Result x = QFiltering_Orders_ByTotalPrice_Result.result;
        List<Orders_ByTotalPrice.Result> results = session.query(Orders_ByTotalPrice.Result.class, Orders_ByTotalPrice.class)
                .where(x.totalPrice.gt(50))
                .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_7_2
        QFiltering_Orders_ByTotalPrice_Result x = QFiltering_Orders_ByTotalPrice_Result.result;
        List<Orders_ByTotalPrice.Result> results = session.advanced().documentQuery(Orders_ByTotalPrice.Result.class, Orders_ByTotalPrice.class)
                .whereGreaterThan(x.totalPrice, 50.0)
                .toList();
        //endregion
      }

      //region filtering_7_3
      store.getDatabaseCommands().query("Orders/ByTotalPrice", new IndexQuery("TotalPrice_Range:{Dx50 TO NULL}"));
      //endregion

    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_6_1
        QFiltering_Orders_ByTotalPrice_Result r = QFiltering_Orders_ByTotalPrice_Result.result;
        List<Orders_ByTotalPrice.Result> results = session
            .query(Orders_ByTotalPrice.Result.class, Orders_ByTotalPrice.class).where(r.totalPrice.gt(50)).toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_6_2
        QFiltering_Orders_ByTotalPrice_Result r = QFiltering_Orders_ByTotalPrice_Result.result;
        List<Orders_ByTotalPrice.Result> results = session.advanced()
            .documentQuery(Orders_ByTotalPrice.Result.class, Orders_ByTotalPrice.class)
            .whereGreaterThan(r.totalPrice, 50.0).toList();
        //endregion
      }

      //region filtering_6_3
      store.getDatabaseCommands().query("Orders/ByTotalPrice", new IndexQuery("TotalPrice_Range:{Dx50 TO NULL}"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_2_1
        QOrder o = QOrder.order;
        List<Order> results = session
          .query(Order.class, Order_ByOrderLinesCount.class) // query 'Order/ByOrderLinesCount' index
          .where(o.lines.size().lt(50))  // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_2_2
        QOrder o = QOrder.order;
        session
          .advanced()
          .documentQuery(Order.class, Order_ByOrderLinesCount.class) // query 'Order/ByOrderLinesCount' index
          .whereGreaterThan(o.lines.size(), 50)   // filtering predicates
          .toList();  // materialize query by sending it to server for processing
        //endregion
      }

      //region filtering_2_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("Order/ByOrderLinesCount",
          new IndexQuery("Lines.Count_Range:{Ix50 TO NULL}"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_3_1
        QOrder o = QOrder.order;
        QOrderLine l = QOrderLine.orderLine;
        List<Order> results = session
          .query(Order.class, Order_ByOrderLines_ProductName.class) // query 'Order/ByOrderLines/ProductName' index
          .where(o.lines.any(l.productName.eq("Teatime Chocolate Biscuits"))) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_3_2
        List<Order> results = session
          .advanced()
          .documentQuery(Order.class, Order_ByOrderLines_ProductName.class) // query 'Order/ByOrderLines/ProductName' index
          .whereEquals("Lines,ProductName", "Teatime Chocolate Biscuits") // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_3_3
        QueryResult result = store
          .getDatabaseCommands()
          .query("Order/ByOrderLinesCount",
            new IndexQuery("Lines,ProductName:\"Teatime Chocolate Biscuits\""));
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_4_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class, Employees_ByFirstAndLastName.class) // query 'Employees/ByFirstAndLastName' index
          .where(e.firstName.in("Robert", "Nancy")) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_4_2
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .advanced()
          .documentQuery(Employee.class, Employees_ByFirstAndLastName.class) // query 'Employees/ByFirstAndLastName' index
          .whereIn(e.firstName, Arrays.asList("Robert", "Nancy")) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      //region filtering_4_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("Employees/ByFirstAndLastName",
             new IndexQuery("@in<FirstName>:(Robert, Nancy)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_5_1
        QBlogPost b = QBlogPost.blogPost;
        List<BlogPost> results = session
          .query(BlogPost.class, BlogPosts_ByTags.class) // query 'BlogPosts/ByTags' index
          .where(b.tags.containsAny(Arrays.asList("Development", "Research"))) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_5_2
        List<BlogPost> results = session
          .advanced()
          .documentQuery(BlogPost.class, BlogPosts_ByTags.class) // query 'BlogPosts/ByTags' index
          .containsAny("Tags", Arrays.<Object>asList("Development", "Research")) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      //region filtering_5_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("BlogPosts/ByTags",
          new IndexQuery("(Tags:Development OR Tags:Research)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region filtering_6_1
        QBlogPost b = QBlogPost.blogPost;
        List<BlogPost> results = session
          .query(BlogPost.class, BlogPosts_ByTags.class) // query 'BlogPosts/ByTags' index
          .where(b.tags.containsAll(Arrays.asList("Development", "Research"))) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region filtering_6_2
        List<BlogPost> results = session
          .advanced()
          .documentQuery(BlogPost.class, BlogPosts_ByTags.class) // query 'BlogPosts/ByTags' index
          .containsAll("Tags", Arrays.<Object>asList("Development", "Research")) // filtering predicates
          .toList(); // materialize query by sending it to server for processing
        //endregion
      }

      //region filtering_6_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("BlogPosts/ByTags",
          new IndexQuery("(Tags:Development AND Tags:Research)"));
      //endregion
    }

  }
}
