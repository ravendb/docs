using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class StoringEntities
    {
        private interface IFoo
        {
            #region store_entities_1
            void Store(object entity);
            #endregion

            #region store_entities_2
            void Store(object entity, string id);
            #endregion

            #region store_entities_3
            void Store(object entity, string changeVector, string id);
            #endregion

        }

        public StoringEntities()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region store_entities_5
                    // generate Id automatically
                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    // send all pending operations to server, in this case only `Put` operation
                    session.SaveChanges();
                    #endregion
                }
            }
        }
    }
}
