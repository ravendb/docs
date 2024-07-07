from ravendb.primitives import constants

from examples_base import ExampleBase


class ExtractCountersFromRevisions(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_extract_counters_from_revisions(self):
        with self.embedded_server.get_document_store("ExtractCountersFromRevisions") as store:
            with store.open_session() as session:
                # region extract_counter
                # Use get_metadata_for to get revisions metadata for document 'orders/1-A'
                revisions_metadata = session.advanced.revisions.get_metadata_for("orders/1-A")

                # Extract the counters data from the metadata
                counters_data_in_revisions = [
                    metadata[constants.Documents.Metadata.REVISION_COUNTERS]
                    for metadata in revisions_metadata
                    if constants.Documents.Metadata.REVISION_COUNTERS in metadata
                ]
                # endregion
