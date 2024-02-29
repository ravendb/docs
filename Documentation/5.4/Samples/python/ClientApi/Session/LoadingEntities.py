from typing import Optional, Union, Dict, Callable, Type, List, TypeVar

from ravendb import IncludeBuilder, LoaderWithInclude, ConditionalLoadResult
from ravendb.infrastructure.entities import User
from ravendb.infrastructure.orders import Employee, Product, Supplier

from examples_base import ExamplesBase

_T = TypeVar("_T")


class LoadingEntities(ExamplesBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("LoadingEntitiesXY").open_session() as session:
            session.store(Product(supplier="suppliers/1"), "products/1")
            session.save_changes()

    class Foo:
        # region loading_entities_1_0
        def load(
            self,
            key_or_keys: Union[List[str], str],
            object_type: Optional[Type[_T]] = None,
            includes: Callable[[IncludeBuilder], None] = None,
        ) -> Union[Dict[str, _T], _T]:
            ...

        # endregion
        # region loading_entities_2_0
        def include(self, path: str) -> LoaderWithInclude:
            ...

        # endregion

        # region loading_entities_3_0
        def load(
            self,
            key_or_keys: Union[List[str], str],  # <- List of ids
            object_type: Optional[Type[_T]] = None,
            includes: Callable[[IncludeBuilder], None] = None,
        ) -> Union[Dict[str, _T], _T]:
            ...

        # endregion
        # region loading_entities_4_0
        def load_starting_with(
            self,
            id_prefix: str,
            object_type: Optional[Type[_T]] = None,
            matches: Optional[str] = None,
            start: Optional[int] = None,
            page_size: Optional[int] = None,
            exclude: Optional[str] = None,
            start_after: Optional[str] = None,
        ) -> List[_T]:
            ...

        def load_starting_with_into_stream(
            self,
            id_prefix: str,
            output: bytes,
            matches: str = None,
            start: int = 0,
            page_size: int = 25,
            exclude: str = None,
            start_after: str = None,
        ):
            ...

    # endregion
    # region loading_entities_5_0
    # unsupported, will be supported from 5.4 client release (https://pypi.org/project/ravendb/)
    # endregion
    # region loading_entities_6_0
    def is_loaded(self, key: str) -> bool:
        ...

    # endregion

    # region loading_entities_7_0
    def conditional_load(self, key: str, change_vector: str, object_type: Type[_T] = None) -> ConditionalLoadResult[_T]:
        ...

    # endregion

    def test_loading_entities_xy(self):
        with self.embedded_server.get_document_store("LoadingEntitiesXY") as store:
            with store.open_session() as session:
                # region loading_entities_1_1

                employee = session.load("employees/1", Employee)

                # endregion

            with store.open_session() as session:
                # region loading_entities_2_1

                # loading 'products/1'
                # including document found in 'supplier' property
                products_by_key = session.include("supplier").load(Product, "products/1")
                product = products_by_key["products/1"]

                supplier = session.load(product.supplier)  # this will not make server call

                # endregion

            with store.open_session() as session:
                # region loading_entities_2_2

                # loading 'products/1'
                # including document found in 'Supplier' property
                products_by_key = session.include("Supplier").load(Product, "products/1")
                product = products_by_key["products/1"]

                supplier = session.load(product.supplier, Supplier)
                # endregion

            with store.open_session() as session:
                # region loading_entities_3_1
                employees = session.load(["employees/1", "employees/2", "employees/3"], Employee)
                # endregion

            with store.open_session() as session:
                # region loading_entities_4_1
                # return up to 128 entities with Id that starts with 'employees'
                result = session.advanced.load_starting_with("employees/", Employee, None, 0, 128)
                # endregion

            with store.open_session() as session:
                # region loading_entities_4_2
                # return up to 128 entities with Id that starts with 'employees'
                # and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
                result = session.advanced.load_starting_with("employees/", Employee, "1*|2*", 0, 128)
                # endregion

            with store.open_session() as session:
                # region loading_entities_5_1
                # unsupported, will be supported from 5.4 client release (https://pypi.org/project/ravendb/)
                # endregion
                ...
            with store.open_session() as session:
                session.advanced.load_starting_with_into_stream = lambda x, y: None
                # region loading_entities_5_2
                stream_bytes = bytes(b"My loaded documents will go here -> ")
                session.advanced.load_starting_with_into_stream("employees/", stream_bytes)
                # endregion

            with store.open_session() as session:
                # region loading_entities_6_1
                is_loaded = session.advanced.is_loaded("employees/1")  # False
                employee = session.load("employees/1")
                is_loaded = session.advanced.is_loaded("employees/1")  # True
                # endregion

            # region loading_entities_7_1
            change_vector: Optional[str] = None
            user = User(name="Bob")

            with store.open_session() as session:
                session.store(user, "users/1")
                session.save_changes()

                change_vector = session.advanced.get_change_vector_for(user)

            # Now session which does not track our User entity
            with store.open_session() as session:
                # The given change vector matches
                # the server-side change vector
                # Does not load the document
                result1 = session.advanced.conditional_load("users/1", change_vector)

                # Modify the document
                user.name = "Bob Smith"
                session.store(user)
                session.save_changes()

                # Change vectors do not natch
                # Loads the document
                result2 = session.advanced.conditional_load("user/1", change_vector)
            # endregion
