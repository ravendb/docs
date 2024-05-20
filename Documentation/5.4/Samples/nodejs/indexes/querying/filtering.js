import { 
    DocumentStore,
    AbstractJavaScriptIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region index_1
class Employees_ByFirstAndLastName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("employees", employee => {
            return {
                FirstName: employee.FirstName,
                LastName: employee.LastName
            };
        });
    }
}
//endregion

//region index_2
class Products_ByUnitsInStock extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("products", product => {
            return {
                UnitsInStock: product.UnitsInStock
            };
        });
    }
}
//endregion

//region index_3
class Orders_ByProductNamesPerOrderLine extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("orders", order => {
            return {
                // Index field 'ProductNames' will contain the product names per Order Line
                ProductNames: order.Lines.map(x => x.ProductName)
            };
        });
    }
}
//endregion

//region index_4
class Orders_ByProductNames extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("orders", order => {
            return {
                // Index field 'ProductNames' will contain a list of all product names
                ProductNames: order.Lines.flatMap(x => x.ProductName.split(" "))
            };
        });
    }
}
//endregion

async function filtering() {
    {
        //region filter_1
        // Basic filtering using "whereEquals":
        // ====================================
        
        const filteredEmployees = await session
             // Query an index 
            .query({ indexName: "Employees/ByFirstAndLastName" })
             // The filtering predicate
            .whereEquals("FirstName", "Robert")
             // By default AND is applied between both 'where' predicates
            .whereEquals("LastName", "King")
             // Execute the query, send it to the server for processing
            .all();
        
        // Results will include all Employee documents 
        // with FirstName equals to 'Robert' AND LastName equal to 'King'
        //endregion
    }
    {
        //region filter_2
        // Filter with "whereGreaterThan":
        // ===============================
        
        const filteredProducts = await session
             // Query an index 
            .query({ indexName: "Products/ByUnitsInStock" })
             // The filtering predicate
            .whereGreaterThan("UnitsInStock", 20)
            .all();
        
        // Results will include all Product documents having 'UnitsInStock' > 20
        //endregion
    }
    {
        //region filter_2_1
        // Filter with "whereLessThan":
        // ============================
        
        const filteredProducts = await session
             // Query an index 
            .query({ indexName: "Products/ByUnitsInStock" })
             // The filtering predicate
            .whereLessThan("UnitsInStock", 20)
            .all();

        // Results will include all Product documents having 'UnitsInStock'< 20
        //endregion
    }
    {
        //region filter_3
        // Filter by a nested property:
        // ============================
        
        const filteredOrders = await session
             // Query a collection
            .query({ collection: "Orders" })
             // Filter by the nested property 'ShipTo.City' from the Order document
            .whereEquals("ShipTo.City", "Albuquerque")
            .all();
        
        // * Results will include all Order documents with an order that ships to 'Albuquerque'
        // * An auto-index will be created
        //endregion
    }
    {
        //region filter_4
        // Filter by multiple values:
        // ==========================
        
        const filteredOrders = await session
             // Query an index 
            .query({ indexName: "Orders/ByProductNamesPerOrderLine" })
             // Filter by multiple values 
            .whereEquals("ProductName", "Teatime Chocolate Biscuits")
            .all();

        // Results will include all Order documents that contain ALL values in "Teatime Chocolate Biscuits"
        //endregion
    }
    {
        //region filter_5
        // Filter with "whereIn":
        // ======================
        
        const filteredEmployees = await session
             // Query an index 
            .query({ indexName: "Employees/ByFirstAndLastName" })
             // The filtering predicate
            .whereIn("FirstName", [ "Robert", "Nancy" ]) 
            .all();
        
        // Results will include all Employee documents that have either 'Robert' OR 'Nancy' in their 'FirstName' field
        //endregion
    }
    {
        //region filter_6
        // Filter with "containsAny":
        // ==========================
        
        const filteredOrders = await session
             // Query an index 
            .query({ indexName: "Orders/ByProductNames" })
             // The filtering predicate
            .containsAny("ProductNames", ["Ravioli", "Coffee"])
            .all();

        // Results will include all Order documents that have either 'Ravioli' OR 'Coffee' in their order
        //endregion
    }
    {
        //region filter_7
        // Filter with "containsAll":
        // ==========================
        
        const filteredOrders = await session
             // Query an index 
            .query({ indexName: "Orders/ByProductNames" })
             // The filtering predicate
            .containsAll("ProductNames", ["Ravioli", "Pepper"])
            .all();

        // Results will include all Order documents that have both 'Ravioli' AND 'Pepper' in their order
        //endregion
    }
    {
        //region filter_8
        // Filter with "whereStartsWith":
        // ==============================
        
        const filteredProducts = await session
             // Query a collection
            .query({ collection: "Products" })
             // The filtering predicate
            .whereStartsWith("Name", "ch")
            .all();
        
        // * Results will include all Product documents with a name that starts with 'ch'
        // * An auto-index will be created
        //endregion
    }
    {
        //region filter_9
        // Filter with "whereEndsWith":
        // ===========================
        
        const filteredProducts = await session
             // Query a collection
            .query({ collection: "Products" })
             // The filtering predicate
            .whereEndsWith("Name", "es")
            .all();

        // * Results will include all Product documents with a name that ends with 'es'
        // * An auto-index will be created
        //endregion
    }
    {
        //region filter_10
        // Filter by id:
        // =============

        const order = await session
             // Query a collection
            .query({ collection: "Orders" })
             // The filtering predicate
            .whereEquals("id", "orders/1-A")
            .firstOrNull();

        // * Results will include the Order document having ID 'orders/1-A'
        // * An auto-index is NOT created
        //endregion
    }
    {
        //region filter_11
        // Filter by whereStartsWith id:
        // =============================

        const filteredOrders = await session
            // Query a collection
            .query({ collection: "Orders" })
            // The filtering predicate
            .whereStartsWith("id", "orders/1")
            .all();

        // * Results will include all Order documents having ID that starts with 'orders/1'
        // * An auto-index is NOT created
        //endregion
    }
    
}
