package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.CollectionStatistics;
import net.ravendb.client.documents.operations.GetCollectionStatisticsOperation;

import java.util.HashMap;
import java.util.Map;

public class CollectionStats {

    private interface IFoo {
        /*
        //region stats_1
        public GetCollectionStatisticsOperation()
        //endregion
        */
    }

    private static class Foo {
        //region stats_2
        public class CollectionStatistics {

            private int countOfDocuments;
            private int countOfConflicts;
            private Map<String, Long> collections;

            public CollectionStatistics() {
                collections = new HashMap<>();
            }

            public Map<String, Long> getCollections() {
                return collections;
            }

            public void setCollections(Map<String, Long> collections) {
                this.collections = collections;
            }

            public int getCountOfDocuments() {
                return countOfDocuments;
            }

            public void setCountOfDocuments(int countOfDocuments) {
                this.countOfDocuments = countOfDocuments;
            }

            public int getCountOfConflicts() {
                return countOfConflicts;
            }

            public void setCountOfConflicts(int countOfConflicts) {
                this.countOfConflicts = countOfConflicts;
            }
        }
        //endregion
    }

    public CollectionStats() {
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_3
            CollectionStatistics stats = store.maintenance().send(new GetCollectionStatisticsOperation());
            //endregion
        }
    }
}
