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
            // load certificate
            X509Certificate2 clientCertificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx");

            using (IDocumentStore store = new DocumentStore()
            {
                Certificate = clientCertificate,
                Database = "Northwind",
                Urls = new[] {"https://my_secured_raven"}
            }.Initialize())
            {
                // do your work here
            }

            #endregion
	    }

	}
}
