using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class ResetIndex
    {
        private interface IFoo
        {
            /*
            #region reset_index_1
            public ResetIndexOperation(string indexName);
            #endregion
            */
        }

        public ResetIndex()
        {

            using (var store = new DocumentStore())
            {
                #region reset_index_2
                store.Maintenance.Send(new ResetIndexOperation("Orders/Totals"));
                #endregion
            }
        }
    }
}
