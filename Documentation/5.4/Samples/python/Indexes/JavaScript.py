import datetime
from typing import Dict, Any, List

from ravendb import IndexFieldOptions, IndexDefinition
from ravendb.documents.indexes.abstract_index_creation_tasks import (
    AbstractJavaScriptIndexCreationTask,
    AbstractIndexCreationTask,
)
from ravendb.documents.indexes.definitions import FieldIndexing


# region javaScriptindexes_1
class Employees_ByFirstAndLastName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        # ...


# endregion


# region javaScriptindexes_2
class Employees_ByFirstAndLastName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return { 
                    FirstName : employee.FirstName, 
                    LastName : employee.LastName
                };
            })
            """
        }


# endregion


# region javaScriptindexes_6
class Employees_ByFirstAndLastName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return { 
                    FirstName : employee.FirstName, 
                    LastName : employee.LastName
                };
            })
            
            """
        }


# endregion
# region javaScriptindexes_7
class Employees_ByFullName(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, full_name: str = None):
            self.full_name = full_name

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return { 
                    FullName  : employee.FirstName + ' ' + employee.LastName
                };
            })
            """
        }


# endregion


# region javaScriptindexes_1_0
class Employees_ByYearOfBirth(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, year_of_birth: int = None):
            self.year_of_birth = year_of_birth

        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]) -> "Employees_ByYearOfBirth.Result":
            return cls(json_dict["Birthday"])

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return {
                    Birthday : employee.Birthday.Year 
                } 
            })
            """
        }


# endregion
# region javaScriptindexes_1_2
class Employees_ByBirthday(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, birthday: datetime.datetime = None):
            self.birthday = birthday

        @classmethod
        def from_json(cls, json_dict: Dict[str, Any]) -> "Employees_ByBirthday.Result":
            return cls(json_dict["Birthday"])

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return {
                    Birthday : employee.Birthday 
                } 
            })
            """
        }


# endregion
# region javaScriptindexes_1_4
class Employees_ByCountry(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, country: str = None):
            self.country = country

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee){ 
                return {
                    country : employee.Address.Country 
                 } 
            })
            """
        }


# endregion


# region javaScriptindexes_1_6
class Employees_Query(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, query: List[str] = None):
            self.query = query

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (employee) { 
            return { 
                query : [employee.FirstName, 
                         employee.LastName,
                         employee.Title,
                         employee.Address.City] 
                    } 
            })
            """
        }

        self.fields = {"query": IndexFieldOptions(indexing=FieldIndexing.SEARCH)}


# endregion


# region multi_map_5
class Animals_ByName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            "map('cats', function (c){ return {Name: c.Name}})",
            "map('dogs', function (d){ return {Name: d.Name}})",
        }


# endregion


# region map_reduce_0_0
class Products_ByCategory(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, category: str = None, count: int = None):
            self.category = category
            self.count = count

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('products', function(p){
                return {
                    Category: load(p.Category, 'Categories').Name,
                    Count: 1
                }
            })
            """
        }

        self.reduce = """groupBy(x => x.Category)
                            .aggregate(g => {
                                return {
                                    category: g.key,
                                    count: g.values.reduce((count, val) => val.Count + count, 0)
                                };
                            })"""


# endregion


