package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.IndexPriority;
import net.ravendb.client.documents.operations.indexes.SetIndexesPriorityOperation;

public class ChangeIndexPriority {

    private interface IFoo {
        /*
        //region set_priority_1
        public SetIndexesPriorityOperation(String indexName, IndexPriority priority) {
        public SetIndexesPriorityOperation(SetIndexesPriorityOperation.Parameters parameters)
        //endregion
        */
    }

    private static class Foo {
        //region set_priority_2
        public enum IndexPriority {
            LOW,
            NORMAL,
            HIGH
        }
        //endregion

        //region set_priority_3
        public static class Parameters {
            private String[] indexNames;
            private IndexPriority priority;

            public String[] getIndexNames() {
                return indexNames;
            }

            public void setIndexNames(String[] indexNames) {
                this.indexNames = indexNames;
            }

            public IndexPriority getPriority() {
                return priority;
            }

            public void setPriority(IndexPriority priority) {
                this.priority = priority;
            }
        }
        //endregion
    }

    public ChangeIndexPriority() {
        try (IDocumentStore store = new DocumentStore()) {
            //region set_priority_4
            store.maintenance().send(
                new SetIndexesPriorityOperation("Orders/Totals", IndexPriority.HIGH));
            //endregion

            //region set_priority_5
            SetIndexesPriorityOperation.Parameters parameters = new SetIndexesPriorityOperation.Parameters();
            parameters.setIndexNames(new String[]{ "Orders/Totals", "Orders/ByCompany" });
            parameters.setPriority(IndexPriority.LOW);

            store.maintenance().send(new SetIndexesPriorityOperation(parameters));
            //endregion
        }
    }
}
