package net.ravendb.ClientApi.Operations.Server;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.serverwide.operations.certificates.*;
import org.apache.commons.codec.binary.Base64;

import java.security.KeyStore;
import java.security.cert.Certificate;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;

public class ClientCertificate {

    private interface IFoo {
        /*
        //region cert_1_1
        public CreateClientCertificateOperation(String name,
                                                Map<String, DatabaseAccess> permissions,
                                                SecurityClearance clearance)

        public CreateClientCertificateOperation(String name,
                                                Map<String, DatabaseAccess> permissions,
                                                SecurityClearance clearance,
                                                String password)
        //endregion
        */

        /*
        //region get_cert_1
        public GetCertificateOperation(String thumbprint)
        //endregion
        */

        /*
        //region get_certs_1
        public GetCertificatesOperation(int start, int pageSize)
        //endregion
        */

        /*
        //region cert_put_1
        public PutClientCertificateOperation(String name,
                                             String certificate,
                                             Map<String, DatabaseAccess> permissions,
                                             SecurityClearance clearance)
        //endregion
        */
    }

    private static class Foo {
        //region cert_1_2
        public enum SecurityClearance {
            CLUSTER_ADMIN,
            CLUSTER_NODE,
            OPERATOR,
            VALID_USER
        }
        //endregion

        //region cert_1_3
        public enum DatabaseAccess {
            READ,
            READ_WRITE,
            ADMIN
        }
        //endregion
    }


    public ClientCertificate() {
        try (IDocumentStore store = new DocumentStore()) {
            //region cert_1_4
            // With user role set to Cluster Administrator or Operator the user of this certificate
            // is going to have access to all databases

            CreateClientCertificateOperation operation = new CreateClientCertificateOperation("admin",
                null, SecurityClearance.OPERATOR);
            CertificateRawData certificateRawData = store.maintenance().server().send(operation);
            byte[] certificatesZipped = certificateRawData.getRawData();
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region cert_1_5
            // when security clearance is ValidUser, you need to specify per database permissions
            CreateClientCertificateOperation operation = new CreateClientCertificateOperation("user1",
                Collections.singletonMap("Northwind", DatabaseAccess.ADMIN),
                SecurityClearance.VALID_USER,
                "myPassword");

            CertificateRawData certificateRawData = store.maintenance().server().send(operation);
            byte[] certificateZipped = certificateRawData.getRawData();
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region get_cert_2
            String thumbprint = "a909502dd82ae41433e6f83886b00d4277a32a7b";
            CertificateDefinition definition = store.maintenance()
                .server()
                .send(new GetCertificateOperation(thumbprint));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region get_certs_2
            CertificateDefinition[] definitions = store.maintenance()
                .server()
                .send(new GetCertificatesOperation(0, 20));
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            KeyStore keyStore = null;

            try {
                //region cert_put_2
                byte[] rawCert = Files.readAllBytes(Paths.get("<path-to-certificate.crt>"));
                String certificateAsBase64 = Base64.getEncoder().encodeToString(rawCert);                

                store.maintenance().server().send(
                    new PutClientCertificateOperation(
                        "certificateName",
                        certificateAsBase64,
                        new HashMap<>(),
                        SecurityClearance.CLUSTER_ADMIN));
                //endregion
            } catch (Exception e) {
            }
        }
    }
}
