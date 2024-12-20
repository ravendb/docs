using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class Creating
    {
        #region indexes_1
        // Define a static-index
        // Inherit from 'AbstractIndexCreationTask'
        public class Orders_ByTotal : AbstractIndexCreationTask<Order>
        {
            // ...
        }
        #endregion

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region indexes_3
                // Call 'Execute' directly on the index instance
                new Orders_ByTotal().Execute(store);
                #endregion
                
                #region indexes_4
                // Call 'ExecuteAsync' directly on the index instance
                await new Orders_ByTotal().ExecuteAsync(store);
                #endregion

                #region indexes_5
                // Call 'ExecuteIndex' on your store object
                store.ExecuteIndex(new Orders_ByTotal());
                #endregion
                
                #region indexes_6
                // Call 'ExecuteIndexAsync' on your store object
                await store.ExecuteIndexAsync(new Orders_ByTotal());
                #endregion

                #region indexes_7
                var indexesToDeploy = new List<AbstractIndexCreationTask>
                {
                    new Orders_ByTotal(),
                    new Employees_ByLastName()
                };
                
                // Call 'ExecuteIndexes' on your store object
                store.ExecuteIndexes(indexesToDeploy);
                #endregion
                
                /*
                // The below is in a comment because I still want to show 'var indexesToDeploy' for each region in the UI.
                #region indexes_8
                var indexesToDeploy = new List<AbstractIndexCreationTask>
                {
                    new Orders_ByTotal(),
                    new Employees_ByLastName()
                };
                
                // Call 'ExecuteIndexesAsync' on your store object
                await store.ExecuteIndexesAsync(indexesToDeploy);
                #endregion
                
                #region indexes_9
                var indexesToDeploy = new List<AbstractIndexCreationTask>
                {
                    new Orders_ByTotal(),
                    new Employees_ByLastName()
                };
                
                // Call the static method 'CreateIndexes' on the IndexCreation class
                IndexCreation.CreateIndexes(indexesToDeploy, store);
                #endregion
                
                #region indexes_10
                var indexesToDeploy = new List<AbstractIndexCreationTask>
                {
                    new Orders_ByTotal(),
                    new Employees_ByLastName()
                };
                
                // Call the static method 'CreateIndexesAsync' on the IndexCreation class
                await IndexCreation.CreateIndexesAsync(indexesToDeploy, store);
                #endregion
                */
                
                #region indexes_11
                // Deploy ALL indexes from the assembly containing the `Orders_ByTotal` class
                IndexCreation.CreateIndexes(typeof(Orders_ByTotal).Assembly, store);
                #endregion
                
                #region indexes_12
                // Deploy ALL indexes from the assembly containing the `Orders_ByTotal` class
                await IndexCreation.CreateIndexesAsync(typeof(Orders_ByTotal).Assembly, store);
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexes_14
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert" && x.LastName == "King")
                        .ToList();
                    #endregion
                }
            }
        }
    }

    public class CreatingWithCustomConfiguration
    {
        #region indexes_2
        public class Orders_ByTotal : AbstractIndexCreationTask<Order>
        {
            public Orders_ByTotal()
            {
                // ...
                // Set an indexing configuration value for this index:
                Configuration["Indexing.MapTimeoutInSec"] = "30";
            }
        }
        #endregion
    }
    
    class IndexExample
    {
        #region indexes_13
        // Define a static-index:
        // ======================
        public class Orders_ByTotal : AbstractIndexCreationTask<Order>
        {
            public class IndexEntry
            {
                // The index-fields:
                public string Employee { get; set; }
                public string Company { get; set; }
                public decimal Total { get; set; }
            }
            
            public Orders_ByTotal()
            {
                Map = orders => from order in orders
                                select new IndexEntry
                                {
                                    // Set the index-fields:
                                    Employee = order.Employee,
                                    Company = order.Company,
                                    Total = order.Lines.Sum(l => 
                                        (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                                };
                
                // Customize the index as needed, for example:
                DeploymentMode = IndexDeploymentMode.Rolling;
                Configuration["Indexing.MapTimeoutInSec"] = "30";
                Indexes.Add(x => x.Company, FieldIndexing.Search);
                // ...
            }
        }
        
        public static void Main(string[] args)
        {
            using (DocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Northwind"
            })
            {
                store.Initialize();
                
                // Deploy the index:
                // =================
                new Orders_ByTotal().Execute(store);
                
                using (IDocumentSession session = store.OpenSession())
                {
                    // Query the index:
                    // ================
                    IList<Order> orders = session
                        .Query<Orders_ByTotal.IndexEntry, Orders_ByTotal>()
                         // Query for Order documents that have Total > 100
                        .Where(x => x.Total > 100)
                        .OfType<Order>()
                        .ToList();
                }
            }
        }
        #endregion
    }

    // This index is used in the multiple indexes deployment overloads above
    public class Employees_ByLastName : AbstractIndexCreationTask<Employee>
    {
        public class IndexEntry
        {
            public string LastName { get; set; }
        }
            
        public Employees_ByLastName()
        {
            Map = employees => from emp in employees
                select new IndexEntry
                {
                    LastName = emp.LastName
                };
        }
    }
    
    public interface IFoo
    {
        #region syntax_1
        // Call this method directly on the index instance
        void Execute(IDocumentStore store, DocumentConventions conventions = null, 
            string database = null);

        // Call these methods on the store object
        void ExecuteIndex(IAbstractIndexCreationTask index, string database = null);
        void ExecuteIndexes(IEnumerable<IAbstractIndexCreationTask> indexes,
            string database = null);

        // Call these static methods on the IndexCreation class
        void CreateIndexes(IEnumerable<IAbstractIndexCreationTask> indexes, IDocumentStore store,
            DocumentConventions conventions = null, string database = null);
        void CreateIndexes(Assembly assemblyToScan, IDocumentStore store,
            DocumentConventions conventions = null, string database = null);
        #endregion
        
        #region syntax_2
        // Call this method directly on the index instance
        Task ExecuteAsync(IDocumentStore store, DocumentConventions conventions = null,
            string database = null, CancellationToken token = default);

        // Call these methods on the store object
        Task ExecuteIndexAsync(IAbstractIndexCreationTask index, string database = null,
            CancellationToken token = default(CancellationToken));
        Task ExecuteIndexesAsync(IEnumerable<IAbstractIndexCreationTask> indexes,
            string database = null, CancellationToken token = default(CancellationToken));

        // Call these static methods on the IndexCreation class
        Task CreateIndexesAsync(IEnumerable<IAbstractIndexCreationTask> indexes,
            IDocumentStore store, DocumentConventions conventions = null, string database = null,
            CancellationToken token = default(CancellationToken));
        Task CreateIndexesAsync(Assembly assemblyToScan, IDocumentStore store,
            DocumentConventions conventions = null, string database = null,
            CancellationToken token = default(CancellationToken));
        #endregion
    }
}
