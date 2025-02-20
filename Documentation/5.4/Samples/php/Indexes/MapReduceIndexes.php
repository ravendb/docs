<?php

namespace RavenDB\Samples\Indexes;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractMultiMapIndexCreationTask;
use RavenDB\Type\StringArray;

class MapReduceIndexes
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region map_reduce_0_1
                /** @var array<Products_ByCategory_Result> $results */
                $results = $session
                    ->query(Products_ByCategory_Result::class, Products_ByCategory::class)
                    ->whereEquals("Category", "Seafood")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region map_reduce_0_2
                /** @var array<Products_ByCategory_Result> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Products_ByCategory_Result::class, Products_ByCategory::class)
                    ->whereEquals("Category", "Seafood")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region map_reduce_1_1
                /** @var array<Products_Average_ByCategory_Result> $results */
                $results = $session
                    ->query(Products_Average_ByCategory_Result::class, Products_Average_ByCategory::class)
                    ->whereEquals("Category", "Seafood")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region map_reduce_1_2
                /** @var array<Products_Average_ByCategory_Result> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Products_Average_ByCategory_Result::class, Products_Average_ByCategory::class)
                    ->whereEquals("Category", "Seafood")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region map_reduce_2_1
                /** @var array<Product_Sales_Result> $results */
                $results = $session
                    ->query(Product_Sales_Result::class, Product_Sales::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region map_reduce_2_2
                /** @var array<Product_Sales_Result> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Product_Sales_Result::class, Product_Sales::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region map_reduce_0_0
class Products_ByCategory_Result
{
    public ?string $category = null;
    public ?int $count = null;

    public function getCategory(): ?string
    {
        return $this->category;
    }

    public function setCategory(?string $category): void
    {
        $this->category = $category;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }
}

class Products_ByCategory extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new { " .
            "    Product = Product, " .
            "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " .
            "}).Select(this0 => new { " .
            "    Category = this0.CategoryName, " .
            "    Count = 1 " .
            "})";

        $this->reduce = "results.GroupBy(result => result.Category).Select(g => new { " .
            "    Category = g.Key, " .
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)) " .
            "})";
    }
}
# endregion

# region map_reduce_1_0
class Products_Average_ByCategory_Result
{
    private ?string $category = null;
    private ?float $priceSum = null;
    private ?float $priceAverage = null;
    private ?int $productCount = null;

    public function getCategory(): ?string
    {
        return $this->category;
    }

    public function setCategory(?string $category): void
    {
        $this->category = $category;
    }

    public function getPriceSum(): ?float
    {
        return $this->priceSum;
    }

    public function setPriceSum(?float $priceSum): void
    {
        $this->priceSum = $priceSum;
    }

    public function getPriceAverage(): ?float
    {
        return $this->priceAverage;
    }

    public function setPriceAverage(?float $priceAverage): void
    {
        $this->priceAverage = $priceAverage;
    }

    public function getProductCount(): ?int
    {
        return $this->productCount;
    }

    public function setProductCount(?int $productCount): void
    {
        $this->productCount = $productCount;
    }
}

class Products_Average_ByCategory extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new { " .
            "    Product = Product, " .
            "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " .
            "}).Select(this0 => new { " .
            "    Category = this0.CategoryName, " .
            "    PriceSum = this0.Product.PricePerUnit, " .
            "    PriceAverage = 0, " .
            "    ProductCount = 1 " .
            "})";

        $this->reduce = "results.GroupBy(result => result.Category).Select(g => new { " .
            "    g = g, " .
            "    ProductCount = Enumerable.Sum(g, x => ((int) x.ProductCount)) " .
            "}).Select(this0 => new { " .
            "    this0 = this0, " .
            "    PriceSum = Enumerable.Sum(this0.g, x0 => ((decimal) x0.PriceSum)) " .
            "}).Select(this1 => new { " .
            "    Category = this1.this0.g.Key, " .
            "    PriceSum = this1.PriceSum, " .
            "    PriceAverage = this1.PriceSum / ((decimal) this1.this0.ProductCount), " .
            "    ProductCount = this1.this0.ProductCount " .
            "})";
    }
}
# endregion

# region map_reduce_2_0
class Product_Sales_Result
{
    private ?string $product = null;
    private ?int $count = null;
    private ?float $total = null;

    public function getProduct(): ?string
    {
        return $this->product;
    }

    public function setProduct(?string $product): void
    {
        $this->product = $product;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getTotal(): ?float
    {
        return $this->total;
    }

    public function setTotal(?float $total): void
    {
        $this->total = $total;
    }
}

class Product_Sales extends AbstractIndexCreationTask
{
        public function __construct()
        {
            parent::__construct();

            $this->map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " .
                "    Product = line.Product, " .
                "    Count = 1, " .
                "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " .
                "})";


            $this->reduce = "results.GroupBy(result => result.Product).Select(g => new { " .
                "    Product = g.Key, " .
                "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " .
                "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " .
                "})";
    }
}
# endregion


# region multi_map_reduce_LINQ
class Cities_Details_IndexEntry
{
    private ?string $city = null;
    private ?int $companies = null;
    private ?int $employees = null;
    private ?int $suppliers = null;

    public function getCity(): ?string
    {
        return $this->city;
    }

    public function setCity(?string $city): void
    {
        $this->city = $city;
    }

    public function getCompanies(): ?int
    {
        return $this->companies;
    }

    public function setCompanies(?int $companies): void
    {
        $this->companies = $companies;
    }

    public function getEmployees(): ?int
    {
        return $this->employees;
    }

    public function setEmployees(?int $employees): void
    {
        $this->employees = $employees;
    }

    public function getSuppliers(): ?int
    {
        return $this->suppliers;
    }

    public function setSuppliers(?int $suppliers): void
    {
        $this->suppliers = $suppliers;
    }
}

class Cities_Details extends AbstractMultiMapIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Map employees collection.
        $this->addMap("docs.Employees.SelectMany(e => new { " .
            "    City = e.Address.City, " .
            "    Companies = 0, " .
            "    Suppliers = 0, " .
            "    Employees = 1 " .
            "})");

        // Map companies collection.
        $this->addMap("docs.Companies.SelectMany(c => new { " .
            "    City = c.Address.City, " .
            "    Companies = 1, " .
            "    Suppliers = 0, " .
            "    Employees = 0 " .
            "})");

        // Map suppliers collection.
        $this->addMap("docs.Suppliers.SelectMany(s => new { " .
            "    City = s.Address.City, " .
            "    Companies = 0, " .
            "    Suppliers = 1, " .
            "    Employees = 0 " .
            "})");


        $this->reduce = "results.GroupBy(result => result.Product).Select(g => new { " .
            "    Product = g.Key, " .
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " .
            "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " .
            "})";

        // Apply reduction/aggregation on multi-map results.
        $this->reduce = "results.GroupBy(result => result.City).Select(g => new { " .
            "    City = g.Key, " .
            "    Companies = Enumerable.Sum(g, x => ((int) x.Companies)), " .
            "    Suppliers = Enumerable.Sum(g, x => ((int) x.Suppliers)), " .
            "    Employees = Enumerable.Sum(g, x => ((int) x.Employees)), " .
            "})";
    }
}
# endregion


class MultiMapReduceIndexQuery
{
    public function samples()
    {
        $store = new DocumentStore();
        try {

            $session = $store->openSession();
            try {
                # region multi-map-reduce-index-query
                // Queries the index "Cities_Details" - filters "Companies" results and orders by "City".
                /** @var array<Cities_Details_IndexEntry> $commerceDetails */
                $commerceDetails = $session
                    ->query(Cities_Details_IndexEntry::class, Cities_Details::class)
                    ->whereGreaterThan("Companies", 5)
                    ->orderBy("City")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region map_reduce_3_0
class DailyProductSale
{
    public ?string $product = null;
    public ?DateTime $date = null;
    public ?int $count = null;
    public ?float $total = null;

    public function getProduct(): ?string
    {
        return $this->product;
    }

    public function setProduct(?string $product): void
    {
        $this->product = $product;
    }

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getTotal(): ?float
    {
        return $this->total;
    }

    public function setTotal(?float $total): void
    {
        $this->total = $total;
    }
}

class ProductSales_ByDate extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Orders.SelectMany(order => order.Lines, (order, line) => new { " .
            "    Product = line.Product, " .
            "    Date = new DateTime(order.OrderedAt.Year, order.OrderedAt.Month, order.OrderedAt.Day), " .
            "    Count = 1, " .
            "    Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) " .
            "})";


        $this->reduce = "results.GroupBy(result => new { " .
            "    Product = result.Product, " .
            "    Date = result.Date " .
            "}).Select(g => new { " .
            "    Product = g.Key.Product, " .
            "    Date = g.Key.Date, " .
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)), " .
            "    Total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) " .
            "})";

        $this->outputReduceToCollection = "DailyProductSales";
        $this->patternReferencesCollectionName = "DailyProductSales/References";
        $this->patternForOutputReduceToCollectionReferences = "sales/daily/{Date:yyyy-MM-dd}";
    }
}
    # endregion

