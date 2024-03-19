from typing import Union, Callable, TypeVar, List

from ravendb.documents.queries.suggestions import (
    SuggestionWithTerm,
    SuggestionWithTerms,
    SuggestionOptions,
    StringDistanceTypes,
    SuggestionSortMode,
    SuggestionDocumentQuery,
    SuggestionBase,
    SuggestionBuilder,
    SuggestionOperations,
)
from ravendb.infrastructure.operations import CreateSampleDataOperation
from ravendb.infrastructure.orders import Product, Company
from ravendb.serverwide.operations.common import DeleteDatabaseOperation

from examples_base import ExampleBase

_T = TypeVar("_T")


class HowToWorkWithSuggestions(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("Suggestions") as store:
            with store.open_session() as session:
                store.maintenance.send(CreateSampleDataOperation())

    def tearDown(self):
        super().tearDown()
        with self.embedded_server.get_document_store("Suggestions") as store:
            store.maintenance.send(DeleteDatabaseOperation("Suggestions", hard_delete=True))

    def test_how_to_work_with_suggestions(self):
        with self.embedded_server.get_document_store("Suggestions") as store:
            with store.open_session() as session:
                # region suggest_1
                # This dynamic query on the 'Products' collection has NO documents
                products = list(session.query(object_type=Product).where_equals("name", "chaig"))
                # endregion

            with store.open_session() as session:
                # region suggest_2
                # Query for suggested terms for single term:
                # ==========================================
                suggestions = (
                    session.query(object_type=Product)
                    .suggest_using(lambda builder: builder.by_field("name", "chaig"))
                    .execute()
                )
                # endregion

            with store.open_session() as session:
                # region suggest_4
                # Define the suggestion request for single term
                suggestion_request = SuggestionWithTerm("name")
                suggestion_request.term = "chaig"

                # Query for suggestions
                suggestions = (
                    session.query(object_type=Product)
                    # Call 'suggest_using' - pass the suggestion request
                    .suggest_using(suggestion_request).execute()
                )

                # endregion

            with store.open_session() as session:
                # region suggest_5
                # Query for suggested terms for single term:
                # ==========================================
                suggestions = (
                    session.advanced
                    # Make a dynamic document-query on collection 'Products'
                    .document_query(object_type=Product)
                    # Call 'SuggestUsing'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from field 'name' that are similar to 'chaig'
                        .by_field("name", "chaig")
                    ).execute()
                )
                # endregion

                # region suggest_6
                # The resulting suggested terms:
                # ==============================

                print("Suggested terms in field 'name' that are similar to 'chaig':")
                for suggested_term in suggestions["name"].suggestions:
                    print(f"\t{suggested_term}")

                #  Suggested terms in field 'Name' that are similar to 'chaig':
                #  chai
                #  chang
                # endregion

            with store.open_session() as session:
                # region suggest_7
                # Query for suggested terms for multiple terms:
                # =============================================

                suggestions = (
                    session
                    # Make a dynamic query on collection 'Products'
                    .query(object_type=Product)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from field 'name' that are similar to 'chaig' OR 'tof'
                        .by_field("name", ["chaig", "tof"])
                    ).execute()
                )
                # endregion
            with store.open_session() as session:
                # region suggest_9
                # Define the suggestion request for multiple terms
                suggestion_request = SuggestionWithTerms("name")
                # Looking for terms from field 'name' that are similar to 'chaig' OR 'tof'
                suggestion_request.terms = ["chaig", "tof"]

                # Query for suggestions
                suggestions = (
                    session.query(object_type=Product)
                    # Call 'suggest_using' - pass the suggestion request
                    .suggest_using(suggestion_request).execute()
                )

                # endregion
            with store.open_session() as session:
                # region suggest_10
                #  Query for suggested terms for multiple terms:
                #  =============================================

                suggestions = (
                    session.advanced
                    # Make a dynamic document-query on collection 'Products'
                    .document_query(object_type=Product)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from field 'name' that are similar to 'chaig' OR 'tof'
                        .by_field("name", ["chaig", "tof"])
                    )
                ).execute()
                # endregion
                # region suggest_11
                # The resulting suggested terms:
                #  ==============================
                #
                # Suggested terms in field 'Name' that are similar to 'chaig' OR to 'tof':
                #      chai
                #      chang
                #      tofu
                # endregion
            with store.open_session() as session:
                # region suggest_12
                # Query for suggested terms in multiple fields:
                # =============================================

                suggestions = (
                    session
                    # Make a dynamic query on collection 'Companies'
                    .query(object_type=Company)
                    # Call 'suggest_using' to get suggestions for terms that are
                    # similar to 'chop-soy china' in first document field (e.g. 'name')
                    .suggest_using(lambda builder: builder.by_field("name", "chop-soy china"))
                    # Call 'and_suggest_using' to get suggestions for terms that are
                    # similar to 'maria larson' in an additional field (e.g. 'Contact.Name')
                    .and_suggest_using(lambda builder: builder.by_field("contact.name", "maria larson")).execute()
                )
                # endregion
            with store.open_session() as session:
                # region suggest_14
                # Define suggestion requests for multiple fields:

                request1 = SuggestionWithTerm("name")
                # Looking for terms from field 'Name' that are similar to 'chop-soy china'
                request1.term = "chop-soy china"

                request2 = SuggestionWithTerm("contact.name")
                # Looking for terms from nested field 'Contact.Name' that are similar to 'maria larson'
                request2.term = ["maria larson"]

                suggestions = (
                    session.query(object_type=Company)
                    # Call 'suggest_using' - pass the suggestion request for the first field
                    .suggest_using(request1)
                    # Call 'and_suggest_using' - pass the suggestion request for the second field
                    .and_suggest_using(request2).execute()
                )
                # endregion
            with store.open_session() as session:
                # region suggest_15
                # Query for suggested terms in multiple fields:
                # =============================================
                suggestions = (
                    session.advanced
                    # Make a dynamic document-query on collection 'Companies'
                    .document_query(object_type=Company)
                    # Call 'suggest_using' to get suggestions for terms that are
                    # similar to 'chop-soy china' in first document field (e.g. 'name')
                    .suggest_using(lambda builder: builder.by_field("name", "chop-soy china"))
                    # Call 'and_suggest_using' to get suggestions for terms that are
                    # similar to 'maria larson' in an additional field (e.g. 'Contact.Name')
                    .and_suggest_using(lambda builder: builder.by_field("contact.name", "maria larson")).execute()
                )
                # endregion
                # region suggest_16
                # The resulting suggested terms:
                # ==============================
                #
                # Suggested terms in field 'name' that is similar to 'chop-soy china':
                #     chop-suey chinese
                #
                # Suggested terms in field 'contact.name' that are similar to 'maria larson':
                #     maria larsson
                #     marie bertrand
                #     aria cruz
                #     paula wilson
                #     maria anders
                # endregion
            with store.open_session() as session:
                # region suggest_17
                #  Query for suggested terms - customize options and display name:
                #  ===============================================================
                suggestions = (
                    session
                    # Make a dynamic query on collection 'Products'
                    .query(object_type=Product)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder.by_field("name", "chaig")
                        # Customize suggestion options
                        .with_options(
                            SuggestionOptions(
                                accuracy=0.4,
                                page_size=5,
                                distance=StringDistanceTypes.JARO_WINKLER,
                                sort_mode=SuggestionSortMode.POPULARITY,
                            )
                        )
                        # Customize display name for results
                        .with_display_name("SomeCustomName")
                    ).execute()
                )
                # endregion
            with store.open_session() as session:
                # region suggest_19
                # Define the suggestion request
                suggestion_request = SuggestionWithTerm("name")
                # Looking for terms from field 'Name' that are similar to term 'chaig'
                suggestion_request.term = "chaig"
                # Customize options
                suggestion_request.options = SuggestionOptions(
                    accuracy=5,
                    page_size=5,
                    distance=StringDistanceTypes.JARO_WINKLER,
                    sort_mode=SuggestionSortMode.POPULARITY,
                )
                # Customize display name
                suggestion_request.display_field = "SomeCustomName"

                # Query for suggestions
                suggestions = (
                    session.query(object_type=Product)
                    # Call 'suggest_using' - pass the suggestion request
                    .suggest_using(suggestion_request).execute()
                )
                # endregion
            with store.open_session() as session:
                # region suggest_20
                # Query for suggested terms - customize options and display name:
                # ===============================================================
                suggestions = (
                    session.advanced
                    # Make a dynamic query on collection 'Products'
                    .document_query(object_type=Product)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder.by_field("name", "chaig")
                        # Customize suggestion options
                        .with_options(
                            SuggestionOptions(
                                accuracy=0.4,
                                page_size=5,
                                distance=StringDistanceTypes.JARO_WINKLER,
                                sort_mode=SuggestionSortMode.POPULARITY,
                            )
                        )
                        # Customize display name for results
                        .with_display_name("SomeCustomName")
                    ).execute()
                )
                # endregion
                # region suggest_21
                # The resulting suggested terms:
                # ==============================

                print("Suggested terms:")
                # Results are available under the custom name entry
                for suggested_term in suggestions["SomeCustomName"].suggestions:
                    print(f"\t{suggested_term}")

                # Suggested terms:
                #     chai
                #     chang
                #     chartreuse verte
                # endregion


