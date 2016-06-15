package net.ravendb.clientapi.session.querying;

import java.util.List;

import com.mysema.query.annotations.QueryEntity;
import com.mysema.query.types.Path;

import net.ravendb.abstractions.data.FacetResult;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.data.FacetValue;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.DynamicAggregationQuery;


public class HowToPerformDynamicAggregation {
  @QueryEntity
  public static class Order {
    private String companyId;
    private String customerId;
    private double totalPrice;

    public String getCompanyId() {
      return companyId;
    }
    public void setCompanyId(String companyId) {
      this.companyId = companyId;
    }
    public String getCustomerId() {
      return customerId;
    }
    public void setCustomerId(String customerId) {
      this.customerId = customerId;
    }
    public double getTotalPrice() {
      return totalPrice;
    }
    public void setTotalPrice(double totalPrice) {
      this.totalPrice = totalPrice;
    }
  }

  @SuppressWarnings("unused")
  private interface IFoo<T> {
    //region aggregate_1
    public DynamicAggregationQuery<T> aggregateBy(String path);

    public DynamicAggregationQuery<T> aggregateBy(String path, String displayName);

    public DynamicAggregationQuery<T> aggregateBy(Path<?> path);

    public DynamicAggregationQuery<T> aggregateBy(Path<?> path, String displayName);
    //endregion
  }

  @SuppressWarnings({"unused", "boxing"})
  public HowToPerformDynamicAggregation() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region aggregate_2
        // sum up all order prices
        // for each customer
        // where price is higher than 500
        QHowToPerformDynamicAggregation_Order o = QHowToPerformDynamicAggregation_Order.order;
        FacetResults aggregationResults = session
          .query(Order.class, "Orders/All")
          .where(o.totalPrice.gt(500))
          .aggregateBy(o.customerId)
            .sumOn(o.totalPrice)
          .toList();

        FacetResult customerAggregation = aggregationResults.getResults().get("CustomerId");
        Double sum = customerAggregation.getValues().get(0).getSum();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region aggregate_3
        // count all orders
        // for each customer
        // where price is higher than 500
        QHowToPerformDynamicAggregation_Order o = QHowToPerformDynamicAggregation_Order.order;
        FacetResults aggregationResults = session
          .query(Order.class, "Orders/All")
          .where(o.totalPrice.gt(500))
          .aggregateBy(o.customerId)
            .countOn(o.totalPrice)
          .toList();

        FacetResult customerAggregation = aggregationResults.getResults().get("CustomerId");
        Integer count = customerAggregation.getValues().get(0).getCount();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region aggregate_4
        // count price average for orders
        // for each customer
        // where price is higher than 500
        QHowToPerformDynamicAggregation_Order o = QHowToPerformDynamicAggregation_Order.order;
        FacetResults aggregationResults = session
          .query(Order.class, "Orders/All")
          .where(o.totalPrice.gt(500))
          .aggregateBy(o.customerId)
            .averageOn(o.totalPrice)
          .toList();

        FacetResult customerAggregation = aggregationResults.getResults().get("CustomerId");
        Double average = customerAggregation.getValues().get(0).getAverage();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region aggregate_5
        // count max and min price for orders
        // for each customer
        // where price is higher than 500
        QHowToPerformDynamicAggregation_Order o = QHowToPerformDynamicAggregation_Order.order;
        FacetResults aggregationResults = session
          .query(Order.class, "Orders/All")
          .where(o.totalPrice.gt(500))
          .aggregateBy(o.customerId)
            .minOn(o.totalPrice)
            .maxOn(o.totalPrice)
          .toList();

        FacetResult customerAggregation = aggregationResults.getResults().get("CustomerId");
        Double min = customerAggregation.getValues().get(0).getMin();
        Double max = customerAggregation.getValues().get(0).getMax();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region aggregate_6
        QHowToPerformDynamicAggregation_Order o = QHowToPerformDynamicAggregation_Order.order;
        FacetResults aggregationResults = session.query(Order.class, "Orders/All")
          .aggregateBy(o.customerId)
            .addRanges(o.totalPrice.lt(10))
            .addRanges(o.totalPrice.goe(100).and(o.totalPrice.lt(500)))
            .addRanges(o.totalPrice.goe(500).and(o.totalPrice.lt(1000)))
            .addRanges(o.totalPrice.goe(1000))
          .toList();

        FacetResult customerAggregation = aggregationResults.getResults().get("CustomerId");
        List<FacetValue> values = customerAggregation.getValues();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region aggregate_7
        // sum up all order prices
        // for each customer
        // count price average for all orders
        // for each company
        QHowToPerformDynamicAggregation_Order o = QHowToPerformDynamicAggregation_Order.order;
        FacetResults aggregationResults = session
          .query(Order.class, "Orders/All")
          .aggregateBy(o.customerId)
            .sumOn(o.totalPrice)
          .andAggregateOn(o.companyId)
            .averageOn(o.totalPrice)
          .toList();

        FacetResult customerAggregation = aggregationResults.getResults().get("CustomerId");
        FacetResult companyAggregation = aggregationResults.getResults().get("CompanyId");

        Double sum = customerAggregation.getValues().get(0).getSum();
        Double average = companyAggregation.getValues().get(0).getAverage();
        //endregion
      }
    }
  }
}
