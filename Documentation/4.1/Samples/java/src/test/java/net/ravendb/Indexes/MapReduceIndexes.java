package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Date;
import java.util.List;

public class MapReduceIndexes {

    //region map_reduce_0_0
    public static class Products_ByCategory extends AbstractIndexCreationTask {
        public static class Result {
            private String category;
            private String count;

            public String getCategory() {
                return category;
            }

            public void setCategory(String category) {
                this.category = category;
            }

            public String getCount() {
                return count;
            }

            public void setCount(String count) {
                this.count = count;
            }
        }

        public Products_ByCategory() {
            map = "docs.Products.Select(product => new { " +
                "    product = product, " +
                "    categoryName = (this.LoadDocument(product.category, \"Categories\")).Name " +
                "}).Select(this0 => new { " +
                "    category = this0.categoryName, " +
                "    count = 1 " +
                "})";

            reduce = "results.GroupBy(result => result.category).Select(g => new { " +
                "    category = g.Key, " +
                "    count = Enumerable.Sum(g, x => ((int) x.count)) " +
                "})";
        }
    }
    //endregion

    //region map_reduce_1_0
    public static class Products_Average_ByCategory extends AbstractIndexCreationTask {
        public static class Result {
            private String category;
            private double priceSum;
            private double priceAverage;
            private int productCount;

            public String getCategory() {
                return category;
            }

            public void setCategory(String category) {
                this.category = category;
            }

            public double getPriceSum() {
                return priceSum;
            }

            public void setPriceSum(double priceSum) {
                this.priceSum = priceSum;
            }

            public double getPriceAverage() {
                return priceAverage;
            }

            public void setPriceAverage(double priceAverage) {
                this.priceAverage = priceAverage;
            }

            public int getProductCount() {
                return productCount;
            }

            public void setProductCount(int productCount) {
                this.productCount = productCount;
            }
        }

        public Products_Average_ByCategory() {
            map = "docs.Products.Select(product => new { " +
                "    product = product, " +
                "    categoryName = (this.LoadDocument(product.category, \"Categories\")).name " +
                "}).Select(this0 => new { " +
                "    category = this0.categoryName, " +
                "    priceSum = this0.product.pricePerUnit, " +
                "    priceAverage = 0, " +
                "    productCount = 1 " +
                "})";

            reduce = "results.GroupBy(result => result.category).Select(g => new { " +
                "    g = g, " +
                "    productCount = Enumerable.Sum(g, x => ((int) x.productCount)) " +
                "}).Select(this0 => new { " +
                "    this0 = this0, " +
                "    priceSum = Enumerable.Sum(this0.g, x0 => ((decimal) x0.priceSum)) " +
                "}).Select(this1 => new { " +
                "    category = this1.this0.g.Key, " +
                "    priceSum = this1.priceSum, " +
                "    priceAverage = this1.priceSum / ((decimal) this1.this0.productCount), " +
                "    productCount = this1.this0.productCount " +
                "})";
        }
    }
    //endregion

    //region map_reduce_2_0
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


            reduce = "results.GroupBy(result => result.Product).Select(g => new { " +
                "    product = g.Key, " +
                "    count = Enumerable.Sum(g, x => ((int) x.count)), " +
                "    total = Enumerable.Sum(g, x0 => ((decimal) x0.total)) " +
                "})";
        }
    }
    //endregion

    //region map_reduce_3_0
    public static class Product_Sales_ByMonth extends AbstractIndexCreationTask {
        public static class Result {
            private String product;
            private Date month;
            private int count;
            private double total;

            public String getProduct() {
                return product;
            }

            public void setProduct(String product) {
                this.product = product;
            }

            public Date getMonth() {
                return month;
            }

            public void setMonth(Date month) {
                this.month = month;
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

        public Product_Sales_ByMonth() {
            map = "docs.Orders.SelectMany(order => order.lines, (order, line) => new { " +
                "    product = line.product, " +
                "    month = new DateTime(order.orderedAt.Year, order.orderedAt.Month, 1), " +
                "    count = 1, " +
                "    total = (((decimal) line.quantity) * line.pricePerUnit) * (1M - line.discount) " +
                "})";

            reduce = "results.GroupBy(result => new { " +
                "    product = result.product, " +
                "    month = result.month " +
                "}).Select(g => new { " +
                "    product = g.Key.product, " +
                "    month = g.Key.month, " +
                "    count = Enumerable.Sum(g, x => ((int) x.count)), " +
                "    total = Enumerable.Sum(g, x0 => ((decimal) x0.total)) " +
                "})";

            outputReduceToCollection = "MonthlyProductSales";
        }
    }
    //endregion

    public MapReduceIndexes() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region map_reduce_0_1
                List<Products_ByCategory.Result> results = session
                    .query(Products_ByCategory.Result.class, Products_ByCategory.class)
                    .whereEquals("category", "Seafood")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region map_reduce_1_1
                List<Products_Average_ByCategory.Result> results = session
                    .query(Products_Average_ByCategory.Result.class, Products_Average_ByCategory.class)
                    .whereEquals("category", "Seafood")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region map_reduce_2_1
                List<Product_Sales.Result> results = session
                    .query(Product_Sales.Result.class, Product_Sales.class)
                    .toList();
                //endregion
            }
        }
    }
}

