using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class IndexTerms
    {
        private interface IFoo
        {
            /*
            #region get_terms1
            public GetTermsOperation(string indexName, string field, string fromValue, int? pageSize = null)
            #endregion
            */
        }

        public IndexTerms()
        {

            using (var store = new DocumentStore())
            {
                #region get_terms2
                string[] terms = store.Maintenance.Send(new GetTermsOperation("Orders/Totals", "Employee", null));
                #endregion

            }
        }
    }
}
