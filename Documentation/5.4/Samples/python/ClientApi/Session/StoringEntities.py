from typing import Optional

from ravendb.infrastructure.orders import Employee

from examples_base import ExamplesBase


class StoringEntities(ExamplesBase):
    def setUp(self):
        super().setUp()

    class IFoo:
        # region store_entities_1
        def store(self, entity: object, key: Optional[str] = None, change_vector: Optional[str] = None) -> None:
            ...

        # endregion

    def test_storing_entities(self):
        with self.embedded_server.get_document_store("StoringEntities") as store:
            with store.open_session() as session:
                # region store_entities_5
                session.store(Employee(first_name="John", last_name="Doe"))

                # send all pending operations to server, in this case only 'Put' operation
                session.save_changes()
                # endregion
