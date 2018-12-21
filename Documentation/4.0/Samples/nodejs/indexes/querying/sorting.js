import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords,
    QueryData
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region sorting_5_6
class Employee_ByFirstName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Employees.Select(employee => new {    
            FirstName = employee.FirstName
        })`;

        this.store("FirstName", "Yes");
    }
}
//endregion

//region sorting_1_4
class Products_ByUnitsInStock extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Products.Select(product => new {    
            UnitsInStock = product.UnitsInStock
        })`;
    }
}
//endregion

//region sorting_1_5
class Products_ByUnitsInStockAndName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Products.Select(product => new {    
            UnitsInStock = product.UnitsInStock,
            Name = product.Name
        })`;
    }
}
//endregion

//region sorting_6_4
class Products_ByName_Search extends AbstractIndexCreationTask {

    constructor() {
        super();

        this.map = `docs.Products.Select(product => new {    
            Name = product.Name,    
            NameForSorting = product.Name
        })`;

        this.index("Name", "Search");
    }
}
//endregion

class Products_ByName extends AbstractIndexCreationTask { }

class Product { }

async function sorting() {
    
        {
            //region sorting_1_1
            const results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .whereGreaterThan("UnitsInStock", 10)
                .all();
            //endregion
        }

        {
            //region sorting_2_1
            const results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .whereGreaterThan("UnitsInStock", 10)
                .orderByDescending("UnitsInStock")
                .all();
            //endregion
        }

        {
            //region sorting_8_1
            const results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .whereGreaterThan("UnitsInStock", 10)
                .orderByDescending("UnitsInStock", "String")
                .all();
            //endregion
        }

        {
            //region sorting_3_1
            const results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .randomOrdering()
                .whereGreaterThan("UnitsInStock", 10)
                .all();
            //endregion
        }

        {
            //region sorting_4_1
            const results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .whereGreaterThan("UnitsInStock", 10)
                .orderByScore()
                .all();
            //endregion
        }

        {
            //region sorting_4_3
            const results = await session
                .query({ indexName: "Products/ByUnitsInStockAndName" })
                .whereGreaterThan("UnitsInStock", 10)
                .orderBy("UnitsInStock")
                .orderByScore()
                .orderByDescending("Name")
                .all();
            //endregion
        }

        {
            //region sorting_6_1
            const results = await session
                .query({ indexName: "Products/ByName/Search" })
                .search("Name", "Louisiana")
                .orderByDescending("NameForSorting")
                .all();
            //endregion
        }

        {
            //region sorting_7_1
            const results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .whereGreaterThan("UnitsInStock", 10)
                .orderBy("Name", "AlphaNumeric")
                .all();
            //endregion
        }
 
}

