import {
    DocumentStore, GetIndexOperation, AbstractCsharpIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region map_reduce_0_0
class Products_ByCategory extends AbstractCsharpIndexCreationTask {

    constructor() {
        super();

        this.map = "docs.Products.Select(product => new { " +
            "    Product = product, " +
            "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " +
            "}).Select(this0 => new { " +
            "    Category = this0.CategoryName, " +
            "    Count = 1 " +
            "})";

        this.reduce = "results.GroupBy(result => result.category).Select(g => new { " +
            "    category = g.Key, " +
            "    count = Enumerable.Sum(g, x => ((int) x.count)) " +
            "})";
    }
}
//endregion

//region map_reduce_1_0
class Products_Average_ByCategory extends AbstractCsharpIndexCreationTask {

    constructor() {
        super();

        this.map = "docs.Products.Select(product => new { " +
            "    Product = product, " +
            "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " +
            "}).Select(this0 => new { " +
            "    Category = this0.CategoryName, " +
            "    PriceSum = this0.product.PricePerUnit, " +
            "    PriceAverage = 0, " +
            "    ProductCount = 1 " +
            "})";

        this.reduce = "results.GroupBy(result => result.Category).Select(g => new { " +
            "    g = g, " +
            "    ProductCount = Enumerable.Sum(g, x => ((int) x.ProductCount)) " +
            "}).Select(this0 => new { " +
            "    this0 = this0, " +
            "    PriceSum = Enumerable.Sum(this0.g, x0 => ((decimal) x0.PriceSum)) " +
            "}).Select(this1 => new { " +
            "    Category = this1.this0.g.Key, " +
            "    PriceSum = this1.PriceSum, " +
            "    PriceAverage = this1.PriceSum / ((decimal) this1.this0.ProductCount), " +
            "    ProductCount = this1.this0.ProductCount " +
            "})";
    }
}
//endregion

//region map_reduce_2_0
class Product_Sales extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " +
            "    Product = line.Product, " +
            "    Count = 1, " +
            "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " +
            "})";


        this.reduce = "results.GroupBy(result => result.Product).Select(g => new { " +
            "    Product = g.Key, " +
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " +
            "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " +
            "})";
    }
}
//endregion

//region map_reduce_3_0
class Product_Sales_ByMonth extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " +
            "    Product = line.Product, " +
            "    Month = new DateTime(order.OrderedAt.Year, order.OrderedAt.Month, 1), " +
            "    Count = 1, " +
            "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " +
            "})";

        this.reduce = "results.GroupBy(result => new { " +
            "    Product = result.Product, " +
            "    Month = result.Month " +
            "}).Select(g => new { " +
            "    Product = g.Key.Product, " +
            "    Month = g.Key.Month, " +
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " +
            "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " +
            "})";

        this.outputReduceToCollection = "MonthlyProductSales";
    }
}
//endregion

async function mapReduceExample() {
    
        {
            //region map_reduce_0_1
            const results = await session
                .query({ indexName: "Products/ByCategory" })
                .whereEquals("Category", "Seafood")
                .all();
            //endregion
        }

        {
            //region map_reduce_1_1
            const results = await session
                .query({ indexName: "Products_Average_ByCategory" })
                .whereEquals("Category", "Seafood")
                .all();
            //endregion
        }

        {
            //region map_reduce_2_1
            const results = await session
                .query({ indexName: "Product/Sales" })
                .all();
            //endregion
        }

    {
        const indexDefinition = await store.maintenance.send(new GetIndexOperation("Orders/ProfitByProductAndOrderedAt"));

        //region syntax_0
        const outputReduceToCollection = indexDefinition.outputReduceToCollection;

        const patternReferencesCollectionName = indexDefinition.patternReferencesCollectionName;

        const patternForOutputReduceToCollectionReferences = indexDefinition.patternForOutputReduceToCollectionReferences;
        //endregion

    }

}


