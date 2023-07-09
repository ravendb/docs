using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using static NodaTime.TimeZones.ZoneEqualityComparer;
using Xunit;
using System.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class UseCount
    {
        public void CanUseCount(Options options)
        {
            using (var store = new DocumentStore())
            {
                using (var s = store.OpenSession())
                {
                    s.Store(new User
                    {
                        Name = "John"
                    });

                    s.SaveChanges();
                }

                using (var s = store.OpenSession())
                {
                    QueryStatistics stats;

                    #region Count
                    // Use Count in a synchronous session
                    System.Int32 Count = 
                        s.Query<User>()
                            .Statistics(out stats)
                            .Search(u => u.Name, "John")
                            .Count();
                    #endregion
                }
            }
        }
    }
}
