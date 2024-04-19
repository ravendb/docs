from typing import Callable, Optional, TypeVar, Dict, List

from ravendb import DocumentQuery, Explanations, ExplanationOptions

from examples_base import ExampleBase, Product

_T = TypeVar("_T")


class IncludeExplanations(ExampleBase):
    def setUp(self):
        super().setUp()

    class Foo:
        # region syntax
        def include_explanations(
            self,
            options: Optional[ExplanationOptions] = None,
            explanations_callback: Callable[[Explanations], None] = None,
        ) -> DocumentQuery[_T]: ...

        # endregion
        # region syntax_2
        class Explanations:
            def __init__(self):
                self.explanations: Dict[str, List[str]] = {}

        # Get explanations from the dict of a resulting Explanations object - document ids' are keys
        # endregion

        # region syntax_3
        class ExplanationOptions:
            def __init__(self, group_key: str = None):
                self.group_key = group_key

        # endregion

    def test_explain(self):
        with self.embedded_server.get_document_store("Explain") as store:
            with store.open_session() as session:
                # region explain
                # Prepare a callback
                explanations_results: Optional[Explanations] = None

                def explanations_callback(explanations: Explanations):
                    explanations_results = explanations

                # Query with 'document_query'

                # Execute the query
                results = list(
                    # Prepare a query
                    session.advanced.document_query(object_type=Product)
                    # Call include_expirations, provide an out param for the explanations results
                    .include_explanations()
                    # Define query criteria
                    # i.e. search for docs containing Syrup -or- Lager in their Name field
                    .search("Name", "Syrup Lager")
                )

                # Get the score details for a specific document from the results
                # Get explanations from the resulting Explanations object
                score_details = explanations_results.explanations[results[0].Id]
                # endregion
