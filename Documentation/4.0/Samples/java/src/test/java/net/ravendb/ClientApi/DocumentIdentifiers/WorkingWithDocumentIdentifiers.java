package net.ravendb.ClientApi.DocumentIdentifiers;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.commands.NextIdentityForCommand;
import net.ravendb.client.documents.commands.PutDocumentCommand;
import net.ravendb.client.documents.commands.SeedIdentityForCommand;
import net.ravendb.client.documents.session.DocumentSession;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Collections;
import java.util.Map;

public class WorkingWithDocumentIdentifiers {

    private static class Order {
        private String id;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }
    }

    private static class Company {
        private String id;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }
    }

    private static class Product {
        private String id;
        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    public WorkingWithDocumentIdentifiers() {
        DocumentStore store = new DocumentStore();

        DocumentSession session = (DocumentSession) store.openSession();

        //region session_id_not_provided
        Order order = new Order();
        order.setId(null); // value not provided

        session.store(order);
        //endregion

        //region session_get_document_id
        String orderId = session.advanced().getDocumentId(order); // "orders/1-A"
        //endregion

        //region session_empty_string_id
        Order orderEmptyId = new Order();
        orderEmptyId.setId(""); // database will create a GUID value for it
        session.store(orderEmptyId);
        session.saveChanges();

        String guidId = session.advanced()
            .getDocumentId(orderEmptyId); // "bc151542-8fa7-45ac-bc04-509b343a8720"
        //endregion

        {
            //region session_semantic_id_1
            Product product = new Product();
            product.setId("products/ravendb");
            product.setName("RavenDB");

            session.store(product);
            //endregion
        }

        {
            //region session_semantic_id_2
            Product product = new Product();
            product.setName("RavenDB");
            session.store(product, "products/ravendb");
            //endregion
        }

        {
            //region session_auto_id
            Company company = new Company();
            company.setId("companies/");

            session.store(company);
            session.saveChanges();
            //endregion
        }

        {
            //region session_identity_id
            Company company = new Company();
            company.setId("companies|");
            session.store(company);
            session.saveChanges();
            //endregion
        }

        {
            //region commands_identity
            Map<String, String> doc = Collections.singletonMap("Name", "My RavenDB");

            ObjectNode jsonDoc = session.advanced().getEntityToJson().convertEntityToJson(doc, store.getConventions());
            PutDocumentCommand comamnd = new PutDocumentCommand("products/", null, jsonDoc);
            session.advanced().getRequestExecutor().execute(comamnd);

            String identityId =
                comamnd.getResult().getId(); // "products/0000000000000000001-A if using only '/' in the seesion"

            PutDocumentCommand commandWithPipe = new PutDocumentCommand("products|", null, jsonDoc);
            session.advanced().getRequestExecutor().execute(commandWithPipe);

            String identityPipeId = comamnd.getResult().getId(); // "products/1"
            //endregion

            //region commands_identity_set
            SeedIdentityForCommand seedIdentityCommand = new SeedIdentityForCommand("products", 1994L);
            //endregion
        }
    }

    public WorkingWithDocumentIdentifiers(String g) {
        DocumentStore store = new DocumentStore();
        IDocumentSession session = store.openSession();

        //region commands_identity_generate
        NextIdentityForCommand command = new NextIdentityForCommand("products");
        session.advanced().getRequestExecutor().execute(command);
        Long identity = command.getResult();

        Map<String, String> doc = Collections.singletonMap("Name", "My RavenDB");

        ObjectNode jsonDoc = session.advanced().getEntityToJson().convertEntityToJson(doc, store.getConventions());
        PutDocumentCommand putCommand = new PutDocumentCommand("products/" + identity, null, jsonDoc);
        session.advanced().getRequestExecutor().execute(putCommand);
        //endregion
    }
}
