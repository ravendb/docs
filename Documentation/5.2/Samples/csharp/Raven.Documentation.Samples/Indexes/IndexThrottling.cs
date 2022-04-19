using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Counters;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.IndexThrottling
{
    public class IndexThrottling
    {
        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region throttlingInterval
                var index = new IndexDefinition
                {
                    Name = "ByOrderedAt",
                    Maps = {"from o in orders select new {o.OrderedAt}"},
                    Configuration = new IndexConfiguration
                    {
                        { "Indexing.Throttling.TimeIntervalInMs", "2000" }
                    },
                };
                #endregion

                var index2 = new IndexDefinition
                {
                    Name = "ByOrderedAt",
                    Maps = { "from o in orders select new {o.OrderedAt}" },
                    #region batchSize
                    Configuration = new IndexConfiguration
                    {
                        { "Indexing.Throttling.TimeIntervalInMs", "2000" },
                        { "Indexing.MapBatchSize", "50" }
                    },
                    #endregion
                };
            }
        }
    }
}
