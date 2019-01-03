package net.ravendb.Indexes;

import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;

public class FanoutIndexes {

    private static class Order {
    }

    //region fanout_index_def_1
    public static class Orders_ByProduct extends AbstractIndexCreationTask {
        public Orders_ByProduct() {
            map = "docs.Orders.SelectMany(order => order.lines, (order, orderLine) => new { " +
                "    product = orderLine.product, " +
                "    productName = orderLine.productName " +
                "})";
        }
    }
    //endregion

    //region fanout_index_def_2
    public static class Product_Sales extends AbstractIndexCreationTask {
        public static class Result {
            private String product;
            private int count;
            private double total;

            public String getProduct() {
                return product;
            }

            public void setProduct(String product) {
                this.product = product;
            }

            public int getCount() {
                return count;
            }

            public void setCount(int count) {
                this.count = count;
            }

            public double getTotal() {
                return total;
            }

            public void setTotal(double total) {
                this.total = total;
            }
        }

        public Product_Sales() {
            map = "docs.Orders.SelectMany(order => order.lines, (order, line) => new { " +
                "    product = line.product, " +
                "    count = 1, " +
                "    total = (((decimal) line.quantity) * line.pricePerUnit) * (1M - line.discount) " +
                "})";

            reduce = "results.GroupBy(result => result.product).Select(g => new {\n" +
                "    product = g.Key,\n" +
                "    count = Enumerable.Sum(g, x => ((int) x.count)),\n" +
                "    total = Enumerable.Sum(g, x0 => ((decimal) x0.total))\n" +
                "})";
        }
    }
    //endregion
}
