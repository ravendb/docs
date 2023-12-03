import * as assert from "assert";
import {DocumentStore, QueryData, AbstractCsharpIndexCreationTask} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

async function examples() {
    {
        //region projections_1
        // Make a dynamic query on the Companies collection
        const projectedResults = await session.query({ collection: "companies" })
             // Call 'selectFields'
             // Pass a list of fields that will be returned per Company document
            .selectFields([ "Name", "Address.City", "Address.Country"])
            .all();
        
        // Each resulting object in the list is Not a 'Company' entity,
        // it is a new object containing ONLY the fields specified in the selectFields method.
        //endregion
    }
    {
        //region projections_2
        // Define a QueryData object that will be used in the selectFields method
        const queryData = new QueryData(
            // Specify the document-fields you want to project from the document
            [ "Name", "Address.City", "Address.Country"],
            // Provide an ALIAS name for each document-field
            [ "CompanyName", "City", "Country"]);
        
        const projectedResults = await session.query({ collection: "companies" })
            // Call 'selectFields', pass the queryData object
            .selectFields(queryData)
            .all();

        // Each resulting object in the list is Not a 'Company' entity,
        // it is a new object containing ONLY the fields specified in the selectFields method
        // using their corresponding alias names.
        //endregion
    }    
    {
        //region projections_3
        // Define the projection with QueryData if you wish to use alias names
        const queryData = new QueryData(
            // Project the 'ShipTo' object and all product names from the Lines array in the document
            [ "ShipTo", "Lines[].ProductName" ],
            // Assign alias names
            [ "ShipTo", "ProductNames" ]);

        const projectedResults = await session.query({ collection: "orders" })
            .selectFields(queryData)
            .all();
        //endregion
    }
    {
        //region projections_4
        // Define the projected data expression within a custom function.
        // Any expression can be provided for the projected content.
        const queryData = QueryData.customFunction("e", `{
            FullName: e.FirstName + " " + e.LastName 
        }`);

        const projectedResults = await session.query({ collection: "employees" })
            .selectFields(queryData)
            .all();
        //endregion
    }
    {
        //region projections_5
        const projectedResults = await session.advanced
             // Can provide an RQL query via the 'rawQuery' method
            .rawQuery(`from "Orders" as x
                       // Using JavaScript object literal syntax:
                       select {
                           // Any calculations can be done within the projection
                           TotalProducts: x.Lines.length,
                           TotalDiscountedProducts: x.Lines.filter(x => x.Discount > 0).length,
                           TotalPrice: x.Lines
                                        .map(l => l.PricePerUnit * l.Quantity)
                                        .reduce((a, b) => a + b, 0) }`)
            .all();
        //endregion
    }
    {
        //region projections_6
        const projectedResults = await session.advanced
            .rawQuery(`// Declare a function
                       declare function output(e) {
                           var format = p => p.FirstName + " " + p.LastName;
                           return { 
                               FullName: format(e)
                           };
                       }
                       // Call the function from the projection
                       from "employees" as e select output(e)`)
            .all();
        //endregion
    }
    {
        //region projections_7
        const projectedResults = await session.advanced
            .rawQuery(`from "Orders" as o
                       load o.Company as c         // load the related Company document
                       select {
                           CompanyName: c.Name,    // info from the related Company document
                           ShippedAt: o.ShippedAt  // info from the Order document
                       }`)
            .all();
        //endregion
    }
    {
        //region projections_8
        const projectedResults = await session.advanced
            .rawQuery(`from "employees" as e
                       select {
                           DayOfBirth: new Date(Date.parse(e.Birthday)).getDate(),
                           MonthOfBirth: new Date(Date.parse(e.Birthday)).getMonth() + 1,
                           Age: new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear()
                       }`)
            .all();
        //endregion
    }
    {
        //region projections_9
        const projectedResults = await session.advanced
            .rawQuery(`from "employees" as e
                       select {
                           Name: e.FirstName,
                           Metadata: getMetadata(e) // Get the metadata
                       }`)
            .all();
        //endregion
    }
}

{
    //region syntax
    selectFields(property);
    selectFields(properties);
    
    selectFields(property, projectionClass);
    selectFields(properties, projectionClass);
    selectFields(properties, projectionClass, projectionBehavior);
    
    selectFields(queryData, projectionClass);
    selectFields(queryData, projectionClass, projectionBehavior);
    //endregion
}
