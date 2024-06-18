import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function queryTimeSeries() {
    {
        //region choose_range_1
        // Define the time series query part (expressed in RQL):
        const tsQueryText = `
            from HeartRates
            between "2020-05-17T00:00:00.0000000"
            and "2020-05-17T00:10:00.0000000"
            offset "03:00"`;

        // Define the query:
        const query = session.query({ collection: "employees" })
            .whereEquals("Address.Country", "UK")
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        // Execute the query:
        const results = await query.all();

        // Access entries results:
        rawResults = results[0];
        assert.equal((rawResults instanceof TimeSeriesRawResult), true);

        const tsEntry = rawResults.results[0];
        assert.equal((tsEntry instanceof TimeSeriesEntry), true);

        const tsValue = tsEntry.value;
        //endregion
        
        //region choose_range_2
        const from = new Date("2020-05-17T00:00:00.0000000");
        const to = new Date("2020-05-17T00:10:00.0000000");
        
        // Define the time series query part (expressed in RQL):
        const tsQueryText = `
            from HeartRates
            between $from and $to
            offset "03:00"`;

        // Define the query:
        const query = session.query({ collection: "employees" })
            .whereEquals("Address.Country", "UK")
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult)
            .addParameter("from", from)
            .addParameter("to", to);

        // Execute the query:
        const results = await query.all();

        // Access entries results:
        rawResults = results[0];
        assert.equal((rawResults instanceof TimeSeriesRawResult), true);

        const tsEntry = rawResults.results[0];
        assert.equal((tsEntry instanceof TimeSeriesEntry), true);

        const tsValue = tsEntry.value;
        //endregion
        
        //region choose_range_3
        const rql = `
            from "Employees" as employee
            where employee.Address.Country == "UK"
            select timeseries(
                from employee.HeartRates
                between "2020-05-17T00:00:00.0000000"
                and "2020-05-17T00:10:00.0000000"
                offset "03:00"
            )`;

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult);

        const result = await query.all();
        //endregion
        
        //region choose_range_4
        const rql = `
            from "Employees" as employee
            where employee.Address.Country == "UK"
            select timeseries(
                from employee.HeartRates
                between $from and $to
                offset "03:00"
            )`;

        const from = new Date("2020-05-17T00:00:00.0000000");
        const to = new Date("2020-05-17T00:10:00.0000000");

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult)
            .addParameter("from", from)
            .addParameter("to", to);

        const result = await query.all();
        //endregion

        //region choose_range_5
        // Define the time series query part (expressed in RQL):
        const tsQueryText = `
            from HeartRates
            last 30 min
            offset "03:00"`;

        // Define the query:
        const query = session.query({ collection: "employees" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        // Execute the query:
        const results = await query.all();
        //endregion

        //region choose_range_6
        const rql = `
            from "Employees" as employee
            select timeseries(
                from employee.HeartRates
                last 30 min
                offset "03:00"
            )`;

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult);

        const result = await query.all();
        //endregion
    }
}
