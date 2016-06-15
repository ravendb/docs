package net.ravendb.indexes.querying;

import java.util.List;

import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.indexing.FieldStorage;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.RavenPagingInformation;
import net.ravendb.client.RavenQueryStatistics;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Order;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QProduct;


public class Paging {

  //region paging_0_4
  public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {
    public Products_ByUnitsInStock() {
      map =
       " from product in docs.Products            " +
       " select new                               " +
       " {                                        " +
       "     UnitsInStock = product.UnitsInStock  " +
       " };";
    }
  }
  //endregion

  //region paging_6_0
  public static class Orders_ByOrderLines_ProductName extends AbstractIndexCreationTask {
    @SuppressWarnings("boxing")
    public Orders_ByOrderLines_ProductName() {
      map =
       " from order in docs.Orders " +
       " from line in order.Lines       " +
       " select new                     " +
       " {                              " +
       "     Product = line.ProductName " +
       " };";

       maxIndexOutputsPerDocument = 1024L;
    }
  }
  //endregion

  //region paging_7_0
  public static class Orders_ByStoredProductName extends AbstractIndexCreationTask {
    public static class Result {
      private String product;
      public String getProduct() {
        return product;
      }
      public void setProduct(String product) {
        this.product = product;
      }
    }
    @SuppressWarnings("boxing")
    public Orders_ByStoredProductName() {
      map =
       " from order in docs.Orders " +
       " from line in order.Lines       " +
       " select new                     " +
       " {                              " +
       "     Product = line.ProductName " +
       " };";

       maxIndexOutputsPerDocument = 1024L;
       store("Product", FieldStorage.YES);
    }
  }
  //endregion

