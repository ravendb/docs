package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.DatabaseStatistics;
import net.ravendb.client.documents.operations.GetStatisticsOperation;

public class Statistics {

    private interface IFoo {
        /*
        //region stats_1
        public GetStatisticsOperation()
        //endregion
        */
    }

    public Statistics() {
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_2
            DatabaseStatistics stats = store.maintenance().send(new GetStatisticsOperation());
            long countOfDocuments = stats.getCountOfDocuments();
            int countOfIndexes = stats.getCountOfIndexes();
            boolean isStale = stats.getIndexes()[0].isStale();
            //endregion
        }
    }
}
