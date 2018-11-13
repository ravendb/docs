using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Json;

namespace Raven.Documentation.Samples.ClientApi.Session.Revisions
{
    public class CounterRevisions
    {
        public void Samples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example
                    List<MetadataAsDictionary> orderRevisionsMetadata =
                        session
                            .Advanced
                            .Revisions
                            .GetMetadataFor(
                                id: "orders/1-A",
                                start: 0,
                                pageSize: 10);

                    List<MetadataAsDictionary> orderCountersSnapshots = 
                        orderRevisionsMetadata
                            .Where(metadata => 
                                metadata.ContainsKey("@counters-snapshot"))
                            .Select(metadata => 
                                (MetadataAsDictionary)metadata["@counters-snapshot"])
                            .ToList();
                    #endregion
                }
            }
        }
    }
}
