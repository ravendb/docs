package net.ravendb.ClientApi.Commands.Documents.HowTo;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.GetDocumentsCommand;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Iterator;

public class Head {
    public static class Foo {

        public static class GetDocumentsCommand {
            //region head_1
            public GetDocumentsCommand(String id, String[] includes, boolean metadataOnly)
            //endregion
            {}

            //region head_3
            public GetDocumentsCommand(String[] ids, String[] includes, boolean metadataOnly)
            //endregion
            {}
        }
    }

    public static class Foo2 {
        public Foo2() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region head_2
                    GetDocumentsCommand command = new GetDocumentsCommand("orders/1-A", null, true);
                    session.advanced().getRequestExecutor().execute(command);

                    JsonNode result = command.getResult().getResults().get(0);
                    ObjectNode documentMetadata = (ObjectNode) result.get("@metadata");

                    // Print out all the metadata properties.
                    Iterator<String> fieldIterator = documentMetadata.fieldNames();

                    while (fieldIterator.hasNext()) {
                        String field = fieldIterator.next();
                        JsonNode value = documentMetadata.get(field);
                        System.out.println(field + " = " + value);
                    }
                    //endregion
                }
            }

        }
    }
}
