package net.ravendb.ClientApi.DataSubscriptions;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.subscriptions.*;
import net.ravendb.client.exceptions.database.DatabaseDoesNotExistException;
import net.ravendb.client.exceptions.documents.subscriptions.*;
import net.ravendb.client.exceptions.security.AuthorizationException;
import net.ravendb.client.primitives.ExceptionsUtils;
import net.ravendb.northwind.Order;
import net.ravendb.northwind.OrderLine;
import net.ravendb.northwind.Product;
import org.apache.commons.logging.Log;

import java.time.Duration;
import java.util.Date;
import java.util.concurrent.CompletableFuture;
import java.util.function.Consumer;

public class DataSubscriptions {

    private interface ISubscriptionCreationOveloads {
        //region subscriptionCreationOverloads
        String create(SubscriptionCreationOptions options);

        String create(SubscriptionCreationOptions options, String database);

        <T> String create(Class<T> clazz);

        <T> String create(Class<T> clazz, SubscriptionCreationOptions options);

        <T> String create(Class<T> clazz, SubscriptionCreationOptions options, String database);

        <T> String createForRevisions(Class<T> clazz);

        <T> String createForRevisions(Class<T> clazz, SubscriptionCreationOptions options);

        <T> String createForRevisions(Class<T> clazz, SubscriptionCreationOptions options, String database);
        //endregion
    }

    public interface ISubscriptionConsumptionOverloads {
        //region subscriptionWorkerGeneration
        SubscriptionWorker<ObjectNode> getSubscriptionWorker(SubscriptionWorkerOptions options);
        SubscriptionWorker<ObjectNode> getSubscriptionWorker(SubscriptionWorkerOptions options, String database);

        SubscriptionWorker<ObjectNode> getSubscriptionWorker(String subscriptionName);
        SubscriptionWorker<ObjectNode> getSubscriptionWorker(String subscriptionName, String database);

        <T> SubscriptionWorker<T> getSubscriptionWorker(Class<T> clazz, SubscriptionWorkerOptions options);
        <T> SubscriptionWorker<T> getSubscriptionWorker(Class<T> clazz, SubscriptionWorkerOptions options, String database);

        <T> SubscriptionWorker<T> getSubscriptionWorker(Class<T> clazz, String subscriptionName);
        <T> SubscriptionWorker<T> getSubscriptionWorker(Class<T> clazz, String subscriptionName, String database);

        <T> SubscriptionWorker<Revision<T>> getSubscriptionWorkerForRevisions(Class<T> clazz, SubscriptionWorkerOptions options);
        <T> SubscriptionWorker<Revision<T>> getSubscriptionWorkerForRevisions(Class<T> clazz, SubscriptionWorkerOptions options, String database);

        <T> SubscriptionWorker<Revision<T>> getSubscriptionWorkerForRevisions(Class<T> clazz, String subscriptionName);
        <T> SubscriptionWorker<Revision<T>> getSubscriptionWorkerForRevisions(Class<T> clazz, String subscriptionName, String database);
        //endregion
    }

    public interface ISubscriptionWorkerRunning<T> {
        //region subscriptionWorkerRunning
        CompletableFuture<Void> run(Consumer<SubscriptionBatch<T>> processDocuments);
        //endregion
    }

    //region subscriptions_example
    public void worker(IDocumentStore store) {
        SubscriptionCreationOptions options = new SubscriptionCreationOptions();
        options.setQuery("from Orders where Company = 'companies/11'");

        String subscriptionName = store.subscriptions().create(Order.class, options);
        SubscriptionWorker<Order> subscription = store
            .subscriptions().getSubscriptionWorker(Order.class, subscriptionName);
        subscription.run(x -> {
            for (SubscriptionBatch.Item<Order> item : x.getItems()) {
                System.out.println("Order #"
                    + item.getResult().getId()
                    + " will be shipped via: " + item.getResult().getShipVia());
            }
        });
    }
    //endregion

    interface FakeDocumentSubscriptions {

    }

    interface FakeStore {

    }