class Foo:
    # region syntax_1
    # Method for requesting suggestions for term(s) in a field:
    def suggest_using(
        self, suggestion_or_builder: Union[SuggestionBase, Callable[[SuggestionBuilder[_T]], None]]
    ) -> SuggestionDocumentQuery[_T]: ...

    # Method for requesting suggestions for term(s) in another field in the same query:
    def and_suggest_using(
        self, suggestion_or_builder: Union[SuggestionBase, Callable[[SuggestionBuilder[_T]], None]]
    ) -> SuggestionDocumentQuery[_T]: ...

    # endregion

    # region syntax_2
    def by_field(self, field_name: str, term_or_terms: Union[str, List[str]]) -> SuggestionOperations[_T]: ...

    def with_display_name(self, display_name: str) -> SuggestionOperations[_T]: ...
    def with_options(self, options: SuggestionOptions) -> SuggestionOperations[_T]: ...

    # endregion


class Foo2:
    # region syntax_3
    DEFAULT_ACCURACY = 0.5
    DEFAULT_PAGE_SIZE = 15
    DEFAULT_DISTANCE = StringDistanceTypes.LEVENSHTEIN
    DEFAULT_SORT_MODE = SuggestionSortMode.POPULARITY

    def __init__(
        self,
        page_size: int = DEFAULT_PAGE_SIZE,
        distance: StringDistanceTypes = DEFAULT_DISTANCE,
        accuracy: float = DEFAULT_ACCURACY,
        sort_mode: SuggestionSortMode = DEFAULT_SORT_MODE,
    ):
        self.page_size = page_size
        self.distance = distance
        self.accuracy = accuracy
        self.sort_mode = sort_mode

    # endregion
