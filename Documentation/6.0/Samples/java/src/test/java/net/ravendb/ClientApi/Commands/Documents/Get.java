package net.ravendb.ClientApi.Commands.Documents;

import com.fasterxml.jackson.databind.node.ArrayNode;
import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.GetDocumentsCommand;
import net.ravendb.client.documents.session.IDocumentSession;

public class Get {

    public static class GetInterfaces {
        public static class GetDocumentsCommand {
            //region get_interface_single
            public GetDocumentsCommand(String id, String[] includes, boolean metadataOnly)
            //endregion
            {

            }

            //region get_interface_multiple
            public GetDocumentsCommand(String[] ids, String[] includes, boolean metadataOnly)
            //endregion
            {

            }

            //region get_interface_paged
            public GetDocumentsCommand(int start, int pageSize)
            //endregion
            {

            }

            //region get_interface_startswith
            public GetDocumentsCommand(
                String startWith,
                String startAfter,
                String matches,
                String exclude,
                int start,
                int pageSize,
                boolean metadataOnly)
            //endregion
            {

            }
        }
    }

    public static class GetSamples{
        public void single() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_single
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        "orders/1-A", null, false);
                    session.advanced().getRequestExecutor().execute(command);
                    ObjectNode order = (ObjectNode) command.getResult().getResults().get(0);
                    //endregion
                }
            }
        }

        public void multiple() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_multiple
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        new String[]{"orders/1-A", "employees/3-A"}, null, false);
                    session.advanced().getRequestExecutor().execute(command);
                    ObjectNode order = (ObjectNode) command.getResult().getResults().get(0);
                    ObjectNode employee = (ObjectNode) command.getResult().getResults().get(1);
                    //endregion
                }
            }
        }

        public void includes() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_includes
                    // Fetch emploees/5-A and his boss.
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        "employees/5-A", new String[]{"ReportsTo"}, false);
                    session.advanced().getRequestExecutor().execute(command);

                    ObjectNode employee = (ObjectNode) command.getResult().getResults().get(0);
                    String bossId = employee.get("ReportsTo").asText();
                    ObjectNode boss = (ObjectNode) command.getResult().getIncludes().get(bossId);
                    //endregion
                }
            }
        }

        public void missing() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_missing
                    // Assuming that products/9999-A doesn't exist.
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        new String[]{"products/1-A", "products/9999-A", "products/3-A"}, null, false);
                    session.advanced().getRequestExecutor().execute(command);
                    ArrayNode products = command.getResult().getResults(); // products/1-A, null, products/3-A
                    //endregion
                }
            }
        }

        public void paged() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_paged
                    GetDocumentsCommand command = new GetDocumentsCommand(0, 128);
                    session.advanced().getRequestExecutor().execute(command);
                    ArrayNode firstDocs = command.getResult().getResults();
                    //endregion
                }
            }
        }

        public void startsWith() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_startswith
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        "products",  //startWith
                        null, //startAfter
                        null, // matches
                        null, //exclude
                        0, // start
                        128, // pageSize
                        false //metadataOnly
                    );

                    session.advanced().getRequestExecutor().execute(command);
                    ArrayNode products = command.getResult().getResults();
                    //endregion
                }
            }
        }

        public void startsWithMatches() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_startswith_matches
                    // return up to 128 documents  with key that starts with 'products/'
                    // and rest of the key begins with "1" or "2", eg. products/10, products/25
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        "products", //startWith
                        null, // startAfter
                        "1*|2*", // matches
                        null, // exclude
                        0, //start
                        128, //pageSize
                        false); //metadataOnly
                    //endregion
                }
            }
        }

        public void startsWithMatchesEnd() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region get_sample_startswith_matches_end
                    // return up to 128 documents with key that starts with 'products/'
                    // and rest of the key have length of 3, begins and ends with "1"
                    // and contains any character at 2nd position e.g. products/101, products/1B1
                    GetDocumentsCommand command = new GetDocumentsCommand(
                        "products", //startWith
                        null, // startAfter
                        "1?1", // matches
                        null, // exclude
                        0, //start
                        128, //pageSize
                        false); //metadataOnly
                    session.advanced().getRequestExecutor().execute(command);
                    ArrayNode products = command.getResult().getResults();
                    //endregion
                }
            }
        }
        
        public getMetadataOnly() {
              try (IDocumentStore store = new DocumentStore()) {
                  try (IDocumentSession session = store.openSession()) {
                      //region get_metadata_only
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
