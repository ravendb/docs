import {CreateClientCertificateOperation, DocumentStore} from 'ravendb';
import { EtlConfiguration } from 'ravendb';
import { SecurityClearance } from 'ravendb';
let urls, database, authOptions;

{
    //document_store_creation
    const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
    store.initialize();
    //const session = store.openSession();

    //region cert_1_1
    const cert1 = await store.maintenance.server.send(
        new CreateClientCertificateOperation([name],[permissions],[clearance],[password]));
    //endregion

    //region cert_1_2
    export type SecurityClearance =
        "UnauthenticatedClients"
        | "ClusterAdmin"
        | "ClusterNode"
        | "Operator"
        | "ValidUser";
    //endregion

    //region cert_1_3
    export type DatabaseAccess =
        "ReadWrite"
        | "Admin";
    //endregion

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
    } as Record<string, DatabaseAccess>;

    const clientCertificateOperation = await store.maintenance.server.send(
        new CreateClientCertificateOperation("user1", clearance, "ValidUser", "myPassword"));
    const certificateRawData = clientCertificateOperation.rawData;

    //endregion

    //region cert_put_1
    const putOperation = new PutClientCertificateOperation([name], [certificate], [permissions], [clearance]);
    //endregion

    //region cert_put_2
    const putOperation = new PutClientCertificateOperation("cert1", publicKey, {}, "ClusterAdmin");
    await store.maintenance.server.send(putOperation);
    //endregion

    //region delete_cert_1
    await store.maintenance.server.send(new DeleteCertificateOperation([thumbprint]));
    //endregion

    //region delete_cert_2
    const thumbprint = "a909502dd82ae41433e6f83886b00d4277a32a7b";
    await store.maintenance.server.send(new DeleteCertificateOperation(thumbprint));
    //endregion

