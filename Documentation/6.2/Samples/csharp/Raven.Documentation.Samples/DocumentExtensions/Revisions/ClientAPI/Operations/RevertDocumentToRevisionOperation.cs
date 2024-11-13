using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Xunit;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class RevertDocumentToRevisionOperation
    {
        public RevertDocumentToRevisionOperation()
        {
            using (var store = new DocumentStore())
            {
                #region revert_document_1
                using (var session = store.OpenSession())
                {
                    // Get the revisions metadata for the document you wish to revert
                    // ==============================================================
                    
                    var revisionsMetadata = session.Advanced.Revisions
                        .GetMetadataFor(id: "orders/1-A");

                    // Get the CV of the revision you wish to revert to:
                    // =================================================
                    
                    // Note: revisionsMetadata[0] is the latest revision,
                    // so specify the index of the revision you want.
                    // In this example, it will be the very first revision of the document:
                    
                    var numberOfRevisions = revisionsMetadata.Count();
                    var changeVector = revisionsMetadata[numberOfRevisions-1]
                        .GetString(Constants.Documents.Metadata.ChangeVector);

                    // Execute the operation
                    store.Operations.Send(
                        // Pass the document ID and the change-vector of the revision to revert to
                        new RevertRevisionsByIdOperation("orders/1-A", changeVector));
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region revert_document_2
                using (var session = store.OpenSession())
                {
                    // Get the revisions metadata for the documents you wish to revert
                    var revisionsMetadata1 = session.Advanced.Revisions
                        .GetMetadataFor(id: "orders/1-A");
                    var revisionsMetadata2 = session.Advanced.Revisions
                        .GetMetadataFor(id: "users/999");

                    // Get the CV of the revisions you wish to revert to
                    var changeVector1 = revisionsMetadata1[2]
                        .GetString(Constants.Documents.Metadata.ChangeVector);
                    var changeVector2 = revisionsMetadata1[3]
                        .GetString(Constants.Documents.Metadata.ChangeVector);

                    // Execute the operation
                    store.Operations.Send(
                        // Pass the document IDs and the change-vector of the revisions to revert to
                        new RevertRevisionsByIdOperation(new Dictionary<string, string>()
                            { { "orders/1-A", changeVector1 }, { "users/999", changeVector2 } }));
                }
                #endregion
            }
        }

        public async Task ConfigRevisionsAsync()
        {
            using (var store = new DocumentStore())
            {
                #region revert_document_1_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Get the revisions metadata for the document you wish to revert
                    // ==============================================================
                    
                    var revisionsMetadata = await asyncSession.Advanced.Revisions
                        .GetMetadataForAsync(id: "Orders/1-A");
                    
                    // Get the CV of the revision you wish to revert to:
                    // =================================================
                    
                    // Note: revisionsMetadata[0] is the latest revision,
                    // so specify the index of the revision you want.
                    // In this example, it will be the very first revision of the document:
                    
                    var numberOfRevisions = revisionsMetadata.Count();
                    var changeVector = revisionsMetadata[numberOfRevisions-1]
                        .GetString(Constants.Documents.Metadata.ChangeVector);

                    // Execute the operation
                    await store.Operations.SendAsync(
                        // Pass the document ID and the change-vector of the revision to revert to
                        new RevertRevisionsByIdOperation("Orders/1-A", changeVector));
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {

                #region revert_document_2_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Get the revisions metadata for the documents you wish to revert
                    var revisionsMetadata1 = await asyncSession.Advanced.Revisions
                        .GetMetadataForAsync(id: "orders/1-A");
                    var revisionsMetadata2 = await asyncSession.Advanced.Revisions
                        .GetMetadataForAsync(id: "users/999");

                    // Get the CV of the revisions you wish to revert to
                    var changeVector1 = revisionsMetadata1[2]
                        .GetString(Constants.Documents.Metadata.ChangeVector);
                    var changeVector2 = revisionsMetadata1[3]
                        .GetString(Constants.Documents.Metadata.ChangeVector);

                    // Execute the operation
                    await store.Operations.SendAsync(
                        // Pass the document IDs and the change-vector of the revisions to revert to
                        new RevertRevisionsByIdOperation(new Dictionary<string, string>()
                            { { "orders/1-A", changeVector1 }, { "users/999", changeVector2 } }));
                }
                #endregion
            }
        }
        
        public class Syntax
        {
            public interface IFoo
            {
                /*
                #region syntax_1
                Available overloads:
                ====================
                public RevertRevisionsByIdOperation(string id, string cv);
                public RevertRevisionsByIdOperation(Dictionary<string, string> idToChangeVector);
                #endregion
                */
            }
        }
    }
}
