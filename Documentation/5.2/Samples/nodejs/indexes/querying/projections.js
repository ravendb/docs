import {
    DocumentStore,
    AbstractIndexCreationTask,
    MoreLikeThisStopWords,
    QueryData, AbstractCsharpIndexCreationTask
} from "ravendb";

const store = new DocumentStore('http://127.0.0.1:8080', 'Northwind2');
const session = store.openSession();

class Employee { }

class EmployeeProjection { }

class FirstAndLastName { }

class ShipToAndProducts { }

class OrderProjection { }

class Company{}
class ContactDetails{}

class Order { }

class Total { }

class FullName {
    constructor(fullName) {
        this.fullName = fullName;
    }
}

//region indexes_1
class Employees_ByFirstAndLastName extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Employees.Select(employee => new {    
            FirstName = employee.FirstName,    
            LastName = employee.LastName
        })`;
    }
}
//endregion

//region indexes_1_stored
class Employees_ByFirstAndLastNameWithStoredFields extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Employees.Select(employee => new {    
            FirstName = employee.FirstName,    
            LastName = employee.LastName
        })`;

        this.storeAllFields("Yes"); // FirstName and LastName fields can be retrieved directly from index
    }
}
//endregion

//region indexes_2
class Employees_ByFirstNameAndBirthday extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Employees.Select(employee => new {    
            FirstName = employee.FirstName,    
            Birthday = employee.Birthday
        })`;
    }
}
//endregion

//region indexes_3
class Orders_ByShipToAndLines extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Orders.Select(order => new {" +
            "    ShipTo = order.ShipTo," +
            "    Lines = order.Lines" +
            "})";
    }
}
//endregion

//region indexes_4
class Orders_ByShippedAtAndCompany extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders.Select(order => new {    
            ShippedAt = order.ShippedAt,    
            Company = order.Company
        })`;
    }
}
//endregion


async function projections() {
    
        {
            //region projections_1
            const results = await session
                .query({ indexName: "Employees/ByFirstAndLastName" })
                .selectFields([ "FirstName", "LastName" ])
                .all();
            //endregion
        }

        {
            //region projections_1_stored
            const results = await session
                .query({ indexName: "Employees/ByFirstAndLastNameWithStoredFields" })
                .selectFields("FirstName", "LastName")
                .all();
            //endregion
        }

        {
            //region projections_2
            const queryData = new QueryData(
                [ "ShipTo", "Lines[].ProductName" ],
                [ "ShipTo", "Products" ]);

            const results = await session.query(Order)
                .selectFields(queryData, Order)
                .all();
            //endregion
        }

        {
            //region projections_3
            const results = await session.advanced.rawQuery(`from Employees as e select {    
                FullName : e.FirstName + " " + e.LastName 
            }`).all();
            //endregion
        }

        {
            //region projections_4
            const results = await session.advanced
                .rawQuery(`declare function output(e) {     
                        var format = function(p) { 
                            return p.FirstName + " " + p.LastName; 
                        };     
    
                        return { FullName : format(e) }; 
                    } from Employees as e select output(e)`)
                    .all();
                    
            //endregion
        }

        {
            //region projections_5
            const results = await session.advanced
                .rawQuery(
                    `from Orders as o load o.Company as c select {     
                        CompanyName: c.Name,    
                        ShippedAt: o.ShippedAt
                    }`).all();
            //endregion
        }

        {
            //region projections_6
            const results = await session.advanced
                .rawQuery(
                `from Employees as e select {     
                    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(),     
                    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1,     
                    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
                    }`).all();
            //endregion
        }

        {
            //region projections_7
            const results = await session.advanced
                .rawQuery(
                    `from Employees as e select {     
                        Date : new Date(Date.parse(e.Birthday)),     
                        Name : e.FirstName.substr(0,3) 
                    }`).all();
            //endregion
        }

        {
            //region projections_8
            const results = await session.advanced
                .rawQuery(`from Employees as e select {     
                    Name : e.FirstName,      
                    Metadata : getMetadata(e)
                }`).all();
            //endregion
        }

        {
            //region projections_9
            const results = await session.advanced.rawQuery(
                `from Orders as o select {     
                    Total: o.Lines.reduce(
                        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0) 
                    }`).all();
            //endregion
        }
}

{

    {
        //region index_10
        class Companies_ByContact  extends AbstractCsharpIndexCreationTask {
            constructor() {
                super();

                this.map = `docs.Company.select(company => new {    
                    Name = company.Contact.Name,    
                    Phone = company.Phone
                })`;
            }
        }
        //endregion
    }

    //region
}

