package net.ravendb.indexes.querying;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.indexing.SortOptions;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;


public class DynamicAggregation {
  //region currency
  public enum Currency {
    EUR,
    USD
  }
  //endregion

  //region order
  @QueryEntity
  public static class Order {
    private double total;
    private String product;
    private Currency currency;

    public double getTotal() {
      return total;
    }
    public void setTotal(double total) {
      this.total = total;
    }
    public String getProduct() {
      return product;
    }
    public void setProduct(String product) {
      this.product = product;
    }
    public Currency getCurrency() {
      return currency;
    }
    public void setCurrency(Currency currency) {
      this.currency = currency;
    }
  }
  //endregion

  //region dynamic_aggregation_index_def
  public static class Orders_All extends AbstractIndexCreationTask {
    public Orders_All() {
      QDynamicAggregation_Order o = QDynamicAggregation_Order.order;
      map =
       " from order in docs.Orders         " +
       " select new                        " +
       " {                                 " +
       "     order.Total,                  " +
       "     order.Product,                " +
       "     Concurrency = order.Currency  " +
       " }; ";

      sort(o.total, SortOptions.DOUBLE);
    }
  }
  //endregion

  @SuppressWarnings({"unused", "boxing"})
  public DynamicAggregation() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region dynamic_aggregation_1
        QDynamicAggregation_Order o = QDynamicAggregation_Order.order;
        FacetResults facetResults = session
          .query(Order.class, Orders_All.class)
          .where(o.total.gt(500))
          .aggregateBy(o.product)
            .sumOn(o.total)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region dynamic_aggregation_range
        QDynamicAggregation_Order o = QDynamicAggregation_Order.order;
        FacetResults facetResults = session
          .query(Order.class, Orders_All.class)
          .aggregateBy(o.product)
          .addRanges(
            o.total.lt(100),
            o.total.goe(100).and(o.total.lt(500)),
            o.total.goe(500).and(o.total.lt(1500)),
            o.total.goe(1500)
          )
          .sumOn(o.total)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region dynamic_aggregation_multiple_items
        QDynamicAggregation_Order o = QDynamicAggregation_Order.order;
        FacetResults facetResults = session
          .query(Order.class, Orders_All.class)
          .aggregateBy(o.product)
            .sumOn(o.total)
            .countOn(o.total)
          .andAggregateOn(o.currency)
            .minOn(o.total)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region dynamic_aggregation_different_fieldss
        QDynamicAggregation_Order o = QDynamicAggregation_Order.order;
        FacetResults facetResults = session
          .query(Order.class, Orders_All.class)
          .aggregateBy(o.product)
            .maxOn(o.total)
            .minOn(o.currency)
          .toList();
        //endregion
      }
    }
  }
}
