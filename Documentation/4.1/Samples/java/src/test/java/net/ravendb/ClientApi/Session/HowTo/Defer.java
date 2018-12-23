package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.batches.DeleteCommandData;
import net.ravendb.client.documents.commands.batches.ICommandData;
import net.ravendb.client.documents.commands.batches.PutCommandDataWithJson;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;

public class Defer {
    public Defer() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region defer_2

                Map<String, Object> value1 = new HashMap<>();
                value1.put("Name", "My Product");
                value1.put("Supplier", "suppliers/999-A");
                value1.put("@metadata", Collections.singletonMap("@collection", "Users"));

                PutCommandDataWithJson putCommand1 =
                    new PutCommandDataWithJson("products/999-A",
                        null,
                        store.getConventions().getEntityMapper().valueToTree(value1));

                HashMap<String, Object> value2 = new HashMap<>();
                value2.put("Name", "My Product");
                value2.put("Supplier", "suppliers/999-A");
                value2.put("@metadata", Collections.singletonMap("@collection", "Suppliers"));

                PutCommandDataWithJson putCommand2 =
                    new PutCommandDataWithJson("suppliers/999-A",
                        null,
                        store.getConventions().getEntityMapper().valueToTree(value2));

                DeleteCommandData command3 = new DeleteCommandData("products/1-A", null);

                session.advanced().defer(putCommand1, putCommand2, command3);
                //endregion
            }
        }
    }

    private interface IFoo {
        //region defer_1
        void defer(ICommandData command, ICommandData... commands);

        void defer(ICommandData[] commands);
        //endregion
    }

}
