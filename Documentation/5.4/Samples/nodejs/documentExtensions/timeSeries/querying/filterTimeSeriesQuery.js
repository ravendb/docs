import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function filterTimeSeriesQuery() {
    {
        //region filter_entries_1
        // For example, in the "HeartRates" time series,
        // retrieve only entries where the value exceeds 75 BPM 
        
        const tsQueryText = `
            from HeartRates
            between "2020-05-17T00:00:00.0000000"
            and "2020-05-17T00:10:00.0000000"
            // Use the "where Value" clause to filter entries by the value
            where Value > 75`;
        
        const query = session.query({ collection: "employees" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);
        
        const results = await query.all();
        //endregion

        //region filter_entries_2
        // For example, in the "HeartRates" time series,
        // retrieve only entries where the value exceeds 75 BPM 
        
        const rql = `
            from Employees
            select timeseries (
                from HeartRates
                between "2020-05-17T00:00:00.0000000"
                and "2020-05-17T00:10:00.0000000"
                // Use the 'where Value' clause to filter by the value
                where Value > 75
            )`;

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult);

        const result = await query.all();
        //endregion

        //region filter_entries_3
        // Retrieve only entries where the tag string content is "watches/fitbit"

        const tsQueryText = `
            from HeartRates
            between "2020-05-17T00:00:00.0000000"
            and "2020-05-17T00:10:00.0000000"
            // Use the "where Tag" clause to filter entries by the tag string content
            where Tag == "watches/fitbit"`;

        const query = session.query({ collection: "employees" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion

        //region filter_entries_4
        // Retrieve only entries where the tag string content is "watches/fitbit"
        
        const rql = `
            from Employees
            select timeseries (
                from HeartRates
                between "2020-05-17T00:00:00.0000000"
                and "2020-05-17T00:10:00.0000000"
                // Use the 'where Tag' clause to filter entries by the tag string content
                where Tag == 'watches/fitbit'
            )`;

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult);

        const result = await query.all();
        //endregion

        //region filter_entries_5
        // Retrieve only entries where the tag string content is one of several options
        
        const tsQueryText = `
            from HeartRates
            between "2020-05-17T00:00:00.0000000"
            and "2020-05-17T00:10:00.0000000"
            // Use the "where Tag in" clause to filter by various tag options
            where Tag in ("watches/apple", "watches/samsung", "watches/xiaomi")`;

        const query = session.query({ collection: "employees" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion

        //region filter_entries_6
        // Retrieve only entries where the tag string content is one of several options

        const optionalTags = ["watches/apple", "watches/samsung", "watches/xiaomi"];
        
        const rql = `
            from Employees
            select timeseries (
                from HeartRates
                between "2020-05-17T00:00:00.0000000"
                and "2020-05-17T00:10:00.0000000"
                // Use the 'where Tag in' clause to filter by various tag options
                where Tag in ($optionalTags)
            )`;

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult)
            .addParameter("optionalTags", optionalTags);

        const result = await query.all();
        //endregion

        //region filter_entries_7
        // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property
        
        const tsQueryText = `
            from StockPrices
                // Use 'load Tag' to load the employee document referenced in the tag
                load Tag as employeeDoc
                // Use 'where <property>' to filter entries by the properties of the loaded document
                where employeeDoc.Title == "Sales Manager"`;

        const query = session.query({ collection: "companies" })
            .whereEquals("Address.Country", "USA")
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion

        //region filter_entries_8
        // Retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

        const rql = `
            from Companies
            where Address.Country == 'USA'
            select timeseries (
                from StockPrices
                // Use 'load Tag' to load the employee document referenced in the tag
                load Tag as employeeDoc
                // Use 'where <property>' to filter entries by the properties of the loaded document 
                where employeeDoc.Title == 'Sales Manager'
            )`;

        const query = session.advanced.rawQuery(rql, TimeSeriesRawResult);

        const result = await query.all();
        //endregion
    }
}



