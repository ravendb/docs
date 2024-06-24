import datetime

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractMultiMapIndexCreationTask

from examples_base import ExampleBase


# region map_reduce_0_0
class Products_ByCategory(AbstractIndexCreationTask):
    class Result:
        def __init__(self, category: str = None, count: int = None):
            self.category = category
            self.count = count

    def __init__(self):
        super().__init__()
        self.map = (
            "docs.Products.Select(product => new { "
            "    Product = Product,"
            '    CategoryName = (this.LoadDocument(product.Category, "Categories")).Name '
            "}).Select(this0 => new { "
            "    Category = this0.CategoryName, "
            "    Count = 1 "
            "})"
        )
        self.reduce = (
            "results.GroupBy(result => result.Category).Select(g => new {"
            "    Category = g.Key, "
            "    Count = Enumerable.Sum(g, x => ((int) x.Count)) "
            "})"
        )


# endregion


# region map_reduce_1_0
class Products_Average_ByCategory(AbstractIndexCreationTask):
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
        self.map = """
                   docs.Products.Select(product => new { 
                       Product = Product, 
                       CategoryName = (this.LoadDocument(product.Category, "Categories")).Name 
                   }).Select(this0 => new { 
                       category = this0.CategoryName, 
                       price_sum = this0.Product.PricePerUnit, 
                       price_average = 0, 
                       product_count = 1 
                   })
                   """
        self.reduce = """
                      results.GroupBy(result => result.Category).Select(g => new { 
                          g = g, 
                          ProductCount = Enumerable.Sum(g, x => ((int) x.ProductCount)) 
                      }).Select(this0 => new { 
                          this0 = this0, 
                          PriceSum = Enumerable.Sum(this0.g, x0 => ((decimal) x0.PriceSum)) 
                      }).Select(this1 => new { 
                          category = this1.this0.g.Key, 
                          price_sum = this1.PriceSum, 
                          price_average = this1.PriceSum / ((decimal) this1.this0.ProductCount), 
                          product_count = this1.this0.ProductCount 
                      })
                      """


# endregion


# region map_reduce_2_0
class Product_Sales(AbstractIndexCreationTask):
    class Result:
        def __init__(self, product: str = None, count: int = None, total: float = None):
            self.product = product
            self.count = count
            self.total = total

    def __init__(self):
        super().__init__()
        self.map = """
                   docs.Orders.SelectMany(order => order.Lines, (order, line) => new { 
                       Product = line.Product, 
                       Count = 1, 
                       Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) 
                   })
                   """
        self.reduce = """
                      results.GroupBy(result => result.Product).Select(g => new { 
                          product = g.Key, 
                          count = Enumerable.Sum(g, x => ((int) x.Count)), 
                          total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) 
                      })
                      """


# endregion
# region map_reduce_3_0
class Product_Sales_ByMonth(AbstractIndexCreationTask):
    class Result:
        def __init__(
            self, product: str = None, month: datetime.datetime = None, count: int = None, total: float = None
        ):
            self.product = product
            self.month = month
            self.count = count
            self.total = total

    def __init__(self):
        super().__init__()
        self.map = """
                   docs.Orders.SelectMany(order => order.Lines, (order, line) => new { 
                       Product = line.Product, 
                       Month = new DateTime(order.OrderedAt.Year, order.OrderedAt.Month, 1), 
                       Count = 1, 
                       Total = (((decimal) line.Quantity) * line.PricePerUnit) * (1M - line.Discount) 
                   })
                   """

        self.reduce = """
                      results.GroupBy(result => new { 
                          Product = result.Product, 
                          Month = result.Month 
                      }).Select(g => new { 
                          product = g.Key.Product, 
                          month = g.Key.Month, 
                          count = Enumerable.Sum(g, x => ((int) x.Count)), 
                          total = Enumerable.Sum(g, x0 => ((decimal) x0.Total)) 
                      })
                      """
        self._output_reduce_to_collection = "MonthlyProductSales"
        self._pattern_references_collection_name = "DailyProductSales/References"
        self._pattern_for_output_reduce_to_collection_references = "sales/daily/{Date:yyyy-MM-dd}"


# endregion


class IndexDefinition:
    def __init__(self):
        configuration = {}

    """
    # region syntax_0
    self._output_reduce_to_collection = output_reduce_to_collection
    self._pattern_references_collection_name = pattern_references_collection_name
    self._pattern_for_output_reduce_to_collection_references = pattern_for_output_reduce_to_collection_references
    # endregion
    """


class MapReduceIndexes(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_map_reduce_indexes(self):
        with self.embedded_server.get_document_store("MapReduceIndexes") as store:
            with store.open_session() as session:
                # region map_reduce_0_1
                results = list(
                    session.query_index_type(Products_ByCategory, Products_ByCategory.Result).where_equals(
                        "category", "Seafood"
                    )
                )
                # endregion
                # region map_reduce_1_1
                results = list(
                    session.query_index_type(
                        Products_Average_ByCategory, Products_Average_ByCategory.Result
                    ).where_equals("category", "Seafood")
                )
                # endregion
                # region map_reduce_2_1
                results = list(session.query_index_type(Product_Sales, Product_Sales.Result))
                # endregion

                # region multi-map-reduce-index-query
                # Queries the index "Cities_Details" - filters "Companies" results and orders by "City".
                commerce_details = list(
                    session.query_index_type(Cities_Details, Cities_Details.IndexEntry)
                    .where_greater_than("companies", 5)
                    .order_by("city")
                )
                # endregion


# region multi_map_reduce_LINQ
class Cities_Details(AbstractMultiMapIndexCreationTask):
    class IndexEntry:
        def __init__(self, city: str = None, companies: int = None, employees: int = None, suppliers: int = None):
            self.city = city
            self.companies = companies
            self.employees = employees
            self.suppliers = suppliers

    def __init__(self):
        super().__init__()
        self._add_map(
            """
        from e in docs.Employees
        select new {
            city = e.Address.City,
            companies = 0,
            supplier = 0,
            employees = 1
        }
        """
        )

        self._add_map(
            """
                from c in docs.Companies
                select new {
                    city = e.Address.City,
                    companies = 1,
                    supplier = 0,
                    employees = 0
                }
                """
        )

        self._add_map(
            """
                from s in docs.Suppliers
                select new {
                    city = e.Address.City,
                    companies = 0,
                    supplier = 1,
                    employees = 0
                }
                """
        )

        self.reduce = """
        from result in results
        group result by result.city
        into g
        select new {
            city = g.Key,
            companies = g.Sum(x => x.companies),
            suppliers = g.Sum(x => x.suppliers),
            employees = g.Sum(x => x.employees)
        }
        """


# endregion
