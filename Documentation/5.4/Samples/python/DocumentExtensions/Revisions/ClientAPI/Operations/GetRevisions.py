from ravendb.documents.operations.revisions import GetRevisionsOperation

from examples_base import ExampleBase, Company


class GetRevisions(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_revisions(self):
        with self.embedded_server.get_document_store("GetRevisionsOperation") as store:
            # region getAllRevisions
            # Define the get revisions operation, pass the document id
            get_revisions_op = GetRevisionsOperation("companies/1-A")

            # Execute the operation by passing it to Operations.Send
            revisions = store.operations.send(get_revisions_op)

            # The revisions info:
            all_revisions = revisions.results  # All the revisions
            revisions_count = revisions.total_results  # Total number of revisions
            # endregion

            # region getRevisionsWithPaging
            start = 0
            page_size = 100

            while True:
                # Execute the get revisions operation
                # Pass the document id, start & page size to get
                revisions = store.operations.send(GetRevisionsOperation("comapnies/1-A", Company, start, page_size))

                # Process the retrieved revisions here

                if len(revisions.results) < page_size:
                    break  # No more revisions to retrieve

                # Increment 'start' by page-size, to get the "next page" in next iteration
                start += page_size

            # endregion

            # region getRevisionsWithPagingParams
            parameters = GetRevisionsOperation.Parameters("companies/1-A", 0, 100)

            revisions = store.operations.send(GetRevisionsOperation.from_parameters(parameters))
            # endregion
