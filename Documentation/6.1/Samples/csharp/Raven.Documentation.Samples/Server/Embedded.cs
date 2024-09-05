using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.ServerWide;
using Raven.Embedded;

namespace Raven.Documentation.Samples.Server
{
    public class Embedded
    {
        public static async Task EmbeddedAsync()
        {
            #region embedded_async_example
            EmbeddedServer.Instance.StartServer();
            using (var store = await EmbeddedServer.Instance.GetDocumentStoreAsync("Embedded"))
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Your code here
                }
            }
            #endregion

            #region restart_server
            await EmbeddedServer.Instance.RestartServerAsync();
            #endregion

            #region get_async_document_store

            await EmbeddedServer.Instance.GetDocumentStoreAsync("Embedded");

            #endregion

            #region get_async_document_store_with_database_options

            var databaseOptions = new DatabaseOptions(new DatabaseRecord
            {
                DatabaseName = "Embedded"
            });
            await EmbeddedServer.Instance.GetDocumentStoreAsync(databaseOptions);
            #endregion

            #region get_server_url_async

            Uri url = await EmbeddedServer.Instance.GetServerUriAsync();

            #endregion

            #region get_server_process_id

            int processID = await EmbeddedServer.Instance.GetServerProcessIdAsync();

            #endregion

            #region server_process_exited
            var args = new ServerProcessExitedEventArgs() { };
            #endregion

        }

        public void EmbeddedSamples()
        {
            #region embedded_example
            EmbeddedServer.Instance.StartServer();
            using (var store = EmbeddedServer.Instance.GetDocumentStore("Embedded"))
            {
                using (var session = store.OpenSession())
                {
                    // Your code here
                }
            }
            #endregion

            #region start_server
            // Start RavenDB Embedded Server with default options
            EmbeddedServer.Instance.StartServer();
            #endregion

            #region start_server_with_options

            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                DataDirectory = "C:\\RavenData",
                ServerUrl = "http://127.0.0.1:8080"
            });

            #endregion

            #region start_server_with_FrameworkVersion_defined

            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                FrameworkVersion = "3.1.15+",
            });

            #endregion

            #region start_server_with_server_directory_option

            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                ServerDirectory = @"C:\RavenDB\Server"
            });

            #endregion

            #region get_document_store

            EmbeddedServer.Instance.GetDocumentStore("Embedded");

            #endregion

            #region get_document_store_with_database_options

            var databaseOptions = new DatabaseOptions(new DatabaseRecord
            {
                DatabaseName = "Embedded"
            });
            EmbeddedServer.Instance.GetDocumentStore(databaseOptions);
            #endregion

            #region open_in_browser
            EmbeddedServer.Instance.OpenStudioInBrowser();
            #endregion

            #region security
            var serverOptions = new ServerOptions();
            serverOptions.Secured(
                certificate: "PathToServerCertificate",
                certPassword: "CertificatePassword");
            #endregion

            #region security2
            var serverOptionsWithExec = new ServerOptions();
            var certificate = new X509Certificate2();
            serverOptionsWithExec.Secured(
                certLoadExec: "powershell",
                certExecArgs: "C:\\secrets\\give_me_cert.ps1",
                serverCertThumbprint: certificate.Thumbprint,
                clientCert: certificate);
            #endregion

            #region run_with_dotnet_path
            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                FrameworkVersion = "2.1.2",
                DotNetPath = "PATH_TO_DOTNET_EXEC"
            });
            #endregion

        }
    }
}
