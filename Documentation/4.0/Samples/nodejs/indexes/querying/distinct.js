import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region index
class Orders_ByShipToCountry extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // The map phase indexes the country listed in each order document
        // countryCount is assigned with 1, which will be aggregated in the reduce phase
        this.map("Orders", order => {
            return {
                country: order.ShipTo.Country,
                countryCount: 1
            }
        });

        // The reduce phase will group the country results and aggregate the countryCount
        this.reduce(results => results.groupBy(x => x.country).aggregate(g => {
            return {
                country: g.key,
                countryCount: g.values.reduce((p, c) => p + c.countryCount, 0)
            }
        }));
    }
}
//endregion

class Order { }

async function distinct() {
        {
            //region distinct_1_1
            // Get a sorted list without duplicates:
            // =====================================
            
            const countries = await session
                .query(Order)
                .orderBy("ShipTo.Country")
                .selectFields("ShipTo.Country")
                 // Call 'distinct' to remove duplicates from results
                 // Items wil be compared based on field 'Country' that is specified in the above 'selectFields' 
                .distinct()
                .all();

            // Running this on the Northwind sample data
            // will result in a sorted list of 21 countries w/o duplicates.
            //endregion
        }
        {
            //region distinct_2_1
            // Count the number of unique countries:
            // =====================================
            
            const numberOfCountries = await session
                .query(Order)
                .selectFields("ShipTo.Country")
                .distinct()
                .count();

            // Running this on the Northwind sample data,
            // will result in 21, which is the number of unique countries.
            //endregion
        }
        {
            //region distinct_3_1
            // Query the map - reduce index defined above
            const session = documentStore.openSession();
            const queryResult = await session
                .query({ indexName: "Orders/ByShipToCountry" })
                .all();

            // The resulting list contains all index-entry items where each entry represents a country. 
            // The size of the list corresponds to the number of unique countries.
            const numberOfUniqueCountries = queryResult.length;
            //endregion
        } 
}

