using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class ResetIndex
    {
        public ResetIndex()
        {
            using (var store = new DocumentStore())
            {
                #region reset
                // Define the reset index operation, pass index name
                var resetIndexOp = new ResetIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.Send
                // An exception will be thrown if index does not exist
                store.Maintenance.Send(resetIndexOp);
                #endregion
            }
        }

        public async Task ResetIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region reset_async
                // Define the reset index operation, pass index name
                var resetIndexOp = new ResetIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.SendAsync
                // An exception will be thrown if index does not exist
                await store.Maintenance.SendAsync(resetIndexOp);
                #endregion
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax
            public ResetIndexOperation(string indexName);
            #endregion
            */
        }
    }
}
