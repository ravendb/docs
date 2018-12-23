package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.DetailedDatabaseStatistics;
import net.ravendb.client.documents.operations.GetDetailedStatisticsOperation;

public class DetailedStatistics {

    private interface IFoo {
        /*
        //region stats_1
        public GetDetailedStatisticsOperation();
        //endregion
        */
    }

    public DetailedStatistics() {
        try (IDocumentStore store = new DocumentStore()) {
            //region stats_2
            DetailedDatabaseStatistics stats
                = store.maintenance().send(new GetDetailedStatisticsOperation());
            long countOfDocuments = stats.getCountOfDocuments();
            int countOfIndexes = stats.getCountOfIndexes();
            long countOfCompareExchange = stats.getCountOfCompareExchange();
            long countOfIdentities = stats.getCountOfIdentities();
            boolean stale = stats.getIndexes()[0].isStale();
            //endregion
        }
    }
}
