using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.CompareExchange
{
    class CompareExchange<T>
    {
        public class DNS
        {
            public string IpAddress;
        }

        public async Task Async()
        {
            using (var store = new DocumentStore())
            {
                #region open_cluster_session_async
                using (var session = store.OpenAsyncSession())
                #endregion
                {
                    #region new_compare_exchange_async
                    // The session must be first opened with cluster-wide mode
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue(
                        key: "Best NoSQL Transactional Database",
                        value: "RavenDB"
                    );
                    
                    await session.SaveChangesAsync();
                    #endregion

                    //#region update_compare_exchange_async
                    //// load the existing dns record of ravendb.net
                    //CompareExchangeValue<DNS> result = await session.Advanced.ClusterTransaction.GetCompareExchangeValueAsync<DNS>(key: "ravendb.net");

                    //// change the ip
                    //result.Value.IpAddress = "52.32.173.150";
                    //session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(result);
                    
                    //// save the changes
                    //await session.SaveChangesAsync();
                    //#endregion

                    var key = "key";
                    var keys = new[] {"key"};
                    var index = 0L;
                    var value = default(T);
                    var item = await session.Advanced.ClusterTransaction.GetCompareExchangeValueAsync<T>(key);

                    #region methods_async_1
                    await session.Advanced.ClusterTransaction.GetCompareExchangeValueAsync<T>(key);
                    #endregion

                    #region methods_async_2
                    await session.Advanced.ClusterTransaction.GetCompareExchangeValuesAsync<T>(keys);
                    #endregion

                    #region methods_async_3
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue<T>(key, value);
                    #endregion

                    #region methods_async_4
                    // Delete by key & index
                    session.Advanced.ClusterTransaction.DeleteCompareExchangeValue(key, index);
                    
                    // Delete by compare-exchange item
                    session.Advanced.ClusterTransaction.DeleteCompareExchangeValue<T>(item);
                    #endregion

                    //#region methods_async_5
                    //session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(new CompareExchangeValue<T>(key, index, value));
                    //#endregion

                    #region methods_async_lazy_1
                    // Single value
                    session.Advanced.ClusterTransaction.Lazily.GetCompareExchangeValueAsync<T>(key);

                    // Multiple values
                    session.Advanced.ClusterTransaction.Lazily.GetCompareExchangeValuesAsync<T>(keys);
                    #endregion
                }
            }
        }

        public void Sync()
        {
            using (var store = new DocumentStore())
            {

                #region open_cluster_session_sync
                using (var session = store.OpenSession())
                #endregion
                {
                    #region new_compare_exchange_sync
                    // The session must be first opened with cluster-wide mode
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue(
                        key: "Best NoSQL Transactional Database",
                        value: "RavenDB"
                    );
                    
                    session.SaveChanges();
                    #endregion

                    //#region update_compare_exchange_sync
                    //// load the existing dns record of ravendb.net
                    //CompareExchangeValue<DNS> result = session.Advanced.ClusterTransaction.GetCompareExchangeValue<DNS>(key: "ravendb.net");

                    //// change the ip
                    //result.Value.IpAddress = "52.32.173.150";
                    //session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(result);

                    //// save the changes
                    //session.SaveChanges();
                    //#endregion


                    var key = "key";
                    var keys = new[] { "key" };
                    var index = 0L;
                    var value = default(T);
                    var item = session.Advanced.ClusterTransaction.GetCompareExchangeValue<T>(key);
                    
                    #region methods_1_sync
                    session.Advanced.ClusterTransaction.GetCompareExchangeValue<T>(key);
                    #endregion

                    #region methods_2_sync
                    session.Advanced.ClusterTransaction.GetCompareExchangeValues<T>(keys);
                    #endregion

                    #region methods_3_sync
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue<T>(key, value);
                    #endregion

                    #region methods_4_sync
                    // Delete by key & index
                    session.Advanced.ClusterTransaction.DeleteCompareExchangeValue(key, index);
                    
                    // Delete by compare-exchange item
                    session.Advanced.ClusterTransaction.DeleteCompareExchangeValue<T>(item);
                    #endregion

                    //#region methods_5_sync
                    //session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(new CompareExchangeValue<T>(key, index, value));
                    //#endregion

                    #region methods_sync_lazy_1
                    // Single value
                    session.Advanced.ClusterTransaction.Lazily.GetCompareExchangeValue<T>(key);
                    
                    // Multiple values
                    session.Advanced.ClusterTransaction.Lazily.GetCompareExchangeValues<T>(keys);
                    #endregion
                }
            }
        }
    }
}
