using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class PutSamples
    {
        public async Task ExamplesWithStore()
        {
            #region put_document_1
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Define the document to 'put' as a blittable object
                var blittableDocument = context.ReadObject(new DynamicJsonValue()
                {
                    ["@metadata"] = new DynamicJsonValue()
                    {
                        ["@collection"] = "Categories"
                    },
                    ["Name"] = "My category",
                    ["Description"] = "My category description"
                }, "categories/999");
                
                // Define the PutDocumentCommand
                var command = new PutDocumentCommand(store.Conventions,
                    "categories/999", null, blittableDocument);
              
                // Call 'Execute' on the Store Request Executor to send the command to the server
                store.GetRequestExecutor().Execute(command, context);

                // Access the command result
                var putResult = command.Result;
                var theDocumentID = putResult.Id;
                var theDocumentCV = putResult.ChangeVector;
            }
            #endregion
            
            #region put_document_1_async
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor().ContextPool.AllocateOperationContext(out var context))
            {
                // Define the document to 'put' as a blittable object
                var blittableDocument = context.ReadObject(new DynamicJsonValue()
                {
                    ["@metadata"] = new DynamicJsonValue()
                    {
                        ["@collection"] = "Categories"
                    },
                    ["Name"] = "My category",
                    ["Description"] = "My category description"
                }, "categories/999");
                
                // Define the PutDocumentCommand
                var command = new PutDocumentCommand(store.Conventions,
                    "categories/999", null, blittableDocument);
              
                // Call 'ExecuteAsync' on the Store Request Executor to send the command to the server
                await store.GetRequestExecutor().ExecuteAsync(command, context);

                // Access the command result
                var putResult = command.Result;
                var theDocumentID = putResult.Id;
                var theDocumentCV = putResult.ChangeVector;
            }
            #endregion
        }
        
        public async Task ExamplesWithSession()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region put_document_2
                // Create a new document entity
                var doc = new Category
                {
                    Name = "My category",
                    Description = "My category description"
                };
                
                // Specify the collection to which the document will belong
                var docInfo = new DocumentInfo
                {
                    Collection = "Categories"
                };
                
                // Convert your entity to a BlittableJsonReaderObject
                var blittableDocument = session.Advanced.JsonConverter.ToBlittable(doc, docInfo);

                // Define the PutDocumentCommand
                var command = new PutDocumentCommand(store.Conventions,
                    "categories/999", null, blittableDocument);

                // Call 'Execute' on the Session Request Executor to send the command to the server
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

                // Access the command result
                var putResult = command.Result;
                var theDocumentID = putResult.Id;
                var theDocumentCV = putResult.ChangeVector;
                #endregion
            }
            
            using (var store = new DocumentStore())
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region put_document_2_async
                // Create a new document entity
                var doc = new Category
                {
                    Name = "My category",
                    Description = "My category description"
                };
                
                // Specify the collection to which the document will belong
                var docInfo = new DocumentInfo
                {
                    Collection = "Categories"
                };
                
                // Convert your entity to a BlittableJsonReaderObject
                var blittableDocument = asyncSession.Advanced.JsonConverter.ToBlittable(doc, docInfo);

                // Define the PutDocumentCommand
                var command = new PutDocumentCommand(store.Conventions,
                    "categories/999", null, blittableDocument);

                // Call 'Execute' on the Session Request Executor to send the command to the server
                await asyncSession.Advanced.RequestExecutor.ExecuteAsync(
                    command, asyncSession.Advanced.Context);
                
                // Access the command result
                var putResult = command.Result;
                var theDocumentID = putResult.Id;
                var theDocumentCV = putResult.ChangeVector;
                #endregion
            }
        }
    }
    
    public class PutInterfaces
    {
        private class PutDocumentCommand
        {
            #region syntax_1
            public PutDocumentCommand(DocumentConventions conventions,
                    string id, string changeVector, BlittableJsonReaderObject document)
            #endregion
            {
            }
        }
        
        #region syntax_2
        // The `PutDocumentCommand` result:
        // ================================
        
        public class PutResult
        {
            /// The ID under which document was stored 
            public string Id { get; set; }

            // The changeVector that was assigned to the stored document
            public string ChangeVector { get; set; }
        }
        #endregion
    }
}
