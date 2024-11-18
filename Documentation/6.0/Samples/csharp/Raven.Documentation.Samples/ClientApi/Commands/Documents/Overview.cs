using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Subscriptions;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class Overview
    {
        public async Task Examples()
        {
            #region execute_1
            // Using the store object
            using (var store = new DocumentStore())
            // Allocate a context from the store's context pool for executing the command
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Define a command
                var cmd = new CreateSubscriptionCommand(store.Conventions,
                    new SubscriptionCreationOptions()
                {
                    Name = "Orders subscription",
                    Query = "from Orders" 
                });
                
                // Call 'Execute' on the store's Request Executor to run the command on the server,
                // pass the command and the store context.
                store.GetRequestExecutor().Execute(cmd, context);
            }
            #endregion

            #region execute_1_async
            // Using the store object
            using (var store = new DocumentStore())
            // Allocate a context from the store's context pool for executing the command
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Define a command
                var cmd = new CreateSubscriptionCommand(store.Conventions,
                    new SubscriptionCreationOptions()
                {
                    Name = "Orders subscription",
                    Query = "from Orders" 
                });
                
                // Call 'ExecuteAsync' on the store's Request Executor to run the command on the server,
                // pass the command and the store context.
                await store.GetRequestExecutor().ExecuteAsync(cmd, context);
            }
            #endregion
            
            using (var store = new DocumentStore())
            {
                #region execute_2
                // Using the session
                using (var session = store.OpenSession())
                {
                    // Define a command
                    var cmd = new GetDocumentsCommand(store.Conventions, "orders/1-A", null, false);

                    // Call 'Execute' on the session's Request Executor to run the command on the server
                    // Pass the command and the 'Session.Advanced.Context'
                    session.Advanced.RequestExecutor.Execute(cmd, session.Advanced.Context);
                    
                    // Access the results
                    var blittable = (BlittableJsonReaderObject)cmd.Result.Results[0];

                    // Deserialize the blittable JSON into your typed object
                    var order = session.Advanced.JsonConverter.FromBlittable<Order>(ref blittable,
                        "orders/1-A", false);

                    // Now you have a strongly-typed Order object that can be accessed
                    var orderedAt = order.OrderedAt;
                }
                #endregion

                #region execute_2_async
                // Using the session
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Define a command
                    var cmd = new GetDocumentsCommand(store.Conventions, "orders/1-A", null, false);
                    
                    // Call 'ExecuteAsync' on the session's Request Executor to run the command on the server
                    // Pass the command and the 'Session.Advanced.Context'
                    await asyncSession.Advanced.RequestExecutor.ExecuteAsync(cmd, 
                        asyncSession.Advanced.Context);
                    
                    // Access the results
                    var blittable = (BlittableJsonReaderObject)cmd.Result.Results[0];

                    // Deserialize the blittable JSON into your typed object
                    var order = asyncSession.Advanced.JsonConverter.FromBlittable<Order>(ref blittable,
                        "orders/1-A", true);

                    // Now you have a strongly-typed Order object that can be accessed
                    var orderedAt = order.OrderedAt;
                }
                #endregion
            }
        }
    }
}
