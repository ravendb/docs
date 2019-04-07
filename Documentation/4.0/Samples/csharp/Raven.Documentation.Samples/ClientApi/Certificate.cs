using System.Security.Cryptography.X509Certificates;

namespace Raven.Documentation.Samples.ClientApi
{
	using System;
	using Client.Documents;

	public class Certificate
	{
	    public Certificate()
	    {
            #region client_cert
            //Load a X.509 certificate
            X509Certificate2 clientCertificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx");

            using (IDocumentStore store = new DocumentStore()
            {
                //Pass your certificate to the `Certificate` property
                Certificate = clientCertificate,
                Database = "your_secured_database_name",
                Urls = new[] {"https://your_secured_RavenDB_server_URL"}
            }.Initialize())
            {
                //Do your work here
            }

            #endregion
	    }

	}
}
