using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class FuzzySearch
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region fuzzy_1
                    List<Company> companies = session.Advanced
                        .DocumentQuery<Company>()
                         // Query with a term that is misspelled
                        .WhereEquals(x => x.Name, "Ernts Hnadel")
                        // Call 'Fuzzy', 
                        // Pass the required similarity, a decimal param
                        .Fuzzy(0.5m)
                        .ToList();
                    
                    // Running the above query on the Northwind sample data returns document: companies/20-A
                    // which contains "Ernst Handel" in its Name field.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fuzzy_2
                    List<Company> companies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Company>()
                         // Query with a term that is for misspelled
                        .WhereEquals(x => x.Name, "Ernts Hnadel")
                         // Call 'Fuzzy', 
                         // Pass the required similarity, a decimal param
                        .Fuzzy(0.5m)
                        .ToListAsync();
                    
                    // Running the above query on the Northwind sample data returns document: companies/20-A
                    // which contains "Ernst Handel" in its Name field.
                    #endregion
                }
            }
        }
        
        private interface IFoo<T>
        {
            #region syntax
            IDocumentQuery<T> Fuzzy(decimal fuzzy);
            #endregion
        }
    }
}
