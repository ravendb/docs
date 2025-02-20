<?php

namespace RavenDB\Samples\Indexes;

use DateTime;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Documents\Indexes\AdditionalSourcesArray;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Documents\Indexes\IndexDefinition;
use RavenDB\Documents\Indexes\IndexFieldOptions;
use RavenDB\Type\StringArray;

class JavaScript {}

/*
# region javaScriptindexes_1
class Employees_ByFirstAndLastName extends AbstractJavaScriptIndexCreationTask
{
    // ...
}
# endregion
*/

/*
# region javaScriptindexes_2
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Employees', function (employee){
                return {
                    FirstName : employee.FirstName,
                    LastName : employee.LastName
                };
            })"
        ]);
    }
# endregion
*/

# region javaScriptindexes_6
class Employees_ByFirstAndLastName extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps(["map('Employees', function (employee){
                    return {
                        FirstName : employee.FirstName,
                        LastName : employee.LastName
                    };
                })"]);
    }
}
# endregion

# region javaScriptindexes_7
class Employees_ByFullName_Result
{
    private ?string $fullName = null;

    public function getFullName(): ?string
    {
        return $this->fullName;
    }

    public function setFullName(?string $fullName): void
    {
        $this->fullName = $fullName;
    }
}
class Employees_ByFullName extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps(["map('Employees', function (employee){
                    return {
                        FullName  : employee.FirstName + ' ' + employee.LastName
                    };
                })"]);
    }
}
# endregion

//# region javaScriptindexes_1_0
class Employees_ByYearOfBirth_Reslut
{
    private ?int $yearOfBirth = null;

    public function getYearOfBirth(): ?int
    {
        return $this->yearOfBirth;
    }

    public function setYearOfBirth(?int $yearOfBirth): void
    {
        $this->yearOfBirth = $yearOfBirth;
    }
}
class Employees_ByYearOfBirth extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Employees', function (employee){
                return {
                    Birthday : employee.Birthday.Year
                }
            })"
        ]);
    }
}
# endregion

# region javaScriptindexes_1_2
class Employees_ByBirthday_Result
{
    private ?DateTime $birthday = null;

    public function getBirthday(): ?DateTime
    {
        return $this->birthday;
    }

    public function setBirthday(?DateTime $birthday): void
    {
        $this->birthday = $birthday;
    }
}
class Employees_ByBirthday extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Employees', function (employee){
                return {
                    Birthday : employee.Birthday
                }
            })"
        ]);
    }
}
# endregion

# region javaScriptindexes_1_4
class Employees_ByCountry_Result
{
    private ?string $country = null;
}
class Employees_ByCountry extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Employees', function (employee){
                return {
                    Country : employee.Address.Country
                }
            })"
        ]);
    }
}
# endregion

# region javaScriptindexes_1_6
class Employees_Query_Result
{
    public ?StringArray $query = null;
}
class Employees_Query extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Employees', function (employee) {
                return {
                    Query : [employee.FirstName,
                             employee.LastName,
                             employee.Title,
                             employee.Address.City]
                }
            })"
        ]);

        $options = new IndexFieldOptions();
        $options->setIndexing(FieldIndexing::search());
        $this->setFields([
            "Query" => $options
        ]);
    }
}
# endregion

# region multi_map_5
class Animals_ByName extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('cats', function (c){ return {Name: c.Name}})",
            "map('dogs', function (d){ return {Name: d.Name}})"
        ]);
    }
}
# endregion

# region map_reduce_0_0
class Products_ByCategory_Result
{
    private ?string $category = null;
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
class Products_ByCategory extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('products', function(p){
                return {
                    Category: load(p.Category, 'Categories').Name,
                    Count: 1
                }
            })"
        ]);

        $this->setReduce(
            "groupBy(x => x.Category)
                    .aggregate(g => {
                        return {
                            Category: g.key,
                            Count: g.values.reduce((count, val) => val.Count + count, 0)
                        };
                    })"
        );
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
class Products_Average_ByCategory extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('products', function(product){
                return {
                    Category: load(product.Category, 'Categories').Name,
                    PriceSum: product.PricePerUnit,
                    PriceAverage: 0,
                    ProductCount: 1
                }
            })"
        ]);

        $this->setReduce("groupBy(x => x.Category)
                    .aggregate(g => {
                        var pricesum = g.values.reduce((sum,x) => x.PriceSum + sum,0);
                        var productcount = g.values.reduce((sum,x) => x.ProductCount + sum,0);
                        return {
                            Category: g.key,
                            PriceSum: pricesum,
                            ProductCount: productcount,
                            PriceAverage: pricesum / productcount
                        }
                    })");
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
class Product_Sales extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('orders', function(order){
                    var res = [];
                    order.Lines.forEach(l => {
                        res.push({
                            Product: l.Product,
                            Count: 1,
                            Total:  (l.Quantity * l.PricePerUnit) * (1- l.Discount)
                        })
                    });
                    return res;
                })"
        ]);

        $this->setReduce("groupBy(x => x.Product)
            .aggregate(g => {
                return {
                    Product : g.key,
                    Count: g.values.reduce((sum, x) => x.Count + sum, 0),
                    Total: g.values.reduce((sum, x) => x.Total + sum, 0)
                }
            })");
    }
}
# endregion

