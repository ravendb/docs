using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class CollectionStats
    {
        private interface IFoo
        {
            /*
            #region stats_1
            public GetCollectionStatisticsOperation()
            #endregion
            */
        }

        private class Foo
        {
            #region stats_2
            public class CollectionStatistics
            {
                public int CountOfDocuments { get; set; }
                public int CountOfConflicts { get; set; }

                public Dictionary<string, long> Collections { get; set; }
            }
            #endregion
        }

        public CollectionStats()
        {
            using (var store = new DocumentStore())
            {
                #region stats_3
                CollectionStatistics stats = store.Maintenance.Send(new GetCollectionStatisticsOperation());
                #endregion
            }
        }
    }
}
