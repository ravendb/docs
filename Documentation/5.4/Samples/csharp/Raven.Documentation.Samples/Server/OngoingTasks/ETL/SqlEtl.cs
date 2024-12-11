using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL
{
    public class SqlEtl
    {
        public SqlEtl()
        {
            using (var store = new DocumentStore())
            {
                #region add_sql_etl_connection_string 
                // Define a connection string to a SQL database destination
                // ========================================================
                var sqlConStr = new SqlConnectionString
                {
                    Name = "sql-connection-string-name",

                    // Define destination factory name
                    FactoryName = "MySql.Data.MySqlClient",

                    // Define the destination database
                    // May also need to define authentication and encryption parameters
                    // By default, encrypted databases are sent over encrypted channels
                    ConnectionString = "host=127.0.0.1;user=root;database=Northwind"
                };
               
                // Deploy (send) the connection string to the server via the PutConnectionStringOperation
                // ======================================================================================
                var PutConnectionStringOp = new PutConnectionStringOperation<SqlConnectionString>(sqlConStr);
                PutConnectionStringResult connectionStringResult = store.Maintenance.Send(PutConnectionStringOp);
                #endregion
            }
        }

        // Add SQL ETL Task
        public void AddSqlEtlTask()
        {
            using (var store = new DocumentStore())
            {
                #region add_sql_etl_task
                // Define the SQL ETL task configuration
                // =====================================
                var sqlConfiguration = new SqlEtlConfiguration()
                {
                    Name = "mySqlEtlTaskName",
                    ConnectionStringName = "sql-connection-string-name",
                    
                    SqlTables =
                    {
                        new SqlEtlTable
                        {
                            TableName = "Orders", DocumentIdColumn = "Id", InsertOnlyMode = false
                        },
                        new SqlEtlTable
                        {
                            TableName = "OrderLines", DocumentIdColumn = "OrderId", InsertOnlyMode = false
                        },
                    },
                    
                    Transforms =
                    {
                        new Transformation()
                        {
                            Name = "scriptName",
                            Collections = { "Orders" },
                            
                            Script = @"
                                var orderData = {
                                    Id: id(this),
                                    OrderLinesCount: this.Lines.length,
                                    TotalCost: 0
                                };

                                for (var i = 0; i < this.Lines.length; i++) {
                                    var line = this.Lines[i];
                                    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                    orderData.TotalCost += cost;

                                    loadToOrderLines({
                                        OrderId: id(this),
                                        Qty: line.Quantity,
                                        Product: line.Product,
                                        Cost: line.PricePerUnit
                                    });
                                }

                                orderData.TotalCost = Math.round(orderData.TotalCost * 100) / 100;
                                loadToOrders(orderData);
                            ",
                            
                            ApplyToAllDocuments = false
                        }
                    }
                };

                // Deploy the SQL ETL task to the server
                // =====================================
                var addSqlEtlOperation = new AddEtlOperation<SqlConnectionString>(sqlConfiguration);
                store.Maintenance.Send(addSqlEtlOperation);
                #endregion
            }
        }
    }
}
