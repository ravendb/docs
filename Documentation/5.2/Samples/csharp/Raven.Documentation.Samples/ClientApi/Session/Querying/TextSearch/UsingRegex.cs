using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class UsingRegex
    {
        private class User
        {
            public string FirstName { get; set; }
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region regex_1
                    // loads all products, which name
                    // starts with 'N' or 'A'
                    List<Product> products = session
                        .Query<Product>()
                        .Where(x => Regex.IsMatch(x.Name, "^[NA]"))
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region regex_1_async
                    // loads all products, which name
                    // starts with 'N' or 'A'
                    List<Product> products = await asyncSession
                        .Query<Product>()
                        .Where(x => Regex.IsMatch(x.Name, "^[NA]"))
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
