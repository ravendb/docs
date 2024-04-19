from typing import List

from examples_base import ExampleBase


class GetCountersFor(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_counters_for(self):
        with self.embedded_server.get_document_store("GetCountersFor") as store:
            with store.open_session() as session:
                # region example
                # Load a document
                employee = session.load("employees/1-A")

                # Get counter names from the loaded entity
                counter_names = session.advanced.get_counters_for(employee)
                # endregion

    class Foo:
        # region syntax
        def get_counters_for(self, entity: object) -> List[str]: ...

        # endregion
