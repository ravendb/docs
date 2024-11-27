package net.ravendb.ClientApi.Commands.Documents;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.PutDocumentCommand;
import net.ravendb.client.documents.session.DocumentInfo;
import net.ravendb.client.documents.session.IDocumentSession;

public class Put {

    private class Foo {
        private class PutDocumentCommand {
            //region put_interface
            public PutDocumentCommand(String id, String changeVector, ObjectNode document)
            //endregion
            {}
        }
    }

    public class PutSamples {
        public void foo() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region put_sample
                    Category doc = new Category();
                    doc.setName("My category");
                    doc.setDescription("My category description");

                    DocumentInfo docInfo = new DocumentInfo();
                    docInfo.setCollection("Categories");

                    ObjectNode jsonDoc = session.advanced().getEntityToJson().convertEntityToJson(doc, docInfo);
                    PutDocumentCommand command = new PutDocumentCommand("categories/999", null, jsonDoc);
                    session.advanced().getRequestExecutor().execute(command);
                    //endregion
                }
            }
        }
    }

    private static class Category {
        private String name;
        private String description;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }
    }
}
