from typing import Dict

from examples_base import ExampleBase, Product


class CountersExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_counters(self):
        with self.embedded_server.get_document_store("CountersExample") as store:
            # region counters_region_CountersFor_with_document_load
            # Use counters_for_entity by passing it a document object

            # 1. Open a session
            with store.open_session() as session:
                # 2. Use the session to load a document.
                document = session.load("products/1-C")

                # 3. Create an instance of 'CountersFor'
                #    Pass the document object returned from session.load as a param.
                document_counters = session.counters_for_entity(document)

                # 4. Use 'CountersFor' methods to manage the product document's counters
                document_counters.delete("ProductLikes")  # Delete the "ProductLikes" counter
                document_counters.increment("ProductModified", 15)  # Add 15 to Counter "ProductModified"
                counter = document_counters.get("DaysLeftForSale")  # Get value of "DaysLeftForSale"

                # 5. Execute all changes by calling save_changes
                session.save_changes()
            # endregion

        # region counters_region_CountersFor_without_document_load
        # Use CountersFor without loading a document

        # 1. Open a session
        with store.open_session() as session:
            # 2. pass an explicit document ID to the CountersFor constructor
            document_counters = session.counters_for("products/1-C")

            # 3. Use 'CountersFor' methods to manage the product document's counters
            document_counters.delete("ProductLikes")  # Delete the "ProductLikes" counter
            document_counters.increment("ProductModified", 15)  # Add 15 to Counter "ProductModified"
            counter = document_counters.get("DaysLeftForSale")  # Get "DaysLeftForSale"'s value

            # 4. Execute all changes by calling save_changes
            session.save_changes()
        # endregion

        # Increment a counter's value
        # region counters_region_Increment
        # Open a session
        with store.open_session() as session:
            # Pass CountersFor's constructor a document ID
            document_counters = session.counters_for("products/1-A")

            # Use 'CountersFor.increment'
            # ===========================

            # Increase "ProductLikes" by 1, or create it if doesn't exist with a value of 1
            document_counters.increment("ProductLikes")

            # Increase "ProductPageViews" by 15, or create it if doesn't exist with a value of 15
            document_counters.increment("ProductPageViews", 15)

            # Decrease "DaysLeftForSale" by 10, or create it if doesn't exist with a value of -10
            document_counters.increment("DaysLeftForSale", -10)

            # Execute all changes by calling save_changes
            session.save_changes()

        # endregion

        # get a counter's value by the counter's name
        # region counters_region_Get
        # 1. Open a session
        with store.open_session() as session:
            # 2. pass CountersFor's constructor a document ID
            document_counters = session.counters_for("products/1-C")

            # 3. Use 'CountersFor.Get' to retrieve a Counter's value
            days_left = document_counters.get("DaysLeftForSale")
            print(f"Days Left For Sale: {days_left}")
        # endregion

        # region counters_region_GetAll
        # 1. Open a session
        with store.open_session() as session:
            # 2. pass CountersFor's constructor a document ID
            document_counters = session.counters_for("products/1-C")

            # 3. Use GetAll to retrieve all of the document's counters' names and values
            counters = document_counters.get_all()

            # list counters' names and values
            for counter_name, counter_value in counters.items():
                print(f"counter name: {counter_name}, counter value: {counter_value}")
        # endregion

        with store.open_session() as session:
            # region counters_region_load_include1
            # include single Counters
            product_page = session.load(
                "products/1-C",
                include_builder=lambda builder: builder.include_counter("ProductLikes")
                .include_counter("ProductDislikes")
                .include_counter("ProductDownloads"),
            )
            # endregion

        with store.open_session() as session:
            # region counters_region_load_include2
            # include multiple Counters
            # note that you can combine the inclusion of Counters and documents.
            product_page = session.load(
                "orders/1-A",
                include_builder=lambda builder: builder.include_documents("products/1-C").include_counters(
                    ["ProductLikes", "ProductDislikes"]
                ),
            )
            # endregion

        with store.open_session() as session:
            # region counters_region_query_include_single_Counter
            # include a single Counter
            query = session.query(object_type=Product).include(lambda builder: builder.include_counter("ProductLikes"))
            # endregion

        with store.open_session() as session:
            # region counters_region_query_include_multiple_Counters
            # include multiple Counters
            query = session.query("Products").include(
                lambda builder: builder.include_counters(["ProductLikes", "ProductDownloads"])
            )
            # endregion

        with store.open_session() as session:
            # region counters_region_rawqueries_counter
            # Various RQL expressions sent to the server using counter()
            # Returned Counter value is accumulated
            rawquery1 = list(session.advanced.raw_query("from products as p select counter(p, 'ProductLikes')"))

            rawquery2 = list(
                session.advanced.raw_query("from products select counter('ProductLikes') as ProductLikesCount")
            )

            rawquery3 = list(
                session.advanced.raw_query("from products where PricePerUnit > 50 select Name, counter('ProductLikes')")
            )
            # endregion

            # region counters_region_rawqueries_counterRaw
            # An RQL expression sent to the server using counterRaw()
            # Returned Counter value is distributed
            query = list(session.advanced.raw_query("from users as u select counterRaw(u, 'downloads')"))
            # endregion


class CounterResult:
    def __init__(self, product_price: int = None, product_likes: int = None, product_section: str = None):
        self.product_price = product_price
        self.product_likes = product_likes
        self.product_section = product_section


class CounterResultRaw:
    def __init__(self, downloads: Dict[str, int]):
        self.downloads = downloads


# region counters_region_CounterItem
# The value given to a Counter by each node, is placed in a CounterItem object.
class CounterItem:
    def __init__(self, name: str = None, doc_id: str = None, change_vector: str = None, value: int = None):
        self.name = name
        self.doc_id = doc_id
        self.change_vector = change_vector
        self.value = value


# endregion


class Foo:
    # region Increment-definition
    def increment(self, counter: str, delta: int = 1) -> None: ...

    # endregion
    # region Delete-definition
    def delete(self, counter: str) -> None: ...

    # endregion
    # region Get-definition
    def get(self, counter) -> int: ...

    # endregion
    # region GetAll-definition
    def get_all(self) -> Dict[str, int]: ...

    # endregion
