using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class AddEtl
    {
        private string myServerAddress;
        private object myDataBase;
        private object myUsername;
        private object myPassword;

        public object Database { get; }
        public object UserId { get; }
        public object Password { get; }

        private interface IFoo
        {
            /*
            #region add_etl_operation
            public AddEtlOperation(EtlConfiguration<T> configuration)
            #endregion
            */
        }

        public AddEtl()
        {
            using (var store = new DocumentStore())
            {
                #region add_raven_etl

                AddEtlOperation<RavenConnectionString> operation = new AddEtlOperation<RavenConnectionString>(
                    new RavenEtlConfiguration
                    {
                        ConnectionStringName = "raven-connection-string-name",
                        Name = "Employees ETL",
                        Transforms =
                        {
                            new Transformation
                            {
                                Name = "Script #1",
                                Collections =
                                {
                                    "Employees"
                                },
                                Script = @"loadToEmployees ({
                                        Name: this.FirstName + ' ' + this.LastName,
                                        Title: this.Title
                                });"
                            }
                        }
                    });

                AddEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
            }
            using (var store = new DocumentStore())

            #region raven_etl_connection_string

            {
                //define connection string
                var ravenConnectionString = new RavenConnectionString()
                {
                    //name connection string
                    Name = "raven-connection-string-name",

                    //define appropriate node
                    //Be sure that the node definition in the connection string has the "s" in https
                    TopologyDiscoveryUrls = new[] { "https://127.0.0.1:8080" },

                    //define database to connect with on the node
                    Database = "Northwind",
                };
                //create the connection string
                var resultRavenString = store.Maintenance.Send(
                    new PutConnectionStringOperation<RavenConnectionString>(ravenConnectionString));
            }
            #endregion

            {
                using (var store = new DocumentStore())
                {
                    #region add_sql_etl
                    AddEtlOperation<SqlConnectionString> operation = new AddEtlOperation<SqlConnectionString>(
                        new SqlEtlConfiguration
                        {
                            ConnectionStringName = "sql-connection-string-name",
                            FactoryName = "System.Data.SqlClient",
                            Name = "Orders to SQL",
                            SqlTables = {
                            new SqlEtlTable {TableName = "Orders", DocumentIdColumn = "Id", InsertOnlyMode = false},
                            new SqlEtlTable {TableName = "OrderLines", DocumentIdColumn = "OrderId", InsertOnlyMode = false},
                            },
                            Transforms =
                            {
                            new Transformation
                            {
                                Name = "Script #1",
                                Collections =
                                {
                                    "Orders"
                                },
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
                            }
                        });

                    AddEtlOperationResult result = store.Maintenance.Send(operation);
                    #endregion
                }

                using (var store = new DocumentStore())

                #region sql_etl_connection_string

                {
                    // define new connection string
                    PutConnectionStringOperation<SqlConnectionString> operation
                    = new PutConnectionStringOperation<SqlConnectionString>(
                        new SqlConnectionString
                        {
                        // name connection string
                        Name = "local_mysql",

                        // define FactoryName
                        FactoryName = "MySql.Data.MySqlClient",

                        // define database - may also need to define authentication and encryption parameters
                        // by default, encrypted databases are sent over encrypted channels
                        ConnectionString = "host=127.0.0.1;user=root;database=Northwind"

                        });

                    // create connection string
                    PutConnectionStringResult connectionStringResult
                    = store.Maintenance.Send(operation);

                }
                #endregion
            }
        }
    }


}
