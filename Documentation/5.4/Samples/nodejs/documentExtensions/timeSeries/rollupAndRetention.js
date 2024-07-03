import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function queryTimeSeries() {
    {
        //region rollup_1
        // Define a policy on the RAW time series data:
        // ============================================
        const rawPolicy = new RawTimeSeriesPolicy(TimeValue.ofYears(5)); // Retain data for five years

        // Define a ROLLUP policy: 
        // =======================
        const rollupPolicy = new TimeSeriesPolicy(
            "By1WeekFor1Year",      // Name of policy
            TimeValue.ofDays(7),    // Aggregation time, roll-up the data for each week
            TimeValue.ofYears(5));  // Retention time, keep data for five years

        // Define the time series configuration for collection "Companies" (use above policies):
        // =====================================================================================
        const collectionConfig = new TimeSeriesCollectionConfiguration();        
        collectionConfig.rawPolicy = rawPolicy;
        collectionConfig.policies = [rollupPolicy];

        const timeSeriesConfig = new TimeSeriesConfiguration();
        timeSeriesConfig.collections.set("Companies", collectionConfig);

        // Deploy the time series configuration to the server
        // by sending the 'ConfigureTimeSeriesOperation' operation:
        // ========================================================
        await documentStore.maintenance.send(new ConfigureTimeSeriesOperation(timeSeriesConfig));

        // NOTE:
        // The time series entries in the RavenDB sample data are dated up to the year 2020.
        // To ensure that you see the rollup time series created when running this example,
        // the retention time should be set to exceed that year.
        //endregion
        
        //region rollup_2
        // Get all data from the RAW time series:
        // ======================================

        const rawData = await session
            .timeSeriesFor("companies/91-A", "StockPrices")
            .get();
        
        // Get all data from the ROLLUP time series:
        // =========================================

        // Either - pass the rollup name explicitly to 'TimeSeriesFor':
        let rollupData = await session
            .timeSeriesFor("companies/91-A", "StockPrices@By1WeekFor1Year")
            .get();

        // Or - get the rollup name by calling 'GetTimeSeriesName':
        rollupData = await session
            .timeSeriesFor("companies/91-A", rollupPolicy.getTimeSeriesName("StockPrices"))
            .get();
        
        // The raw time series has 100 entries
        assert.equal(rawData.length, 100);
        assert.equal(rawData[0].isRollup, false);
        
        // The rollup time series has only 22 entries
        // as each entry aggregates 1 week's data from the raw time series
        assert.equal(rollupData.length, 22);
        assert.equal(rollupData[0].isRollup, true);
        //endregion
    }
}

//region syntax_1
class RawTimeSeriesPolicy extends TimeSeriesPolicy {
    retentionTime;  // TimeValue
}

class TimeSeriesPolicy {
    name;           // string;
    retentionTime   // TimeValue
    aggregationTime // TimeValue
}
//endregion

//region syntax_2
class TimeValue {
    static ofSeconds(seconds);
    static ofMinutes(minutes);
    static ofHours(hours);
    static ofDays(days); 
    static ofMonths(months);
    static ofYears(years);
}
//endregion

//region syntax_3
class TimeSeriesConfiguration {
    collections; // Map<string, TimeSeriesCollectionConfiguration>
}

class TimeSeriesCollectionConfiguration {
    disabled;  // boolean
    policies;  // TimeSeriesPolicy[]
    rawPolicy; // RawTimeSeriesPolicy
}
//endregion

//region syntax_4
ConfigureTimeSeriesOperation(configuration);
//endregion
