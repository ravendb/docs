using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session.Loaders;
using Raven.Client.Documents.Subscriptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.Exceptions.Documents.Subscriptions;
using Raven.Client.Exceptions.Security;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.DataSubscriptions
{
    public class DataSubscriptions
    {
        private interface ISubscriptionCreationOverloads
        {
            #region subscriptionCreationOverloads
            string Create(SubscriptionCreationOptions options,
                          string database = null);
            
            string Create<T>(SubscriptionCreationOptions<T> options,
                             string database = null);
            
            string Create<T>(Expression<Func<T, bool>> predicate = null,
                             SubscriptionCreationOptions options = null,
                             string database = null);

            Task<string> CreateAsync(SubscriptionCreationOptions options,
                                     string database = null,
                                     CancellationToken token = default);

            Task<string> CreateAsync<T>(SubscriptionCreationOptions<T> options,
                                        string database = null,
                                        CancellationToken token = default);

            Task<string> CreateAsync<T>(Expression<Func<T, bool>> predicate = null,
                                        SubscriptionCreationOptions options = null,
                                        string database = null,
                                        CancellationToken token = default);
            #endregion
        }

        private interface IUpdatingSubscription
        {
            #region updating_subscription
            string Update(SubscriptionUpdateOptions options, string database = null);

            Task<string> UpdateAsync(SubscriptionUpdateOptions options, string database = null,
                                CancellationToken token = default);
            #endregion
        }

        private interface ISubscriptionCreationOptions
        {
            #region sub_create_options
            public class SubscriptionCreationOptions
            {
                public string Name { get; set; }
                public string Query { get; set; }
                public string ChangeVector { get; set; }
                public string MentorNode { get; set; }
            }
            #endregion

            #region sub_create_options_strong
            public class SubscriptionCreationOptions<T>
            {
                public string Name { get; set; }
                public Expression<Func<T, bool>> Filter { get; set; }
                public Expression<Func<T, object>> Projection { get; set; }
                public Action<ISubscriptionIncludeBuilder<T>> Includes { get; set; }
                public string ChangeVector { get; set; }
                public string MentorNode { get; set; }
            }
            #endregion

            #region sub_update_options
            public class SubscriptionUpdateOptions : SubscriptionCreationOptions
            {
                public long? Id { get; set; }
                public bool CreateNew { get; set; }
            }
            #endregion
        }

        public interface ISubscriptionConsumptionOverloads
        {
            #region subscriptionWorkerGeneration
            SubscriptionWorker<dynamic> GetSubscriptionWorker(string subscriptionName, string database = null);
            SubscriptionWorker<dynamic> GetSubscriptionWorker(SubscriptionWorkerOptions options, string database = null);
            SubscriptionWorker<T> GetSubscriptionWorker<T>(string subscriptionName, string database = null) where T : class;
            SubscriptionWorker<T> GetSubscriptionWorker<T>(SubscriptionWorkerOptions options, string database = null) where T : class;
            #endregion
        }

        public interface ISubscriptionWorkerRunning<T>
        {
            #region subscriptionWorkerRunning
            Task Run(Action<SubscriptionBatch<T>> processDocuments, CancellationToken ct = default(CancellationToken));
            Task Run(Func<SubscriptionBatch<T>, Task> processDocuments, CancellationToken ct = default(CancellationToken));
            #endregion
        }

        #region subscriptions_example
        public async Task Worker(IDocumentStore store, CancellationToken cancellationToken)
        {
            // Create the ongoing subscription task on the server
            string subscriptionName = await store.Subscriptions
                .CreateAsync<Order>(x => x.Company == "companies/11");
            
            // Create a worker on the client that will consume the subscription
            SubscriptionWorker<Order> worker = store.Subscriptions
                .GetSubscriptionWorker<Order>(subscriptionName);
            
            // Run the worker task and process data received from the subscription
            Task workerTask = worker.Run(x => x.Items.ForEach(item =>
                Console.WriteLine($"Order #{item.Result.Id} will be shipped via: {item.Result.ShipVia}")),
                cancellationToken);

            await workerTask;
        }
        #endregion

        [Fact]
        public async Task CreationExamples()
        {
            string subscriptionName;
            IDocumentStore store = new DocumentStore();
            
            #region create_whole_collection_generic_with_name
            subscriptionName = store.Subscriptions.Create<Order>(new SubscriptionCreationOptions<Order>
            {
                // Set a custom name for the subscription 
                Name = "OrdersProcessingSubscription"
            });
            #endregion

            #region create_whole_collection_generic_with_mentor_node
            subscriptionName = store.Subscriptions.Create(new SubscriptionCreationOptions<Order>
            {
                MentorNode = "D"
            });
            #endregion

            #region create_whole_collection_generic1
            // With the following subscription definition, the server will send all documents
            // from the 'Orders' collection to a client that connects to this subscription.
            subscriptionName = store.Subscriptions.Create<Order>();
            #endregion

            #region create_whole_collection_RQL
            subscriptionName = store.Subscriptions.Create(new SubscriptionCreationOptions()
            {
                Query = "From Orders",
                Name = "OrdersProcessingSubscription"
            });
            #endregion

            #region create_filter_only_generic
            subscriptionName = store.Subscriptions.Create<Order>(x =>
                // Only documents matching this criteria will be sent
                x.Lines.Sum(line => line.PricePerUnit * line.Quantity) > 100);
            #endregion

            #region create_filter_only_RQL
            subscriptionName = store.Subscriptions.Create(new SubscriptionCreationOptions()
            {
                Query = @"declare function getOrderLinesSum(doc) {
                              var sum = 0;
                              for (var i in doc.Lines) {
                                  sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                              }
                              return sum;
                          }

                          From Orders as o 
                          Where getOrderLinesSum(o) > 100"
            });
            #endregion

            #region create_filter_and_projection_generic
            subscriptionName = store.Subscriptions.Create<Order>(new SubscriptionCreationOptions<Order>()
            {
                // The subscription criteria:
                Filter = x => x.Lines.Sum(line => line.PricePerUnit * line.Quantity) > 100,
                
                // The object properties that will be sent for each matching document:
                Projection = x => new
                {
                    Id = x.Id,
                    Total = x.Lines.Sum(line => line.PricePerUnit * line.Quantity)
                }
            });
            #endregion

            #region create_filter_and_projection_RQL
            subscriptionName = store.Subscriptions.Create(new SubscriptionCreationOptions()
            {
                Query = @"declare function getOrderLinesSum(doc) {
                              var sum = 0;
                              for (var i in doc.Lines) {
                                  sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                              }
                              return sum;
                          }

                          declare function projectOrder(doc) {
                              return {
                                  Id: doc.Id,
                                  Total: getOrderLinesSum(doc)
                              };
                          }

                          From Orders as o 
                          Where getOrderLinesSum(o) > 100
                          Select projectOrder(o)"
            });
            #endregion

            #region create_filter_and_load_document_generic
            subscriptionName = store.Subscriptions.Create<Order>(
                new SubscriptionCreationOptions<Order>()
                {
                    // The subscription criteria:
                    Filter = x => x.Lines.Sum(line => line.PricePerUnit * line.Quantity) > 100,
                    
                    // The object properties that will be sent for each matching document:
                    Projection = x => new
                    {
                        Id = x.Id,
                        Total = x.Lines.Sum(line => line.PricePerUnit * line.Quantity),
                        ShipTo = x.ShipTo,
                        
                        // 'Load' the related Employee document and use its data in the projection
                        EmployeeName = RavenQuery.Load<Employee>(x.Employee).FirstName + " " +
                                       RavenQuery.Load<Employee>(x.Employee).LastName
                    }
                });
            #endregion

            #region create_filter_and_load_document_RQL
            subscriptionName = store.Subscriptions.Create(new SubscriptionCreationOptions()
            {
                Query = @"declare function getOrderLinesSum(doc) {
                              var sum = 0;
                              for (var i in doc.Lines) {
                                  sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                              }
                              return sum;
                          }

                          declare function projectOrder(doc) {
                              var employee = load(doc.Employee);
                              return {
                                  Id: doc.Id,
                                  Total: getOrderLinesSum(doc),
                                  ShipTo: doc.ShipTo,
                                  EmployeeName: employee.FirstName + ' ' + employee.LastName
                              };
                          }

                          From Orders as o 
                          Where getOrderLinesSum(o) > 100
                          Select projectOrder(o)"
            });
            #endregion

            #region create_simple_revisions_subscription_generic
            subscriptionName = store.Subscriptions.Create(
                new SubscriptionCreationOptions<Revision<Order>>());
            #endregion

            #region create_simple_revisions_subscription_RQL
            subscriptionName = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
            {
                Query = @"From Orders (Revisions = true)"
            });
            #endregion

            #region use_simple_revision_subscription_generic
            SubscriptionWorker<Revision<Order>> revisionWorker = store.Subscriptions.GetSubscriptionWorker<Revision<Order>>(subscriptionName);

            await revisionWorker.Run((SubscriptionBatch<Revision<Order>> x) =>
            {
                foreach (var documentsPair in x.Items)
                {
                    var prev = documentsPair.Result.Previous;
                    var current = documentsPair.Result.Current;

                    ProcessOrderChanges(prev, current);
                }
            }
            );
            #endregion

            void ProcessOrderChanges(Order prev, Order cur)
            {

            }
            
            #region create_projected_revisions_subscription_generic
            subscriptionName = store.Subscriptions.Create(
                new SubscriptionCreationOptions<Revision<Order>>()
                {
                    Filter = tuple => tuple.Current.Lines.Count > tuple.Previous.Lines.Count,
                    Projection = tuple => new
                    {
                        PreviousRevenue = tuple.Previous.Lines.Sum(x => x.PricePerUnit * x.Quantity),
                        CurrentRevenue = tuple.Current.Lines.Sum(x => x.PricePerUnit * x.Quantity)
                    }
                });
            #endregion

            #region create_projected_revisions_subscription_RQL
            subscriptionName = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
            {
                Query = @"declare function getOrderLinesSum(doc){
                                var sum = 0;
                                for (var i in doc.Lines) { sum += doc.Lines[i];}
                                return sum;
                            }

                        From Orders (Revisions = true)
                        Where getOrderLinesSum(this.Current)  > getOrderLinesSum(this.Previous)
                        Select 
                        {
                            PreviousRevenue: getOrderLinesSum(this.Previous),
                            CurrentRevenue: getOrderLinesSum(this.Current)                            
                        }"
            });
            #endregion

            #region consume_revisions_subscription_generic
            SubscriptionWorker<OrderRevenues> revenuesComparisonWorker = store.Subscriptions.GetSubscriptionWorker<OrderRevenues>(subscriptionName);

            await revenuesComparisonWorker.Run(x =>
            {
                foreach (var item in x.Items)
                {
                    Console.WriteLine($"Revenue for order with Id: {item.Id} grown from {item.Result.PreviousRevenue} to {item.Result.CurrentRevenue}");
                }
            });
            #endregion

            SubscriptionWorker<Order> subscription;
            var cancellationToken = new CancellationTokenSource().Token;
            
            #region consumption_0
            subscriptionName = await store.Subscriptions.CreateAsync<Order>(x => x.Company == "companies/11");

            subscription = store.Subscriptions.GetSubscriptionWorker<Order>(subscriptionName);
            var subscriptionTask = subscription.Run(x =>
                x.Items.ForEach(item =>
                    Console.WriteLine($"Order #{item.Result.Id} will be shipped via: {item.Result.ShipVia}")),
                    cancellationToken);

            await subscriptionTask;
            #endregion
            
            #region open_1
            subscription = store.Subscriptions.GetSubscriptionWorker<Order>(subscriptionName);
            #endregion

            #region open_2
            subscription = store.Subscriptions.GetSubscriptionWorker<Order>(new SubscriptionWorkerOptions(subscriptionName)
            {
                Strategy = SubscriptionOpeningStrategy.WaitForFree
            });
            #endregion

            #region open_3
            subscription = store.Subscriptions.GetSubscriptionWorker<Order>(new SubscriptionWorkerOptions(subscriptionName)
            {
                Strategy = SubscriptionOpeningStrategy.WaitForFree,
                MaxDocsPerBatch = 500,
                IgnoreSubscriberErrors = true
            });
            #endregion

            #region create_subscription_with_includes_strongly_typed
            store.Subscriptions.Create<Order>(new SubscriptionCreationOptions<Order>()
            {
                Includes = builder => builder
                     // The documents whose IDs are specified in the 'Product' property
                     // will be included in the batch
                    .IncludeDocuments(x => x.Lines.Select(y => y.Product))
            });
            #endregion
            
            #region create_subscription_with_includes_rql_path
            store.Subscriptions.Create(new SubscriptionCreationOptions()
            {
                Query = @"from Orders include Lines[].Product"
            });
            #endregion

            #region create_subscription_with_includes_rql_javascript
            store.Subscriptions.Create(new SubscriptionCreationOptions()
            {
                Query = @"declare function includeProducts(doc) {
                              let includedFields = 0;
                              let linesCount = doc.Lines.length;

                              for (let i = 0; i < linesCount; i++) {
                                  includedFields++;
                                  include(doc.Lines[i].Product);
                              }

                              return doc;
                          }

                          from Orders as o select includeProducts(o)"
            });
            #endregion

            /*
            #region include_builder_counter_methods
            // Include a single counter
            ISubscriptionIncludeBuilder<T> IncludeCounter(string name);

            // Include multiple counters
            ISubscriptionIncludeBuilder<T> IncludeCounters(string[] names);

            // Include ALL counters from ALL subscribed documents.
            ISubscriptionIncludeBuilder<T> IncludeAllCounters();
            #endregion
            */

            #region create_subscription_include_counters_builder
            store.Subscriptions.Create<Order>(new SubscriptionCreationOptions<Order>()
            {
                Includes = builder => builder
                    .IncludeCounter("Likes")
                    .IncludeCounters(new[] { "Pros", "Cons" })
                    .IncludeAllCounters()
            });
            #endregion

            #region update_subscription_example_0
            store.Subscriptions.Update(new SubscriptionUpdateOptions()
            {
                Name = "my subscription",
                Query = "From Orders" // Provide the new query string
            });
            #endregion

            #region update_subscription_example_1
            // Get the subscription's ID
            SubscriptionState mySubscription = store.Subscriptions.GetSubscriptionState("my subscription");
            long subscriptionId = mySubscription.SubscriptionId;

            // Update the subscription's name
            store.Subscriptions.Update(new SubscriptionUpdateOptions()
            {
                Id = subscriptionId,
                Name = "new name"
            });
            #endregion
        }

        public interface IMaintainanceOperations
        {
            #region interface_subscription_deletion
            void Delete(string name, string database = null);
            Task DeleteAsync(string name, string database = null, CancellationToken token = default);
            #endregion

            #region interface_subscription_dropping
            void DropConnection(string name, string database = null);
            Task DropConnectionAsync(string name, string database = null, CancellationToken token = default);
            #endregion

            #region interface_subscription_enabling
            void Enable(string name, string database = null);
            Task EnableAsync(string name, string database = null, CancellationToken token = default);
            #endregion

            #region interface_subscription_disabling
            void Disable(string name, string database = null);
            Task DisableAsync(string name, string database = null, CancellationToken token = default);
            #endregion

            #region interface_subscription_state
            SubscriptionState GetSubscriptionState(string subscriptionName, string database = null);
            Task<SubscriptionState> GetSubscriptionStateAsync(string subscriptionName, string database = null, CancellationToken token = default);
            #endregion
        }
        
        public async Task SubscriptionMaintainance()
        {
            string subscriptionName = string.Empty;
            using (var store = new DocumentStore())
            {
                #region subscription_enabling
                store.Subscriptions.Enable(subscriptionName);
                #endregion

                #region subscription_disabling
                store.Subscriptions.Disable(subscriptionName);
                #endregion

                #region subscription_deletion
                store.Subscriptions.Delete(subscriptionName);
                #endregion

                #region connection_dropping
                store.Subscriptions.DropConnection(subscriptionName);
                #endregion

                #region subscription_state
                var subscriptionState = store.Subscriptions.GetSubscriptionState(subscriptionName);
                #endregion
            }
        }

        public class UnsupportedCompanyException : Exception
        {
            public UnsupportedCompanyException(string message, Exception ex = null)
            {

            }
        }

        public class Logger
        {
            public static void Error(string text) { }

            public static void Error(string text, Exception ex) { }
        }

        public class OrderRevenues
        {
            public int PreviousRevenue { get; set; }
            public int CurrentRevenue { get; set; }
        }

        public class OrderAndCompany
        {
            public string OrderId;
            public Company Company;
        }

        [Fact]
        public async Task OpeningExamples()
        {
            string name;
            IDocumentStore store = new DocumentStore();
            SubscriptionWorker<Order> subscriptionWorker;
            Task subscriptionRuntimeTask;
            string subscriptionName = null;
            CancellationToken cancellationToken = new CancellationToken();

            #region subscription_open_simple
            subscriptionWorker = store.Subscriptions.GetSubscriptionWorker<Order>(subscriptionName);
            #endregion

            #region subscription_run_simple
            subscriptionRuntimeTask = subscriptionWorker.Run(batch =>
            {
                // your logic here
            });
            #endregion

            #region subscription_worker_with_batch_size
            var workerWBatch = store.Subscriptions.GetSubscriptionWorker<Order>(
                new SubscriptionWorkerOptions(subscriptionName)
                {
                    MaxDocsPerBatch = 20
                });
            _ = workerWBatch.Run(x => { /* custom logic */ });
            #endregion

            #region throw_during_user_logic
            _ = workerWBatch.Run(x => throw new Exception());
            #endregion

            #region reconnecting_client
            while (true)
            {
                var options = new SubscriptionWorkerOptions(subscriptionName);

                // here we configure that we allow a down time of up to 2 hours, and will wait for 2 minutes for reconnecting
                options.MaxErroneousPeriod = TimeSpan.FromHours(2);
                options.TimeToWaitBeforeConnectionRetry = TimeSpan.FromMinutes(2);

                subscriptionWorker = store.Subscriptions.GetSubscriptionWorker<Order>(options);

                try
                {
                    // here we are able to be informed of any exception that happens during processing                    
                    subscriptionWorker.OnSubscriptionConnectionRetry += exception =>
                    {
                        Logger.Error("Error during subscription processing: " + subscriptionName, exception);
                    };

                    await subscriptionWorker.Run(async batch =>
                    {
                        foreach (var item in batch.Items)
                        {
                            // we want to force close the subscription processing in that case
                            // and let the external code decide what to do with that
                            if (item.Result.Company == "companies/832-A")
                                throw new UnsupportedCompanyException("Company Id can't be 'companies/832-A', you must fix this");
                            await ProcessOrder(item.Result);
                        }
                    }, cancellationToken);

                    // Run will complete normally if you have disposed the subscription
                    return;
                }
                catch (Exception e)
                {
                    Logger.Error("Failure in subscription: " + subscriptionName, e);

                    if (e is DatabaseDoesNotExistException ||
                        e is SubscriptionDoesNotExistException ||
                        e is SubscriptionInvalidStateException ||
                        e is AuthorizationException)
                        throw; // not recoverable


                    if (e is SubscriptionClosedException)
                        // closed explicitly by admin, probably
                        return;

                    if (e is SubscriberErrorException se)
                    {
                        // for UnsupportedCompanyException type, we want to throw an exception, otherwise
                        // we continue processing
                        if (se.InnerException != null && se.InnerException is UnsupportedCompanyException)
                        {
                            throw;
                        }

                        continue;
                    }

                    // handle this depending on subscription
                    // open strategy (discussed later)
                    if (e is SubscriptionInUseException)
                        continue;

                    return;
                }
                finally
                {
                    subscriptionWorker.Dispose();
                }
            }
            #endregion

            while (true)
            {
                #region worker_timeout_minimal_sample
                var options = new SubscriptionWorkerOptions(subscriptionName);

                // Set the worker's timeout period
                options.ConnectionStreamTimeout = TimeSpan.FromSeconds(45);
                #endregion
            }
            
            while (true)
            {
                #region worker_timeout
                var options = new SubscriptionWorkerOptions(subscriptionName);

                // Set the worker's timeout period
                options.ConnectionStreamTimeout = TimeSpan.FromSeconds(45);

                subscriptionWorker = store.Subscriptions.GetSubscriptionWorker<Order>(options);

                try
                {
                    subscriptionWorker.OnSubscriptionConnectionRetry += exception =>
                    {
                        Logger.Error("Error during subscription processing: " + subscriptionName, exception);
                    };

                    await subscriptionWorker.Run(async batch =>
                    {
                        foreach (var item in batch.Items)
                        {
                            //...
                        }
                    }, cancellationToken);

                    // Run will complete normally if you have disposed the subscription
                    return;
                }
                catch (Exception e)
                {
                    Logger.Error("Error during subscription process: " + subscriptionName, e);
                }
                finally
                {
                    subscriptionWorker.Dispose();
                }
                #endregion
            }

            async Task ProcessOrder(Order o)
            {

            }
        }

        private static async Task SingleRun(DocumentStore store)
        {
            var subsId = store.Subscriptions.Create<Order>(
                            new SubscriptionCreationOptions<Order>
                            {
                                Filter = order => order.Lines.Sum(line => line.PricePerUnit * line.Quantity) > 10000,
                                Projection = order => new OrderAndCompany
                                {
                                    OrderId = order.Id,
                                    Company = RavenQuery.Load<Company>(order.Company)
                                }
                            });

            #region single_run
            var highValueOrdersWorker = store.Subscriptions.GetSubscriptionWorker<OrderAndCompany>(
                new SubscriptionWorkerOptions(subsId)
                {
                    // Here we ask the worker to stop when there are no documents left to send. 
                    // Will throw SubscriptionClosedException when it finishes it's job
                    CloseWhenNoDocsLeft = true
                });

            try
            {
                await highValueOrdersWorker.Run(async batch =>
                {
                    foreach (var item in batch.Items)
                    {
                        await SendThankYouNoteToEmployee(item.Result);
                    }
                });
            }
            catch (SubscriptionClosedException)
            {
                // that's expected
            }
            #endregion

            async Task SendThankYouNoteToEmployee(OrderAndCompany oac)
            {

            }
        }

        public async Task DynamicWorkerSubscription(DocumentStore store)
        {
            #region dynamic_worker
            var subscriptionName = "My dynamic subscription";
            
            await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions<Order>()
            {
                Name = "My dynamic subscription",
                Projection = order => new { DynanamicField_1 = "Company: " + order.Company + " Employee: " + order.Employee }
            });

            var subscriptionWorker = store.Subscriptions.GetSubscriptionWorker(subscriptionName);
            _ = subscriptionWorker.Run(async batch =>
            {
                foreach (var item in batch.Items)
                {
                    await RaiseNotification(item.Result.DynanamicField_1);
                }
            });
            #endregion

            async Task RaiseNotification(string message)
            {

            }
        }

        public async Task SubscriptionWithIncludesPath(DocumentStore store)
        {
            #region subscription_with_includes_path_usage
            var subscriptionName = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
            {
                Query = @"from Orders include Lines[].Product"
            });

            var subscriptionWorker = store.Subscriptions.GetSubscriptionWorker<Order>(subscriptionName);
            _ = subscriptionWorker.Run(async batch =>
            {
                using (var session = batch.OpenAsyncSession())
                {
                    foreach (var order in batch.Items.Select(x => x.Result))
                    {
                        foreach (var orderLine in order.Lines)
                        {
                            // this line won't generate a request, because orderLine.Product was included
                            var product = await session.LoadAsync<Product>(orderLine.Product);
                            await RaiseNotification(order, product);
                        }

                    }
                }
            });
            #endregion

            async Task RaiseNotification(Order modifiedOrder, Product productInOrder)
            {

            }
        }

        public async Task SubscriptionsWithOpenSession(DocumentStore store)
        {
            #region subscription_with_open_session_usage
            var subscriptionName = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
            {
                Query = @"from Orders as o where o.ShippedAt = null"
            });

            var subscriptionWorker = store.Subscriptions.GetSubscriptionWorker<Order>(subscriptionName);
            _ = subscriptionWorker.Run(async batch =>
            {
                using (var session = batch.OpenAsyncSession())
                {
                    foreach (var order in batch.Items.Select(x => x.Result))
                    {
                        await TransferOrderToShipmentCompanyAsync(order);
                        order.ShippedAt = DateTime.UtcNow;

                    }

                    // we know that we have at least one order to ship,
                    // because the subscription query above has that in it's WHERE clause
                    await session.SaveChangesAsync();
                }
            });
            #endregion

            async Task TransferOrderToShipmentCompanyAsync(Order modifiedOrder)
            {
            }
        }

        public async Task BlittableWorkerSubscription(DocumentStore store, string subscriptionId)
        {
            #region blittable_worker
            await store.Subscriptions.CreateAsync(
                new SubscriptionCreationOptions<Order>
                {
                    Projection = x => new
                    {
                        x.Employee
                    }
                });

            var subscriptionWorker = store.Subscriptions.GetSubscriptionWorker<BlittableJsonReaderObject>(subscriptionId);
            _ = subscriptionWorker.Run(async batch =>
            {
                foreach (var item in batch.Items)
                {
                    await RaiseNotification(item.Result["Employee"].ToString());
                }
            });
            #endregion

            async Task RaiseNotification(string message)
            {

            }
        }

        public async Task WaitForFreeSubscription(DocumentStore store, string subscriptionName)
        {
            #region waitforfree
            var worker = store.Subscriptions.GetSubscriptionWorker<Order>
                    (new SubscriptionWorkerOptions(subscriptionName)
            {
                Strategy = SubscriptionOpeningStrategy.WaitForFree
            });
            #endregion
        }

        public async Task TwoSubscriptions(DocumentStore store, string subscriptionName)
        {
            #region waiting_subscription_1
            var primaryWorker = store.Subscriptions.GetSubscriptionWorker<Order>(new SubscriptionWorkerOptions(subscriptionName)
            {
                Strategy = SubscriptionOpeningStrategy.TakeOver
            });

            while (true)
            {
                try
                {
                    await primaryWorker.Run(x =>
                    {
                        // your logic
                    });
                }
                catch (Exception)
                {
                    // retry
                }
            }
            #endregion

            #region waiting_subscription_2
            var secondaryWorker = store.Subscriptions.GetSubscriptionWorker<Order>(new SubscriptionWorkerOptions(subscriptionName)
            {
                Strategy = SubscriptionOpeningStrategy.WaitForFree
            });

            while (true)
            {
                try
                {
                    await secondaryWorker.Run(x =>
                    {
                        // your logic
                    });
                }
                catch (Exception)
                {
                    // retry
                }
            }
            #endregion
        }
        
        public DataSubscriptions()
        {
            IDocumentStore store = new DocumentStore();

            //{

            //	#region open_2
            //	var orders = store.Subscriptions.Open<Order>(id, new SubscriptionConnectionOptions()
            //	{
            //		BatchOptions = new SubscriptionBatchOptions()
            //		{
            //			MaxDocCount = 16*1024,
            //			MaxSize = 4*1024*1024,
            //			AcknowledgmentTimeout = TimeSpan.FromMinutes(3)
            //		},
            //		IgnoreSubscribersErrors = false,
            //		ClientAliveNotificationInterval = TimeSpan.FromSeconds(30)
            //	});
            //	#endregion

            //	#region open_3
            //	orders.Subscribe(x =>
            //	{
            //		GenerateInvoice(x);
            //	});

            //	orders.Subscribe(x =>
            //	{
            //		if(x.RequireAt > DateTime.Now)
            //			SendReminder(x.Employee, x.Id);
            //	});
            //	#endregion

            //	#region open_4
            //	var subscriber = orders.Subscribe(x => { });

            //	subscriber.Dispose();
            //	#endregion

            //	#region delete_2
            //	store.Subscriptions.Delete(id);
            //	#endregion

            //	#region get_subscriptions_2
            //	var configs = store.Subscriptions.GetSubscriptions(0, 10);
            //	#endregion

            //	#region release_2
            //	store.Subscriptions.Release(id);
            //	#endregion
            //}
        }

        private void SendReminder(string employee, string id)
        {
        }

        public void GenerateInvoice(Order o)
        {

        }

        //private interface IFoo
        //{

        //	#region open_1
        //	Subscription<RavenJObject> Open(long id, SubscriptionConnectionOptions options, string database = null);

        //	Subscription<T> Open<T>(long id, SubscriptionConnectionOptions options, string database = null) 
        //	#endregion
        //		 where T : class;

        //	#region delete_1
        //	void Delete(long id, string database = null);
        //	#endregion

        //	#region get_subscriptions_1
        //	List<SubscriptionConfig> GetSubscriptions(int start, int take, string database = null);
        //	#endregion

        //	#region release_1
        //	void Release(long id, string database = null);
        //	#endregion
        //}

        //#region events
        //public delegate void BeforeBatch();

        //public delegate bool BeforeAcknowledgment();

        //public delegate void AfterAcknowledgment(Etag lastProcessedEtag);

        //public delegate void AfterBatch(int documentsProcessed);
        //#endregion
    }
}