# region map_reduce_1_0
class Products_Average_ByCategory(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(
            self, category: str = None, price_sum: float = None, price_average: float = None, product_count: int = None
        ):
            self.category = category
            self.price_sum = price_sum
            self.price_average = price_average
            self.product_count = product_count

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('products', function(product){
                        return {
                            Category: load(product.Category, 'Categories').Name,
                            PriceSum: product.PricePerUnit,
                            PriceAverage: 0,
                            ProductCount: 1
                        }
                    })
            """
        }

        self.reduce = """
        groupBy(x => x.Category)
                            .aggregate(g => {
                                var pricesum = g.values.reduce((sum,x) => x.PriceSum + sum,0);
                                var productcount = g.values.reduce((sum,x) => x.ProductCount + sum,0);
                                return {
                                    category: g.key,
                                    price_sum: pricesum,
                                    product_count: productcount,
                                    price_average: pricesum / productcount
                                }
                            })
        """


# endregion
# region map_reduce_2_0
class Product_Sales(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, product: str = None, count: int = None, total: float = None):
            self.product = product
            self.count = count
            self.total = total

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('orders', function(order){
                var res = [];
                order.Lines.forEach(l => {
                    res.push({
                        Product: l.Product,
                        Count: 1,
                        Total:  (l.Quantity * l.PricePerUnit) * (1- l.Discount)
                    })
                });
                return res;
            })
            """
        }
        self.reduce = """
        groupBy(x => x.Product)
        .aggregate(g => {
            return {
                Product : g.key,
                Count: g.values.reduce((sum, x) => x.Count + sum, 0),
                Total: g.values.reduce((sum, x) => x.Total + sum, 0)
            }
        })
        """


# endregion


# region map_reduce_3_0
class Product_Sales_ByDate(AbstractIndexCreationTask):
    def create_index_definition(self) -> IndexDefinition:
        return IndexDefinition(
            maps={
                """
                from order in docs.Orders
                from line in order.Lines
                select new {
                    line.Product, 
                    Date = order.OrderedAt,
                    Profit = line.Quantity * line.PricePerUnit * (1 - line.Discount)
                };
                """
            },
            reduce="""
            from r in results
            group r by new { r.OrderedAt, r.Product }
            into g
            select new { 
                Product = g.Key.Product,
                Date = g.Key.Date,
                Profit = g.Sum(r => r.Profit)
            };
            """,
            output_reduce_to_collection="DailyProductSales",
            pattern_references_collection_name="DailyProductSales/References",
            pattern_for_output_reduce_to_collection_references="sales/daily/{Date:yyyy-MM-dd}",
        )


# endregion


# region fanout_index_def_1
class Orders_ByProduct(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
        map('Orders', function (order){ 
           var res = [];
            order.Lines.forEach(l => {
                res.push({
                    Product: l.Product,
                    ProductName: l.ProductName
                })
            });
            return res;
        })
        """
        }


# endregion


# region static_sorting2
class Products_ByName(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('products', function (u){
                return {
                    Name: u.Name,
                    _: {$value: u.Name, $name:'analyzed_name'}
                };
            })
            """
        }
        self.fields = {"analyzed_name": IndexFieldOptions(indexing=FieldIndexing.SEARCH, analyzer="StandardAnalyzer")}

    class Result:
        def __init__(self, analyzed_name: str = None):
            self.analyzed_name = analyzed_name


# endregion
# region indexing_related_documents_2
class Products_ByCategoryName(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, category_name: str = None):
            self.category_name = category_name

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('products', function(product ){
                return {
                    CategoryName : load(product .Category, 'Categories').Name,
                }
            })
            """
        }


# endregion


# region indexing_related_documents_5
class Authors_ByNameAndBookNames(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, name: str = None, books: List[str] = None):
            self.name = name
            self.books = books

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Author', function(a){
                return {
                    name: a.Name,
                    books: a.BooksIds.forEach(x => load(x, 'Book').Name)
                }
            })
            """
        }


# endregion
# region indexes_2
class BlogPosts_ByCommentAuthor(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, authors: List[str] = None):
            self.authors = authors

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('BlogPosts', function(b){
            var names = [];
            b.Comments.forEach(x => getNames(x, names));
            return {
                authors : names
            };})
            """
        }
        self.additional_sources = {
            "The Script": """function getNames(x, names){
                                        names.push(x.Author);
                                        x.Comments.forEach(x => getNames(x, names));
                                 }"""
        }


# endregion


# region spatial_search_!
class Events_ByNameAndCoordinates(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('events', function (e){
                return { 
                    Name: e.Name  ,
                    Coordinates: createSpatialField(e.Latitude, e.Longitude)
                };                            
            })
            """
        }


# endregion
