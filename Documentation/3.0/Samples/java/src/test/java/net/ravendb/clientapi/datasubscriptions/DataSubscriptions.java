package net.ravendb.clientapi.datasubscriptions;

import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.basic.CleanCloseable;
import net.ravendb.abstractions.data.SubscriptionBatchOptions;
import net.ravendb.abstractions.data.SubscriptionConfig;
import net.ravendb.abstractions.data.SubscriptionConnectionOptions;
import net.ravendb.abstractions.data.SubscriptionCriteria;
import net.ravendb.abstractions.exceptions.subscriptions.SubscriptionException;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.ObserverAdapter;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.document.IReliableSubscriptions;
import net.ravendb.client.document.Subscription;
import net.ravendb.samples.northwind.Order;
import net.ravendb.samples.northwind.QOrder;


public class DataSubscriptions {

  @SuppressWarnings({"boxing", "unused"})
  public DataSubscriptions() {
    IDocumentStore store = new DocumentStore();
    //region accessing_subscriptions
    IReliableSubscriptions subscriptions = store.subscriptions();
    //endregion

    {
      //region create_2
      SubscriptionCriteria criteria = new SubscriptionCriteria();
      criteria.setKeyStartsWith("employees/");
      criteria.propertyNotMatch("Address.City", RavenJToken.fromObject("Seattle"));
      long id = store.subscriptions().create(criteria);
      //endregion
    }

    {
      //region create_3
      QOrder o = QOrder.order;
      SubscriptionCriteria criteria = new SubscriptionCriteria();
      criteria.propertyMatch(o.company, "company/1");
      long id = store.subscriptions().create(criteria);
      //endregion
    }

    long id = 0L;
    //region open_2
    SubscriptionConnectionOptions connectionOptions = new SubscriptionConnectionOptions();
    connectionOptions.setIgnoreSubscribersErrors(false);
    connectionOptions.setClientAliveNotificationInterval(30 * 60 * 1000);
    SubscriptionBatchOptions batchOptions = new SubscriptionBatchOptions();
    connectionOptions.setBatchOptions(batchOptions);
    batchOptions.setMaxSize(4 * 1024 * 1024);
    batchOptions.setMaxDocCount(16 * 1024);
    batchOptions.setAcknowledgmentTimeout(3 * 60 * 1000L);
    Subscription<Order> orders = store.subscriptions().open(Order.class, id, connectionOptions);
    //endregion

    //region open_3
    orders.subscribe(new ObserverAdapter<Order>() {
      @Override
      public void onNext(Order value) {
        generateInvoice(value);
      }
    });

    orders.subscribe(new ObserverAdapter<Order>() {
      @Override
      public void onNext(Order value) {
        if (value.getRequireAt().getTime() > new Date().getTime())
          sendReminder(value.getEmployee(), value.getId());
      }
    });
    //endregion

    //region open_4
    CleanCloseable subscriber = orders.subscribe(null);
    subscriber.close();
    //endregion

    //region delete_2
    store.subscriptions().delete(id);
    //endregion

    //region get_subscriptions_2
    List<SubscriptionConfig> configs = store.subscriptions().getSubscriptions(0, 10);
    //endregion

    //region release_2
    store.subscriptions().release(id);
    //endregion
  }

  @SuppressWarnings("unused")
  protected void sendReminder(String employee, String id) {
    // empty by design
  }

  @SuppressWarnings("unused")
  protected void generateInvoice(Order value) {
    // empty by design
  }

  @SuppressWarnings("unused")
  private interface IFoo {
    //region create_1
    long create(Class<?> expectedType, SubscriptionCriteria criteria);

    long create(Class<?> expectedType, SubscriptionCriteria criteria, String database);

    long create(SubscriptionCriteria criteria);

    long create(SubscriptionCriteria criteria, String database);
    //endregion

    //region open_1
    <T> Subscription<T> open(Class<T> clazz, long id, SubscriptionConnectionOptions options) throws SubscriptionException;

    <T> Subscription<T> open(Class<T> clazz, long id, SubscriptionConnectionOptions options, String database) throws SubscriptionException;

    Subscription<RavenJObject> open(long id, SubscriptionConnectionOptions options) throws SubscriptionException;

    Subscription<RavenJObject> open(long id, SubscriptionConnectionOptions options, String database) throws SubscriptionException;
    //endregion

    //region delete_1
    void delete(long id);

    void delete(long id, String database);
    //endregion

    //region get_subscriptions_1
    List<SubscriptionConfig> getSubscriptions(int start, int take);

    List<SubscriptionConfig> getSubscriptions(int start, int take, String database);
    //endregion

    //region release_1
    void release(long id);

    void release(long id, String database);
    //endregion

  }
}
