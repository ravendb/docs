using System;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetLastModified
    {
        private interface IFoo
        {
            #region get_last_modified_1
            DateTime? GetLastModifiedFor<T>(T instance);
            #endregion
        }

        public GetLastModified()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region get_last_modified_2
                    Employee employee = session.Load<Employee>("employees/1-A");
                    DateTime? lastModified = session.Advanced.GetLastModifiedFor(employee);
                    #endregion
                }
            }
        }
    }
}
