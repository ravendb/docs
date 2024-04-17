using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetCountersFor
    {
        public GetCountersFor()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region example

                    // Load a document
                    var employee = session.Load<Employee>("employees/1-A");
                    
                    // Get counter names from the loaded entity
                    List<string> counterNames = session.Advanced.GetCountersFor(employee);

                    #endregion
                }
            }
        }

        private interface IFoo
        {
            #region syntax

            List<string> GetCountersFor<T>(T instance);

            #endregion
        }
    }
}
