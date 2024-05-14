using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class ResumeIndexing
    {
        private interface IFoo
        {
            /*
            #region syntax
            // class name has "Start", but this is ok, this is the "Resume" operation
            public StartIndexingOperation()
            #endregion
            */
        }

        public ResumeIndexing()
        {
            using (var store = new DocumentStore())
            {
                #region resume_indexing
                // Define the resume indexing operation
                var resumeIndexingOp = new StartIndexingOperation();
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(resumeIndexingOp);

                // At this point,
                // you can be sure that all indexes on the preferred node are 'running'
                        
                // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = store.Maintenance.Send(new GetIndexingStatusOperation());
                Assert.Equal(IndexRunningStatus.Running, indexingStatus.Status);
                #endregion
            }
        }
        
        public async Task ResumeIndexingAsync()
        {
            using (var store = new DocumentStore())
            {
                #region resume_indexing_async
                // Define the resume indexing operation
                var resumeIndexingOp = new StartIndexingOperation();
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(resumeIndexingOp);

                // At this point,
                // you can be sure that all indexes on the preferred node are 'running'
                        
                // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = await store.Maintenance.SendAsync(new GetIndexingStatusOperation());
                Assert.Equal(IndexRunningStatus.Running, indexingStatus.Status);
                #endregion
            }
        }
    }
}
