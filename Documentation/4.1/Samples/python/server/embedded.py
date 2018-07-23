from pyravendb_embedded.embedded_server import EmbeddedServer
from pyravendb_embedded.server_options import ServerOptions
from pyravendb_embedded.database_options import DatabaseOptions
from pyravendb.tools.utils import Utils


class Embedded:
    @staticmethod
    def embedded():
        # region embedded_example
        EmbeddedServer().start_server()
        with EmbeddedServer().get_document_store("Embedded") as store:
            with store.open_session() as session:
                # Your Code Here
                pass
            # endregion

        # region start_server
        # Will start RavenDB Server with deafult values
        EmbeddedServer().start_server()
        # endregion

        # region start_server_with_options
        server_options = ServerOptions(data_directory="C:/DataDir", server_url="http://127.0.0.1:8080")
        EmbeddedServer().start_server(server_options)
        # endregion

        # region get_document_store
        EmbeddedServer().get_document_store("Embedded")
        # endregion

        # region get_document_store_with_database_options
        database_options = DatabaseOptions(database_name="Embedded", skip_creating_database=True)
        EmbeddedServer().get_document_store(database_options)
        # endregion

        # region open_in_browser
        EmbeddedServer().open_studio_in_browser()
        # endregion

        # region security
        server_options = ServerOptions()
        server_options.secured(certificate_path="PathToServerCertificate.pfx",
                               certificate_password="CertificatePassword")
        # endregion

        # region security2
        server_options_with_exec = ServerOptions()
        server_options_with_exec.secured_with_exec("powershell",
                                                   "C:\\secrets\\give_me_cert.ps1",
                                                   Utils.get_cert_file_fingerprint("PATH_TO_PEM_CERT_FILE"),
                                                   "PATH_TO_PEM_CERT_FILE")
        EmbeddedServer.start_server(server_options_with_exec)
        # endregion
        
        
        # region run_with_dotnet_path
        EmbeddedServer.start_server(ServerOptions(dotnet_path="PATH_TO_DOTNET_EXEC"))
        # endregion
