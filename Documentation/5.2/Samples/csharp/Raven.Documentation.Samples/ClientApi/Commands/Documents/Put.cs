
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class PutInterfaces
    {
        private class PutDocumentCommand
        {
            #region put_interface
            public PutDocumentCommand(string id, string changeVector, BlittableJsonReaderObject document)
            #endregion
            {
            }
        }
    }

    public class PutSamples
    {
        public void Foo()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region put_sample
                // Create a new document
                var doc = new Category
                {
                    Name = "My category",
                    Description = "My category description"
                };
                // Create metadata on the document
                var docInfo = new DocumentInfo
                {
                    Collection = "Categories"
                };
                // Convert your entity to a BlittableJsonReaderObject
                var blittableDoc = session.Advanced.JsonConverter.ToBlittable(doc, docInfo);

                // The Put command (parameters are document ID, changeVector check is null, the document to store)
                var command = new PutDocumentCommand(null, "categories/999", null, blittableDoc);

                // RequestExecutor sends the command to the server
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                #endregion
            }
        }
    }
}
