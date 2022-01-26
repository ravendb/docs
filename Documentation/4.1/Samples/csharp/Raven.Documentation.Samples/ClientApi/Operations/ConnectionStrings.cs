using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class ConnectionStrings
    {
        private interface IFoo
        {
            /*
            #region add_connection_string
            public PutConnectionStringOperation(T connectionString)
            #endregion
            
            #region get_connection_strings
            public GetConnectionStringsOperation()
            public GetConnectionStringsOperation(string connectionStringName, ConnectionStringType type)
            #endregion
            
            #region remove_connection_string
            public RemoveConnectionStringOperation(T connectionString)
            #endregion
            */
        }

        public ConnectionStrings()
        {
            using (var store = new DocumentStore())
            {
                #region add_raven_connection_string

                PutConnectionStringOperation<RavenConnectionString> operation
                    = new PutConnectionStringOperation<RavenConnectionString>(
                        new RavenConnectionString
                {
                    Name = "raven2",
                    Database = "Northwind2",
                    TopologyDiscoveryUrls = new[]
                    {
                        "https://rvn2:8080"
                    }
                });

                PutConnectionStringResult connectionStringResult 
                    = store.Maintenance.Send(operation);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_sql_connection_string

                PutConnectionStringOperation<SqlConnectionString> operation 
                    = new PutConnectionStringOperation<SqlConnectionString>(
                        new SqlConnectionString
                    {
                        Name = "local_mysql",
                        FactoryName = "MySql.Data.MySqlClient",
                        ConnectionString = "host=127.0.0.1;user=root;database=Northwind"
                    });

                PutConnectionStringResult connectionStringResult 
                    = store.Maintenance.Send(operation);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region get_all_connection_strings
                GetConnectionStringsOperation operation = new GetConnectionStringsOperation();
                GetConnectionStringsResult connectionStrings = store.Maintenance.Send(operation);
                Dictionary<string, SqlConnectionString> sqlConnectionStrings = connectionStrings.SqlConnectionStrings;
                Dictionary<string, RavenConnectionString> ravenConnectionStrings = connectionStrings.RavenConnectionStrings;
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region get_connection_string_by_name
                GetConnectionStringsOperation operation = 
                    new GetConnectionStringsOperation("local_mysql", ConnectionStringType.Sql);
                GetConnectionStringsResult connectionStrings = store.Maintenance.Send(operation);
                Dictionary<string, SqlConnectionString> sqlConnectionStrings = connectionStrings.SqlConnectionStrings;
                SqlConnectionString mysqlConnectionString = sqlConnectionStrings["local_mysql"];
                #endregion
            }
            
            using (var store = new DocumentStore())
            {

                RavenConnectionString connectionString = null;
                #region remove_raven_connection_string
                RemoveConnectionStringOperation<RavenConnectionString> operation 
                    = new RemoveConnectionStringOperation<RavenConnectionString>(
                        connectionString);

                store.Maintenance.Send(operation);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                SqlConnectionString connectionString = null;
                
                #region remove_sql_connection_string
                RemoveConnectionStringOperation<SqlConnectionString> operation 
                    = new RemoveConnectionStringOperation<SqlConnectionString>(
                        connectionString);

                store.Maintenance.Send(operation);
                #endregion
            }

            using (var store = GetDocumentStore())
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


    public class Foo
    {
        #region raven_connection_string
        public class RavenConnectionString : ConnectionString
        {
            public string Database { get; set; } // target database name
            public string[] TopologyDiscoveryUrls; // list of server urls

            public ConnectionStringType Type => ConnectionStringType.Raven;

        }
        #endregion
    
        #region sql_connection_string
        public class SqlConnectionString : ConnectionString
        {
            public string ConnectionString { get; set; }

            public string FactoryName { get; set; }

            public ConnectionStringType Type => ConnectionStringType.Sql;

        }
        #endregion
    
        #region connection_string
        public class ConnectionString
        {
            public string Name { get; set; } // name of connection string        
        }
        #endregion
    }
   
}
