from typing import List

from ravendb import AbstractIndexCreationTask

from examples_base import ExampleBase


# region intersection_1
class TShirtType:
    def __init__(self, color: str = None, size: str = None):
        self.color = color
        self.size = size


class TShirt:
    def __init__(
        self, Id: str = None, release_year: int = None, manufacturer: str = None, types: List[TShirtType] = None
    ):
        self.Id = Id
        self.release_year = release_year
        self.manufacturer = manufacturer
        self.types = types


# endregion


# region intersection_2
class TShirts_ByManufacturerColorSizeAndReleaseYear(AbstractIndexCreationTask):
    class Result:
        def __init__(self, manufacturer: str = None, color: str = None, size: str = None, release_year: int = None):
            self.manufacturer = manufacturer
            self.color = color
            self.size = size
            self.release_year = release_year

    def __init__(self):
        super().__init__()
        self.map = (
            "from tshirt in docs.TShirts from type in tshirt.types select new {"
            "  manufacturer = tshirt.manufacturer,"
            "  color = tshirt.color,"
            "  size = type.size,"
            "  release_year = tshirt.release_year"
            "}"
        )


# endregion


class Intersection(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_sample(self):
        with self.embedded_server.get_document_store("Intersection") as store:
            with store.open_session() as session:
                # region intersection_3
                session.store(
                    TShirt(
                        Id="tshirts/1",
                        manufacturer="Raven",
                        release_year=2010,
                        types=[
                            TShirtType(color="Blue", size="Small"),
                            TShirtType(color="Black", size="Small"),
                            TShirtType(color="Black", size="Medium"),
                            TShirtType(color="Gray", size="Large"),
                        ],
                    )
                )

                session.store(
                    TShirt(
                        Id="tshirts/2",
                        manufacturer="Wolf",
                        release_year=2011,
                        types=[
                            TShirtType(color="Blue", size="Small"),
                            TShirtType(color="Black", size="Large"),
                            TShirtType(color="Gray", size="Medium"),
                        ],
                    )
                )

                session.store(
                    TShirt(
                        Id="tshirts/3",
                        manufacturer="Raven",
                        release_year=2011,
                        types=[TShirtType(color="Yellow", size="Small"), TShirtType(color="Gray", size="Large")],
                    )
                )

                session.store(
                    TShirt(
                        Id="tshirts/4",
                        manufacturer="Raven",
                        release_year=2012,
                        types=[TShirtType(color="Blue", size="Small"), TShirtType(color="Gray", size="Large")],
                    )
                )
                # endregion
                session.save_changes()
            with store.open_session() as session:
                # region intersection_4
                results = list(
                    session.query_index_type(
                        TShirts_ByManufacturerColorSizeAndReleaseYear,
                        TShirts_ByManufacturerColorSizeAndReleaseYear.Result,
                    )
                    .where_equals("Manufacturer", "Raven")
                    .intersect()
                    .where_equals("Color", "Blue")
                    .and_also()
                    .where_equals("Size", "Small")
                    .intersect()
                    .where_equals("Color", "Gray")
                    .and_also()
                    .where_equals("Size", "Large")
                )

                # endregion
