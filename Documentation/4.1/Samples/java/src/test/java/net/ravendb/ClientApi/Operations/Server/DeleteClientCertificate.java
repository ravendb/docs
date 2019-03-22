package net.ravendb.ClientApi.Operations.Server;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.serverwide.operations.certificates.DeleteCertificateOperation;

public class DeleteClientCertificate {

    private interface IFoo {
        /*
        //region delete_cert_1
        public DeleteCertificateOperation(String thumbprint);
        //endregion
        */
    }

    public DeleteClientCertificate() {
        try (IDocumentStore store = new DocumentStore()) {
            //region delete_cert_2
            String thumbprint = "a909502dd82ae41433e6f83886b00d4277a32a7b";
            store.maintenance().server().send(new DeleteCertificateOperation(thumbprint));
            //endregion
        }
    }
}
