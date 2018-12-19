import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region distinct_3_1
class Order_Countries extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders.Select(o => new {    
                Country = o.ShipTo.Country
            })`;

        this.reduce = `results.GroupBy(r => r.Country)
            .Select(g => new {    
                Country = g.Key
            })`;
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
            const numberOfCountries = await session
                .query(Order)
                .selectFields("ShipTo.Country")
                .distinct()
                .count();
            //endregion
        }

        {
            //region distinct_3_2
            const numberOfCountries = await session
                .query({ indexName: "Order/Countries" })
                .count();
            //endregion
        }
 
}

