using System.Linq;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToPerformProximitySearch
    {
        private interface IFoo<T>
        {
            #region proximity_1
            IDocumentQuery<T> Proximity(int proximity);
            #endregion
        }

        public void Sample1(IDocumentSession session)
        {
            #region proximity_2
            session
                .Advanced
                .DocumentQuery<Fox>()
                .Search(x => x.Name, "quick fox").Proximity(2)
                .ToList();
            #endregion
        }

        public void Sample2(IAsyncDocumentSession session)
        {
            #region proximity_3
            session
                .Advanced
                .AsyncDocumentQuery<Fox>()
                .Search(x => x.Name, "quick fox").Proximity(2)
                .ToListAsync();
            #endregion
        }

        public class Fox
        {
            public string Name { get; set; }
        }
    }
}