    public void creationExamples() {
        String name;
        IDocumentStore store = new DocumentStore();

        {
            //region create_whole_collection_generic_with_name
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setName("OrdersProcessingSubscription");
            name = store.subscriptions().create(Order.class, options);
            //endregion
        }

        {
            //region create_whole_collection_generic_with_mentor_node
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setMentorNode("D");
            name = store.subscriptions().create(Order.class, options);
            //endregion
        }

        {
            //region create_whole_collection_generic1
            name = store.subscriptions().create(Order.class);
            //endregion
        }

        {
            //region create_whole_collection_RQL
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery("from Orders");
            name = store.subscriptions().create(options);
            //endregion
        }

        {
            //region create_filter_only_RQL
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery("declare function getOrderLinesSum(doc) {" +
                " var sum = 0;" +
                " for (var i in doc.Lines) { sum += doc.Lines[i]; }" +
                " return sum;" +
                "}" +
                "from Orders as o " +
                "where getOrderLinesSum(o) > 100 ");

            name = store.subscriptions().create(options);
            //endregion
        }

        {
            //region create_filter_and_projection_RQL
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery(" declare function getOrderLinesSum(doc) {" +
                "  var sum = 0; " +
                "  for (var i in doc.Lines) { sum += doc.Lines[i]; }" +
                "  return sum;" +
                "}" +
                "" +
                " declare function projectOrder(doc) {" +
                "    return {" +
                "        Id: order.Id," +
                "        Total: getOrderLinesSum(order)" +
                "    }" +
                " }" +
                " from order as o " +
                " where getOrderLinesSum(o) > 100 " +
                " select projectOrder(o)");

            name = store.subscriptions().create(options);
            //endregion
        }

        {
            //region create_filter_and_load_document_RQL
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery(" declare function getOrderLinesSum(doc) {" +
                "  var sum = 0; " +
                "  for (var i in doc.Lines) { sum += doc.Lines[i]; }" +
                "  return sum;" +
                "}" +
                "" +
                " declare function projectOrder(doc) {" +
                "    var employee = LoadDocument(doc.Employee); " +
                "    return {" +
                "        Id: order.Id," +
                "        Total: getOrderLinesSum(order)," +
                "        ShipTo: order.ShipTo," +
                "        EmployeeName: employee.FirstName + ' ' + employee.LastName " +
                "    }" +
                " }" +
                " from order as o " +
                " where getOrderLinesSum(o) > 100 " +
                " select projectOrder(o)");

            name = store.subscriptions().create(options);
            //endregion
        }

        {
            //region create_simple_revisions_subscription_generic
            name = store.subscriptions().createForRevisions(Order.class);
            //endregion
        }

        {
            //region create_simple_revisions_subscription_RQL
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery("from orders (Revisions = true)");
            name = store.subscriptions().createForRevisions(Order.class, options);
            //endregion
        }

        {
            //region use_simple_revision_subscription_generic
            SubscriptionWorker<Revision<Order>> revisionWorker = store
                .subscriptions().getSubscriptionWorkerForRevisions(Order.class, name);
            revisionWorker.run(x -> {
                for (SubscriptionBatch.Item<Revision<Order>> documentsPair : x.getItems()) {

                    Order prev = documentsPair.getResult().getPrevious();
                    Order current = documentsPair.getResult().getCurrent();

                    processOrderChanges(prev, current);
                }
            });
            //endregion
        }

        {
            //region create_projected_revisions_subscription_RQL
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery("declare function getOrderLinesSum(doc) {" +
                "    var sum = 0;" +
                "    for (var i in doc.Lines) { sum += doc.Lines[i]; } " +
                "    return sum;" +
                "}" +
                "" +
                "  from orders (Revisions = true) " +
                " where getOrderLinesSum(this.Current) > getOrderLinesSum(this.Previous) " +
                " select {" +
                "  previousRevenue: getOrderLinesSum(this.Previous)," +
                "  currentRevenue: getOrderLinesSum(this.Current)" +
                "}");

            name = store.subscriptions().create(options);
            //endregion
        }

        {
            //region consume_revisions_subscription_generic
            SubscriptionWorker<OrderRevenues> revenuesComparisonWorker = store
                .subscriptions().getSubscriptionWorker(OrderRevenues.class, name);

            revenuesComparisonWorker.run(x -> {
                for (SubscriptionBatch.Item<OrderRevenues> item : x.getItems()) {
                    System.out.println("Revenue for order with Id: "
                        + item.getId() + " grown from "
                        + item.getResult().getPreviousRevenue()
                        + " to " + item.getResult().getCurrentRevenue());
                }
            });
            //endregion
        }
    }

