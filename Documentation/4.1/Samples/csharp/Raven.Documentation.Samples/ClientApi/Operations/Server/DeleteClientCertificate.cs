using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Operations.Certificates;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{

    public class DeleteClientCertificate
    {
        private interface IFoo
        {
            /*
            #region delete_cert_1
            public DeleteCertificateOperation(string thumbprint)
            #endregion
            */
        }

        public DeleteClientCertificate()
        {
            using (var store = new DocumentStore())
            {
                #region delete_cert_2
                string thumbprint = "a909502dd82ae41433e6f83886b00d4277a32a7b";
                store.Maintenance.Server.Send(new DeleteCertificateOperation(thumbprint));
                #endregion
            }
        }
    }
}
