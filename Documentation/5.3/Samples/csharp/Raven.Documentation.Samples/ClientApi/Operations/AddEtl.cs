using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.SQL;


namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class AddEtl
    {


        private string connectionStringName;
        private string path;


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
                        },

                        // Do not prevent task failover to another node
                        PinToMentorNode = false

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
                        },

                        // Do not prevent task failover to another node
                        PinToMentorNode = false

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

            /*
                        using (var store = new DocumentStore())
                        {
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
                        }
            */

            /*
            #region olap_connection_string_config
        public class OlapConnectionString : ConnectionString
        {
            public string Name { get; set; }
            public LocalSettings LocalSettings { get; set; }
            public S3Settings S3Settings { get; set; }
            public AzureSettings AzureSettings { get; set; }
            public GlacierSettings GlacierSettings { get; set; }
            public GoogleCloudSettings GoogleCloudSettings { get; set; }
            public FtpSettings FtpSettings { get; set; }
            public override ConnectionStringType Type => ConnectionStringType.Olap;

        }
        #endregion
            */
            using (var store = new DocumentStore())

        {

                #region olap_Etl_Connection_String

                var myOlapConnectionString = new OlapConnectionString
                {
                    Name = connectionStringName,
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = path
                    }
                };

                var resultOlapString = store.Maintenance.Send
                    (new PutConnectionStringOperation<OlapConnectionString>(myOlapConnectionString));
        #endregion
        }
            using (var store = new DocumentStore())

            {
                #region olap_Etl_Connection_String

                var myOlapConnectionString = new OlapConnectionString
            {
                Name = connectionStringName,
                LocalSettings = new LocalSettings
                {
                    FolderPath = path
                }
            };

                var resultOlapString = store.Maintenance.Send
                    (new PutConnectionStringOperation<OlapConnectionString>(myOlapConnectionString));
        #endregion
        }

            using (var store = new DocumentStore())

            {
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

                var resultOlapString = store.Maintenance.Send(new PutConnectionStringOperation<OlapConnectionString>(myOlapConnectionString));

                #endregion
            }

    using (var store = new DocumentStore())
            {
                #region create-connection-string
                // Create a Connection String to Elasticsearch
                var elasticSearchConnectionString = new ElasticSearchConnectionString
                {
                    // Connection String Name
                    Name = "ElasticConStr", 
                    // Elasticsearch Nodes URLs
                    Nodes = new[] { "http://localhost:9200" }, 
                    // Authentication Method
                    Authentication = new Raven.Client.Documents.Operations.ETL.ElasticSearch.Authentication 
                    { 
                        Basic = new BasicAuthentication
                        {
                            Username = "John",
                            Password = "32n4j5kp8"
                        }
                    }
                };

                store.Maintenance.Send(new PutConnectionStringOperation<ElasticSearchConnectionString>(elasticSearchConnectionString));
                #endregion

                #region add_elasticsearch_etl
                // Create an Elasticsearch ETL task
                AddEtlOperation<ElasticSearchConnectionString> operation = new AddEtlOperation<ElasticSearchConnectionString>(
                new ElasticSearchEtlConfiguration()
                {
                    ConnectionStringName = elasticSearchConnectionString.Name, // Connection String name
                    Name = "ElasticsearchEtlTask", // ETL Task name
                        
                    ElasticIndexes =
                    {
                        // Define Elasticsearch Indexes
                        new ElasticSearchIndex { // Elasticsearch Index name
                                                 IndexName = "orders", 
                                                 // The Elasticsearch document property that will contain
                                                 // the source RavenDB document id.
                                                 // Make sure this property is also defined inside the
                                                 // transform script.
                                                 DocumentIdProperty = "DocId", 
                                                 InsertOnlyMode = false }, 
                        new ElasticSearchIndex { IndexName = "lines",
                                                 DocumentIdProperty = "OrderLinesCount", 
                                                 // If true, don't send _delete_by_query before appending docs
                                                 InsertOnlyMode = true 
                                               }
                    },
                    Transforms =
                    {   // Transformation script configuration
                        new Transformation()
                        {
                            // RavenDB collections that the script uses
                            Collections = { "Orders" }, 

                            Script = @"var orderData = {
                                       DocId: id(this),
                                       OrderLinesCount: this.Lines.length,
                                       TotalCost: 0
                                       };

                                       // Write the `orderData` as a document to the Elasticsearch 'orders' index
                                       loadToOrders(orderData);", 
                            
                            // Transformation script Name
                            Name = "TransformIDsAndLinesCount" 
                        }
                    }
                });

                store.Maintenance.Send(operation);
                #endregion
            }
        }



    }
}

