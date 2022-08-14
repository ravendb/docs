using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using Raven.Documentation.Samples.Indexes.Querying;
using System.Threading.Tasks;
using static Raven.Documentation.Samples.ClientApi.Session.Configuration.PerSessionTopology;

namespace Raven.Documentation.Samples.ClientApi.DocumentIdentifiers
{
    class HiloAlgorithm
    {
        public HiloAlgorithm()
        {
            #region return_hilo_1
            var store = new DocumentStore();

            using (var session = store.OpenSession())
            {
                // Store an entity will give us the hilo range (ex. 1-32)
                session.Store(new Employee 
                {
                    FirstName = "John",
                    LastName = "Doe"
                });

                session.SaveChanges();
            }
            // Release the range when it is no longer relevant
            store.Dispose(); 

            #endregion

            #region return_hilo_2
            var newStore = new DocumentStore();
            using (var session = newStore.OpenSession())
            {
                // Store an entity after disposing the last store will give us  (ex. 2-33) 
                session.Store(new Employee
                {
                    FirstName = "John",
                    LastName = "Doe"
                });

                session.SaveChanges();
            }
            #endregion

        }
    }
}
