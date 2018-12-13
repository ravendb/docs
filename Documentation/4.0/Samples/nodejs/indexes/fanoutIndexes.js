import { 
    DocumentStore,
    AbstractIndexCreationTask,
    IndexDefinition,
    PutIndexesOperation
} from "ravendb";

//region fanout_index_def_1
class Orders_ByProduct extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Orders.SelectMany(order => order.Lines, (order, orderLine) => new { " +
            "    Product = orderLine.Product, " +
            "    ProductName = orderLine.ProductName " +
            "})";
    }
}
//endregion

//region fanout_index_def_2
class Product_Sales extends AbstractIndexCreationTask {

    constructor() {
        super();

        this.map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " +
            "    Product = line.Product, " +
            "    Count = 1, " +
            "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " +
            "})";

        this.reduce = "results.GroupBy(result => result.product).Select(g => new {\n" +
            "    Product = g.Key,\n" +
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)),\n" +
            "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total))\n" +
            "})";
    }
}
//endregion

