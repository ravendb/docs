using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Operations.TimeSeries;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class GetSamples
    {
        public async Task Single()
        {
            #region get_1_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Define the 'GetDocumentsCommand'
                var command = new GetDocumentsCommand(store.Conventions,
                    "orders/1-A", null, metadataOnly: false);
                
                // Call 'Execute' on the Store's Request Executor to send the command to the server
                store.GetRequestExecutor().Execute(command, context);
                
                // Access the results
                var blittable = (BlittableJsonReaderObject)command.Result.Results[0];
                
                // Deserialize the blittable JSON into a strongly-typed 'Order' object
                var order = (Order)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Order), blittable);
                
                var orderedAt = order.OrderedAt;
            }
            #endregion
            
            #region get_1_storeContext_async
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Define the 'GetDocumentsCommand'
                var command = new GetDocumentsCommand(store.Conventions,
                    "orders/1-A", null, metadataOnly: false);
                
                // Call 'ExecuteAsync' on the Store's Request Executor to send the command to the server
                await store.GetRequestExecutor().ExecuteAsync(command, context);
                
                // Access the results
                var blittable = (BlittableJsonReaderObject)command.Result.Results[0];
                
                // Deserialize the blittable JSON into a strongly-typed 'Order' object
                var order = (Order)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Order), blittable);
                
                var orderedAt = order.OrderedAt;
            }
            #endregion
            
            #region get_1_sessionContext
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                // Define the 'GetDocumentsCommand'
                var command = new GetDocumentsCommand(store.Conventions,
                    "orders/1-A", null, metadataOnly: false);
                
                // Call 'Execute' on the Session's Request Executor to send the command to the server
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                
                // Access the results
                var blittable  = (BlittableJsonReaderObject)command.Result.Results[0];
                
                // Deserialize the blittable JSON into a strongly-typed 'Order' object
                // Setting the last param to 'true' will cause the session to track the 'Order' entity
                var order = session.Advanced.JsonConverter.FromBlittable<Order>(ref blittable,
                    "orders/1-A", trackEntity: true);

                var orderedAt = order.OrderedAt;
            }
            #endregion
            
            #region get_1_sessionContext_async
            using (var store = new DocumentStore())
            using (var asyncSession = store.OpenAsyncSession())
            {
                // Define the 'GetDocumentsCommand'
                var command = new GetDocumentsCommand(store.Conventions,
                    "orders/1-A", null, metadataOnly: false);
                
                // Call 'ExecuteAsync' on the Session's Request Executor to send the command to the server
                await asyncSession.Advanced.RequestExecutor.ExecuteAsync(
                    command, asyncSession.Advanced.Context);
                
                // Access the results
                var blittable  = (BlittableJsonReaderObject)command.Result.Results[0];
                
                // Deserialize the blittable JSON into a strongly-typed 'Order' object
                // Setting the last param to 'true' will cause the session to track the 'Order' entity
                var order = asyncSession.Advanced.JsonConverter.FromBlittable<Order>(ref blittable,
                    "orders/1-A", trackEntity: true);

                var orderedAt = order.OrderedAt;
            }
            #endregion
        }

        public void Multiple()
        {
            #region get_2_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Pass a list of document IDs to the get command
                var command = new GetDocumentsCommand(store.Conventions,
                    new[] { "orders/1-A", "employees/2-A", "products/1-A" }, null, false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access results
                var orderBlittable = (BlittableJsonReaderObject)command.Result.Results[0];
                var orderDocument = (Order)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Order), orderBlittable);
                
                var employeeBlittable = (BlittableJsonReaderObject)command.Result.Results[1];
                var employeeDocument = (Employee)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Employee), orderBlittable);
                
                var productBlittable = (BlittableJsonReaderObject)command.Result.Results[2];
                var productDocument = (Product)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Product), productBlittable);
            }
            #endregion
            
            #region get_3_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Assuming that employees/9999-A doesn't exist
                var command = new GetDocumentsCommand(store.Conventions,
                    new[] { "orders/1-A", "employees/9999-A", "products/3-A" }, null, false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Results will contain 'null' for any missing document
                var results = command.Result.Results;  // orders/1-A, null, products/3-A
                Assert.Null(results[1]);
            }
            #endregion
        }

        public void MetadataOnly()
        {
            #region get_4_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Pass 'true' in the 'metadataOnly' param to retrieve only the document METADATA
                var command = new GetDocumentsCommand(store.Conventions,
                    "orders/1-A", null, metadataOnly: true);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access results
                var blittable = (BlittableJsonReaderObject)command.Result.Results[0];
                var documentMetadata = (BlittableJsonReaderObject)blittable["@metadata"];

                // Print out all metadata properties
                foreach (var propertyName in documentMetadata.GetPropertyNames())
                {
                    documentMetadata.TryGet<object>(propertyName, out var propertyValue);
                    Console.WriteLine("{0} = {1}", propertyName, propertyValue);
                }
            }
            #endregion
        }

        public void Paged()
        {
            #region get_5_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Specify the number of documents to skip (start)
                // and the number of documents to get (pageSize)
                var command = new GetDocumentsCommand(start: 0, pageSize: 128);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // The documents are sorted by the last modified date,
                // with the most recent modifications appearing first.
                var firstDocs = command.Result.Results;
            }
            #endregion
        }

        public void StartsWith()
        {
            #region get_6_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Return up to 50 documents with ID that starts with 'products/'
                var command = new GetDocumentsCommand(store.Conventions,
                    startWith: "products/",
                    startAfter: null,
                    matches: null,
                    exclude: null,
                    start: 0,
                    pageSize: 50,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access a Product document
                var blittable = (BlittableJsonReaderObject)command.Result.Results[0];
                var product = (Product)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Product), blittable);
            }
            #endregion
        }

        public void StartsWithMatches()
        {
            #region get_7_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Return up to 50 documents with IDs that start with 'orders/'
                // and the rest of the ID either begins with '23',
                // or contains any character at the 1st position and ends with '10-A'
                // e.g. orders/234-A, orders/810-A
                var command = new GetDocumentsCommand(store.Conventions,
                    startWith: "orders/",
                    startAfter: null,
                    matches: "23*|?10-A",
                    exclude: null,
                    start: 0,
                    pageSize: 50,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access an Order document
                var blittable = (BlittableJsonReaderObject)command.Result.Results[0];
                var order = (Order)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Order), blittable);

                Assert.True(order.Id.StartsWith("orders/23") || 
                            Regex.IsMatch(order.Id, @"^orders/.{1}10-A$"));
            }
            #endregion
        }

        public void StartsWithExclude()
        {
            #region get_8_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Return up to 50 documents with IDs that start with 'orders/'
                // and the rest of the ID excludes documents ending with '10-A',
                // e.g. will return orders/820-A, but not orders/810-A
                var command = new GetDocumentsCommand(store.Conventions,
                    startWith: "orders/",
                    startAfter: null,
                    matches: null,
                    exclude: "*10-A",
                    start: 0,
                    pageSize: 50,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access an Order document
                var blittable = (BlittableJsonReaderObject)command.Result.Results[0];
                var order = (Order)store.Conventions.Serialization.DefaultConverter
                    .FromBlittable(typeof(Order), blittable);

                Assert.True(order.Id.StartsWith("orders/") && !order.Id.EndsWith("10-A"));
            }
            #endregion
        }

        public void Includes()
        {
            #region get_9_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Fetch document products/77-A and include its related Supplier document
                var command = new GetDocumentsCommand(store.Conventions,
                    id:"products/77-A",
                    includes: new[] { "Supplier" },
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                var productBlittable = (BlittableJsonReaderObject)command.Result.Results[0];
                if (productBlittable.TryGet<string>("Supplier", out var supplierId))
                {
                    // Access the related document that was included
                    var supplierBlittable = 
                        (BlittableJsonReaderObject)command.Result.Includes[supplierId];
                    
                    var supplier = (Supplier)store.Conventions.Serialization.DefaultConverter
                        .FromBlittable(typeof(Supplier), supplierBlittable);
                }
            }
            #endregion
            
            #region get_10_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Fetch document products/77-A and include the specified counters
                var command = new GetDocumentsCommand(store.Conventions,
                    ids:new[] {"products/77-A"},
                    includes: null, 
                    // Pass the names of the counters to include. In this example,
                    // the counter names in RavenDB's sample data are stars... 
                    counterIncludes: new[] { "⭐", "⭐⭐" },
                    timeSeriesIncludes: null,
                    compareExchangeValueIncludes: null,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
               
                // Access the included counters results
                var counters = (BlittableJsonReaderObject)command.Result.CounterIncludes;
                var countersBlittableArray = 
                    (BlittableJsonReaderArray)counters["products/77-A"];
                
                var counter = (BlittableJsonReaderObject)countersBlittableArray[0];
                var counterName = counter["CounterName"];
                var counterValue = counter["TotalValue"];
            }
            #endregion
            
            #region get_11_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Fetch document employees/1-A and include the specified time series
                var command = new GetDocumentsCommand(store.Conventions,
                    ids:new[] {"employees/1-A"},
                    includes: null, 
                    counterIncludes: null,
                    // Specify the time series name and the time range
                    timeSeriesIncludes: new[] { new TimeSeriesRange
                    {
                        Name = "HeartRates",
                        From = DateTime.MinValue,
                        To = DateTime.MaxValue
                    } },
                    compareExchangeValueIncludes:null,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
               
                // Access the included time series results
                var timeSeriesBlittable = 
                    (BlittableJsonReaderObject)command.Result.TimeSeriesIncludes["employees/1-A"];
                
                var timeSeriesBlittableArray = 
                    (BlittableJsonReaderArray)timeSeriesBlittable["HeartRates"];
                
                var ts = (BlittableJsonReaderObject)timeSeriesBlittableArray[0];
                var entries = (BlittableJsonReaderArray)ts["Entries"];

                var tsEntry = (BlittableJsonReaderObject)entries[0];
                var entryTimeStamp = tsEntry["Timestamp"];
                var entryValues = tsEntry["Values"];
            }
            #endregion
            
            #region get_12_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Fetch document orders/826-A and include the specified revisions
                var command = new GetDocumentsCommand(store.Conventions,
                    ids:new[] {"orders/826-A"},
                    includes: null, 
                    counterIncludes: null,
                    // Specify list of document fields (part of document orders/826-A),
                    // where each field is expected to contain the change-vector
                    // of the revision you wish to include.
                    revisionsIncludesByChangeVector: new[]
                    {
                        "RevisionChangeVectorField1",
                        "RevisionChangeVectorField2"
                    },
                    revisionIncludeByDateTimeBefore: null,
                    timeSeriesIncludes: null,
                    compareExchangeValueIncludes: null,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access the included revisions 
                var revisions = (BlittableJsonReaderArray)command.Result.RevisionIncludes;
                
                var revisionObj = (BlittableJsonReaderObject)revisions[0];
                var revision = (BlittableJsonReaderObject)revisionObj["Revision"];
            }
            #endregion
            
            #region get_13_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Fetch document orders/826-A and include the specified revisions
                var command = new GetDocumentsCommand(store.Conventions,
                    ids:new[] {"orders/826-A"},
                    includes: null, 
                    counterIncludes: null,
                    // Another option is to specify a single document field (part of document orders/826-A).
                    // This field is expected to contain a list of all the change-vectors
                    // for the revisions you wish to include.
                    revisionsIncludesByChangeVector: new[]
                    {
                        "RevisionsChangeVectors"
                    },
                    revisionIncludeByDateTimeBefore: null,
                    timeSeriesIncludes: null,
                    compareExchangeValueIncludes: null,
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access the included revisions 
                var revisions = (BlittableJsonReaderArray)command.Result.RevisionIncludes;
                
                var revisionObj = (BlittableJsonReaderObject)revisions[0];
                var revision = (BlittableJsonReaderObject)revisionObj["Revision"];
            }
            #endregion
            
            #region get_14_storeContext
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Fetch document orders/826-A and include the specified compare-exchange
                var command = new GetDocumentsCommand(store.Conventions,
                    ids:new[] {"orders/826-A"},
                    includes: null, 
                    counterIncludes: null,
                    revisionsIncludesByChangeVector: null,
                    revisionIncludeByDateTimeBefore: null,
                    timeSeriesIncludes: null,
                    // Similar to the previous "include revisions" examples,
                    // EITHER:
                    // Specify a list of document fields (part of document orders/826-A),
                    // where each field is expected to contain a compare-exchange KEY
                    // for the compare-exchange item you wish to include
                    // OR:
                    // Specify a single document field that contains a list of all keys to include.
                    compareExchangeValueIncludes: [
                        "CmpXchgItemField1",
                        "CmpXchgItemField2"
                    ],
                    metadataOnly: false);
                
                store.GetRequestExecutor().Execute(command, context);
                
                // Access the included compare-exchange items
                var cmpXchgItems = 
                    (BlittableJsonReaderObject)command.Result.CompareExchangeValueIncludes;
                
                var cmpXchgItemKey = cmpXchgItems.GetPropertyNames()[0]; // The cmpXchg KEY NAME
                var cmpXchgItemObj = (BlittableJsonReaderObject)cmpXchgItems[cmpXchgItemKey];
                
                var cmpXchgItemValueObj = (BlittableJsonReaderObject)cmpXchgItemObj["Value"];
                var cmpXchgItemValue = cmpXchgItemValueObj["Object"]; // The cmpXchg KEY VALUE
            }
            #endregion
        }
    }
    
    public interface GetInterfaces
    {
        /*
        #region syntax_1
        // Available overloads:
        // ====================
        
        public GetDocumentsCommand(int start, int pageSize)
        
        public GetDocumentsCommand(DocumentConventions conventions, 
            string id,
            string[] includes,
            bool metadataOnly);
            
        public GetDocumentsCommand(DocumentConventions conventions,
            string[] ids,
            string[] includes,
            bool metadataOnly);
            
        public GetDocumentsCommand(DocumentConventions conventions, 
            string[] ids,
            string[] includes, 
            string[] counterIncludes, 
            IEnumerable<AbstractTimeSeriesRange> timeSeriesIncludes, 
            string[] compareExchangeValueIncludes, 
            bool metadataOnly);
            
        public GetDocumentsCommand(DocumentConventions conventions, 
            string[] ids,
            string[] includes,
            string[] counterIncludes, 
            IEnumerable<string> revisionsIncludesByChangeVector, 
            DateTime? revisionIncludeByDateTimeBefore, 
            IEnumerable<AbstractTimeSeriesRange> timeSeriesIncludes, 
            string[] compareExchangeValueIncludes,
            bool metadataOnly);
            
        public GetDocumentsCommand(DocumentConventions conventions,
            string[] ids,
            string[] includes,
            bool includeAllCounters,
            IEnumerable<AbstractTimeSeriesRange> timeSeriesIncludes,
            string[] compareExchangeValueIncludes,
            bool metadataOnly);
            
        public GetDocumentsCommand(DocumentConventions conventions,
            string startWith,
            string startAfter,
            string matches, string exclude,
            int start, int pageSize,
            bool metadataOnly);
            
        public GetDocumentsCommand(int start, int pageSize);            
        #endregion
        */
        
        #region syntax_2
        // The `GetDocumentCommand` result:
        // ================================
        
        public class PutResult
        {
            public BlittableJsonReaderObject Includes { get; set; }
            public BlittableJsonReaderArray Results { get; set; }
            public BlittableJsonReaderObject CounterIncludes { get; set; }
            public BlittableJsonReaderArray RevisionIncludes { get; set; }
            public BlittableJsonReaderObject TimeSeriesIncludes { get; set; }
            public BlittableJsonReaderObject CompareExchangeValueIncludes { get; set; }
        }
        #endregion
    }
}
