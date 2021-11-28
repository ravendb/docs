using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.SQL;
//using Raven.Client.Documents.Operations.ETL.OLAP;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class AddEtl
    {
        private string myServerAddress;
        private object myDataBase;
        private object myUsername;
        private object myPassword;
        private object connectionStringName;
        private string path;
        private object configuration;

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

            #region raven_etl_connection_string

            using (var store = GetDocumentStore())
            {
                //define connection string
                var ravenConnectionString = new RavenConnectionString()
                {
                    //name connection string
                    Name = "raven-connection-string-name",

                    //define appropriate node
                    TopologyDiscoveryUrls = new[] { "http://127.0.0.1:8080" },

                    //define database to connect with on the node
                    Database = "Northwind",
                }));
                //send the connection string to connect
                var resultRavenString = store.Maintenance.Send(new PutConnectionStringOperation<RavenConnectionString>(ravenConnectionString));

                #endregion


                using (var store = new DocumentStore())
            {
                #region add_sql_etl
                AddEtlOperation<SqlConnectionString> operation = new AddEtlOperation<SqlConnectionString>(
                    new SqlEtlConfiguration
                    {
                        ConnectionStringName = "sql-connection-string-name",
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

            #region sql_etl_connection_string

            // define new connection string
            var sqlConnectionString = new SqlConnectionString
            {
                // name connection string
                Name = "SqlConnectionString",
                // enter the configurations to access your database
                ConnectionString = myServerAddress;
                Database = myDataBase;
                UserId = myUsername;
                Password = myPassword;
            };
                #endregion

            using (var store = new DocumentStore())
            {
                /*
                #region add_olap_etl
                AddEtlOperation<OlapConnectionString> operation = new AddEtlOperation<OlapConnectionString>(
                    new OlapEtlConfiguration
                    {
                        ConnectionStringName = "olap-connection-string-name",
                        Name = "Orders ETL",
                        Transforms =
                        {
                            new Transformation
                            {
                                Name = "Script #1",
                                Collections =
                                {
                                    "Orders"
                                },
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
                    });

                AddEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
                */

                #region olap_Etl_Connection_String

                var connectionString = new OlapConnectionString
                {
                    Name = connectionStringName,
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = path
                    }
                };
                AddEtl(store, configuration, connectionString);
                #endregion


                #region olap_Etl_AWS_connection_string

                var myOlapConnectionString = new OlapConnectionString
                {
                    Name = "myConnectionStringName",
                    S3Settings = new S3Settings
                    {
                        BucketName = "myBucket",
                        RemoteFolderName = "my/folder/name",
                        AwsAccessKey = "myAccessKey",
                        AwsSecretKey = "myPassword",
                        AwsRegionName = "us-east-1"
                    }
                };
            #endregion

            }



        }

        private void AddEtl(DocumentStore store, object configuration, OlapConnectionString connectionString)
        {
            throw new NotImplementedException();
        }
    }
    }
}

