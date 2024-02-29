package net.ravendb.ClientApi.Commands.Documents;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.DeleteDocumentCommand;
import net.ravendb.client.documents.session.IDocumentSession;

public class Delete {


    public static class DeleteInterfaces {
        private static class DeleteDocumentCommand {
            //region delete_interface
            public DeleteDocumentCommand(String id, String changeVector)
            //endregion
            {}
        }
    }

    public class DeleteSamples {
        public void foo() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region delete_sample
                    DeleteDocumentCommand command = new DeleteDocumentCommand("employees/1-A", null);
                    session.advanced().getRequestExecutor().execute(command);
                    //endregion
                }
            }
        }
    }
}
