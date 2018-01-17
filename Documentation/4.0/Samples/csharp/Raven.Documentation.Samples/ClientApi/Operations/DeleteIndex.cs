using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class DeleteIndex
    {
        private interface IFoo
        {
            /*
            #region delete_1
            public DeleteIndexOperation(string indexName)
            #endregion
            */
        }

        public DeleteIndex()
        {

            using (var store = new DocumentStore())
            {
                #region delete_2
                store.Maintenance.Send(new DeleteIndexOperation("Orders/Totals"));
                #endregion
            }
        }
    }
}
