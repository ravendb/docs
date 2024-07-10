import datetime
from typing import Type, List, Dict, TypeVar

from ravendb.primitives import constants

from examples_base import ExampleBase, Order

_T = TypeVar("_T")


class GetRevisions(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_revisions(self):
        with self.embedded_server.get_document_store("GetRevisions") as store:
            with store.open_session() as session:
                # region example_1_sync
                # Get revisions for document 'orders/1-A'
                # Revisions will be ordered by most recent revision first
                order_revisions = session.advanced.revisions.get_for("orders/1-A", Order, 0, 10)
                # endregion

                # region example_2_sync
                # Get 'revisions' metadata for document 'orders/1-A'
                order_revisions_metadata = session.advanced.revisions.get_metadata_for("orders/1-A", 0, 10)

                # Each item returned is a revision's metadata, as can be verified in the @flags key
                metadata = order_revisions_metadata[0]
                flags_value = metadata[constants.Documents.Metadata.FLAGS]

                self.assertIn("Revision", flags_value)
                # endregion

                # region example_3_sync
                # Get a revision by its creation time
                revision_from_last_year = (
                    session.advanced.revisions
                    # If no revision was created at the specified time,
                    # then the first revision that precedes it will be returned
                    .get_by_before_date("orders/1-A", datetime.datetime.utcnow() - datetime.timedelta(days=365))
                )
                # endregion
                # region example_4_sync
                # Get revisions metadata
                revisions_metadata = session.advanced.revisions.get_metadata_for("orders/1-A", 0, 25)

                # Get the change-vector from the metadata
                change_vector = revisions_metadata[0][constants.Documents.Metadata.CHANGE_VECTOR]

                # Get the revision by its change-vector
                revision = session.advanced.revisions.get_by_change_vector(change_vector, Order)
                # endregion


class Foo:
    # region syntax_1
    def get_for(self, id_: str, object_type: Type[_T] = None, start: int = 0, page_size: int = 25) -> List[_T]: ...

    # endregion
    # region syntax_2
    def get_metadata_for(self, id_: str, start: int = 0, page_size: int = 25) -> List["MetadataAsDictionary"]: ...

    # endregion
    # region syntax_3
    def get_by_before_date(self, id_: str, before_date: datetime.datetime, object_type: Type[_T] = None) -> _T: ...

    # endregion
    # region syntax_4
    # Get a revision by its change vector
    def get_by_change_vector(self, change_vector: str, object_type: Type[_T] = None) -> _T: ...

    # Get multiple revisions by their change vectors
    def get_by_change_vectors(self, change_vectors: List[str], object_type: Type[_T] = None) -> Dict[str, _T]: ...

    # endregion
