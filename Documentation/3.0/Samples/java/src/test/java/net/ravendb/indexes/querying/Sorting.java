package net.ravendb.indexes.querying;

import java.util.List;
import java.util.UUID;

import net.ravendb.abstractions.data.Constants;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.data.SortedField;
import net.ravendb.abstractions.indexing.SortOptions;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QProduct;


public class Sorting {

  //region sorting_1_4
  public static class Products_ByUnitsInStock extends AbstractIndexCreationTask {
    public Products_ByUnitsInStock() {
      map =
       " from product in docs.Products " +
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
      map =
       " from product in docs.Products " +
       " select new                    " +
       "   {                           " +
       "       product.Name    " +
       "   };";
    }
  }

  @SuppressWarnings("unused")
  public Sorting() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region sorting_1_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .where(p.unitsInStock.gt(10))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region sorting_1_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .whereGreaterThan(p.unitsInStock, 10)
          .toList();
        //endregion
      }

      //region sorting_1_3
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock",
          new IndexQuery("UnitsInStock_Range:{Ix10 TO NULL}"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region sorting_2_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .where(p.unitsInStock.gt(10))
          .orderBy(p.unitsInStock.desc())
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region sorting_2_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .whereGreaterThan(p.unitsInStock, 10)
          .orderByDescending(p.unitsInStock)
          .toList();
        //endregion
      }

      //region sorting_2_3
      IndexQuery query = new IndexQuery();
      query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
      query.setSortedFields(new SortedField[] { new SortedField("-UnitsInStock_Range") });
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock", query);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region sorting_3_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .customize(new DocumentQueryCustomizationFactory().randomOrdering())
          .where(p.unitsInStock.gt(10))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region sorting_3_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .randomOrdering()
          .whereGreaterThan(p.unitsInStock, 10)
          .toList();
        //endregion
      }

      //region sorting_3_3
      IndexQuery query = new IndexQuery();
      query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
      query.setSortedFields(new SortedField[] {
        new SortedField(Constants.RANDOM_FIELD_NAME + ";" + UUID.randomUUID())
      });
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock", query);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region sorting_4_1
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class, Products_ByUnitsInStock.class)
          .where(p.unitsInStock.gt(10))
          .orderByScore()
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region sorting_4_2
        QProduct p = QProduct.product;
        List<Product> results = session
          .advanced()
          .documentQuery(Product.class, Products_ByUnitsInStock.class)
          .whereGreaterThan(p.unitsInStock, 10)
          .orderByScore()
          .toList();
        //endregion
      }

      //region sorting_4_3
      IndexQuery query = new IndexQuery();
      query.setQuery("UnitsInStock_Range:{Ix10 TO NULL}");
      query.setSortedFields(new SortedField[] {
        new SortedField(Constants.TEMPORARY_SCORE_VALUE)
      });
      QueryResult result = store
        .getDatabaseCommands()
        .query("Products/ByUnitsInStock", query);
      //endregion
    }
  }
}
