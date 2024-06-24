from __future__ import annotations
from typing import List, Any, Dict

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractJavaScriptIndexCreationTask

from examples_base import ExampleBase


# region online_shop_class
class OnlineShop:
    def __init__(self, shop_name: str = None, email: str = None, t_shirts: List[TShirt] = None):
        self.shop_name = shop_name
        self.email = email
        self.t_shirts = t_shirts

    @classmethod
    def from_json(cls, json_data: Dict[str, Any]) -> OnlineShop:
        return cls(
            json_data["shop_name"],
            json_data["email"],
            [TShirt.from_json(shirt_json_dict) for shirt_json_dict in json_data["t_shirts"]],
        )

    def to_json(self) -> Dict[str, Any]:
        return {
            "shop_name": self.shop_name,
            "email": self.email,
            "t_shirts": [tshirt.to_json() for tshirt in self.t_shirts],
        }


class TShirt:
    def __init__(self, color: str = None, size: str = None, logo: str = None, price: float = None, sold: int = None):
        self.color = color
        self.size = size
        self.logo = logo
        self.price = price
        self.sold = sold

    @classmethod
    def from_json(cls, json_data: Dict[str, Any]) -> TShirt:
        return cls(json_data["color"], json_data["size"], json_data["logo"], json_data["price"], json_data["sold"])

    def to_json(self) -> Dict[str, Any]:
        return {"color": self.color, "size": self.size, "logo": self.logo, "price": self.price, "sold": self.sold}


# endregion


# region simple_index
class Shops_ByTShirt_Simple(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, colors: List[str] = None, sizes: List[str] = None, logos: List[str] = None):
            # The index-fields:
            self.colors = colors
            self.sizes = sizes
            self.logos = logos

    def __init__(self):
        super().__init__()
        # Creating a SINGLE index-entry per document:
        self.map = (
            "from shop in docs.OnlineShops "
            "select new { "
            # Each index-field will hold a collection of nested values from the document
            "    colors = shop.t_shirts.Select(x => x.color),"
            "    sizes = shop.t_shirts.Select(x => x.size),"
            "    logos = shop.t_shirts.Select(x => x.logo)"
            "}"
        )


# endregion


# region fanout_index_1
# A fanout map-index:
# ===================
class Shops_ByTShirt_Fanout(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, color: str = None, size: str = None, logo: str = None):
            self.color = color
            self.size = size
            self.logo = logo

    def __init__(self):
        super().__init__()
        # Creating MULTIPLE index-entries per document,
        # an index-entry for each sub-object in the TShirts list
        self.map = (
            "from shop in docs.OnlineShops from shirt in shop.t_shirts "
            "select new {"
            "    color = shirt.color,"
            "    size = shirt.size,"
            "    logo = shirt.logo"
            "}"
        )


# endregion


# region fanout_index_2
class Sales_ByTShirtColor_Fanout(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, color: str = None, items_sold: int = None, total_sales: float = None):
            self.color = color
            self.items_sold = items_sold
            self.total_sales = total_sales

    def __init__(self):
        super().__init__()
        # Creating MULTIPLE index-entries per document,
        # an index-entry for each sub-object in the TShirts list
        self.map = (
            "from shop in docs.OnlineShops from shirt in shop.t_shirts "
            "select new {"
            "    color = shirt.color, "
            "    items_sold = shirt.sold, "
            "    total_sales = shirt.price * shirt.sold"
            "}"
        )
        self.reduce = (
            "from result in results group result by result.color into g select new {"
            "    color = g.Key,"
            "    items_sold = g.Sum(x => x.items_sold),"
            "    total_sales = g.Sum(x => x.total_sales)"
            "}"
        )


# endregion


