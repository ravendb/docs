import { 
    DocumentStore, 
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Employee { }

class Product { }

class Order { }

class BlogPost { }

//region filtering_0_4
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Employees.Select(employee => new {    
            FirstName = employee.FirstName,   
            LastName = employee.LastName
        })`;
    }
}
//endregion

//region filtering_1_4
class Products_ByUnitsInStock extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Products.Select(product => new {        
            UnitsInStock = product.UnitsInStock    
        })`;
    }
}
//endregion

//region filtering_7_4
class Orders_ByTotalPrice extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders.Select(order => new {    
            totalPrice = Enumerable.Sum(order.Lines, 
                x => ((decimal)((((decimal) x.Quantity) * x.PricePerUnit) * (1M - x.Discount))))
            })`; 
    }
}
//endregion

//region filtering_2_4
class Order_ByOrderLinesCount extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders.Select(order => new {    
            Lines_Count = order.Lines.Count
        })`; 
    }
}
//endregion

//region filtering_3_4
class Order_ByOrderLines_ProductName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders.Select(order => new {    
            Lines_ProductName = order.Lines.Select(x => x.ProductName)
        })`; 
    }
}
//endregion

//region filtering_5_4
class BlogPosts_ByTags extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.BlogPosts.Select(post => new {    
            tags = post.tags
        })`;
    }
}
//endregion

async function filtering() {

    {
        //region filtering_0_1
        const results = await session
            .query({ indexName: "Employees/ByFirstAndLastName" }) // query 'Employees/ByFirstAndLastName' index
            .whereEquals("FirstName", "Robert") // filtering predicates
            .andAlso()   // by default OR is between each condition
            .whereEquals("LastName", "King") // materialize query by sending it to server for processing
            .all();
        //endregion
    }

    {
        //region filtering_1_1
        const results = await session
            .query({ indexName: "Products/ByUnitsInStock" }) // query 'Products/ByUnitsInStock' index
            .whereGreaterThan("UnitsInStock", 50) // filtering predicates
            .all(); // materialize query by sending it to server for processing

        //endregion
    }

    {
        //region filtering_7_1
        const results = await session
            .query({ indexName: "Orders/ByTotalPrice" })
            .whereGreaterThan("TotalPrice", 50)
            .ofType(Order)
            .all();
        //endregion
    }

    {
        //region filtering_2_1
        const results = await session
            .query({ indexName: "Order/ByOrderLinesCount" }) // query 'Order/ByOrderLinesCount' index
            .whereGreaterThan("Lines_Count", 50) // filtering predicates
            .all();   // materialize query by sending it to server for processing
        //endregion
    }

    {
        //region filtering_3_1
        const results = await session
            .query({ indexName: "Order/ByOrderLines/ProductName" }) // query 'Order/ByOrderLines/ProductName' index
            .whereEquals("Lines_ProductName", "Teatime Chocolate Biscuits") // filtering predicates
            .all(); // materialize query by sending it to server for processing
        //endregion
    }

    {
        //region filtering_4_1
        const results = await session
            .query({ indexName: "Employees/ByFirstAndLastName" }) // query 'Employees/ByFirstAndLastName' index
            .whereIn("FirstName", [ "Robert", "Nancy" ]) // filtering predicates
            .all();// materialize query by sending it to server for processing
        //endregion
    }

    {
        //region filtering_5_1
        const results = await session
            .query({ indexName: "BlogPosts/ByTags" })  // query 'BlogPosts/ByTags' index
            .containsAny("tags", [ "Development", "Research" ]) // filtering predicates
            .all(); // materialize query by sending it to server for processing
        //endregion
    }

    {
        //region filtering_6_1
        const results = await session
            .query({ indexName: "BlogPosts/ByTags" }) // query 'BlogPosts/ByTags' index
            .containsAll("tags", [ "Development", "Research" ]) // filtering predicates
            .all(); // materialize query by sending it to server for processing
        //endregion
    }

    {
        //region filtering_8_1
        // return all products which name starts with 'ch'
        const results = await session
            .query(Product)
            .whereStartsWith("Name", "ch")
            .all();
        //endregion
    }

    {
        //region filtering_9_1
        // return all products which name ends with 'ra'
        const results = await session
            .query(Product)
            .whereEndsWith("Name", "ra")
            .all();
        //endregion
    }

    {
        //region filtering_10_1
        // return all orders that were shipped to 'Albuquerque'
        const results = session
            .query(Order)
            .whereEquals("ShipTo_City", "Albuquerque")
            .all();
        //endregion
    }
}

