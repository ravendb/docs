package net.ravendb.Indexes;

import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;

public class FanoutIndexes {

    private static class Order {
    }


    //region fanout_index_def_1
    public static class Orders_ByProduct extends AbstractIndexCreationTask {
        public Orders_ByProduct() {
            map = "docs.Orders.SelectMany(order => order.Lines, (order, orderLine) => new { " +
                "    Product = orderLine.Product, " +
                "    ProductName = orderLine.ProductName " +
                "})";
        }
    }
    //endregion

    //region fanout_index_def_2
    public static class Product_Sales extends AbstractIndexCreationTask {
        public Product_Sales() {
            map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " +
                "    Product = line.Product, " +
                "    Count = 1, " +
                "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " +
                "})";

            reduce = "results.GroupBy(result => result.Product).Select(g => new {\n" +
                "    Product = g.Key,\n" +
                "    Count = Enumerable.Sum(g, x => ((int) x.Count)),\n" +
                "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total))\n" +
                "})";
        }
    }
    //endregion
}
