using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Json;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Session
{
    public class ExtractCountersFromRevisions
    {
        public ExtractCountersFromRevisions()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region extract_counters
                    // Use GetMetadataFor to get revisions metadata for document 'orders/1-A'
                    List<MetadataAsDictionary> revisionsMetadata = session
                        .Advanced.Revisions.GetMetadataFor(id: "orders/1-A");

                    // Extract the counters data from the metadata
                    List<MetadataAsDictionary> countersDataInRevisions = revisionsMetadata
                        .Where(metadata =>
                            metadata.ContainsKey(Constants.Documents.Metadata.RevisionCounters))
                        .Select(metadata =>
                            (MetadataAsDictionary)metadata[Constants.Documents.Metadata.RevisionCounters])
                        .ToList();
                    #endregion
                }
            }
        }

        public async Task ExtractCountersFromRevisionsAsync()
        {
            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region extract_counters_async
                    // Use GetMetadataForAsync to get revisions metadata for document 'orders/1-A'
                    List<MetadataAsDictionary> revisionsMetadata = await asyncSession
                        .Advanced.Revisions.GetMetadataForAsync(id: "orders/1-A");

                    // Extract the counters data from the metadata
                    List<MetadataAsDictionary> countersDataInRevisions = revisionsMetadata
                        .Where(metadata =>
                            metadata.ContainsKey(Constants.Documents.Metadata.RevisionCounters))
                        .Select(metadata =>
                            (MetadataAsDictionary)metadata[Constants.Documents.Metadata.RevisionCounters])
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