    void processOrderChanges(Order prev, Order cur) {

    }

    public interface IMaintainanceOperations {
        //region interface_subscription_deletion
        void delete(String name);

        void delete(String name, String database);
        //endregion

        //region interface_subscription_dropping
        void dropConnection(String name);

        void dropConnection(String name, String database);
        //endregion

        //region interface_subscription_state
        SubscriptionState getSubscriptionState(String subscriptionName);

        SubscriptionState getSubscriptionState(String subscriptionName, String database);
        //endregion
    }

    public void subscriptionMaintainance() {
        String subscriptionName = "";
        try (IDocumentStore store = new DocumentStore()) {
            //region subscription_deletion
            store.subscriptions().delete(subscriptionName);
            //endregion

            //region connection_dropping
            store.subscriptions().dropConnection(subscriptionName);
            //endregion

            //region subscription_state
            SubscriptionState subscriptionState = store.subscriptions().getSubscriptionState(subscriptionName);
            //endregion
        }
    }

    public static class OrderRevenues {
        private int previousRevenue;
        private int currentRevenue;

        public int getPreviousRevenue() {
            return previousRevenue;
        }

        public void setPreviousRevenue(int previousRevenue) {
            this.previousRevenue = previousRevenue;
        }

        public int getCurrentRevenue() {
            return currentRevenue;
        }

        public void setCurrentRevenue(int currentRevenue) {
            this.currentRevenue = currentRevenue;
        }
    }

    public static class OrderAndCompany {
        private String orderId;
        private Company company;

        public String getOrderId() {
            return orderId;
        }

        public void setOrderId(String orderId) {
            this.orderId = orderId;
        }

        public Company getCompany() {
            return company;
        }

        public void setCompany(Company company) {
            this.company = company;
        }
    }

    private class Company {
    }

