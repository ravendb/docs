package net.ravendb.Server;

import com.fasterxml.jackson.databind.ser.std.EnumSerializer;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.serverwide.DatabaseRecord;
import net.ravendb.embedded.DatabaseOptions;
import net.ravendb.embedded.EmbeddedServer;
import net.ravendb.embedded.ServerOptions;

import java.security.KeyStore;

public class Embedded {
    public Embedded() {

        //region get_server_url
        String serverUri = EmbeddedServer.INSTANCE.getServerUri();
        //endregion

        //region embedded_example
        EmbeddedServer.INSTANCE.startServer();

        try (IDocumentStore store = EmbeddedServer.INSTANCE.getDocumentStore("Embedded")) {
            try (IDocumentSession session = store.openSession()) {
                // your code here
            }
        }
        //endregion

        //region start_server
        // Start RavenDB Embedded Server with default options
        EmbeddedServer.INSTANCE.startServer();
        //endregion

        {
            //region start_server_with_options
            ServerOptions serverOptions = new ServerOptions();
            // target location of RavenDB data
            serverOptions.setDataDirectory("C:\\RavenData");
            serverOptions.setServerUrl("http://127.0.0.1:8080");

            // location where server binaries will be extracted
            serverOptions.setTargetServerLocation("c:\\RavenServer");
            EmbeddedServer.INSTANCE.startServer(serverOptions);
            //endregion
        }


        //region get_document_store
        EmbeddedServer.INSTANCE.getDocumentStore("Embedded");
        //endregion

        //region get_document_store_with_database_options
        DatabaseRecord databaseRecord = new DatabaseRecord();
        databaseRecord.setDatabaseName("Embedded");
        DatabaseOptions databaseOptions = new DatabaseOptions(databaseRecord);
        EmbeddedServer.INSTANCE.getDocumentStore(databaseOptions);
        //endregion

        //region open_in_browser
        EmbeddedServer.INSTANCE.openStudioInBrowser();
        //endregion

        {
            //region security
            ServerOptions serverOptions = new ServerOptions();
            serverOptions.secured("PathToCertificate", "CertificatePassword");
            //endregion
        }

        {

            KeyStore clientCertificate = null;
            //region security2
            ServerOptions serverOptions = new ServerOptions();
            serverOptions.secured("powershell",
                "c:\\secrets\\give_me_cert.ps1",
                "a909502dd82ae41433e6f83886b00d4277a32a7b",
                clientCertificate,
                "PathToCaCertificateFile");
            //endregion
        }

        {
            //region run_with_dotnet_path
            ServerOptions serverOptions = new ServerOptions();
            serverOptions.setFrameworkVersion("2.1.1");
            serverOptions.setDotNetPath("PATH_TO_DOTNET_EXEC");
            EmbeddedServer.INSTANCE.startServer(serverOptions);
            //endregion
        }
    }
}
