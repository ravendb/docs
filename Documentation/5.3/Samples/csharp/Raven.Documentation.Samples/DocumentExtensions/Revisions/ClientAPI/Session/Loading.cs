using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Session
{
    public class GetRevisions
    {
        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example_1_sync
                    // Get revisions for document 'orders/1-A'
                    // Revisions will be ordered by most recent revision first
                    List<Order> orderRevisions = session
                        .Advanced
                        .Revisions
                        .GetFor<Order>(id: "orders/1-A", start: 0, pageSize: 10);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_1_async
                    // Get revisions for document 'orders/1-A'
                    // Revisions will be ordered by most recent revision first
                    List<Order> orderRevisions = await asyncSession
                        .Advanced
                        .Revisions
                        .GetForAsync<Order>(id: "orders/1-A", start: 0, pageSize: 10);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example_2_sync
                    // Get revisions' metadata for document 'orders/1-A'
                    List<MetadataAsDictionary> orderRevisionsMetadata = session
                        .Advanced
                        .Revisions
                        .GetMetadataFor(id: "orders/1-A", start: 0, pageSize: 10);
                    
                    // Each item returned is a revision's metadata, as can be verified in the @flags key
                    var metadata = orderRevisionsMetadata[0];
                    var flagsValue = metadata.GetString(Constants.Documents.Metadata.Flags);
                    
                    Assert.Contains("Revision", flagsValue);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_2_async
                    // Get revisions' metadata for document 'orders/1-A'
                    List<MetadataAsDictionary> orderRevisionsMetadata = await asyncSession
                        .Advanced
                        .Revisions
                        .GetMetadataForAsync(id: "orders/1-A", start: 0, pageSize: 10);
                    
                    // Each item returned is a revision's metadata, as can be verified in the @flags key
                    var metadata = orderRevisionsMetadata[0];
                    var flagsValue = metadata.GetString(Constants.Documents.Metadata.Flags);
                    
                    Assert.Contains("Revision", flagsValue);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example_3_sync
                    // Get a revision by its creation time
                    Order revisionFromLastYear = session
                        .Advanced
                        .Revisions
                         // If no revision was created at the specified time,
                         // then the first revision that precedes it will be returned
                        .Get<Order>("orders/1-A", DateTime.Now.AddYears(-1));
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_3_async
                    // Get a revision by its creation time
                    Order revisionFromLastYear = await asyncSession
                        .Advanced
                        .Revisions
                        // If no revision was created at the specified time,
                        // then the first revision that precedes it will be returned
                        .GetAsync<Order>("orders/1-A", DateTime.Now.AddYears(-1));
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example_4_sync
                    // Get revisions metadata 
                    List<MetadataAsDictionary> revisionsMetadata = session
                        .Advanced
                        .Revisions
                        .GetMetadataFor("orders/1-A", start: 0, pageSize: 25);

                    // Get the change-vector from the metadata
                    var changeVector = revisionsMetadata[0].GetString(Constants.Documents.Metadata.ChangeVector);
                    
                    // Get the revision by its change-vector
                    Order revision = session
                        .Advanced
                        .Revisions
                        .Get<Order>(changeVector);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_4_async
                    // Get revisions metadata 
                    List<MetadataAsDictionary> revisionsMetadata = await asyncSession
                        .Advanced
                        .Revisions
                        .GetMetadataForAsync("orders/1-A", start: 0, pageSize: 25);

                    // Get the change-vector from the metadata
                    var changeVector = revisionsMetadata[0].GetString(Constants.Documents.Metadata.ChangeVector);
                    
                    // Get the revision by its change-vector
                    Order revision = await asyncSession
                        .Advanced
                        .Revisions
                        .GetAsync<Order>(changeVector);
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region syntax_1
            List<T> GetFor<T>(string id, int start = 0, int pageSize = 25);
            #endregion

            #region syntax_2
            List<MetadataAsDictionary> GetMetadataFor(string id, int start = 0, int pageSize = 25);
            #endregion

            #region syntax_3
            T Get<T>(string id, DateTime date);
            #endregion
            
            #region syntax_4
            // Get a revision by its change vector
            T Get<T>(string changeVector);

            // Get multiple revisions by their change vectors
            Dictionary<string, T> Get<T>(IEnumerable<string> changeVectors);
            #endregion
        }
    }
}