    public void openingExamples() throws Exception {
        String name;
        DocumentStore store = new DocumentStore();
        SubscriptionWorker<Order> subscriptionWorker;
        CompletableFuture<Void> subscriptionRuntimeTask;
        String subscriptionName = null;

        //region subscription_open_simple
        subscriptionWorker = store.subscriptions().getSubscriptionWorker(Order.class, subscriptionName);
        //endregion

        //region subscription_run_simple
        subscriptionRuntimeTask = subscriptionWorker.run(batch -> {
            // your logic here
        });
        //endregion

        {
            //region subscription_worker_with_batch_size
            SubscriptionWorkerOptions options = new SubscriptionWorkerOptions(subscriptionName);
            options.setMaxDocsPerBatch(20);
            SubscriptionWorker<Order> workerWBatch = store.subscriptions().getSubscriptionWorker(Order.class, options);
            workerWBatch.run(x -> { /* custom logic */});
            //endregion
        }

        {
            SubscriptionWorker<Order> workerWBatch = null;
            //region throw_during_user_logic
            workerWBatch.run(x -> {
                throw new RuntimeException();
            });
            //endregion
        }

        Log logger = null;

        //region reconnecting_client
        while (true) {
            SubscriptionWorkerOptions options = new SubscriptionWorkerOptions(subscriptionName);
            // here we configure that we allow a down time of up to 2 hours,
            // and will wait for 2 minutes for reconnecting

            options.setMaxErroneousPeriod(Duration.ofHours(2));
            options.setTimeToWaitBeforeConnectionRetry(Duration.ofMinutes(2));

            subscriptionWorker = store.subscriptions().getSubscriptionWorker(Order.class, options);

            try {
                // here we are able to be informed of any exception that happens during processing
                subscriptionWorker.addOnSubscriptionConnectionRetry(exception -> {
                    logger.error("Error during subscription processing: " + subscriptionName, exception);
                });

                subscriptionWorker.run(batch -> {
                    for (SubscriptionBatch.Item<Order> item : batch.getItems()) {
                        // we want to force close the subscription processing in that case
                        // and let the external code decide what to do with that
                        if ("Europe".equalsIgnoreCase(item.getResult().getShipVia())) {
                            throw new IllegalStateException("We cannot ship via Europe");
                        }
                        processOrder(item.getResult());
                    }
                }).get();


                // Run will complete normally if you have disposed the subscription
                return;
            } catch (Exception e) {
                logger.error("Failure in subscription: " + subscriptionName, e);

                e = ExceptionsUtils.unwrapException(e);
                if (e instanceof DatabaseDoesNotExistException ||
                    e instanceof SubscriptionDoesNotExistException ||
                    e instanceof SubscriptionInvalidStateException ||
                    e instanceof AuthorizationException) {
                    throw e; // not recoverable
                }

                if (e instanceof SubscriptionClosedException) {
                    // closed explicitly by admin, probably
                    return;
                }

                if (e instanceof SubscriberErrorException) {
                    SubscriberErrorException se = (SubscriberErrorException) e;
                    // for IllegalStateException type, we want to throw an exception, otherwise
                    // we continue processing
                    if (se.getCause() != null && se.getCause() instanceof IllegalStateException) {
                        throw e;
                    }

                    continue;
                }

                // handle this depending on subscription
                // open strategy (discussed later)
                if (e instanceof SubscriptionInUseException) {
                    continue;
                }

                return;
            } finally {
                subscriptionWorker.close();
            }
        }
        //endregion
    }

    private void processOrder(Order result) {
    }

    private static void singleRun() {
        IDocumentStore store = null;
        String subsId = null;
        //region single_run
        SubscriptionWorkerOptions options = new SubscriptionWorkerOptions(subsId);

        // Here we ask the worker to stop when there are no documents left to send.
        // Will throw SubscriptionClosedException when it finishes it's job
        options.setCloseWhenNoDocsLeft(true);
        SubscriptionWorker<OrderAndCompany> highValueOrdersWorker = store
            .subscriptions().getSubscriptionWorker(OrderAndCompany.class, options);

        try {
            highValueOrdersWorker.run(batch -> {
                for (SubscriptionBatch.Item<OrderAndCompany> item : batch.getItems()) {
                    sendThankYouNoteToEmployee(item.getResult());
                }
            });
        } catch (SubscriptionClosedException e) {
            //that's expected
        }
        //endregion
    }

    private static void sendThankYouNoteToEmployee(OrderAndCompany oac) {
        // empty
    }

    public void twoSubscription1(DocumentStore store, String subscriptionName) {
        //region waiting_subscription_1
        SubscriptionWorkerOptions options1 = new SubscriptionWorkerOptions(subscriptionName);
        options1.setStrategy(SubscriptionOpeningStrategy.TAKE_OVER);
        SubscriptionWorker<Order> worker1 = store.subscriptions().getSubscriptionWorker(Order.class, options1);


        while (true) {
            try {
                worker1
                    .run(x -> {
                        // your logic
                    });
            } catch (Exception e) {
                // retry
            }
        }
        //endregion
    }
    public void twoSubscription2(DocumentStore store, String subscriptionName) {
        //region waiting_subscription_2
        SubscriptionWorkerOptions options2 = new SubscriptionWorkerOptions(subscriptionName);
        options2.setStrategy(SubscriptionOpeningStrategy.WAIT_FOR_FREE);
        SubscriptionWorker<Order> worker2 = store.subscriptions().getSubscriptionWorker(Order.class, options2);

        while (true) {
            try {
                worker2
                    .run(x -> {
                        // your logic
                    });
            } catch (Exception e) {
                // retry
            }
        }
        //endregion
    }


