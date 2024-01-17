import { DocumentStore, AbstractCsharpIndexCreationTask } from "ravendb";

const store = new DocumentStore();

//region index_1
class Orders_ByCountries_BoostByField extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `from order in docs.Orders
             let company = LoadDocument(order.Company, "Companies")
             select new {
             
                 // Boost index-field 'ShipToCountry':
                 // * Use method 'Boost', pass a numeric value to boost by 
                 // * Documents that match the query criteria for this field will rank higher
                
                 ShipToCountry = order.ShipTo.Country.Boost(10),
                 CompanyCountry = company.Address.Country
             }`;
    }
}
//endregion

//region index_2
class Orders_ByCountries_BoostByIndexEntry extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `from order in docs.Orders
             let company = LoadDocument(order.Company, "Companies")
             select new {
                 ShipToCountry = order.ShipTo.Country,
                 CompanyCountry = company.Address.Country
             }
             
             // Boost the whole index-entry:
             // * Use method 'Boost'
             // * Pass a document-field that will set the boost level dynamically per document indexed.  
             // * The boost level will vary from one document to another based on the value of this field.
            
             .Boost(order.Freight)`;
    }
}
//endregion

async function boosting() {
    
    {
        const session = store.openSession();

        //region query_1
        const orders = await session
            .query({ indexName: "Orders/ByCountries/BoostByField" })
            .whereEquals("ShipToCountry", "Poland")
            .orElse()
            .whereEquals("CompanyCountry", "Portugal")
            .all();

        // Because index-field 'ShipToCountry' was boosted (inside the index definition),
        // then documents containing 'Poland' in their 'ShipTo.Country' field will get a higher score than
        // documents containing a company that is located in 'Portugal'.
        //endregion

        //region query_2
        const orders = await session
            .query({ indexName: "Orders/ByCountries/BoostByIndexEntry" })
            .whereEquals("ShipToCountry", "Poland")
            .orElse()
            .whereEquals("CompanyCountry", "Portugal")
            .all();

        // The resulting score per matching document is affected by the value of the document-field 'Freight'. 
        // Documents with a higher 'Freight' value will rank higher.
        //endregion
    }
}
