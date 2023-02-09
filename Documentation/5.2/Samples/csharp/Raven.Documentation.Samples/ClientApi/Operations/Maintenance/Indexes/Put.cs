using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class Put
    {
        private interface IFoo
        {
            /*
            #region syntax
            public PutIndexesOperation(params IndexDefinition[] indexesToAdd)
            #endregion
            */
        }

        public Put()
        {
            using (var store = new DocumentStore())
            {
                #region put_1
                // Create an index definition
                var indexDefinition = new IndexDefinition 
                {
                    // Name is mandatory, can use any string
                    Name = "OrdersByTotal",

                    // Define the index Map functions, string format
                    // A single string for a map-index, multiple strings for a multi-map-index
                    Maps = new HashSet<string>
                    {
                        @"
                          // Define the collection that will be indexed:
                          from order in docs.Orders

                            // Define the index-entry:
                            select new 
                            {
                                // Define the index-fields within each index-entry:
                                Employee = order.Employee,
                                Company = order.Company,
                                Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                            }"
                    },
                    
                    // Reduce = ...,
                    
                    // Can provide other index definitions available on the IndexDefinition class
                    // Override the default values, e.g.:
                    DeploymentMode = IndexDeploymentMode.Rolling,
                    Priority = IndexPriority.High,
                    Configuration = new IndexConfiguration
                    {
                        { "Indexing.IndexMissingFieldsAsNull", "true" }
                    }
                    // See all available properties in syntax below
                };

                // Define the put indexes operation, pass the index definition
                // Note: multiple index definitions can be passed, see syntax below
                IMaintenanceOperation<PutIndexResult[]> putIndexesOp = new PutIndexesOperation(indexDefinition);

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(putIndexesOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region put_1_JS
                // Create an index definition
                var indexDefinition = new IndexDefinition
                {
                    // Name is mandatory, can use any string
                    Name = "OrdersByTotal",
                    
                    // Define the index Map functions, string format
                    // A single string for a map-index, multiple strings for a multi-map-index
                    Maps = new HashSet<string>
                    {
                        @"map('Orders', function(order) {
                              return {
                                  Employee: order.Employee,
                                  Company: order.Company,
                                  Total: order.Lines.reduce(function(sum, l) {
                                      return sum + (l.Quantity * l.PricePerUnit) * (1 - l.Discount);
                                  }, 0)
                              };
                        });"
                    },
                    
                    // Reduce = ...,
                   
                    // Can provide other index definitions available on the IndexDefinition class
                    // Override the default values, e.g.:
                    DeploymentMode = IndexDeploymentMode.Rolling,
                    Priority = IndexPriority.High,
                    Configuration = new IndexConfiguration
                    {
                        { "Indexing.IndexMissingFieldsAsNull", "true" }
                    }
                    // See all available properties in syntax below
                };

                // Define the put indexes operation, pass the index definition
                // Note: multiple index definitions can be passed, see syntax below
                IMaintenanceOperation<PutIndexResult[]> putIndexesOp = new PutIndexesOperation(indexDefinition);

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(putIndexesOp);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region put_2
                // Create an index definition builder
                var builder = new IndexDefinitionBuilder<Order>
                {
                    // Define the map function, strongly typed LINQ format
                    Map =
                        // Define the collection that will be indexed:
                        orders => from order in orders
                            // Define the index-entry:
                            select new
                            {
                                // Define the index-fields within each index-entry:
                                Employee = order.Employee,
                                Company = order.Company,
                                Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                            },
                    
                    // Can provide other properties available on the IndexDefinitionBuilder class, e.g.:
                    DeploymentMode = IndexDeploymentMode.Rolling,
                    Priority = IndexPriority.High,
                    // Reduce = ..., etc.
                };
                
                // Generate index definition from builder
                // Pass the conventions, needed for building the Maps property
                var indexDefinition = builder.ToIndexDefinition(store.Conventions);
                
                // Optionally, set the index name, can use any string
                // If not provided then default name from builder is used, e.g.: "IndexDefinitionBuildersOfOrders"
                indexDefinition.Name = "OrdersByTotal";
                
                // Define the put indexes operation, pass the index definition
                // Note: multiple index definitions can be passed, see syntax below
                IMaintenanceOperation<PutIndexResult[]> putIndexesOp = new PutIndexesOperation(indexDefinition);

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(putIndexesOp);
                #endregion
            }
        }

        public async Task PutAsync()
        {
            using (var store = new DocumentStore())
            {
                #region put_1_async
                // Create an index definition
                var indexDefinition = new IndexDefinition
                {
                    // Name is mandatory, can use any string
                    Name = "OrdersByTotal",
                    
                    // Define the index Map functions, string format
                    // A single string for a map-index, multiple strings for a multi-map-index
                    Maps = new HashSet<string>
                    {
                        @"
                          // Define the collection that will be indexed:
                          from order in docs.Orders

                            // Define the index-entry:
                            select new 
                            {
                                // Define the index-fields within each index-entry:
                                Employee = order.Employee,
                                Company = order.Company,
                                Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                            }"
                    },

                    // Reduce = ...,
                   
                    // Can provide other index definitions available on the IndexDefinition class
                    // Override the default values, e.g.:
                    DeploymentMode = IndexDeploymentMode.Rolling,
                    Priority = IndexPriority.High,
                    Configuration = new IndexConfiguration
                    {
                        { "Indexing.IndexMissingFieldsAsNull", "true" }
                    }
                    // See all available properties in syntax below
                };

                // Define the put indexes operation, pass the index definition
                // Note: multiple index definitions can be passed, see syntax below
                IMaintenanceOperation<PutIndexResult[]> putIndexesOp = new PutIndexesOperation(indexDefinition);

                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(putIndexesOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region put_2_async
                // Create an index definition builder
                var builder = new IndexDefinitionBuilder<Order>
                {
                    // Define the map function, strongly typed LINQ format
                    Map = 
                        // Define the collection that will be indexed:
                        orders => from order in orders
                            // Define the index-entry:
                            select new
                            {
                                // Define the index-fields within each index-entry:
                                Employee = order.Employee,
                                Company = order.Company,
                                Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                            },
                    
                    // Can provide other properties available on the IndexDefinitionBuilder class, e.g.:
                    DeploymentMode = IndexDeploymentMode.Rolling,
                    Priority = IndexPriority.High,
                    // Reduce = ..., etc.
                };
                
                // Generate index definition from builder
                // Pass the conventions, needed for building the Maps property
                var indexDefinition = builder.ToIndexDefinition(store.Conventions);
                
                // Optionally, set the index name, can use any string
                // If not provided then default name from builder is used, e.g.: "IndexDefinitionBuildersOfOrders"
                indexDefinition.Name = "OrdersByTotal";
                
                // Define the put indexes operation, pass the index definition
                // Note: multiple index definitions can be passed, see syntax below
                IMaintenanceOperation<PutIndexResult[]> putIndexesOp = new PutIndexesOperation(indexDefinition);

                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(putIndexesOp);
                #endregion
            }
        }
    }
}
