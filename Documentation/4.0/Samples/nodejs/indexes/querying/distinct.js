import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region distinct_3_1
class Employees_ByCountry extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // The map phase indexes the country listed in each employee document
        // countryCount is assigned with 1, which will be aggregated in the reduce phase
        this.map("Employees", employee => {
            return {
                country: employee.Address.Country,
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
            // returns sorted list of countries w/o duplicates
            const countries = await session
                .query(Order)
                .orderBy("ShipTo.Country")
                .selectFields("ShipTo.Country")
                .distinct()
                .all();
            //endregion
        }

        {
            //region distinct_2_1
            // results will contain the number of unique countries
            const numberOfCountries = await session
                .query(Order)
                .selectFields("ShipTo.Country")
                .distinct()
                .count();
            //endregion
        }

        {
            //region distinct_3_2
            // Query the map - reduce index defined above
            const session = documentStore.openSession();
            const queryResult = await session.query({ indexName: 'Employees/ByCountry' })
                .whereEquals('country', country)
                .firstOrNull();

            const numberOfEmployeesFromCountry = queryResult != null ? queryResult.countryCount : 0;
            //endregion
        }
 
}

