using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class GetIndexErrors
    {
        public GetIndexErrors()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_errors_all
                    // Define the get index errors operation
                    var getIndexErrorsOp = new GetIndexErrorsOperation();
                    
                    // Execute the operation by passing it to Maintenance.Send
                    IndexErrors[] indexErrors = store.Maintenance.Send(getIndexErrorsOp);
                    
                    // indexErrors will contain errors for ALL indexes
                    #endregion
                }

                {
                    #region get_errors_specific
                    // Define the get index errors operation for specific indexes
                    var getIndexErrorsOp = new GetIndexErrorsOperation(new[] { "Orders/Totals" });
                    
                    // Execute the operation by passing it to Maintenance.Send
                    // An exception will be thrown if any of the specified indexes do not exist
                    IndexErrors[] indexErrors = store.Maintenance.Send(getIndexErrorsOp);

                    // indexErrors will contain errors only for index "Orders/Totals"
                    #endregion
                }
            }
        }
        
        public async Task GetIndexErrorsAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_errors_all_async
                    // Define the get index errors operation
                    var getIndexErrorsOp = new GetIndexErrorsOperation();

                    // Execute the operation by passing it to Maintenance.SendAsync
                    IndexErrors[] indexErrors = await store.Maintenance.SendAsync(getIndexErrorsOp);

                    // indexErrors will contain errors for ALL indexes
                    #endregion
                }

                {
                    #region get_errors_specific_async
                    // Define the get index errors operation for specific indexes
                    var getIndexErrorsOp = new GetIndexErrorsOperation(new[] { "Orders/Totals" });
                    
                    // Execute the operation by passing it to Maintenance.SendAsync
                    // An exception will be thrown if any of the specified indexes do not exist
                    IndexErrors[] indexErrors = await store.Maintenance.SendAsync(getIndexErrorsOp);
                    
                    // indexErrors will contain errors only for index "Orders/Totals"
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            // Available overloads:
            public GetIndexErrorsOperation()                    // Get errors for all indexes
            public GetIndexErrorsOperation(string[] indexNames) // Get errors for specific indexes
            #endregion
            */
        }

        private class Foo
        {
            #region syntax_2
            public class IndexErrors
            {
                public string Name { get; set; }            // Index name
                public IndexingError[] Errors { get; set; } // List of errors for this index
            }
            #endregion

            #region syntax_3
            public class IndexingError
            {
                // The error message
                public string Error { get; set; }
                
                // Time of error
                public DateTime Timestamp { get; set; }
                
                // If Action is 'Map'    - field will contain the document ID
                // If Action is 'Reduce' - field will contain the Reduce key value
                // For all other Actions - field will be null 
                public string Document { get; set; }
                
                // Area where error has occurred, e.g. Map/Reduce/Analyzer/Memory/etc.
                public string Action { get; set; }      
            }
            #endregion
        }
    }
}
