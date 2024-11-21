package net.ravendb.ClientApi.Commands.Batches;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.batches.*;
import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.documents.session.IDocumentSession;

import java.time.Duration;
import java.util.Arrays;
import java.util.List;

public class HowToSendMultipleCommandsUsingBatch {

    public class BatchInterface {
        private class BatchCommand {
            //region batch_1
            public BatchCommand(DocumentConventions conventions, List<ICommandData> commands, BatchOptions options)
            //endregion
            {
            }
        }
    }

    //region batch_2
    public class BatchOptions {
        private boolean waitForReplicas;
        private int numberOfReplicasToWaitFor;
        private Duration waitForReplicasTimeout;
        private boolean majority;
        private boolean throwOnTimeoutInWaitForReplicas;

        private boolean waitForIndexes;
        private Duration waitForIndexesTimeout;
        private boolean throwOnTimeoutInWaitForIndexes;
        private String[] waitForSpecificIndexes;

        // getters and setters
    }
    //endregion

    public HowToSendMultipleCommandsUsingBatch() {
        try (IDocumentStore documentStore = new DocumentStore()) {
            ObjectMapper mapper = new ObjectMapper();


            //region batch_3

            try (IDocumentSession session = documentStore.openSession()) {

                ObjectNode user3 = mapper.createObjectNode();
                user3.put("Name", "James");

                PutCommandDataWithJson user3Cmd = new PutCommandDataWithJson("users/3", null, user3);

                DeleteCommandData deleteCmd = new DeleteCommandData("users/2-A", null);
                List<ICommandData> commands = Arrays.asList(user3Cmd, deleteCmd);

                BatchCommand batch = new BatchCommand(documentStore.getConventions(), commands);
                session.advanced().getRequestExecutor().execute(batch);

            }
            //endregion
        }
    }
}
