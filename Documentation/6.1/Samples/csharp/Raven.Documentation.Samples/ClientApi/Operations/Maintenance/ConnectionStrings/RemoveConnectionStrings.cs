using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class RemoveConnectionStrings
    {
        public RemoveConnectionStrings()
        {
            using (var store = new DocumentStore())
            {
                #region remove_raven_connection_string
                var ravenConnectionString = new RavenConnectionString()
                {
                    // Note:
                    // Only the 'Name' property of the connection string is needed for the remove operation.
                    // Other properties are not considered.
                    Name = "ravendb-connection-string-name"
                };
               
                // Define the remove connection string operation,
                // pass the connection string to be removed.
                var removeConStrOp
                    = new RemoveConnectionStringOperation<RavenConnectionString>(ravenConnectionString);

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(removeConStrOp);
                #endregion
            }
        }
        
        private interface IFoo
        {
            /*
            #region remove_connection_string
            public RemoveConnectionStringOperation(T connectionString)
            #endregion
            */
        }
    }
}
