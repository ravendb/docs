using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
    public class CustomizeCollectionAssignmentForEntities
    {
        public CustomizeCollectionAssignmentForEntities()
        {
            var store = new DocumentStore();

            #region custom_collection_name
            store.Conventions.FindCollectionName = type =>
            {
                if (typeof(Category).IsAssignableFrom(type))
                    return "ProductGroups";

                return DocumentConventions.DefaultGetCollectionName(type);
            };
            #endregion
        }
    }
}
