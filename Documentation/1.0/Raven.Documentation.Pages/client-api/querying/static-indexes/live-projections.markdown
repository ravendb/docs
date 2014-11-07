# Live Projections

Usually, RavenDB can tell what types you want to return based on the query type and the CLR type encoded in a document, but there are some cases where you want to query on one thing, but the result is completely different. This is usually the case when you are using live projections.

For example, let us take a look at the following index:

    public class PurchaseHistoryIndex : AbstractIndexCreationTask<Order, Order>
    {
        public PurchaseHistoryIndex()
        {
            Map = orders => from order in orders
                            from product in order.Items
                            select new
                                       {
                                           UserId = order.UserId,
                                           ProductId = product.Id
                                       };
            TransformResults = (database, orders) =>
                               from order in orders
                               from item in order.Items
                               let product = database.Load<Product>(item.Id)
                               where product != null
                               select new
                                          {
                                              ProductId = item.Id,
                                              ProductName = product.Name
                                          };

        }
    }

Note that when we query this index, we can query based on UserId or ProductId, but the result that we get back aren't of the same type that we query on. For that reason, we have the As<T>() extension method. We can use it to change the result type of the query:

    documentSession.Query<Shipment, PurchaseHistoryIndex>()
       .Where(x => x.UserId == userId)
       .As<PurchaseHistoryViewItem>()
       .ToArray(),

In the code above, we query the `PurchaseHistoryIndex` using `Shipment` as the entity type to search on, but we get the results as `PurchaseHistoryViewItem`.