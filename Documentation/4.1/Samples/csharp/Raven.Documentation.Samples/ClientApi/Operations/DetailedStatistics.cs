using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class DetailedStatistics
    {
        private interface IFoo
        {
            /*
            #region stats_1
            public GetDetailedStatisticsOperation()
            #endregion
            */
        }

        public DetailedStatistics()
        {
            using (var store = new DocumentStore())
            {
                #region stats_2
                DetailedDatabaseStatistics stats = store.Maintenance.Send(new GetDetailedStatisticsOperation());
                long countOfDocuments = stats.CountOfDocuments;
                long countOfIndexes = stats.CountOfIndexes;
                long countOfCompareExchange = stats.CountOfCompareExchange;
                long countOfIdentities = stats.CountOfIdentities;
                bool isStale = stats.Indexes[0].IsStale;
                #endregion

            }
        }
    }
}