  @SuppressWarnings({"unused", "boxing"})
  public Paging() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_0_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .where(p.unitsInStock.gt(10))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_0_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .whereGreaterThan(p.unitsInStock, 10)
          .toList();
        //endregion
      }

      //region paging_0_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock",
          new IndexQuery("UnitsInStock_Range:{Ix10 TO NULL}"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_1_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .where(p.unitsInStock.gt(10))
          .take(9999) // server will decrease this value to 1024
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_1_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .whereGreaterThan(p.unitsInStock, 10)
          .take(9999) // server will decrease this value to 1024
          .toList();
        //endregion
      }

      //region paging_1_3
      IndexQuery query = new IndexQuery();
      query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
      query.setPageSize(9999);
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock", query);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_2_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .where(p.unitsInStock.gt(10))
          .skip(20) // skip 2 pages worth of products
          .take(10) // take up to 10 products
          .toList(); // execute query
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_2_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .whereGreaterThan(p.unitsInStock, 10)
          .skip(20) // skip 2 pages worth of products
          .take(10) // take up to 10 products
          .toList(); // execute query
        //endregion
      }

      //region paging_2_3
      IndexQuery query = new IndexQuery();
      query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
      query.setStart(20); // skip 2 pages worth of products
      query.setPageSize(10); // take up to 10 products

      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock", query);
      //endregion
    }


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_3_1
        QProduct p = QProduct.product;
        Reference<RavenQueryStatistics> statsRef = new Reference<RavenQueryStatistics>();
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .statistics(statsRef) // fill query statistics
          .where(p.unitsInStock.gt(10))
          .skip(20)
          .take(10)
          .toList();

        int totalResults = statsRef.value.getTotalResults();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_3_2
        QProduct p = QProduct.product;
        Reference<RavenQueryStatistics> statsRef = new Reference<RavenQueryStatistics>();
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .statistics(statsRef) // fill query statistics
          .whereGreaterThan(p.unitsInStock, 10)
          .skip(20)
          .take(10)
          .toList();

        int totalResults = statsRef.value.getTotalResults();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region paging_3_3
      IndexQuery query = new IndexQuery();
      query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
      query.setStart(20);
      query.setPageSize(10);
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock", query);

      int totalResults = result.getTotalResults();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_4_1
        Reference<RavenQueryStatistics> statsRef = new Reference<RavenQueryStatistics>();
        List<Product> results;
        int pageNumber = 0;
        int pageSize = 10;
        int skippedResults = 0;

        QProduct p = QProduct.product;

        do {
          results = session
            .query(Product.class, Products_ByUnitsInStock.class)
            .statistics(statsRef)
            .skip((pageNumber * pageSize) + skippedResults)
            .take(pageSize)
            .where(p.unitsInStock.gt(10))
            .distinct()
            .toList();

          skippedResults += statsRef.value.getSkippedResults();
          pageNumber++;
        } while (results.size() > 0);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_4_2
        Reference<RavenQueryStatistics> statsRef = new Reference<RavenQueryStatistics>();
        List<Product> results;
        int pageNumber = 0;
        int pageSize = 10;
        int skippedResults = 0;

        QProduct p = QProduct.product;

        do {
          results = session
            .advanced()
            .documentQuery(Product.class, Products_ByUnitsInStock.class)
            .statistics(statsRef)
            .skip((pageNumber * pageSize) + skippedResults)
            .take(pageSize)
            .whereGreaterThan(p.unitsInStock, 10)
            .distinct()
            .toList();

          skippedResults += statsRef.value.getSkippedResults();
          pageNumber++;
        } while (results.size() > 0);
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region paging_4_3
      QueryResult result;
      int pageNumber = 0;
      int pageSize = 10;
      int skippedResults = 0;

      do {
        IndexQuery query = new IndexQuery();
        query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
        query.setStart((pageNumber * pageSize) + skippedResults);
        query.setPageSize(pageSize);
        query.setDistinct(true);
        result = store
          .getDatabaseCommands()
          .query("Products/ByUnitsInStock", query);

        skippedResults += result.getSkippedResults();
        pageNumber++;
      } while (result.getResults().size() > 0);

      //endregion
    }


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_6_1
        Reference<RavenQueryStatistics> statsRef = new Reference<RavenQueryStatistics>();
        List<Order> results;
        int pageNumber = 0;
        int pageSize = 10;
        int skippedResults = 0;

        do {
          results = session
            .query(Order.class, Orders_ByOrderLines_ProductName.class)
            .statistics(statsRef)
            .skip((pageNumber * pageSize) + skippedResults)
            .take(pageSize)
            .toList();

          skippedResults += statsRef.value.getSkippedResults();
          pageNumber++;
        } while  (results.size() > 0);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_6_2
        Reference<RavenQueryStatistics> statsRef = new Reference<RavenQueryStatistics>();
        List<Order> results;
        int pageNumber = 0;
        int pageSize = 10;
        int skippedResults = 0;

        do {
          results = session
            .advanced()
            .documentQuery(Order.class, Orders_ByOrderLines_ProductName.class)
            .statistics(statsRef)
            .skip((pageNumber * pageSize) + skippedResults)
            .take(pageSize)
            .toList();

          skippedResults += statsRef.value.getSkippedResults();
          pageNumber++;
        } while  (results.size() > 0);
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region paging_6_3
      QueryResult result;
      int pageNumber = 0;
      int pageSize = 10;
      int skippedResults = 0;

      do {
        IndexQuery query = new IndexQuery();
        query.setStart((pageNumber * pageSize) + skippedResults);
        query.setPageSize(pageSize);

        result = store
          .getDatabaseCommands()
          .query("Orders/ByOrderLines/ProductName", query);

        skippedResults += result.getSkippedResults();
        pageNumber++;
      } while (result.getResults().size() > 0);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_5_1
        RavenPagingInformation pagingInformation = new RavenPagingInformation();
        Product[] results = session
          .advanced()
          .loadStartingWith(Product.class,
            "products/", // all documents starting with 'products/'
            "1*|2*", // rest of the key must begin with "1" or "2" e.g. products/10, products/25
            0 * 25,  // skip 0 records (page 1)
            25,  // take up to 25
            null,
            pagingInformation); // fill `RavenPagingInformation` with operation data

        results = session
          .advanced()
          .loadStartingWith(Product.class,
            "products/", // fill `RavenPagingInformation` with operation data
            "1*|2*",  // rest of the key must begin with "1" or "2" e.g. products/10, products/25
            1 * 25, // skip 25 records (page 2)
            25, // take up to 25
            null,
            pagingInformation); // since this is a next page to 'page 1' and we are passing 'RavenPagingInformation' that was filled during 'page 1' retrieval, rapid pagination will take place
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region paging_7_1
        List<String> results;
        int pageNumber = 0;
        int pageSize = 10;

        do {
          results = session
             .query(Orders_ByStoredProductName.Result.class, Orders_ByStoredProductName.class)
             .select(String.class, "Product")
             .skip(pageSize * pageNumber)
             .take(pageSize)
             .distinct()
             .toList();

          pageNumber++;
        } while (results.size() > 0);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region paging_7_2
        List<Orders_ByStoredProductName.Result> results;
        int pageNumber = 0;
        int pageSize = 10;

        do {
          results = session
             .advanced()
             .documentQuery(Order.class, Orders_ByStoredProductName.class)
             .selectFields(Orders_ByStoredProductName.Result.class, "Product")
             .skip(pageSize * pageNumber)
             .take(pageSize)
             .distinct()
             .toList();

          pageNumber++;
        } while (results.size() > 0);
        //endregion
      }

      //region paging_7_3
      QueryResult result;
      int pageNumber = 0;
      int pageSize = 10;

      do {
        IndexQuery query = new IndexQuery();
        query.setStart(pageNumber * pageSize);
        query.setPageSize(pageSize);
        query.setDistinct(true);
        query.setFieldsToFetch(new String[] { "product" });

        result = store.getDatabaseCommands()
           .query("Orders/ByStoredProductName", query);
        pageNumber++;
      } while (result.getResults().size() > 0);
      //endregion
    }
  }
}
