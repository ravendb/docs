using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Revisions
{
    public class RevisionsLoading
    {
        private interface IFoo
        {
            #region syntax_1
            List<T> GetFor<T>(string id, int start = 0, int pageSize = 25);
            #endregion

            #region syntax_2
            List<MetadataAsDictionary> GetMetadataFor(string id, int start = 0, int pageSize = 25);
            #endregion

            #region syntax_3
            // Get a revision by its change vector
            T Get<T>(string changeVector);

            // Get multiple revisions by their change vectors
            Dictionary<string, T> Get<T>(IEnumerable<string> changeVectors);

            // Get a revision by its creation time
            // If no revision was created at that precise time, get the first revision to precede it
            T Get<T>(string id, DateTime date);
            #endregion
        }

        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example_1_sync
                    List<Order> orderRevisions = session
                        .Advanced
                        .Revisions
                        .GetFor<Order>(
                            id: "orders/1-A",
                            start: 0,
                            pageSize: 10);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_1_async
                    List<Order> orderRevisions = await asyncSession
                        .Advanced
                        .Revisions
                        .GetForAsync<Order>(
                            id: "orders/1-A",
                            start: 0,
                            pageSize: 10);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example_2_sync
                    List<MetadataAsDictionary> orderRevisionsMetadata =
                        session
                            .Advanced
                            .Revisions
                            .GetMetadataFor(
                                id: "orders/1-A",
                                start: 0,
                                pageSize: 10);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_2_async
                    List<MetadataAsDictionary> orderRevisionsMetadata =
                        await asyncSession
                            .Advanced
                            .Revisions
                            .GetMetadataForAsync(
                                id: "orders/1-A",
                                start: 0,
                                pageSize: 10);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                string orderRevisionChangeVector = null;
                using (var session = store.OpenSession())
                {
                    #region example_3.1_sync
                    Order orderRevision =
                        session
                            .Advanced
                            .Revisions
                            // Get revisions by their change vectors
                            .Get<Order>(orderRevisionChangeVector);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_3.1_async
                    Order orderRevision =
                        await asyncSession
                            .Advanced
                            .Revisions
                            // Get revisions by their change vectors
                            .GetAsync<Order>(orderRevisionChangeVector);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                string orderRevisionChangeVector = null;
                using (var session = store.OpenSession())
                {
                    #region example_3.2_sync
                    // Get revisions metadata 
                    List<MetadataAsDictionary> revisionsMetadata = 
                        session
                            .Advanced
                            .Revisions
                            .GetMetadataFor("users/1", start: 0, pageSize: 25);

                    // Get revision by its change vector
                    User revison = 
                        session
                            .Advanced
                            .Revisions
                            .Get<User>(revisionsMetadata[0].GetString(Constants.Documents.Metadata.ChangeVector));
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_3.2_async
                    // Get revisions metadata 
                    List<MetadataAsDictionary> revisionsMetadata =
                        await asyncSession
                            .Advanced
                            .Revisions
                            .GetMetadataForAsync("users/1", start: 0, pageSize: 25);

                    // Get revision by its change vector
                    User revison =
                        await asyncSession
                            .Advanced
                            .Revisions
                            .GetAsync<User>(revisionsMetadata[0].GetString(Constants.Documents.Metadata.ChangeVector));
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                string orderRevisionChangeVector = null;
                using (var session = store.OpenSession())
                {
                    #region example_3.3_sync
                    User revisonAtYearAgo = 
                        session
                            .Advanced
                            .Revisions
                            // Get a revision by its creation time
                            .Get<User>("users/1", DateTime.Now.AddYears(-1));
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_3.3_async
                    User revisonAtYearAgo =
                        await asyncSession
                            .Advanced
                            .Revisions
                            // Get a revision by its creation time
                            .GetAsync<User>("users/1", DateTime.Now.AddYears(-1));
                    #endregion
                }
            }
        }

        private class User
        {
            public string Name { get; set; }
        }
    }
}