# region fanout_index_js
class Shops_ByTShirt_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('OnlineShops', function (shop){ 
                       var res = [];
                       shop.t_shirts.forEach(shirt => {
                           res.push({
                               color: shirt.color,
                               size: shirt.size,
                               logo: shirt.logo
                           })
                        });
                        return res;
                    })
            """
        }


# endregion


class IndexingNestedData(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_nested_data(self):
        with self.embedded_server.get_document_store("NestedData") as store:
            # region sample_data
            # Creating sample data for the examples in this article:
            # ======================================================
            shop1_tshirts = [
                TShirt(color="Red", size="S", logo="Bytes and Beyond", price=25, sold=2),
                TShirt(color="Red", size="M", logo="Bytes and Beyond", price=25, sold=4),
                TShirt(color="Blue", size="M", logo="Query Everything", price=28, sold=5),
                TShirt(color="Green", size="L", logo="Data Driver", price=30, sold=3),
            ]

            shop2_tshirts = [
                TShirt(color="Blue", size="S", logo="Coffee, Code, Repeat", price=22, sold=12),
                TShirt(color="Blue", size="M", logo="Coffee, Code, Repeat", price=22, sold=7),
                TShirt(color="Green", size="M", logo="Big Data Dreamer", price=25, sold=9),
                TShirt(color="Black", size="L", logo="Data Mining Expert", price=20, sold=11),
            ]

            shop3_tshirts = [
                TShirt(color="Red", size="S", logo="Bytes of Wisdom", price=18, sold=2),
                TShirt(color="Blue", size="M", logo="Data Geek", price=20, sold=6),
                TShirt(color="Black", size="L", logo="Data Revolution", price=15, sold=8),
                TShirt(color="Black", size="XL", logo="Data Revolution", price=15, sold=10),
            ]

            online_shops = [
                OnlineShop(shop_name="Shop1", email="sales@shop1.com", t_shirts=shop1_tshirts),
                OnlineShop(shop_name="Shop2", email="sales@shop2.com", t_shirts=shop2_tshirts),
                OnlineShop(shop_name="Shop3", email="sales@shop3.com", t_shirts=shop3_tshirts),
            ]

            Shops_ByTShirt_Simple().execute(store)
            Shops_ByTShirt_Fanout().execute(store)
            Sales_ByTShirtColor_Fanout().execute(store)

            with store.open_session() as session:
                for shop in online_shops:
                    session.store(shop)

                session.save_changes()

            # endregion

            # region simple_index_query_1
            # Query for all shop documents that have a red TShirt
            shops_that_have_red_shirts = list(
                session.query_index_type(Shops_ByTShirt_Simple, Shops_ByTShirt_Simple.IndexEntry)
                .contains_any("colors", ["Red"])
                .of_type(OnlineShop)
            )
            # endregion
            # region results_1
            # Results will include the following shop documents:
            # ==================================================
            # * Shop1
            # * Shop3
            # endregion

            # region results_2
            # You want to query for shops containing "Large Green TShirts",
            # aiming to get only "Shop1" as a result since it has such a combination,
            # so you attempt this query:
            green_and_large = list(
                session.query_index_type(Shops_ByTShirt_Simple, Shops_ByTShirt_Simple.IndexEntry)
                .contains_any("colors", ["green"])
                .and_also()
                .contains_any("sizes", "L")
                .of_type(OnlineShop)
            )

            # But, the results of this query will include BOTH "Shop1" & "Shop2"
            # since the index-queries do not keep the original sub-subjects structure.
            # endregion
            # region fanout_index_query_1
            # Query the fanout index:
            # =======================
            shops_that_have_medium_red_shirts = list(
                session.query_index_type(Shops_ByTShirt_Fanout, Shops_ByTShirt_Fanout.IndexEntry)
                # Query for documents that have a "Medium Red TShirt"
                .where_equals("color", "red")
                .and_also()
                .where_equals("size", "M")
                .of_type(OnlineShop)
            )
            # endregion
            # region results_3
            # Query results:
            # ==============
            #
            # Only the 'Shop1' document will be returned,
            # since it is the only document that has the requested combination within the TShirt list.
            # endregion

            # region fanout_index_query_4
            # Query the fanout index:
            # =======================
            query_result = (
                session.query_index_type(Sales_ByTShirtColor_Fanout, Sales_ByTShirtColor_Fanout.IndexEntry)
                # Query for index-entries that contain "black"
                .where_equals("color", "black").first()
            )

            # Get total sales for black TShirts
            black_shirts_sales = query_result.total_sales or 0
            # endregion
