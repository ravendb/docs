import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let statsCallback;
    const query = session.query({ collection: "test" });
    //region stats_1
    query.statistics(statsCallback);
    //endregion
}

async function example() {
    //region stats_2
    let stats;

    const employees = await session.query({ collection: "Employees" })
        .whereEquals("FirstName", "Robert")
        .statistics(s => stats = s)
        .all();

    const totalResults = stats.value.totalResults;
    const durationInMs = stats.value.durationInMs;
    // QueryStatistics {
    //   isStale: false,
    //   durationInMs: 744,
    //   totalResults: 1,
    //   skippedResults: 0,
    //   timestamp: 2018-09-24T05:34:15.260Z,
    //   indexName: 'Auto/Employees/ByFirstName',
    //   indexTimestamp: 2018-09-24T05:34:15.260Z,
    //   lastQueryTime: 2018-09-24T05:34:15.260Z,
    //   resultEtag: 8426908718162809000 }
    //endregion
}