/*
# region map_reduce_reference_doc
{
    "Product": "products/77-A",
    "Date": "1998-05-06T00:00:00.0000000",
    "Count": 1,
    "Total": 26,
    "@metadata": {
        "@collection": "DailyProductSales",
        "@flags": "Artificial, FromIndex"
    }
}
# endregion
*/

# region map_reduce_4_0
class NumberOfOrders_ByProduct extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();


        $this->map =
            "docs.DailyProductSales.Select(sale => " .
            "   LoadDocument(\"sales/daily/\" + sale.Date.ToString(\"yyyy-MM-dd\"), \"DailyProductSales/References\") " .
            "       .ReduceOutputs.Select(refDoc => { " .
            "           var outputDoc = LoadDocument(refDoc, \"DailyProductSales\"); " .
            "           return new { " .
            "               Product = outputDoc.Product, " .
            "               Count = outputDoc.Count, " .
            "               NumOrders = 1 " .
            "           }; " .
            "       }) " .
            ").SelectMany(x => x)";

        $this->reduce =
            "results.GroupBy(r => new { " .
            "    Count = r.Count, " .
            "    Product = r.Product " .
            "}).Select(g => new { " .
            "    Product = g.Key.Product, " .
            "    Count = g.Key.Count, " .
            "    NumOrders = Enumerable.Sum(g, x => ((int) x.NumOrders)) " .
            "})";
    }
}

class OutputDocument {
    private ?string $product = null;
    private ?int $count = null;
    private ?int $numOrders = null;

    public function getProduct(): ?string
    {
        return $this->product;
    }

    public function setProduct(?string $product): void
    {
        $this->product = $product;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getNumOrders(): ?int
    {
        return $this->numOrders;
    }

    public function setNumOrders(?int $numOrders): void
    {
        $this->numOrders = $numOrders;
    }
}

class OutputReduceToCollectionReference
{
    public ?string $id = null;
    public ?StringArray $reduceOutputs = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getReduceOutputs(): ?StringArray
    {
        return $this->reduceOutputs;
    }

    public function setReduceOutputs(?StringArray $reduceOutputs): void
    {
        $this->reduceOutputs = $reduceOutputs;
    }
}
# endregion

/*
class foo
{
    # region syntax_0
    private ?string $outputReduceToCollection = null;

    private ?string $patternReferencesCollectionName = null;

    // Using IndexDefinition
    private ?string $patternForOutputReduceToCollectionReferences = null;
    # endregion

    private class TReduceResult
    {
    }
}
*/



