using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class EndsWith
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region endsWith_1
                    List<Product> products = session
                        .Query<Product>()
                         // Call 'EndsWith' on the field
                         // Pass the postfix to search by
                        .Where(x => x.Name.EndsWith("Lager"))
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that ends with 'Lager' OR 'lager'
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region endsWith_2
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Call 'EndsWith' on the field
                         // Pass the postfix to search by
                        .Where(x => x.Name.EndsWith("Lager"))
                        .ToListAsync();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that ends with 'Lager' OR 'lager'
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region endsWith_3
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'WhereEndsWith'
                         // Pass the document field and the postfix to search by
                        .WhereEndsWith(x => x.Name, "Lager")
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that ends with 'Lager' OR 'lager'
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region endsWith_4
                    List<Product> products = session
                        .Query<Product>()
                         // Pass 'true' as the 2'nd parameter to search for an EXACT postfix match
                        .Where(x => x.Name.EndsWith("Lager"), true)
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that ends with 'Lager'
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region endsWith_5
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Pass 'true' as the 2'nd parameter to search for an EXACT postfix match
                        .Where(x => x.Name.EndsWith("Lager"), true)
                        .ToListAsync();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that ends with 'Lager'
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region endsWith_6
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'WhereEndsWith'
                         // Pass 'true' as the 3'rd parameter to search for an EXACT postfix match
                        .WhereEndsWith(x => x.Name, "Lager", true)
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that ends with 'Lager'
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region endsWith_7
                    List<Product> products = session
                        .Query<Product>()
                         // Call 'EndsWith' on the field
                         // Pass the postfix to search by
                        .Where(x => x.Name.EndsWith("Lager") == false)
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that does NOT end with 'Lager' or 'lager'
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region endsWith_8
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Call 'EndsWith' on the field
                         // Pass the postfix to search by
                        .Where(x => x.Name.EndsWith("Lager") == false)
                        .ToListAsync();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that does NOT end with 'Lager' or 'lager'
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region endsWith_9
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'Not' to negate the next predicate
                        .Not
                         // Call 'WhereEndsWith'
                         // Pass the document field and the postfix to search by
                        .WhereEndsWith(x => x.Name, "Lager")
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that does NOT end with 'Lager' or 'lager'
                    #endregion
                }
            }
        }
    }
}
