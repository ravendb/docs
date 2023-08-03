using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class CountQueryResults
    {
        public async Task CanUseCount()
        {
            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region count_2
                    // using Raven.Client.Documents;
                    // using Raven.Client.Documents.Linq;
                    // ==================================
                    
                    int numberOfOrders = await asyncSession
                        .Query<Order>()
                         // Calling 'Where' from Raven.Client.Documents.Linq
                        .Where(order => order.ShipTo.Country == "UK")
                         // Calling 'CountAsync' from Raven.Client.Documents
                        .CountAsync();
                    
                    // The query returns the NUMBER of orders shipped to UK (Int32)
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region count_3
                    // using Raven.Client.Documents.Session;
                    // =====================================
                    
                    int numberOfOrders = session.Advanced
                        .DocumentQuery<Order>()
                        .WhereEquals(order => order.ShipTo.Country, "UK")
                         // Calling 'Count' from Raven.Client.Documents.Session
                        .Count();

                    // The query returns the NUMBER of orders shipped to UK (Int32)
                    #endregion
                }
            }
        }

        public async Task CanUseLongCount()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region count_4
                    // using Raven.Client.Documents;
                    // using Raven.Client.Documents.Linq;
                    // ==================================
                    
                    long numberOfOrders = session
                        .Query<Order>()
                         // Calling 'Where' from Raven.Client.Documents.Linq
                        .Where(order => order.ShipTo.Country == "UK")
                         // Calling 'LongCount' from Raven.Client.Documents
                        .LongCount();
                    
                    // The query returns the NUMBER of orders shipped to UK (Int64)
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region count_5
                    // using Raven.Client.Documents;
                    // using Raven.Client.Documents.Linq;
                    // ==================================
                    
                    long numberOfOrders = await asyncSession
                        .Query<Order>()
                         // Calling 'Where' from Raven.Client.Documents.Linq
                        .Where(order => order.ShipTo.Country == "UK")
                         // Calling 'LongCountAsync' from Raven.Client.Documents
                        .LongCountAsync();
                    
                    // The query returns the NUMBER of orders shipped to UK (Int64)
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region count_6
                    // using Raven.Client.Documents.Session;
                    // =====================================
                    
                    long numberOfOrders = session.Advanced
                        .DocumentQuery<Order>()
                        .WhereEquals(order => order.ShipTo.Country, "UK")
                         // Calling 'LongCount' from  Raven.Client.Documents.Session
                        .LongCount();
                    
                    // The query returns the NUMBER of orders shipped to UK (Int64)
                    #endregion
                }
            }
        }
    }
}
