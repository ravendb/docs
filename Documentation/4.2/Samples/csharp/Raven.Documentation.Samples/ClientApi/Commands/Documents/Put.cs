
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
                var doc = new Category
                {
                    Name = "My category",
                    Description = "My category description"
                };
                var docInfo = new DocumentInfo
                {
                    Collection = "Categories"
                };
                var blittableDoc = session.Advanced.EntityToBlittable.ConvertEntityToBlittable(doc, docInfo);

                var command = new PutDocumentCommand("categories/999", null, blittableDoc);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                #endregion
            }
        }
    }
}
