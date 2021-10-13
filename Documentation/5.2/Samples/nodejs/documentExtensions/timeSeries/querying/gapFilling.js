import {
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    IndexDefinitionBuilder, AbstractJavaScriptIndexCreationTask, TimeSeriesAggregationResult
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class People{}

    //region rql_query
    const query = session.advanced.rawQuery<TimeSeriesAggregationResult>(
        "from People\n" +
        "select timeseries(\n" +
        "    from HeartRates\n" +
        "    group by  1 second\n" +
        "    with interpolation(liner)\n" +
        ")"
    );
    //endregion
