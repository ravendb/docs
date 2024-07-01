import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function indexing() {

    //region index_1
    class StockPriceTimeSeriesFromCompanyCollection extends
        AbstractRawJavaScriptTimeSeriesIndexCreationTask {
        
        constructor() {
            super();

            this.maps.add(`
                // Call timeSeries.map(), pass:
                // * The collection to index
                // * The time series name
                // * The fuction that defines the index-entries
                // ============================================
                timeSeries.map("Companies", "StockPrices", function (segment) {
                     
                     // Return the index-entries:
                     // =========================
                     return segment.Entries.map(entry => {
                          let employee = load(entry.Tag, "Employees");
                     
                         // Define the index-fields per entry:
                         // ==================================
                         
                         return {
                             // Retrieve content from the time series ENTRY:
                             tradeVolume: entry.Values[4],
                             date: new Date(entry.Timestamp),
                             
                             // Retrieve content from the SEGMENT:
                             companyID: segment.DocumentId,
                             
                             // Retrieve content from the loaded DOCUMENT:
                             employeeName: employee.FirstName + " " + employee.LastName
                         };
                     });
                })
            `);
        }
    }
    //endregion
    
    //region index_2
    class AllTimeSeriesFromCompanyCollection extends AbstractRawJavaScriptTimeSeriesIndexCreationTask {

        constructor() {
            super();
         
            this.maps.add(`
                // Call timeSeries.map(), pass:
                // * The collection to index and the function that defines the index-entries
                // * No time series is specified - so ALL time series from the collection will be indexed
                // ======================================================================================
                timeSeries.map("Companies", function (segment) {
                     
                     return segment.Entries.map(entry => ({
                         value: entry.Value,
                         date: new Date(entry.Timestamp)
                     }));
                })
            `);
        }
    }
    //endregion
    
    //region index_3
    class AllTimeSeriesFromAllCollections extends AbstractRawJavaScriptTimeSeriesIndexCreationTask {

        constructor() {
            super();

            this.maps.add(`
                // No collection and time series are specified -
                // so ALL time series from ALL collections will be indexed
                // =======================================================
                timeSeries.map(function (segment) {
                     
                     return segment.Entries.map(entry => ({
                         value: entry.Value,
                         date: new Date(entry.Timestamp),
                         documentID: segment.DocumentId,
                     }));
                })
            `);
        }
    }
    //endregion
    
    //region index_4
    class Vehicles_ByLocation  extends AbstractRawJavaScriptTimeSeriesIndexCreationTask {
        
        constructor() {
            super();

            // Call 'timeSeries.map()' for each collection you wish to index
            // =============================================================
            
            this.maps.add(`
                timeSeries.map("Planes", "GPS_Coordinates", function (segment) {
                     
                     return segment.Entries.map(entry => ({
                          latitude: entry.Values[0],
                          longitude: entry.Values[1],
                          date: new Date(entry.Timestamp),
                          documentID: segment.DocumentId
                     }));
                })
            `);

            this.maps.add(`
                timeSeries.map("Ships", "GPS_Coordinates", function (segment) {
                     
                     return segment.Entries.map(entry => ({
                          latitude: entry.Values[0],
                          longitude: entry.Values[1],
                          date: new Date(entry.Timestamp),
                          documentID: segment.DocumentId
                     }));
                })
            `);
        }
    }
    //endregion

    //region index_5
    class TradeVolume_PerDay_ByCountry extends AbstractRawJavaScriptTimeSeriesIndexCreationTask {

        constructor() {
            super();

            // Define the Map part:
            this.maps.add(`
                timeSeries.map("Companies", "StockPrices", function (segment) {
                     
                     return segment.Entries.map(entry => {
                          let company = load(segment.DocumentId, "Companies");
                         
                         return {
                             date: new Date(entry.Timestamp),
                             country: company.Address.Country,
                             totalTradeVolume: entry.Values[4],
                         };
                     });
                })
            `);

            // Define the Reduce part:
            this.reduce = `
                groupBy(x => ({date: x.date, country: x.country}))
                    .aggregate(g => {
                        return {
                            date: g.key.date,
                            country: g.key.country,
                            totalTradeVolume: g.values.reduce((sum, x) => x.totalTradeVolume + sum, 0)
                        };
                    })
            `;
        }
    }
    //endregion

    {
        //region index_definition_1
        const timeSeriesIndexDefinition = new TimeSeriesIndexDefinition();
        
        timeSeriesIndexDefinition.name = "StockPriceTimeSeriesFromCompanyCollection";
        
        timeSeriesIndexDefinition.maps = new Set([`
            from segment in timeSeries.Companies.StockPrices
            from entry in segment.Entries

            let employee = LoadDocument(entry.Tag, "Employees")

            select new
            {
                tradeVolume = entry.Values[4],
                date = entry.Timestamp.Date,
                companyID = segment.DocumentId,
                employeeName = employee.FirstName + " " + employee.LastName
            }`
        ]);

        // Deploy the index to the server via 'PutIndexesOperation'
        await documentStore.maintenance.send(new PutIndexesOperation(timeSeriesIndexDefinition));
        //endregion
        
        //region query_1
        const results = await session
             // Retrieve time series data for the specified company:
             // ====================================================
            .query({ indexName: "StockPriceTimeSeriesFromCompanyCollection" })
            .whereEquals("companyID", "Companies/91-A")
            .all();
        
        // Results will include data from all 'StockPrices' entries in document 'Companies/91-A'. 
        //endregion

        //region query_2
        const results = await session
             // Find what companies had a very high trade volume:
             // ==================================================
            .query({ indexName: "StockPriceTimeSeriesFromCompanyCollection" })
            .whereGreaterThan("tradeVolume", 150_000_000)
            .selectFields(["companyID"])
            .distinct()
            .all();

        // Results will contain company "Companies/65-A"
        // since it is the only company with time series entries having such high trade volume.
        //endregion
    }
}


