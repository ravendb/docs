using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class GetIndexTerms
    {
        public GetIndexTerms()
        {
            using (var store = new DocumentStore())
            {
                #region get_index_terms
                // Define the get terms operation
                // Pass the requested index-name, index-field, start value & page size
                var getTermsOp = new GetTermsOperation("Orders/Totals", "Employee", "employees/5-a", 10);

                // Execute the operation by passing it to Maintenance.Send
                string[] fieldTerms = store.Maintenance.Send(getTermsOp);

                // fieldTerms will contain the all terms that come after term 'employees/5-a' for index-field 'Employee'
                #endregion
            }
        }
        
        public async Task GetIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region get_index_terms_async
                // Define the get terms operation
                // Pass the requested index-name, index-field, start value & page size
                var getTermsOp = new GetTermsOperation("Orders/Totals", "Employee", "employees/5-a", 10);

                // Execute the operation by passing it to Maintenance.SendAsync
                string[] fieldTerms = await store.Maintenance.SendAsync(getTermsOp);

                // fieldTerms will contain the all terms that come after term 'employees/5-a' for index-field 'Employee'
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region get_index_terms_syntax
            public GetTermsOperation(string indexName, string field, string fromValue, int? pageSize = null)
            #endregion
            */
        }
    }
}
