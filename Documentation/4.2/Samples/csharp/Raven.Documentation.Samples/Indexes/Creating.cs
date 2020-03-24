using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    namespace Raven.Documentation.CodeSamples.Transformers.Foo
    {
        class Foo
        {
            #region indexes_8
            public class Orders_Totals : AbstractIndexCreationTask<Order>
            {
                public class Result
                {
                    public string Employee { get; set; }

                    public string Company { get; set; }

                    public decimal Total { get; set; }
                }

                public Orders_Totals()
                {
                    Map = orders => from order in orders
                                    select new Result
                                    {
                                        Employee = order.Employee,
                                        Company = order.Company,
                                        Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                                    };
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

                    new Orders_Totals().Execute(store);

                    using (IDocumentSession session = store.OpenSession())
                    {
                        IList<Order> orders = session
                            .Query<Orders_Totals.Result, Orders_Totals>()
                            .Where(x => x.Total > 100)
                            .OfType<Order>()
                            .ToList();
                    }
                }
            }
            #endregion
        }
    }

    public class Creating
    {
        #region indexes_1
        public class Orders_Totals : AbstractIndexCreationTask<Order>
        {
            // ...
        }
        #endregion

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region indexes_2
                // deploy index to database defined in `DocumentStore.Database` property
                // using default DocumentStore `Conventions`
                new Orders_Totals().Execute(store);

                // deploy asynchronously index to database defined in `DocumentStore.Database` property
                // using default DocumentStore `Conventions`
                await new Orders_Totals().ExecuteAsync(store, store.Conventions);
                #endregion

                #region indexes_3
                // deploy index to `Northwind` database
                // using default DocumentStore `Conventions`
                new Orders_Totals().Execute(store, store.Conventions, "Northwind");
                #endregion

                #region indexes_4
                // deploy all indexes 
                // from assembly where `Orders_Totals` is found
                // to database defined in `DocumentStore.Database` property
                IndexCreation.CreateIndexes(typeof(Orders_Totals).Assembly, store);
                #endregion

                #region indexes_5
                store
                    .Maintenance
                    .Send(new PutIndexesOperation(new IndexDefinition
                    {
                        Name = "Orders/Totals",
                        Maps =
                        {
                            @"from order in docs.Orders	
                              select new 
                              { 
                                  order.Employee, 
                                  order.Company,
                                  Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                              }"
                        }
                    }));
                #endregion

                #region indexes_6
                IndexDefinitionBuilder<Order> builder = new IndexDefinitionBuilder<Order>();
                builder.Map = orders => from order in orders
                                        select new
                                        {
                                            order.Employee,
                                            order.Company,
                                            Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                                        };

                store
                    .Maintenance
                    .Send(new PutIndexesOperation(builder.ToIndexDefinition(store.Conventions)));
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexes_7
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
        #region indexes_9
        public class Orders_Totals : AbstractIndexCreationTask<Order>
        {
            public Orders_Totals()
            {
                // ...
                Configuration["Indexing.MapTimeoutInSec"] = "30";
            }

        }
        #endregion
    }
}
