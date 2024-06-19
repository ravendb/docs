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
                "    Product = Product, " +
                "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " +
                "}).Select(this0 => new { " +
                "    Category = this0.CategoryName, " +
                "    Count = 1 " +
                "})";

            reduce = "results.GroupBy(result => result.Category).Select(g => new { " +
                "    Category = g.Key, " +
                "    Count = Enumerable.Sum(g, x => ((int) x.Count)) " +
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
                "    Product = Product, " +
                "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " +
                "}).Select(this0 => new { " +
                "    Category = this0.CategoryName, " +
                "    PriceSum = this0.Product.PricePerUnit, " +
                "    PriceAverage = 0, " +
                "    ProductCount = 1 " +
                "})";

            reduce = "results.GroupBy(result => result.Category).Select(g => new { " +
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
            map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " +
                "    Product = line.Product, " +
                "    Count = 1, " +
                "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " +
                "})";


            reduce = "results.GroupBy(result => result.Product).Select(g => new { " +
                "    Product = g.Key, " +
                "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " +
                "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " +
                "})";
        }
    }
    //endregion


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
        //region map_reduce_3_0
        public Product_Sales_ByMonth() {
            map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " +
                "    Product = line.Product, " +
                "    Month = new DateTime(order.OrderedAt.Year, order.OrderedAt.Month, 1), " +
                "    Count = 1, " +
                "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " +
                "})";

            reduce = "results.GroupBy(result => new { " +
                "    Product = result.Product, " +
                "    Month = result.Month " +
                "}).Select(g => new { " +
                "    Product = g.Key.Product, " +
                "    Month = g.Key.Month, " +
                "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " +
                "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " +
                "})";

            outputReduceToCollection = "MonthlyProductSales";
            patternReferencesCollectionName = "DailyProductSales/References";
            patternForOutputReduceToCollectionReferences = "sales/daily/{Date:yyyy-MM-dd}";
        }
    }
    //endregion


    public class IndexDefinition {

        public IndexDefinition() {
            configuration = new IndexConfiguration();
        }
        //region syntax_0
        private String outputReduceToCollection;

        private String patternReferencesCollectionName;

        private String patternForOutputReduceToCollectionReferences;

        //endregion
    }
    public MapReduceIndexes() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region map_reduce_0_1
                List<Products_ByCategory.Result> results = session
                    .query(Products_ByCategory.Result.class, Products_ByCategory.class)
                    .whereEquals("Category", "Seafood")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region map_reduce_1_1
                List<Products_Average_ByCategory.Result> results = session
                    .query(Products_Average_ByCategory.Result.class, Products_Average_ByCategory.class)
                    .whereEquals("Category", "Seafood")
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

