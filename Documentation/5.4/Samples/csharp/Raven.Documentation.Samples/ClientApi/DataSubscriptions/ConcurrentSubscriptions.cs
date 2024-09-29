using Raven.Client.Documents;
using Raven.Client.Documents.Subscriptions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.ConcurrentDataSubscriptions
{
    public class ConcurrentDataSubscriptions
    {
        public void Create3ConcurrentSubscriptions()
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "sampleDB"
            };
            store.Initialize();

            #region conSub_defineWorkers
            // Define concurrent subscription workers
            var worker1 = store.Subscriptions.GetSubscriptionWorker<Order>(
                // Set the worker to connect to the "Get all orders" subscription task
                new SubscriptionWorkerOptions("Get all orders")
                {
                    // Set Concurrent strategy
                    Strategy = SubscriptionOpeningStrategy.Concurrent,
                    MaxDocsPerBatch = 20
                });

            var worker2 = store.Subscriptions.GetSubscriptionWorker<Order>(
                new SubscriptionWorkerOptions("Get all orders")
                {
                    Strategy = SubscriptionOpeningStrategy.Concurrent,
                    MaxDocsPerBatch = 20
                });
            #endregion

            /*
            #region conSub_OnEstablishedSubscriptionConnection
            worker1.OnEstablishedSubscriptionConnection += () =>
            {
                // your logic here
            };
            #endregion
            */

            #region conSub_defineStrategy
            var worker3 = store.Subscriptions.GetSubscriptionWorker<Order>(
                new SubscriptionWorkerOptions("Get all orders")
                {
                    // Concurrent strategy
                    Strategy = SubscriptionOpeningStrategy.Concurrent,
                    MaxDocsPerBatch = 20
                });
            #endregion

            #region conSub_runWorkers
            // Start the concurrent worker.
            // Workers will connect concurrently to the "Get all rders" subscription task.
            var worker1Task = worker1.Run(batch =>
            {
                // Process batch
                foreach (var item in batch.Items)
                {
                    // Process item
                }
            });

            var worker2Task = worker2.Run(batch =>
            {
                // Process batch
                foreach (var item in batch.Items)
                {
                    // Process item
                }
            });
            #endregion

            #region conSub_dropWorker
            // Drop a concurrent subscription worker
            store.Subscriptions.DropSubscriptionWorker(worker2);
            #endregion

            var worker3Task = worker3.Run(batch =>
            {
                foreach (var item in batch.Items)
                {
                    // Process item 
                }
            });
        }
    }
}
