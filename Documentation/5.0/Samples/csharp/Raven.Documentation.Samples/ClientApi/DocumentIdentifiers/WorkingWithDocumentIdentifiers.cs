using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Operations.Identities;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.ClientApi.DocumentIdentifiers
{
    public class WorkingWithDocumentIdentifiers
    {
        public WorkingWithDocumentIdentifiers()
        {
            var store = new DocumentStore();

            var session = store.OpenSession();

            #region session_id_not_provided

            var order = new Order
            {
                Id = null // value not provided
            };

            session.Store(order);

            #endregion


            #region session_get_document_id

            var orderId = session.Advanced.GetDocumentId(order); // "orders/1-A"

            #endregion

            #region session_empty_string_id

            var orderEmptyId = new Order
            {
                Id = string.Empty // database will create a GUID value for it
            };

            session.Store(orderEmptyId);

            session.SaveChanges();

            var guidId = session.Advanced.GetDocumentId(orderEmptyId); // "bc151542-8fa7-45ac-bc04-509b343a8720"

            #endregion

            #region session_semantic_id_1

            var product = new Product
            {
                Id = "products/ravendb",
                Name = "RavenDB"
            };

            session.Store(product);

            #endregion

            #region session_semantic_id_2

            session.Store(new Product
            {
                Name = "RavenDB"
            }, "products/ravendb");

            #endregion


            #region session_auto_id

            session.Store(new Company
            {
                Id = "companies/"
            });

            session.SaveChanges();

            #endregion

            #region session_identity_id

            session.Store(new Company
            {
                Id = "companies|"
            });

            session.SaveChanges();

            #endregion


            #region commands_identity

            var doc = new DynamicJsonValue
            {
                ["Name"] = "My RavenDB"
            };

            var blittableDoc = session.Advanced.JsonConverter.ToBlittable(doc, null);

            var command = new PutDocumentCommand("products/", null, blittableDoc);

            session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

            var identityId = command.Result.Id; // "products/0000000000000000001-A if using only '/' in the seesion"

            var commandWithPipe = new PutDocumentCommand("products|", null, blittableDoc);
            session.Advanced.RequestExecutor.Execute(commandWithPipe, session.Advanced.Context);

            var identityPipeId = command.Result.Id; // "products/1"

            #endregion
        }

        public WorkingWithDocumentIdentifiers(string g)
        {
            var store = new DocumentStore();
            var session = store.OpenSession();

            #region commands_identity_generate

            var command = new NextIdentityForCommand("products");
            session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
            var identity = command.Result;

            var doc = new DynamicJsonValue
            {
                ["Name"] = "My RavenDB"
            };

            var blittableDoc = session.Advanced.JsonConverter.ToBlittable(doc, null);

            var putCommand = new PutDocumentCommand("products/" + identity, null, blittableDoc);

            session.Advanced.RequestExecutor.Execute(putCommand, session.Advanced.Context);

            #endregion

            #region operation_identity_generate

            store.Maintenance.Send(new NextIdentityForOperation("products"));

            #endregion

            #region commands_identity_set

            var seedIdentityCommand = new SeedIdentityForCommand("products", 1994);

            #endregion

            #region operation_identity_set

            var seedIdentityOperation = store.Maintenance.Send(new SeedIdentityForOperation("products", 1994));

            #endregion

        }
    }
}
