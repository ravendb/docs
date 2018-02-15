using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Revisions
{
    public class Loading
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
            T Get<T>(string changeVector);

            Dictionary<string, T> Get<T>(IEnumerable<string> changeVectors);
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
                    #region example_3_sync
                    Order orderRevision =
                        session
                            .Advanced
                            .Revisions
                            .Get<Order>(orderRevisionChangeVector);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_3_async
                    Order orderRevision =
                        await asyncSession
                            .Advanced
                            .Revisions
                            .GetAsync<Order>(orderRevisionChangeVector);
                    #endregion
                }
            }
        }
    }
}
