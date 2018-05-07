package net.ravendb.ClientApi.HowTo;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.DeleteDocumentCommand;
import net.ravendb.client.documents.commands.GetDocumentsCommand;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.SessionInfo;
import net.ravendb.client.http.RavenCommand;

public class UseLowLevelCommands {

    public interface IInterface {
        //region Execute
        public <TResult> void execute(RavenCommand<TResult> command);

        public <TResult> void execute(RavenCommand<TResult> command, SessionInfo sessionInfo);
        //endregion
    }

    public void Examples() {
        try (IDocumentStore store = new DocumentStore()) {
            //region commands_1
            try (IDocumentSession session = store.openSession()) {
                GetDocumentsCommand command = new GetDocumentsCommand("orders/1-A", null, false);
                session.advanced().getRequestExecutor().execute(command);
                ObjectNode order = (ObjectNode) command.getResult().getResults().get(0);
            }
            //endregion

            //region commands_2

            try (IDocumentSession session = store.openSession()) {
                DeleteDocumentCommand command = new DeleteDocumentCommand("employees/1-A", null);
                session.advanced().getRequestExecutor().execute(command);
            }
            //endregion
        }
    }
}
