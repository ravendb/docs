import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region paging_0_4
class Products_ByUnitsInStock extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Products.Select(product => new {    
            UnitsInStock = product.UnitsInStock
        })`;
    }
}
//endregion

//region paging_6_0
class Orders_ByOrderLines_ProductName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders
            .SelectMany(
                order => order.Lines, 
                (order, line) => new {    
                    Product = line.ProductName 
                })`;
    }
}
//endregion

//region paging_7_0
class Orders_ByStoredProductName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Orders.SelectMany(
            order => order.Lines, 
            (order, line) => new {    
                Product = line.ProductName
            })`;

        this.store("Product", "Yes");
    }
}
//endregion

class Order_ByOrderLines_ProductName extends AbstractIndexCreationTask { }

class Product { }

class Order { }

async function paging() {
    
    {
        //region paging_0_1
        const results = await session
            .query({ indexName: "Products/ByUnitsInStock" })
            .whereGreaterThan("UnitsInStock", 10)
            .all();
        //endregion
    }

    {
        //region paging_2_1
        const results = await session
            .query({ indexName: "Products_ByUnitsInStock" })
            .whereGreaterThan("UnitsInStock", 10)
            .skip(20) // skip 2 pages worth of products
            .take(10) // take up to 10 products
            .all(); // execute query
        //endregion
    }

    {
        //region paging_3_1
        let stats;

        const results = await session
            .query({ indexName: "Products/ByUnitsInStock" })
            .statistics(s => stats = s)
            .whereGreaterThan("UnitsInStock", 10)
            .skip(20)
            .take(10)
            .all();

        let totalResults = stats.totalResults;
        //endregion
    }

    {
        //region paging_4_1
        const PAGE_SIZE = 10;
        let pageNumber = 0;
        let skippedResults = 0;
        let results;
        let stats;

        do {
            results = await session
                .query({ indexName: "Products/ByUnitsInStock" })
                .statistics(s => stats = s)
                .skip((pageNumber * PAGE_SIZE) + skippedResults)
                .take(PAGE_SIZE)
                .whereGreaterThan("UnitsInStock", 10)
                .distinct()
                .all();

            skippedResults += stats.skippedResults;
            pageNumber++;
        } while (results.length > 0);
        //endregion
    }

    {
        //region paging_6_1
        const PAGE_SIZE = 10;
        let pageNumber = 0;
        let skippedResults = 0;
        let results;
        let stats;

        do {
            results = await session
                .query({ indexName: "Order/ByOrderLines/ProductName" })
                .statistics(s => stats = s)
                .skip((pageNumber * PAGE_SIZE) + skippedResults)
                .take(PAGE_SIZE)
                .all();

            skippedResults += stats.skippedResults;
            pageNumber++;
        } while (results.length > 0);
        //endregion
    }

    {
        //region paging_7_1
        const PAGE_SIZE = 10;
        let pageNumber = 0;
        let results;

        do {
            results = await session
                .query({ indexName: "Orders/ByStoredProductName" })
                .selectFields("Product")
                .skip(pageNumber * PAGE_SIZE)
                .take(PAGE_SIZE)
                .distinct()
                .all();

            pageNumber++;
        } while (results.length > 0);
        //endregion
    }


}

