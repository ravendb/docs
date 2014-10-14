package net.ravendb.clientapi.session.querying;

import static org.junit.Assert.assertEquals;

import java.util.List;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractTransformerCreationTask;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.samples.northwind.Category;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QProduct;
import net.ravendb.samples.northwind.Supplier;


public class HowToUseTransformers {
  private class Products_Name extends AbstractTransformerCreationTask {
    // ...
  }

  //region transformers_5
  public class ProductWithCategoryAndSupplier {
    private String name;
    private Supplier supplier;
    private Category category;

    public String getName() {
      return name;
    }
    public void setName(String name) {
      this.name = name;
    }
    public Supplier getSupplier() {
      return supplier;
    }
    public void setSupplier(Supplier supplier) {
      this.supplier = supplier;
    }
    public Category getCategory() {
      return category;
    }
    public void setCategory(Category category) {
      this.category = category;
    }
  }
  //endregion

  //region transformers_4
  public class Products_WithCategoryAndSupplier extends AbstractTransformerCreationTask {
    public Products_WithCategoryAndSupplier() {
      transformResults = "from product in results               " +
               " select new {                                   " +
               "     Name = product.Name,                       " +
               "     Category = LoadDocument(product.Category), " +
               "     Supplier = LoadDocument(product.Supplier)  " +
               " }";
    }
  }
  //endregion

  @SuppressWarnings("unused")
  private interface IFoo {
    //region transformers_1
    <S> IRavenQueryable<S> transformWith(String transformerName, Class<S> resultClass);

    <S> IRavenQueryable<S> transformWith(Class<? extends AbstractTransformerCreationTask> transformerClazz, Class<S> resultClass);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToUseTransformers() throws Exception {
    try (IDocumentStore store = new DocumentStore()){
      try (IDocumentSession session = store.openSession()) {
        //region transformers_2
        // return up to 128 entities from 'Products' collection
        // transform results using 'Products_Name' transformer
        // which returns only 'name' field, rest will be 'null'
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class)
          .where(p.name.eq("Chocolade"))
          .transformWith(Products_Name.class, Product.class)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region transformers_3
        // return 1 entity from 'Products' collection
        // transform results using 'Products_WithCategoryAndSupplier' transformer
        // project results to 'ProductWithCategoryAndSupplier' class
        QProduct p = QProduct.product;
        ProductWithCategoryAndSupplier product = session
          .query(Product.class)
          .where(p.name.eq("Chocolade"))
          .transformWith(Products_WithCategoryAndSupplier.class, ProductWithCategoryAndSupplier.class)
          .first();

        assertEquals("Chocolade", product.getName());
        assertEquals("Confections", product.getCategory().getName());
        assertEquals("Zaanse Snoepfabriek", product.getSupplier().getName());
        //endregion
      }
    }
  }
}
