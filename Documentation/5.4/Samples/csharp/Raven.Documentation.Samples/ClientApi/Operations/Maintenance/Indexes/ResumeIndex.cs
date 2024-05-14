using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
    public class ResumeIndex
    {
        private interface IFoo
        {
            /*
            #region syntax
            // class name has "Start", but this is ok, this is the "Resume" operation
            public StartIndexOperation(string indexName)
            #endregion
            */
        }

        public ResumeIndex()
        {
            using (var store = new DocumentStore())
            {
                #region resume_index
                // Define the resume index operation, pass the index name
                var resumeIndexOp = new StartIndexOperation("Orders/Totals");
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(resumeIndexOp);
                
                // At this point:
                // Index 'Orders/Totals' is resumed on the preferred node
                
                // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = store.Maintenance.Send(new GetIndexingStatusOperation());

                var index = indexingStatus.Indexes.FirstOrDefault(x => x.Name == "Orders/Totals");
                Assert.Equal(IndexRunningStatus.Running, index.Status);
                #endregion
            }
        }
        
        public async Task ResumeIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region resume_index_async
                // Define the resume index operation, pass the index name
                var resumeIndexOp = new StartIndexOperation("Orders/Totals");
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(resumeIndexOp);
                
                // At this point:
                // Index 'Orders/Totals' is resumed on the preferred node
                
                // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = await store.Maintenance.SendAsync(new GetIndexingStatusOperation());

                var index = indexingStatus.Indexes.FirstOrDefault(x => x.Name == "Orders/Totals");
                Assert.Equal(IndexRunningStatus.Running, index.Status);
                #endregion
            }
        }
    }
}
