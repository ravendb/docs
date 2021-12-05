import * as assert from "assert";
import {DocumentStore, QueryData, AbstractCsharpIndexCreationTask} from "ravendb";

class Product { }
class Companies { }
class Orders { }

const store = new DocumentStore();
const session = store.openSession();

async function examples() {
    {
        //region projections_1
        // request name, city and country for all entities from 'Companies' collection
        const queryData = new QueryData(
            [ "Name", "Address.City", "Address.Country"],
            [ "Name", "City", "Country"]);
        const results = await session
            .query(Companies)
            .selectFields(queryData,Companies)
            .all();
        //endregion
    }

    {
        //region projections_2
        const queryData = new QueryData(
            [ "ShipTo", "Lines[].ProductName" ],
            [ "ShipTo", "Products" ]);

        const results = await session.query(Orders)
            .selectFields(queryData,Orders)
            .all();
        //endregion
    }

    {
        //region projections_3
        const results = await session.advanced
            .rawQuery(
            `from Employees as e select {
                FullName: e.FirstName + " " + e.LastName 
            }`)
            .all();
        //endregion
    }

    {
        //region projections_4
        const results = await session.advanced
            .rawQuery(`from Orders as o select { 
                Total : o.Lines.reduce( 
                    (acc , l) => 
                        acc += l.PricePerUnit * l.Quantity, 0) 
            }`)
            .all();
        //endregion
    }

    {
        //region projections_5
        const results = await session.advanced
            .rawQuery(`from Orders as o 
            load o.Company as c 
            select { 
                CompanyName: c.Name,
                ShippedAt: o.ShippedAt
            }`)
            .all();
        //endregion
    }

    {
        //region projections_6
        const results = await session.advanced
            .rawQuery(
                `from Employees as e 
                select { 
                    DayOfBirth: new Date(Date.parse(e.Birthday)).getDate(), 
                    MonthOfBirth: new Date(Date.parse(e.Birthday)).getMonth() + 1, 
                    Age: new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() 
                }`)
            .all();
        //endregion
    }

    {
        //region projections_7
        const results = session.advanced
            .rawQuery(
                `from Employees as e 
                select { 
                    Date : new Date(Date.parse(e.Birthday)), 
                    Name : e.FirstName.substr(0,3) 
                }`)
            .all();
        //endregion
    }

    {
        //region projections_8
        const results = await session.query({ indexName: "Companies/ByContact" })
            .selectFields([ "Name", "Phone" ])
            .all();
        //endregion
    }

    {
        //region projections_10
        // query index 'Products_BySupplierName'
        // return documents from collection 'Products' that have a supplier 'Norske Meierier'
        // project them to instance of Product
        const results = await session.query({ indexName: "Products/BySupplierName" })
            .whereEquals("Name", "Norske Meierier")
            .ofType(Product)
            .all();
        assert.ok(results[0] instanceof Product);
        //endregion
    }

    {
        //region projections_12
        const results = await session.advanced
            .rawQuery(
                `declare function output(e) { 
                    var format = function(p){ return p.FirstName + " " + p.LastName; }; 
                    return { FullName : format(e) }; 
                } 
                from Employees as e select output(e)`)
            .all();
        //endregion
    }

    {
        //region projections_13
        const results = await session.advanced
            .rawQuery(
                `from Employees as e 
                select {
                     Name : e.FirstName, 
                     Metadata : getMetadata(e)
                }`)
            .all();
        //endregion
    }

    //region projections_9_0
    class Companies_ByContact extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Company.select(company => new {    
                                Name = company.Contact.Name,    
                                Phone = company.Phone
                            })`;
            this.storeAllFields("Yes"); // name and phone fields can be retrieved directly from index
        }
    }
    //endregion

    //region projections_9_1
    class ContactDetails {
        constructor(name, phone) {
            this.name = name;
            this.phone = phone;
        }
    }
    //endregion

    //region projections_11
    class Products_BySupplierName_Result { }

    class Products_BySupplierName extends AbstractCsharpIndexCreationTask { }
    //endregion

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

}
