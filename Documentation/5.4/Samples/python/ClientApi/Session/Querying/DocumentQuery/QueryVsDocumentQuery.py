from ravendb import QueryOperator

from examples_base import ExampleBase


class User:
    def __init__(self, Id: str = None, name: str = None, eye_color: str = None, age: int = None):
        self.Id = Id
        self.name = name
        self.eye_color = eye_color
        self.age = age


class LoadingEntities(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_query_vs_document_query(self):
        with self.embedded_server.get_document_store("QueryVsDocumentQuery") as store:
            with store.open_session() as session:
                # region immutable_query
                query = session.query(object_type=User).where_starts_with("name", "A")

                age_query = query.where_greater_than_or_equal("age", 21)

                eye_query = query.where_equals("eye_color", "blue")
                # endregion

                # region mutable_query
                document_query = session.advanced.document_query(object_type=User).where_starts_with("name", "A")

                age_document_query = document_query.where_greater_than_or_equal("age", 21)

                eye_document_query = document_query.where_equals("eye_color", "blue")

                # Here all of the DocumentQuery variables have the same reference
                # endregion

                # region default_operator
                session.advanced.document_query(object_type=User).using_default_operator(QueryOperator.OR)
                # endregion
