from typing import TypeVar

from ravendb import DocumentQuery

from examples_base import ExampleBase

_T = TypeVar("_T")


class TShirt:
    pass


class HowToUseIntersect(ExampleBase):
    def setUp(self):
        super().setUp()

    class Foo:
        # region intersect_1
        def intersect(self) -> DocumentQuery[_T]: ...

        # endregion

    def test_how_to_use_intersect(self):
        with self.embedded_server.get_document_store() as store:
            with store.open_session() as session:
                # region intersect_2
                # return all T-shirts that are manufactured by 'Raven'
                # and contain both 'Small Blue' and 'Large Gray' types
                tshirts = list(
                    session.query_index("TShirts/ByManufacturerColorSizeAndReleaseYear")
                    .where_equals("manufacturer", "Raven")
                    .intersect()
                    .where_equals("color", "Blue")
                    .and_also()
                    .where_equals("size", "Small")
                    .intersect()
                    .where_equals("color", "Gray")
                    .and_also()
                    .where_equals("size", "Large")
                    .of_type(TShirt)
                )
                # endregion
