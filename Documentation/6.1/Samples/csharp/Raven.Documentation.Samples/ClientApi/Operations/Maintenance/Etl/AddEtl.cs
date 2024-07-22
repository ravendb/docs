using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Etl
{
    public class AddEtl
    {
        public AddEtl()
        {
            using (var store = new DocumentStore())
            {
                #region add_raven_etl
                // Define the RavenDB ETL task configuration object
                // ================================================
                var ravenEtlConfig = new RavenEtlConfiguration
                {
                    Name = "task-name",
                    ConnectionStringName = "raven-connection-string-name",
                    Transforms =
                    {
                        new Transformation
                        {
                            // The script name
                            Name = "script-name",
                            
                            // RavenDB collections the script uses
                            Collections = { "Employees" },
                            
                            // The transformation script
                            Script = @"loadToEmployees ({
                                       Name: this.FirstName + ' ' + this.LastName,
                                       Title: this.Title
                                     });"
                        }
                    },

                    // Do not prevent task failover to another node (optional) 
                    PinToMentorNode = false
                };
                
                // Define the AddEtlOperation
                // ==========================
                var operation = new AddEtlOperation<RavenConnectionString>(ravenEtlConfig);
                
                // Execute the operation by passing it to Maintenance.Send
                // =======================================================
                AddEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_sql_etl
                // Define the SQL ETL task configuration object
                // ============================================
                var sqlEtlConfig = new SqlEtlConfiguration
                {
                    Name = "task-name",
                    ConnectionStringName = "sql-connection-string-name",
                    SqlTables =
                    {
                        new SqlEtlTable {TableName = "Orders", DocumentIdColumn = "Id", InsertOnlyMode = false},
                        new SqlEtlTable {TableName = "OrderLines", DocumentIdColumn = "OrderId", InsertOnlyMode = false},
                    },
                    Transforms =
                    {
                        new Transformation
                        {
                            Name = "script-name",
                            Collections = { "Orders" },
                            Script = @"var orderData = {
                                                Id: id(this),
                                                OrderLinesCount: this.Lines.length,
                                                TotalCost: 0
                                            };

                                            for (var i = 0; i < this.Lines.length; i++) {
                                                var line = this.Lines[i];
                                                orderData.TotalCost += line.PricePerUnit;
                                                
                                                // Load to SQL table 'OrderLines'
                                                loadToOrderLines({
                                                    OrderId: id(this),
                                                    Qty: line.Quantity,
                                                    Product: line.Product,
                                                    Cost: line.PricePerUnit
                                                });
                                            }
                                            orderData.TotalCost = Math.round(orderData.TotalCost  * 100) / 100;

                                            // Load to SQL table 'Orders'
                                            loadToOrders(orderData)"
                        }
                    },

                    // Do not prevent task failover to another node (optional) 
                    PinToMentorNode = false
                };
                
                // Define the AddEtlOperation
                // ===========================
                var operation = new AddEtlOperation<SqlConnectionString>(sqlEtlConfig);
                
                // Execute the operation by passing it to Maintenance.Send
                // =======================================================
                AddEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region add_olap_etl
                // Define the OLAP ETL task configuration object
                // =============================================
                var olapEtlConfig = new OlapEtlConfiguration
                {
                    Name = "task-name",
                    ConnectionStringName = "olap-connection-string-name",
                    Transforms =
                    {
                        new Transformation
                        {
                            Name = "script-name",
                            Collections = {"Orders"},
                            Script = @"var orderDate = new Date(this.OrderedAt);
                                           var year = orderDate.getFullYear();
                                           var month = orderDate.getMonth();
                                           var key = new Date(year, month);
                                           loadToOrders(key, {
                                               Company : this.Company,
                                               ShipVia : this.ShipVia
                                           })"
                        }
                    }
                };
                
                // Define the AddEtlOperation
                // ==========================
                var operation = new AddEtlOperation<OlapConnectionString>(olapEtlConfig);
                
                // Execute the operation by passing it to Maintenance.Send
                // =======================================================
                AddEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_elasticsearch_etl
                // Define the Elasticsearch ETL task configuration object
                // ======================================================
                var elasticsearchEtlConfig = new ElasticSearchEtlConfiguration
                {
                    Name = "task-name",
                    ConnectionStringName = "elasticsearch-connection-string-name",
                    ElasticIndexes =
                    {
                        // Define Elasticsearch Indexes
                        new ElasticSearchIndex
                        {
                            // Elasticsearch Index name
                            IndexName = "orders",
                            // The Elasticsearch document property that will contain the source RavenDB document id.
                            // Make sure this property is also defined inside the transform script.
                            DocumentIdProperty = "DocId",
                            InsertOnlyMode = false
                        },
                        new ElasticSearchIndex
                        {
                            IndexName = "lines",
                            DocumentIdProperty = "OrderLinesCount",
                            // If true, don't send _delete_by_query before appending docs
                            InsertOnlyMode = true
                        }
                    },
                    Transforms =
                    {
                        new Transformation()
                        {
                            Collections = { "Orders" },
                            Script = @"var orderData = {
                                       DocId: id(this),
                                       OrderLinesCount: this.Lines.length,
                                       TotalCost: 0
                                       };

                                       // Write the `orderData` as a document to the Elasticsearch 'orders' index
                                       loadToOrders(orderData);",

                            Name = "script-name"
                        }
                    }
                };
                
                // Define the AddEtlOperation
                // ==========================
                var operation = new AddEtlOperation<ElasticSearchConnectionString>(elasticsearchEtlConfig);

                // Execute the operation by passing it to Maintenance.Send
                // =======================================================
                store.Maintenance.Send(operation);
                #endregion
            }
        }
        
        private interface IFoo
        {
            /*
            #region add_etl_operation
            public AddEtlOperation(EtlConfiguration<T> configuration)
            #endregion
            */
        }
    }
}

