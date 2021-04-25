using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Operations.Certificates;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{

    public class ClientCertificate
    {
        private interface IFoo
        {
            /*
            #region cert_1_1
            public CreateClientCertificateOperation(string name, 
                Dictionary<string, DatabaseAccess> permissions, 
                SecurityClearance clearance, 
                string password = null)
            #endregion

            #region get_cert_1
            public GetCertificateOperation(string thumbprint)
            #endregion

            #region get_certs_1
            public GetCertificatesOperation(int start, int pageSize)
            #endregion

            #region cert_put_1
            public PutClientCertificateOperation(
                string name, 
                X509Certificate2 certificate, 
                Dictionary<string, DatabaseAccess> permissions, 
                SecurityClearance clearance)
            #endregion
            */
        }

        private class Foo
        {
            #region cert_1_2
            public enum SecurityClearance
            {
                ClusterAdmin,
                ClusterNode,
                Operator,
                ValidUser
            }
            #endregion

            #region cert_1_3
            public enum DatabaseAccess
            {
                Read,
                ReadWrite,
                Admin
            }
            #endregion
        }

        public ClientCertificate()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region cert_1_4
                    // With user role set to Cluster Administrator or Operator the user of this certificate 
                    // is going to have access to all databases
                    CreateClientCertificateOperation operation = 
                        new CreateClientCertificateOperation(
                            "admin", null, SecurityClearance.Operator);
                    CertificateRawData certificateRawData = 
                        store.Maintenance.Server.Send(operation);
                    byte[] cert = certificateRawData.RawData;
                    #endregion
                }

                {
                    #region cert_1_5
                    // when security clearance is ValidUser, you need to specify per database permissions
                    CreateClientCertificateOperation operation = 
                        new CreateClientCertificateOperation(
                            "user1", new Dictionary<string, DatabaseAccess>
                    {
                        { "Northwind", DatabaseAccess.Admin }
                    }, SecurityClearance.ValidUser, "myPassword");
                    CertificateRawData certificateRawData = 
                        store.Maintenance.Server.Send(operation);
                    byte[] cert = certificateRawData.RawData;
                    #endregion
                }

                {
                    #region get_cert_2
                    string thumbprint = "a909502dd82ae41433e6f83886b00d4277a32a7b";
                    CertificateDefinition definition = 
                        store.Maintenance.Server.Send(new GetCertificateOperation(thumbprint));
                    #endregion
                }

                {
                    #region get_certs_2
                    CertificateDefinition[] definitions = 
                        store.Maintenance.Server.Send(new GetCertificatesOperation(0, 20));
                    #endregion
                }

                {

                    #region cert_put_2
                    X509Certificate2 certificate = new X509Certificate2("c:\\path_to_pfx_file");
                    store.Maintenance.Server.Send(
                        new PutClientCertificateOperation(
                            "cert1", certificate, null, SecurityClearance.ClusterAdmin));
                    #endregion
                }
            }
        }
    }
}
