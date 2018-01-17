using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class IndexHasChagned
    {
        private interface IFoo
        {
            /*
            #region index_has_changed_1
            public IndexHasChangedOperation(IndexDefinition definition)
            #endregion
            */
        }

        public IndexHasChagned()
        {

            using (var store = new DocumentStore())
            {
                IndexDefinition ordersIndexDefinition;
                #region index_has_changed_2
                bool ordersIndexHasChanged = store.Maintenance.Send(new IndexHasChangedOperation(ordersIndexDefinition));
                #endregion
            }
        }
    }
}
