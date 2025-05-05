import {
    CreateClientCertificateOperation,
    DeleteCertificateOperation,
    DocumentStore,
    PutClientCertificateOperation
} from 'ravendb';

let urls, database, authOptions,permissions,clearance,password,certificate,publicKey,thumbprint;

{
    //document_store_creation
    const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
    store.initialize();
    //const session = store.openSession();

    //region cert_1_1
    const cert1 = await store.maintenance.server.send(
        new CreateClientCertificateOperation([name], [permissions], [clearance], [password]));
    //endregion

    async function foo1() {

        //region cert_1_4
        // With user role set to Cluster Administrator or Operator the user of this certificate
        // is going to have access to all databases
        const clientCertificateOperation = await store.maintenance.server.send(
            new CreateClientCertificateOperation("admin", {}, "Operator"));
        const certificateRawData = clientCertificateOperation.rawData;
        //endregion

        //region cert_1_5
        // when security clearance is ValidUser, you need to specify per database permissions

        const clearance = {
            [store.database]: "ReadWrite"
        };
    }

    const clientCertificateOperation = await store.maintenance.server.send(
        new CreateClientCertificateOperation("user1", clearance, "ValidUser", "myPassword"));
    const certificateRawData = clientCertificateOperation.rawData;
    //endregion

    async function foo2() {
        //region syntax
        const putOperation = 
            new PutClientCertificateOperation(name, certificate, permissions, clearance);
        //endregion
    }

    async function foo3() {
        //region cert_put_2
        const rawCert = fs.readFileSync("<path-to-certificate.crt>");
        const certificateAsBase64 = rawCert.toString("base64");
        
        const putClientCertificateOp = new PutClientCertificateOperation(
            "certificateName",
            certificateAsBase64,
            {},
            "ClusterAdmin");
        
        await store.maintenance.server.send(putClientCertificateOp);
        //endregion

        //region delete_cert_1
        await store.maintenance.server.send(new DeleteCertificateOperation([thumbprint]));
        //endregion

        //region delete_cert_2
        const thumbprint = "a909502dd82ae41433e6f83886b00d4277a32a7b";
        await store.maintenance.server.send(new DeleteCertificateOperation(thumbprint));
        //endregion
    }
}
