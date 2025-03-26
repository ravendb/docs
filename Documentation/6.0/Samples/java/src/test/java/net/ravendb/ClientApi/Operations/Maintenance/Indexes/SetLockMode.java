package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.IndexLockMode;
import net.ravendb.client.documents.operations.indexes.SetIndexesLockOperation;

public class SetLockMode {

    private interface IFoo {
        /*
        //region set_lock_mode_1
        public SetIndexesLockOperation(String indexName, IndexLockMode mode)
        public SetIndexesLockOperation(SetIndexesLockOperation.Parameters parameters)
        //endregion
        */
    }

    private static class Foo {
        //region set_lock_mode_2
        public enum IndexLockMode {
            UNLOCK,
            LOCKED_IGNORE,
            LOCKED_ERROR
        }
        //endregion

        //region set_lock_mode_3
        public static class Parameters {
            private String[] indexNames;
            private IndexLockMode mode;

            public String[] getIndexNames() {
                return indexNames;
            }

            public void setIndexNames(String[] indexNames) {
                this.indexNames = indexNames;
            }

            public IndexLockMode getMode() {
                return mode;
            }

            public void setMode(IndexLockMode mode) {
                this.mode = mode;
            }
        }
        //endregion
    }

    public SetLockMode() {
        try (IDocumentStore store = new DocumentStore()) {
            //region set_lock_mode_4
            store.maintenance().send(new SetIndexesLockOperation("Orders/Totals", IndexLockMode.LOCKED_IGNORE));
            //endregion

            //region set_lock_mode_5

            SetIndexesLockOperation.Parameters parameters = new SetIndexesLockOperation.Parameters();
            parameters.setIndexNames(new String[]{ "Orders/Totals", "Orders/ByCompany" });
            parameters.setMode(IndexLockMode.LOCKED_IGNORE);

            store.maintenance().send(new SetIndexesLockOperation(parameters));
            //endregion
        }
    }
}
