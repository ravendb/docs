package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.connectionStrings.*;
import net.ravendb.client.documents.operations.etl.RavenConnectionString;
import net.ravendb.client.documents.operations.etl.sql.SqlConnectionString;
import net.ravendb.client.serverwide.ConnectionStringType;

import java.util.Map;

public class ConnectionStrings {

    private interface IFoo {
        /*
        //region add_connection_string
        public PutConnectionStringOperation(T connectionString)
        //endregion

        //region get_connection_strings
        public GetConnectionStringsOperation()
        public GetConnectionStringsOperation(String connectionStringName, ConnectionStringType type)
        //endregion

        //region remove_connection_string
        public RemoveConnectionStringOperation(T connectionString)
        //endregion
        */
    }

    public ConnectionStrings() {
        try (IDocumentStore store = new DocumentStore()) {

            //region add_raven_connection_string
            RavenConnectionString connectionString = new RavenConnectionString();
            connectionString.setName("raven2");
            connectionString.setDatabase("Northwind2");
            connectionString.setTopologyDiscoveryUrls(new String[]{"http://rvn2:8080"});

            PutConnectionStringOperation<RavenConnectionString> operation =
                new PutConnectionStringOperation<>(connectionString);
            PutConnectionStringResult connectionStringResult =
                store.maintenance().send(operation);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region add_sql_connection_string
            SqlConnectionString connectionString = new SqlConnectionString();
            connectionString.setName("local_mysql");
            connectionString.setFactoryName("MySql.Data.MySqlClient");
            connectionString.setConnectionString("host=127.0.0.1;user=root;database=Northwind");
            PutConnectionStringOperation<SqlConnectionString> operation
                = new PutConnectionStringOperation<>(connectionString);

            PutConnectionStringResult connectionStringResult = store.maintenance().send(operation);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region get_all_connection_strings
            GetConnectionStringsOperation operation = new GetConnectionStringsOperation();
            GetConnectionStringsResult connectionStrings = store.maintenance().send(operation);
            Map<String, SqlConnectionString> sqlConnectionStrings =
                connectionStrings.getSqlConnectionStrings();
            Map<String, RavenConnectionString> ravenConnectionStrings =
                connectionStrings.getRavenConnectionStrings();
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region get_connection_string_by_name
            GetConnectionStringsOperation operation = new GetConnectionStringsOperation(
                "local_mysql", ConnectionStringType.SQL);
            GetConnectionStringsResult connectionStrings = store.maintenance().send(operation);
            Map<String, SqlConnectionString> sqlConnectionStrings =
                connectionStrings.getSqlConnectionStrings();
            SqlConnectionString mysqlConnectionString = sqlConnectionStrings.get("local_mysql");
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            RavenConnectionString connectionString = null;
            //region remove_raven_connection_string
            RemoveConnectionStringOperation<RavenConnectionString> operation =
                new RemoveConnectionStringOperation<>(connectionString);

            store.maintenance().send(operation);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            SqlConnectionString connectionString = null;
            //region remove_sql_connection_string
            RemoveConnectionStringOperation<SqlConnectionString> operation =
                new RemoveConnectionStringOperation<>(connectionString);

            store.maintenance().send(operation);
            //endregion
        }
    }

    public static class Foo {
        //region raven_connection_string
        public class RavenConnectionString extends ConnectionString {
            private String database; // target database name
            private String[] topologyDiscoveryUrls; // list of server urls

            public ConnectionStringType getType() {
                return ConnectionStringType.RAVEN;
            }

            // getters and setters
        }
        //endregion

        //region sql_connection_string
        public class SqlConnectionString extends ConnectionString {
            private String connectionString;
            private String factoryName;

            public ConnectionStringType getType() {
                return ConnectionStringType.SQL;
            }

            // getters and setters
        }
        //endregion

        //region connection_string
        public class ConnectionString {
            private String name; // name of connection string

            // getters and setters
        }
        //endregion
    }

}
