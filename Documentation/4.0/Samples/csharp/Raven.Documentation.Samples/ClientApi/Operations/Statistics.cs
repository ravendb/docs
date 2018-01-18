using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class Statistics
    {
        private interface IFoo
        {
            /*
            #region stats_1
            public GetStatisticsOperation()
            #endregion
            */
        }

        public Statistics()
        {
            using (var store = new DocumentStore())
            {
                #region stats_2
                DatabaseStatistics stats = store.Maintenance.Send(new GetStatisticsOperation());
                long countOfDocuments = stats.CountOfDocuments;
                long countOfIndexes = stats.CountOfIndexes;
                bool isStale = stats.Indexes[0].IsStale;
                #endregion

            }
        }
    }
}
