import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function aggregatingValues() {
    {
        //region aggregation_1
        // Define the time series query part (expressed in RQL):
        const tsQueryText = `
            from HeartRates
            // Use 'group by' to group the time series entries by the specified time frame
            group by "1 hour"
            // Use 'select' to choose aggregation functions that will be evaluated
            // Project the lowest and highest value of each group
            select min(), max()`;

        // Define the query:
        const query = session.query({ collection: "employees" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        // Execute the query:
        const results = await query.all();
        //endregion

        //region aggregation_2
        const tsQueryText = `
            from StockPrices
            // Query stock price behavior when trade volume is high
            where Values[4] > 500000 
            // Group entries into consecutive 7-day groups
            group by "7 day"
            // Project the lowest and highest value of each group         
            select max(), min()`;

        const query = session.query({ collection: "companies" })
            .whereEquals("Address.Country", "USA")
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion

        //region aggregation_3
        const tsQueryText = `
            from StockPrices
            where Values[4] > 500_000
            select max(), min()`;

        const query = session.query({ collection: "companies" })
            .whereEquals("Address.Country", "USA")
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion

        //region aggregation_4
        const tsQueryText = `
            from StockPrices
            // Load the referenced document into variable 'employee'
            load Tag as employee
            // Filter entries by the 'Title' field of the employee document
            where employee.Title == "Sales Representative"
            group by "1 month"
            select min(), max()`;

        const query = session.query({ collection: "companies" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion

        //region aggregation_5
        const tsQueryText = `
            from StockPrices
            // Use the 'tag' keyword to perform a secondary grouping by the entries' tags
            // Group by months and by tag
            group by "6 months", tag
            // Project the highest and lowest values of each group  
            select max(), min()`;

        const query = session.query({ collection: "companies" })
            .selectTimeSeries(b => b.raw(tsQueryText), TimeSeriesRawResult);

        const results = await query.all();
        //endregion
    }
}