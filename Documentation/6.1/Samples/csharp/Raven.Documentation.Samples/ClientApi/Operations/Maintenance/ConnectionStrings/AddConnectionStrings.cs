using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.ConnectionStrings
{
    public class AddConnectionStrings
    {
        public AddConnectionStrings()
        {
            using (var store = new DocumentStore())
            {
                #region add_raven_connection_string
                // Define a connection string to a RavenDB database destination
                // ============================================================
                var ravenDBConStr = new RavenConnectionString
                {
                    Name = "ravendb-connection-string-name",
                    Database = "target-database-name",
                    TopologyDiscoveryUrls = new[] { "https://rvn2:8080" }
                };
                
                // Deploy (send) the connection string to the server via the PutConnectionStringOperation
                // ======================================================================================
                var PutConnectionStringOp = new PutConnectionStringOperation<RavenConnectionString>(ravenDBConStr);
                PutConnectionStringResult connectionStringResult = store.Maintenance.Send(PutConnectionStringOp);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_sql_connection_string
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

            using (var store = new DocumentStore())
            {
                #region add_olap_connection_string_1
                // Define a connection string to a local OLAP destination
                // ======================================================
                OlapConnectionString olapConStr = new OlapConnectionString
                {
                    Name = "olap-connection-string-name",
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = "path-to-local-folder"
                    }
                };

                // Deploy (send) the connection string to the server via the PutConnectionStringOperation
                // ======================================================================================
                var PutConnectionStringOp = new PutConnectionStringOperation<OlapConnectionString>(olapConStr);
                PutConnectionStringResult connectionStringResult = store.Maintenance.Send(PutConnectionStringOp);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_olap_connection_string_2
                // Define a connection string to an AWS OLAP destination
                // =====================================================
                var olapConStr = new OlapConnectionString
                {
                    Name = "myOlapConnectionStringName",
                    S3Settings = new S3Settings
                    {
                        BucketName = "myBucket",
                        RemoteFolderName = "my/folder/name",
                        AwsAccessKey = "myAccessKey",
                        AwsSecretKey = "myPassword",
                        AwsRegionName = "us-east-1"
                    }
                };

                // Deploy (send) the connection string to the server via the PutConnectionStringOperation
                // ======================================================================================
                var PutConnectionStringOp = new PutConnectionStringOperation<OlapConnectionString>(olapConStr);
                PutConnectionStringResult connectionStringResult = store.Maintenance.Send(PutConnectionStringOp);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_elasticsearch_connection_string
                // Define a connection string to an Elasticsearch destination
                // ==========================================================
                var elasticSearchConStr = new ElasticSearchConnectionString
                {
                    Name = "elasticsearch-connection-string-name",
                    
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

                // Deploy (send) the connection string to the server via the PutConnectionStringOperation
                // ======================================================================================
                var PutConnectionStringOp = 
                    new PutConnectionStringOperation<ElasticSearchConnectionString>(elasticSearchConStr);
                PutConnectionStringResult connectionStringResult = store.Maintenance.Send(PutConnectionStringOp);
                #endregion
            }
        }

        public class Foo
        {
            #region raven_connection_string
            public class RavenConnectionString : ConnectionString
            {
                public ConnectionStringType Type => ConnectionStringType.Raven;
                
                public string Database { get; set; }   // Target database name
                public string[] TopologyDiscoveryUrls; // List of server urls in the target RavenDB cluster
            }
            #endregion

            #region sql_connection_string
            public class SqlConnectionString : ConnectionString
            {
                public ConnectionStringType Type => ConnectionStringType.Sql;
                
                public string ConnectionString { get; set; }
                public string FactoryName { get; set; }
            }
            #endregion

            #region olap_connection_string
            public class OlapConnectionString : ConnectionString
            {
                public ConnectionStringType Type => ConnectionStringType.Olap;
                
                public LocalSettings LocalSettings { get; set; }
                public S3Settings S3Settings { get; set; }
                public AzureSettings AzureSettings { get; set; }
                public GlacierSettings GlacierSettings { get; set; }
                public GoogleCloudSettings GoogleCloudSettings { get; set; }
                public FtpSettings FtpSettings { get; set; }
            }
            #endregion

            #region elasticsearch_connection_string
            public class ElasticsearchConnectionString : ConnectionString
            {
                public ConnectionStringType Type => ConnectionStringType.ElasticSearch;
                
                public string Nodes { get; set; }
                public string Authentication { get; set; }
                public string Basic { get; set; }
                public string Username { get; set; }
                public string Password { get; set; }
            }
            #endregion

            #region connection_string_class
            // All the connection string class types inherit from this abstract ConnectionString class:
            // ========================================================================================
            
            public abstract class ConnectionString
            {
                public string Name { get; set; } // A name for the connection string        
            }
            #endregion
            
            private interface IFoo
            {
                /*
                #region put_connection_string
                public PutConnectionStringOperation(T connectionString)
                #endregion
                */
            }
        }
    }
}
