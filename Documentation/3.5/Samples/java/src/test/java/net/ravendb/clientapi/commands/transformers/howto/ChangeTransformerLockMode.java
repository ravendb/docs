package net.ravendb.clientapi.commands.transformers.howto;

import net.ravendb.abstractions.indexing.TransformerLockMode;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;

public class ChangeTransformerLockMode {

    private interface IFoo {
        //region change_transformer_lock_1
        public void setTransformerLock(String name, TransformerLockMode lockMode);
        //endregion
    }

    public ChangeTransformerLockMode() {
        try (IDocumentStore store = new DocumentStore()) {
            //region change_transformer_lock_2
            store.getDatabaseCommands().setTransformerLock("Orders/Company", TransformerLockMode.LOCKED_IGNORE);
            //endregion
        }
    }
}