# region map_reduce_3_0
class Product_Sales_ByDate extends AbstractIndexCreationTask
{
    public function createIndexDefinition(): IndexDefinition
    {
        $indexDefinition = new IndexDefinition();
        $indexDefinition->setMaps([
            "from order in docs.Orders
            from line in order.Lines
            select new {
                line.Product,
                Date = order.OrderedAt,
                Profit = line.Quantity * line.PricePerUnit * (1 - line.Discount)
            };"
        ]);
        $indexDefinition->setReduce(
            "from r in results
              group r by new { r.OrderedAt, r.Product }
              into g
              select new {
                  Product = g.Key.Product,
                  Date = g.Key.Date,
                  Profit = g.Sum(r => r.Profit)
              };"
        );

        $indexDefinition->setOutputReduceToCollection( "DailyProductSales");
        $indexDefinition->setPatternReferencesCollectionName("DailyProductSales/References");
        $indexDefinition->setPatternForOutputReduceToCollectionReferences("sales/daily/{Date:yyyy-MM-dd}");

        return $indexDefinition;
    }
}
# endregion

# region fanout_index_def_1
class Orders_ByProduct extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Orders', function (order){
               var res = [];
                order.Lines.forEach(l => {
                    res.push({
                        Product: l.Product,
                        ProductName: l.ProductName
                    })
                });
                return res;
            })",
        ]);
    }
}
# endregion

# region static_sorting2
class Products_ByName_Result
{
    private ?string $analyzedName = null;

    public function getAnalyzedName(): ?string
    {
        return $this->analyzedName;
    }

    public function setAnalyzedName(?string $analyzedName): void
    {
        $this->analyzedName = $analyzedName;
    }
}

class Products_ByName extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('products', function (u){
                return {
                    Name: u.Name,
                    _: {\$value: u.Name, \$name:'AnalyzedName'}
                };
            })"
        ]);

        $options = new IndexFieldOptions();
        $options->setIndexing(FieldIndexing::search());
        $options->setAnalyzer("StandardAnalyzer");

        $this->setFields([
            "AnalyzedName" => $options
        ]);
    }
}
# endregion

# region indexing_related_documents_2
class Products_ByCategoryName_Result
{
    private ?string $categoryName = null;

    public function getCategoryName(): ?string
    {
        return $this->categoryName;
    }

    public function setCategoryName(?string $categoryName): void
    {
        $this->categoryName = $categoryName;
    }
}
class Products_ByCategoryName extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('products', function(product ){
                return {
                    CategoryName : load(product .Category, 'Categories').Name,
                }
            })"
        ]);
    }
}
# endregion

# region indexing_related_documents_5
class Authors_ByNameAndBookNames_Result
{
    private ?string $name = null;
    private ?StringArray $books = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getBooks(): ?StringArray
    {
        return $this->books;
    }

    public function setBooks(?StringArray $books): void
    {
        $this->books = $books;
    }
}
class Authors_ByNameAndBookNames extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Author', function(a){
                return {
                    Name: a.Name,
                    Books: a.BooksIds.forEach(x => load(x, 'Book').Name)
                }
            })"
        ]);
    }
}
# endregion

# region indexes_2
class BlogPosts_ByCommentAuthor_Result
{
    private ?StringArray $authors = null;

    public function getAuthors(): ?StringArray
    {
        return $this->authors;
    }

    public function setAuthors(?StringArray $authors): void
    {
        $this->authors = $authors;
    }
}
class BlogPosts_ByCommentAuthor extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('BlogPosts', function(b){
                var names = [];
                b.Comments.forEach(x => getNames(x, names));
                return {
                    Authors : names
            };})"
        ]);

        $additionalSources = new AdditionalSourcesArray();
        $additionalSources->offsetSet("The Script", "function getNames(x, names) {
                                names.push(x.Author);
                                x.Comments.forEach(x => getNames(x, names));
                         }");
        $this->setAdditionalSources($additionalSources);
    }
}
# endregion

# region spatial_search_1
class Events_ByNameAndCoordinates extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('events', function (e){
                return {
                    Name: e.Name  ,
                    Coordinates: createSpatialField(e.Latitude, e.Longitude)
                };
            })"
        ]);

    }
}
# endregion

# region indexes_3
class BlogPosts_ByCommentAuthor_JS_Result
{
    private ?StringArray $authors = null;

    public function getAuthors(): ?StringArray
    {
        return $this->authors;
    }

    public function setAuthors(?StringArray $authors): void
    {
        $this->authors = $authors;
    }
}
class BlogPosts_ByCommentAuthor_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('BlogPosts', function (blogpost) {
                return recurse(blogpost, x => x.Comments).map(function (comment) {
                    if (comment.Author != null) {
                        return {
                            Authors: comment.Author
                        };
                    }
                });
            });"
        ]);
    }
}
# endregion
