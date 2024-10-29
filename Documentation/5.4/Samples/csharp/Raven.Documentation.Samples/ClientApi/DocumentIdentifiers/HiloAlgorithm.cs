using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.DocumentIdentifiers
{
    class HiloAlgorithm
    {
        public HiloAlgorithm()
        {
            #region hilo_1
            var store = new DocumentStore();

            using (var session = store.OpenSession())
            {
                // Storing the first entity causes the client to receive the initial HiLo range (1-32)
                session.Store(new Employee
                {
                    FirstName = "John",
                    LastName = "Doe"
                });

                session.SaveChanges(); 
                // The document ID will be: employees/1-A
            }
            
            // Release the range when it is no longer relevant
            store.Dispose();
            #endregion

            #region hilo_2
            var newStore = new DocumentStore();
            using (var session = newStore.OpenSession())
            {
                // Storing an entity after disposing the store in the previous example
                // causes the client to receive the next HiLo range (2-33)
                session.Store(new Employee
                {
                    FirstName = "Dave",
                    LastName = "Brown"
                });

                session.SaveChanges();
                // The document ID will be: employees/2-A
            }
            #endregion
        }

        public async Task Generate_HiLo_Ids()
        {
            var store = new DocumentStore();

            #region manual_hilo_sample
            using (var session = store.OpenSession())
            {
                // Use any overload to get the next id:
                // (Note how the id increases with each call)
                // ==========================================
                
                var nextId = await store.HiLoIdGenerator.GenerateNextIdForAsync(null, "Products");
                // nextId = 1
                
                nextId = await store.HiLoIdGenerator.GenerateNextIdForAsync(null, new Product());
                // nextId = 2
                
                nextId = await store.HiLoIdGenerator.GenerateNextIdForAsync(null, typeof(Product));
                // nextId = 3

                // Now you can create a new document with the nextId received
                // ==========================================================
                
                var product = new Product
                {
                    Id = "MyCustomId/" + nextId.ToString()
                };
                
                // Store the new document
                // The document ID will be: "MyCustomId/3"  
                session.Store(product);
                session.SaveChanges();
            }
            #endregion
            
            #region manual_hilo_getFullId
            using (var session = store.OpenSession())
            {
                var nextFullId = await store.HiLoIdGenerator.GenerateDocumentIdAsync(null, "Products");
                // nextFullId = "products/4-A"

                // You can now use the nextFullId and customize the document ID as you wish:
                var product = new Product
                {
                    Id = "MyCustomId/" + nextFullId
                };
                
                session.Store(product);
                session.SaveChanges();
            }
            #endregion

            /*
            #region manual_hilo_overloads
            Task<long> GenerateNextIdForAsync(string database, object entity);

            Task<long> GenerateNextIdForAsync(string database, Type type);

            Task<long> GenerateNextIdForAsync(string database, string collectionName);
            #endregion
            */
            
            /*
            #region manual_hilo_getFullId_syntax
            Task<string> GenerateDocumentIdAsync(string database, object entity);
            #endregion
            */
        }
    }
}
