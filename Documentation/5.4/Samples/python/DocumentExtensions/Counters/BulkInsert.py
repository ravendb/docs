from examples_base import ExampleBase, User


class BulkInsertCounters(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_bulk_insert(self):
        with self.embedded_server.get_document_store("CountersBulkInsert") as store:
            with store.open_session() as session:
                user1 = User(name="Lilly", age=20)
                user2 = User(name="Betty", age=25)
                user3 = User(name="Robert", age=29)
                session.store(user1)
                session.store(user2)
                session.store(user3)
                session.save_changes()

            # region bulk-insert-counters
            with store.open_session() as session:
                result = list(session.query(object_type=User).where_less_than("Age", 30))
                users_ids = [session.advanced.get_document_id(user) for user in result]

            with store.bulk_insert() as bulk_insert:
                for user_id in users_ids:
                    # Choose document
                    counters_for = bulk_insert.counters_for(user_id)

                    # Add or Increment a counter
                    bulk_insert.counters_for(user_id).increment("download", 100)

            # endregion


class Foo:
    """
    # region CountersFor-definition
    def counters_for(self, id_: str) -> CountersBulkInsert:
        ...
    # endregion
    """

    # region Increment-definition
    def increment(self, name: str, delta: int = 1) -> None: ...

    # endregion
