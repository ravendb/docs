using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.DQ
{
    public class HowToUseLucene
    {
        private interface IFoo<T>
        {
            #region lucene_1
            IDocumentQuery<T> WhereLucene(string fieldName, string whereClause);
            #endregion
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region lucene_2
                    List<Company> results = session
                        .Advanced
                        .DocumentQuery<Company>()
                        .WhereLucene("Name", "bistro")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region lucene_3
                    List<Company> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Company>()
                        .WhereLucene("Name", "bistro")
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
