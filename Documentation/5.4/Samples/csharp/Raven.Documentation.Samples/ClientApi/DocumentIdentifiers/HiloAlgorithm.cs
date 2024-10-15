using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using System.Threading.Tasks;

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

        public async Task Generate_HiLo_Ids()
        {
            var store = new DocumentStore();

            #region manual_hilo_sample
            using (var session = store.OpenSession())
            {
                // Using overload - GenerateNextIdFor(string database, string collectionName);
                var nextId = await store.HiLoIdGenerator.GenerateNextIdForAsync(null, "Products");

                // Using overload - GenerateNextIdFor(string database, object entity);
                nextId = await store.HiLoIdGenerator.GenerateNextIdForAsync(null, new Product());

                // Using overload - GenerateNextIdFor(string database, Type type);
                nextId = await store.HiLoIdGenerator.GenerateNextIdForAsync(null, typeof(Product));

                // Now you can create a new document with the ID received.
                var product = new Product
                {
                    Id = "myCustomId/" + nextId.ToString()
                };
                
                // Store the new document.
                session.Store(product);
                session.SaveChanges();
            }
            #endregion

            /*
             #region manual_HiLo_Signatures
        Task<long> GenerateNextIdForAsync(string database, object entity);

        Task<long> GenerateNextIdForAsync(string database, Type type);

        Task<long> GenerateNextIdForAsync(string database, string collectionName);
            #endregion

             */
        }
    }
}
