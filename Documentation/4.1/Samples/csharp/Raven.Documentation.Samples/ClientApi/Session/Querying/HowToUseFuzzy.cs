using System.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToUseFuzzy
    {
        private interface IFoo<T>
        {
            #region fuzzy_1
            IDocumentQuery<T> Fuzzy(decimal fuzzy);
            #endregion
        }

        public void Sample1(IDocumentSession session)
        {
            #region fuzzy_2
            session
                .Advanced
                .DocumentQuery<Company>()
                .WhereEquals(x => x.Name, "Ernts Hnadel").Fuzzy(0.5m)
                .ToList();
            #endregion
        }

        public void Sample2(IAsyncDocumentSession session)
        {
            #region fuzzy_3
            session
                .Advanced
                .AsyncDocumentQuery<Company>()
                .WhereEquals(x => x.Name, "Ernts Hnadel").Fuzzy(0.5m)
                .ToListAsync();
            #endregion
        }
    }
}
