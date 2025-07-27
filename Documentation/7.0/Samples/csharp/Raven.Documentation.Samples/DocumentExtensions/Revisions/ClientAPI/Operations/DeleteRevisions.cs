using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Xunit;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class DeleteRevisions
    {
        public DeleteRevisions()
        {
            using (var store = new DocumentStore())
            {
                #region delete_revisions_1
                // Define the delete revisions operation:
                
                // Delete ALL existing revisions for document "orders/830-A"
                var deleteRevisionsOp = new DeleteRevisionsOperation(documentId: "orders/830-A",
                    // Revisions that were created manually will also be removed
                    removeForceCreatedRevisions: true);
                
                // Execute the operation by passing it to Maintenance.Send
                var numberOfRevisionsDeleted = store.Maintenance.Send(deleteRevisionsOp);
                
                // Running the above code on RavenDB's sample data results in the removal of 29 revisions
                Assert.Equal(29, numberOfRevisionsDeleted.TotalDeletes);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_revisions_2
                // Delete existing revisions for the specified documents
                var deleteRevisionsOp = new DeleteRevisionsOperation(
                    documentIds: new List<string>() { "orders/829-A", "orders/828-A", "orders/827-A" },
                    // Revisions that were created manually will Not be removed
                    removeForceCreatedRevisions: false);

                var numberOfRevisionsDeleted = store.Maintenance.Send(deleteRevisionsOp);
                
                // Running the above on RavenDB's sample data results in the removal of 19 revisions
                Assert.Equal(19, numberOfRevisionsDeleted.TotalDeletes);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_revisions_3
                var deleteFrom = DateTime.Parse("2018-07-27T09:11:52.0Z");
                var deleteTo = DateTime.Parse("2018-07-27T09:11:54.0Z");
                
                // Delete existing revisions within the specified time frame
                var deleteRevisionsOp =
                    new DeleteRevisionsOperation(documentId: "orders/826-A", from: deleteFrom, to: deleteTo);

                var numberOfRevisionsDeleted = store.Maintenance.Send(deleteRevisionsOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region delete_revisions_4
                    // Get the change-vectors for the revisions of the specified document
                    var revisionsChangeVectors = session.Advanced.Revisions
                        .GetMetadataFor("orders/825-A")
                        .Select(m => m.GetString(Constants.Documents.Metadata.ChangeVector))
                        .ToList();
                   
                    // Delete the revisions by their change-vector 
                    var revisionToDelete = 
                        new List<string>() { revisionsChangeVectors[0], revisionsChangeVectors[1] };
                   
                    var deleteRevisionsOp =
                        new DeleteRevisionsOperation(documentId: "orders/825-A", revisionToDelete);

                    var numberOfRevisionsDeleted = store.Maintenance.Send(deleteRevisionsOp);
                    #endregion
                }
            }
        }

        public async Task ConfigRevisionsAsync()
        {
            using (var store = new DocumentStore())
            {
                #region delete_revisions_1_async
                // Define the delete revisions operation:
                
                // Delete ALL existing revisions for document "orders/830-A"
                var deleteRevisionsOp = new DeleteRevisionsOperation(documentId: "orders/830-A",
                    // Revisions that were created manually will also be removed
                    removeForceCreatedRevisions: true);
               
                // Execute the operation by passing it to Maintenance.SendAsync
                var numberOfRevisionsDeleted = await store.Maintenance.SendAsync(deleteRevisionsOp);
                
                // Running the above code on RavenDB's sample data results in the removal of 29 revisions
                Assert.Equal(29, numberOfRevisionsDeleted.TotalDeletes);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {

                #region delete_revisions_2_async
                // Delete existing revisions for the specified documents
                var deleteRevisionsOp = new DeleteRevisionsOperation(
                    documentIds: new List<string>() { "orders/829-A", "orders/828-A", "orders/827-A" },
                    // Revisions that were created manually will Not be removed
                    removeForceCreatedRevisions: false);

                var numberOfRevisionsDeleted = await store.Maintenance.SendAsync(deleteRevisionsOp);
                
                // Running the above on RavenDB's sample data results in the removal of 19 revisions
                Assert.Equal(19, numberOfRevisionsDeleted.TotalDeletes);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region delete_revisions_3_async
                var deleteFrom = DateTime.Parse("2018-07-27T09:11:52.0Z");
                var deleteTo = DateTime.Parse("2018-07-27T09:11:54.0Z");
                
                // Delete existing revisions within the specified time frame
                var deleteRevisionsOp =
                    new DeleteRevisionsOperation(documentId: "orders/826-A", from: deleteFrom, to: deleteTo);

                var numberOfRevisionsDeleted = await store.Maintenance.SendAsync(deleteRevisionsOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region delete_revisions_4_async
                    // Get the change-vectors for the revisions of the specified document
                    var metadata = await asyncSession.Advanced.Revisions
                        .GetMetadataForAsync("orders/825-A");
                        
                    var revisionsChangeVectors = metadata
                        .Select(m => m.GetString(Constants.Documents.Metadata.ChangeVector))
                        .ToList();
                   
                    // Delete the revisions by their change-vector 
                    var revisionToDelete = 
                        new List<string>() { revisionsChangeVectors[0], revisionsChangeVectors[1] };
                   
                    var deleteRevisionsOp =
                        new DeleteRevisionsOperation(documentId: "orders/825-A", revisionToDelete);

                    var numberOfRevisionsDeleted = await store.Maintenance.SendAsync(deleteRevisionsOp);
                    #endregion
                }
            }
        }
    }

    public class DeleteRevisionsSyntax
    {
        public interface IFoo
        {
            /*
            #region syntax_1
            Available overloads:
            ====================
            public DeleteRevisionsOperation(string documentId, 
                bool removeForceCreatedRevisions = false);
                
            public DeleteRevisionsOperation(string documentId,
                DateTime? from, DateTime? to, bool removeForceCreatedRevisions = false);
                
            public DeleteRevisionsOperation(List<string> documentIds,
                bool removeForceCreatedRevisions = false);
                
            public DeleteRevisionsOperation(List<string> documentIds,
                DateTime? from, DateTime? to, bool removeForceCreatedRevisions = false);
                
            public DeleteRevisionsOperation(string documentId,
                List<string> revisionsChangeVectors, bool removeForceCreatedRevisions = false);
            #endregion
            */
        }
    }
}
