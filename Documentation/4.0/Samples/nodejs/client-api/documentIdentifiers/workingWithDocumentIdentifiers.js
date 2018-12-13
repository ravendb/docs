import { 
    DocumentStore, 
    DocumentConventions, 
    NextIdentityForCommand, 
    PutDocumentCommand,
    SeedIdentityForCommand
} from "ravendb";

const store = new DocumentStore();

class Order {}

class Company {}

class Product {}

async function example() {
    const session = store.openSession();

    //region session_id_not_provided
    const order = new Order();
    order.id = null; // value not provided

    await session.store(order);
    //endregion

    //region session_get_document_id
    const orderId = session.advanced.getDocumentId(order); // "orders/1-A"
    //endregion

    //region session_empty_string_id
    const orderEmptyId = new Order();
    orderEmptyId.id = ""; // database will create a GUID value for it
    await session.store(orderEmptyId);
    await session.saveChanges();

    const guidId = await session.advanced
        .getDocumentId(orderEmptyId); // "bc151542-8fa7-45ac-bc04-509b343a8720"
    //endregion

    {
        //region session_semantic_id_1
        const product = new Product();
        product.id = "products/ravendb";
        product.name = "RavenDB";

        await session.store(product);
        //endregion
    }

    {
        //region session_semantic_id_2
        const product = new Product();
        product.name = "RavenDB";
        await session.store(product, "products/ravendb");
        //endregion
    }

    {
        //region session_auto_id
        const company = new Company();
        company.id = "companies/";

        await session.store(company);
        await session.saveChanges();
        //endregion
    }

    {
        //region session_identity_id
        const company = new Company();
        company.id = "companies|";
        await session.store(company);
        await session.saveChanges();
        //endregion
    }

    
        //region commands_identity
        const doc = { "Name": "My RavenDB" };

        const jsonDoc = session.advanced.entityToJson.convertEntityToJson(doc, store.conventions);
        const command = new PutDocumentCommand("products/", null, jsonDoc);
        await session.advanced.getRequestExecutor().execute(command);

        const identityId =
            command.result.id; // "products/0000000000000000001-A if using only '/' in the session"

        const commandWithPipe = new PutDocumentCommand("products|", null, jsonDoc);
        await session.advanced.getRequestExecutor().execute(commandWithPipe);

        const identityPipeId = command.result.id; // "products/1"
        //endregion

        //region commands_identity_set
        const seedIdentityCommand = new SeedIdentityForCommand("products", 1994);
        //endregion
 
}

async function workingWithDocumentIdentifiers(g) {

    const session = store.openSession();
    //region commands_identity_generate
    const command = new NextIdentityForCommand("products");
    await session.advanced.getRequestExecutor().execute(command);
    const identity = command.result;

    const doc = { "Name": "My RavenDB" };
    const jsonDoc = session.advanced.entityToJson.convertEntityToJson(doc, store.conventions);
    const putCommand = new PutDocumentCommand("products/" + identity, null, jsonDoc);
    session.advanced.getRequestExecutor().execute(putCommand);
    //endregion
}
