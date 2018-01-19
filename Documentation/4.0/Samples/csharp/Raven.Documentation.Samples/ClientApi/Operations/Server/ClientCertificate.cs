using System.Collections.Generic;
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
            #region cert_1
            public CreateClientCertificateOperation(string name, Dictionary<string, DatabaseAccess> permissions, SecurityClearance clearance, string password = null)
            #endregion
            */
        }

        private class Foo
        {
            #region cert_2
            public enum SecurityClearance
            {
                ClusterAdmin,
                ClusterNode,
                Operator,
                ValidUser
            }
            #endregion

            #region cert_3
            public enum DatabaseAccess
            {
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
                    #region cert_4
                    // With user role set to Cluster Administator or Operator the user of this certificate 
                    // is going to have access to all databases
                    CreateClientCertificateOperation operation = new CreateClientCertificateOperation("admin", null, SecurityClearance.Operator);
                    CertificateRawData certificateRawData = store.Maintenance.Server.Send(operation);
                    byte[] cert = certificateRawData.RawData;
                    #endregion
                }

                {
                    #region cert_5
                    // when security clearance is ValidUser, you need to specify per database permissions
                    CreateClientCertificateOperation operation = new CreateClientCertificateOperation("user1", new Dictionary<string, DatabaseAccess>
                    {
                        { "Northwind", DatabaseAccess.Admin }
                    }, SecurityClearance.ValidUser, "myPassword");
                    CertificateRawData certificateRawData = store.Maintenance.Server.Send(operation);
                    byte[] cert = certificateRawData.RawData;
                    #endregion
                }
            }
        }
    }
}
