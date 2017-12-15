using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetChangeVector
    {
        private interface IFoo
        {
            #region get_change_vector_1
            string GetChangeVectorFor<T>(T instance);
            #endregion
        }

        public GetChangeVector()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region get_change_vector_2
                    Employee employee = session.Load<Employee>("employees/1-A");
                    string changeVector = session.Advanced.GetChangeVectorFor(employee);
                    #endregion
                }
            }
        }
    }
}
