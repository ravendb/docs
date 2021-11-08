using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Subscriptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.Exceptions.Documents.Subscriptions;
using Raven.Client.Exceptions.Security;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Sparrow.Logging;
using Xunit;

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
            var subscriptionWorker1 = store.Subscriptions.GetSubscriptionWorker<Order>(
                // Set the worker to connect to the "All Orders" subscription task
                new SubscriptionWorkerOptions("All Orders")
                {
                    // Set Concurrent strategy
                    Strategy = SubscriptionOpeningStrategy.Concurrent,
                    MaxDocsPerBatch = 20
                });

            var subscriptionWorker2 = store.Subscriptions.GetSubscriptionWorker<Order>(
                new SubscriptionWorkerOptions("All Orders")
                {
                    Strategy = SubscriptionOpeningStrategy.Concurrent,
                    MaxDocsPerBatch = 20
                });
            #endregion

            /*
            #region conSub_OnEstablishedSubscriptionConnection
            subscriptionWorker1.OnEstablishedSubscriptionConnection += () =>
            {
                // your logic here
            };
            #endregion

            */

            #region conSub_defineStrategy
            var subscriptionWorker3 = store.Subscriptions.GetSubscriptionWorker<Order>(
                new SubscriptionWorkerOptions("All Orders")
                {
                    // Concurrent strategy
                    Strategy = SubscriptionOpeningStrategy.Concurrent,
                    MaxDocsPerBatch = 20
                });
            #endregion

            #region conSub_runWorkers
            // Start the concurrent worker. Workers will connect concurrently to the "All Orders" subscription task.
            var subscriptionRuntimeTask1 = subscriptionWorker1.Run(batch =>
            {
                // process batch
                foreach (var item in batch.Items)
                {
                    // process item
                }
            });

            var subscriptionRuntimeTask2 = subscriptionWorker2.Run(batch =>
            {
                // process batch
                foreach (var item in batch.Items)
                {
                    // process item
                }
            });
            #endregion

            #region conSub_dropWorker
            //drop a concurrent subscription worker
            store.Subscriptions.DropSubscriptionWorker(subscriptionWorker2);
            #endregion

            var subscriptionRuntimeTask3 = subscriptionWorker3.Run(batch =>
            {
                foreach (var item in batch.Items)
                {
                    /* handle item */
                }
            });
        }
    }
}
