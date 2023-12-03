import {DocumentStore, QueryData, AbstractJavaScriptIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region index_1
class Employees_ByNameAndTitle extends AbstractJavaScriptIndexCreationTask  {
    constructor() {
        super();

        this.map("Employees", e => {
            return {
                FirstName: e.FirstName,
                LastName: e.LastName,
                Title: e.Title
            };
        });
    }
}
//endregion

//region index_1_stored
class Employees_ByNameAndTitleWithStoredFields extends AbstractJavaScriptIndexCreationTask  {
    constructor() {
        super();

        this.map("Employees", e => {
            return {
                FirstName: e.FirstName,
                LastName: e.LastName,
                Title: e.Title
            };
        });

        // Store some fields in the index:
        this.store('FirstName', 'Yes');
        this.store('LastName', 'Yes');
    }
}
//endregion

//region index_2
class Orders_ByCompanyAndShipToAndLines extends AbstractJavaScriptIndexCreationTask  {
    constructor() {
        super();

        this.map("Orders", o => {
            return {
                Company : o.Company,
                ShipTo: o.ShipTo,
                Lines: o.Lines
            };
        });
    }
}
//endregion

//region index_3
class Orders_ByCompanyAndShippedAt extends AbstractJavaScriptIndexCreationTask  {
    constructor() {
        super();

        this.map("Orders", o => {
            return {
                Company: o.Company,
                ShippedAt: o.ShippedAt
            };
        });
    }
}
//endregion

//region index_4
class Employees_ByFirstNameAndBirthday extends AbstractJavaScriptIndexCreationTask  {
    constructor() {
        super();

        this.map("Employees", e => {
            return {
                FirstName: e.FirstName,
                Birthday: e.Birthday
            };
        });
    }
}
//endregion

async function projections() {
    {
        //region projections_1
        // Alias names for the projected fields can be defined using a QueryData object 
        const queryData = new QueryData(
            ["FirstName", "LastName"],                   // Document-fields to project
            ["EmployeeFirstName ", "EmployeeLastName"]); // An alias for each field

        const projectedResults = await session
             // Query the index
            .query({indexName: "Employees/ByNameAndTitle"})
             // Can filter by any index-field, e.g.filter by index-field 'Title'
            .whereEquals('Title', 'sales representative')
             // Call 'selectFields' 
             // Only the fields defined in 'queryData' will be returned per matching document
            .selectFields(queryData)
            .all();

        // Each resulting object in the list is Not an 'Employee' entity,
        // it is a new object containing ONLY the fields specified in the selectFields method
        // ('EmployeeFirstName' & 'EmployeeLastName').
        //endregion
    }
    {
        //region projections_1_stored
        const projectedResults = await session
            .query({ indexName: "Employees/ByNameAndTitleWithStoredFields" })
             // Call 'selectFields' 
             // Project fields 'FirstName' and 'LastName' which are STORED in the index
            .selectFields(["FirstName", "LastName"])
            .all();
        //endregion
    }
    {
        //region projections_2
        const queryData = new QueryData(
            // Retrieve the City property from the ShipTo object
            // and all product names from the Lines array
            [ "ShipTo.City", "Lines[].ProductName" ],
            [ "ShipToCity", "Products" ]);

        const projectedResults = await session
            .query({ indexName: "Employees/ByCompanyAndShipToAndLines" })
            .selectFields(queryData)
            .all();
        //endregion
    }
    {
        //region projections_3
        // Define the projected data expression within a custom function.
        // Any expression can be provided for the projected content.
        const queryData = QueryData.customFunction("e", `{
            FullName: e.FirstName + " " + e.LastName 
        }`);

        const projectedResults = await session
            .query({indexName: "Employees/ByNameAndTitle"})
            .selectFields(queryData)
            .all();
        //endregion
    }
    {
        //region projections_4
        const projectedResults = await session.advanced
            .rawQuery(`from index "Orders/ByCompanyAndShipToAndLines" as x
                       select {
                           // Any calculations can be done within a projection
                           TotalProducts: x.Lines.length,
                           TotalDiscountedProducts: x.Lines.filter(x => x.Discount > 0).length,
                           TotalPrice: x.Lines
                                        .map(l => l.PricePerUnit * l.Quantity)
                                        .reduce((a, b) => a + b, 0)
                       }`)
            .all();
        //endregion
    }
    {
        //region projections_5
        const projectedResults = await session.advanced
            .rawQuery(`// Define a function
                       declare function output(x) {
                           var format = p => p.FirstName + " " + p.LastName;
                           return { FullName: format(x) };
                       }
                        
                       from index "Employees/ByNameAndTitle" as e
                       select output(e)`)  // Call the function from the projection
            .all();
        //endregion
    }
    {
        //region projections_6
        const projectedResults = await session.advanced
            .rawQuery(`from index "Orders/ByCompanyAndShippedAt" as o
                       load o.Company as c         // Load the related document to use in the projection
                       select {
                           CompanyName: c.Name,    // Info from the related Company document
                           ShippedAt: o.ShippedAt  // Info from the Order document
                       }`)
            .all();
        //endregion
    }
    {
        //region projections_7
        const projectedResults = await session.advanced
            .rawQuery(`from index "Employees/ByFirstNameAndBirthday" as x
                       select {
                           DayOfBirth: new Date(Date.parse(x.Birthday)).getDate(),
                           MonthOfBirth: new Date(Date.parse(x.Birthday)).getMonth() + 1,
                           Age: new Date().getFullYear() - new Date(Date.parse(x.Birthday)).getFullYear()
                       }`)
            .all();
        //endregion
    }
    {
        //region projections_8
        const projectedResults = await session.advanced
            .rawQuery(`from index "Employees/ByFirstNameAndBirthday" as x
                       select {
                           Name: x.FirstName,
                           Metadata: getMetadata(x) // Get the metadata
                       }`)
            .all();
        //endregion
    }
    {
        //region projections_9
        const projectedResults = await session
            .query({ indexName: "Employees/ByNameAndTitleWithStoredFields" })
             // Pass the requested projection behavior to the 'SelectFields' method
            .selectFields(["FirstName", "Title"], ProjectionClass, "FromIndexOrThrow")
            .all();
        //endregion
    }
}

//region projection_class
class ProjectionClass {
    constructor(firstName, title) {
        // The projection class contains field names from the index-fields
        this.FirstName = firstName;
        this.Title = title;
    }
}
//endregion

