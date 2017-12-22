using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Basics
    {
        public Basics()
        {
            using (var store = new DocumentStore())
            {
                #region basics_1_2
                store.Conventions.ThrowIfQueryPageSizeIsNotSet = true;
                #endregion

                using (var session = store.OpenSession())
                {
                    #region basics_1_0
                    List<Employee> employees = session
                        .Query<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_1_1
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Take(128)
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
