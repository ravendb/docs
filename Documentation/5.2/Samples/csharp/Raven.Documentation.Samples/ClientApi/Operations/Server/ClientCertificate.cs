using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
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

            #region put_syntax
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
            // The role assigned to the certificate:
            public enum SecurityClearance
            {
                ClusterAdmin,
                ClusterNode,
                Operator,
                ValidUser
            }
            #endregion

            #region cert_1_3
            // The access level for a 'ValidUser' security clearance:
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
                    // With the security clearance set to Cluster Administrator or Operator,
                    // the user of this certificate will have access to all databases
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
                    // When the security clearance is ValidUser, you must specify an access level for each database
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
            }
        }
        
        public async Task PutClientCertificate()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region put_client_certificate
                    X509Certificate2 certificate = new X509Certificate2("c:\\path_to_pfx_file");

                    // Define the put client certificate operation 
                    var putClientCertificateOp = new PutClientCertificateOperation(
                        "certificateName",
                        certificate, 
                        new Dictionary<string, DatabaseAccess>(),
                        SecurityClearance.ClusterAdmin);

                    // Execute the operation by passing it to Maintenance.Server.Send
                    store.Maintenance.Server.Send(putClientCertificateOp);
                    #endregion
                }

                {
                    #region put_client_certificate_async

                    X509Certificate2 certificate = new X509Certificate2("c:\\path_to_pfx_file");

                    // Define the put client certificate operation 
                    var putClientCertificateOp = new PutClientCertificateOperation(
                        "certificateName",
                        certificate, 
                        new Dictionary<string, DatabaseAccess>(),
                        SecurityClearance.ClusterAdmin);

                    // Execute the operation by passing it to Maintenance.Server.SendAsync
                    await store.Maintenance.Server.SendAsync(putClientCertificateOp);
                    #endregion
                }
            }
        }
    }
}