    public void createSubscriptionWithIncludeStatement(DocumentStore store) {
        {
            //region create_subscription_with_includes_rql_path
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery("from Orders include Lines[].Product");
            store.subscriptions().create(options);
            //endregion
        }

        {
            //region create_subscription_with_includes_rql_javascript
            SubscriptionCreationOptions options = new SubscriptionCreationOptions();
            options.setQuery("declare function includeProducts(doc) " +
                "   {" +
                "       doc.IncludedFields=0;" +
                "       doc.LinesCount = doc.Lines.length;" +
                "       for (let i=0; i< doc.Lines.length; i++)" +
                "       {" +
                "           doc.IncludedFields++;" +
                "           include(doc.Lines[i].Product);" +
                "       }" +
                "       return doc;" +
                "   }" +
                "   from Orders as o select includeProducts(o)");
            store.subscriptions().create(options);
            //endregion
        }

        {
            //region dynamic_worker
            String subscriptionName = "My dynamic subscription";

            SubscriptionCreationOptions subscriptionCreationOptions = new SubscriptionCreationOptions();
            subscriptionCreationOptions.setName("My dynamic subscription");
            subscriptionCreationOptions.setQuery("from Orders as o \n" +
                "select { \n" +
                "   DynamicField_1: 'Company:' + o.Company + ' Employee: ' + o.Employee \n" +
                "}");

            SubscriptionWorker<ObjectNode> worker = store.subscriptions().getSubscriptionWorker(subscriptionName);
            worker.run(x -> {
                for (SubscriptionBatch.Item<ObjectNode> item : x.getItems()) {
                    ObjectNode result = item.getResult();
                    raiseNotification(result.get("DynamicField_1"));
                }
            });
            //endregion
        }

    }

    public void raiseNotification(Object str) {
    }

    public void consumeSubscriptionWithIncludeStatement(DocumentStore store) {
        //region subscription_with_open_session_usage
        SubscriptionCreationOptions subscriptionCreationOptions = new SubscriptionCreationOptions();
        subscriptionCreationOptions.setQuery("from Orders as o where o.ShippedAt = null");
        String subscriptionName = store.subscriptions().create(subscriptionCreationOptions);

        SubscriptionWorker<Order> subscriptionWorker = store.subscriptions().getSubscriptionWorker(Order.class, subscriptionName);

        subscriptionWorker.run(batch -> {
            try (IDocumentSession session = batch.openSession()) {
                for (SubscriptionBatch.Item<Order> orderItem : batch.getItems()) {
                    transferOrderToShipmentCompany(orderItem.getResult());
                    orderItem.getResult().setShippedAt(new Date());
                }

                // we know that we have at least one order to ship,
                // because the subscription query above has that in it's WHERE clause
                session.saveChanges();
            }
        });
        //endregion
    }

    private void transferOrderToShipmentCompany(Order order) {

    }

    public void consumeSubscriptionWithIncludeStatements(DocumentStore store) {
        //region subscription_with_includes_path_usage
        SubscriptionCreationOptions subscriptionCreationOptions = new SubscriptionCreationOptions();
        subscriptionCreationOptions.setQuery("from Orders include Lines[].Product");


        String subscriptionName = store.subscriptions().create(subscriptionCreationOptions);

        SubscriptionWorker<Order> subscriptionWorker = store.subscriptions().getSubscriptionWorker(Order.class, subscriptionName);

        subscriptionWorker.run(batch -> {
            try (IDocumentSession session = batch.openSession()) {
                for (SubscriptionBatch.Item<Order> orderItem : batch.getItems()) {
                    Order order = orderItem.getResult();
                    for (OrderLine orderLine : order.getLines()) {
                        // this line won't generate a request, because orderLine.Product was included
                        Product product = session.load(Product.class, orderLine.getProduct());
                        raiseProductNotification(order, product);
                    }
                }
            }
        });
        //endregion
    }

    private void raiseProductNotification(Order order, Product product) {
    }

}
