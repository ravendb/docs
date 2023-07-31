using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class CountQueryResultsUsingLinq
    {
        public async Task CountUsingLinq()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region count_1
                    // using System.Linq;
                    // ==================
                    
                    int numberOfOrders = session
                        .Query<Order>()
                        .Where(order => order.ShipTo.Country == "UK")
                         // Calling 'Count' from System.Linq
                        .Count();
                    
                    // The query returns the NUMBER of orders shipped to UK (Int32)
                    #endregion
                }
            }
        }
    }
}
