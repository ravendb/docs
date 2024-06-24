from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing
from ravendb.documents.queries.suggestions import (
    SuggestionWithTerm,
    SuggestionWithTerms,
    SuggestionOptions,
    StringDistanceTypes,
    SuggestionSortMode,
)

from examples_base import ExampleBase, Product0


# region suggestions_index_1
class Products_ByName(AbstractIndexCreationTask):
    # The IndexEntry class defines the index-fields
    class IndexEntry:
        def __init__(self, product_name: str = None):
            self.product_name = product_name

    def __init__(self):
        super().__init__()
        # The 'map' function defines the content of the index-fields
        self.map = "from product in docs.Products select new {product_name = product.Name}"
        self._suggestion("product_name")
        self._index("product_name", FieldIndexing.SEARCH)


# endregion


# region suggestions_index_2
class Companies_ByNameAndByContactName(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, company_name: str = None, contact_name: str = None):
            self.company_name = company_name
            self.contact_name = contact_name

    def __init__(self):
        super().__init__()
        self.map = "from company in docs.Companies select new {company_name = company.Name, contact_name = company.Contact.Name}"

        # Configure the index-fields for suggestions
        self._suggestion("company_name")
        self._suggestion("contact_name")

        # Optionally: set 'search' on the index-fields
        # This will split the fields' content into multiple terms allowing for a full-text search
        self._index("company_name", FieldIndexing.SEARCH)
        self._index("contact_name", FieldIndexing.SEARCH)


# endregion


class Suggestions(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_suggestions(self):
        with self.embedded_server.get_document_store("SuggestionsIndexes") as store:
            with store.open_session() as session:
                # region suggestions_2
                # This query on index 'Products/ByName' has NO resulting documents
                products = list(
                    session.query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    .search("product_name", "chokolade")
                    .of_type(Product)
                )
                # endregion
                # region suggestions_3
                # Query the index for suggested terms for single term:
                # ====================================================

                suggestions = (
                    session
                    # Query the index
                    .query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from index-field 'ProductName' that are similar to 'chokolade'
                        .by_field("product_name", "chokolade")
                    ).execute()
                )

                # endregion

                # region suggestions_5
                # Define the suggestion request for single term
                suggestion_request = SuggestionWithTerm("product_name")
                # Looking for terms from index-field 'product_name' that are similar to 'chokolade'
                suggestion_request.term = "chokolade"

                # Query the index for suggestions
                suggestions = (
                    session.query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using' - pass the suggestion request
                    .suggest_using(suggestion_request).execute()
                )
                # endregion

                # region suggestions_6
                # Query the index for suggested terms for single term:
                # ====================================================

                suggestions = (
                    session
                    # Query the index
                    .query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from index-field 'product_name' that are similar to 'chokolade'
                        .by_field("product_name", "chokolade")
                    ).execute()
                )

                # endregion

                # region suggestions_7
                # The resulting suggested terms:
                # ==============================

                print("Suggested terms in index-field 'product_name' that are similar to 'chokolade':")
                for suggested_term in suggestions["product_name"].suggestions:
                    print(f"\t{suggested_term}")

                # Suggested terms in index-field 'product_name' that are similar to 'chokolade':
                #     schokolade
                #     chocolade
                #     chocolate
                # endregion

                # region suggestions_8
                # Query the index for suggested terms for multiple terms:
                # =======================================================

                suggestions = (
                    session
                    # Query the index
                    .query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from index-field 'product_name' that are similar to 'chokolade' OR 'syrop'
                        .by_field("product_name", ["chokolade", "syrop"])
                    ).execute()
                )
                # endregion
                # region suggestions_10
                # Define the suggestion request for multiple terms
                suggestion_request = SuggestionWithTerms("product_name")
                # Looking for terms from index-field 'product_name' that are similar to 'chokolade' OR 'syrop'
                suggestion_request.terms = ["chokolade", "syrop"]

                # Query the index for suggestions
                suggestions = (
                    session.query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using' - pass the suggestion request
                    .suggest_using(suggestion_request).execute()
                )
                # endregion
                # region suggestions_11
                # Query the index for suggested terms for multiple terms:
                # =======================================================

                suggestions = (
                    session.advanced
                    # Query the index
                    .document_query(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder
                        # Request to get terms from index-field 'product_name' that are similar to 'chokolade' OR 'syrop'
                        .by_field("product_name", ["chokolade", "syrop"])
                    ).execute()
                )
                # endregion

                # region suggestions_12
                # The resulting suggested terms:
                # ==============================

                # Suggested terms in index-field 'product_name' that are similar to 'chokolade' OR to 'syrop':
                #     schokolade
                #     chocolade
                #     chocolate
                #     sirop
                #     syrup
                # endregion
                # region suggestions_13
                # Query the index for suggested terms in multiple fields:
                # =======================================================

                suggestions = (
                    session
                    # Query the index
                    .query_index_type(Companies_ByNameAndByContactName, Companies_ByNameAndByContactName.IndexEntry)
                    # Call 'suggest_using' to get suggestions for terms that are
                    # similar to 'chese' in first index-field (e.g. 'company_name')
                    .suggest_using(lambda builder: builder.by_field("company_name", "chese"))
                    # Call 'and_suggest_using' to get suggestions for terms that are
                    # similar to 'frank' in an additional index-field (e.g. 'company_name')
                    .and_suggest_using(lambda builder: builder.by_field("contact_name", "frank")).execute()
                )
                # endregion

                # region suggestions_15
                # Define suggestion requests for multiple fields:

                request1 = SuggestionWithTerm("company_name")
                # Looking for terms from index-field 'company_name' that are similar to 'chese'
                request1.term = "chese"

                request2 = SuggestionWithTerm("contact_name")
                # Looking for terms from nested index-field 'contact_name' that are similar to 'frank'
                request2.term = "frank"

                # Query the index for suggestions
                suggestions = (
                    session.query_index_type(
                        Companies_ByNameAndByContactName, Companies_ByNameAndByContactName.IndexEntry
                    )
                    # Call 'suggest_using' - pass the suggestion request for the first index-field
                    .suggest_using(request1)
                    # Call 'and_suggest_using' - pass the suggestion request for the second index-field
                    .and_suggest_using(request2).execute()
                )
                # endregion

                # region suggestions_16
                # Query the index for suggested terms in multiple fields:
                # =======================================================

                suggestions = (
                    session.advanced
                    # Query the index
                    .document_query(Companies_ByNameAndByContactName, Companies_ByNameAndByContactName.IndexEntry)
                    # Call 'suggest_using' to get suggestions for terms that are
                    # similar to 'chese' in first index-field (e.g. 'company_name')
                    .suggest_using(lambda builder: builder.by_field("company_name", "chese"))
                    # Call 'and_suggest_using' to get suggestions for terms that are
                    # similar to 'frank' in an additional index-field (e.g. 'contact_name')
                    .and_suggest_using(lambda builder: builder.by_field("contact_name", "frank")).execute()
                )
                # endregion

                # region suggestions_17
                # The resulting suggested terms:
                # ==============================

                # Suggested terms in index-field 'company_name' that is similar to 'chese':
                #     cheese
                #     chinese

                # Suggested terms in index-field 'contact_name' that are similar to 'frank':
                #     fran
                #     franken
                # endregion

                # region suggestions_18
                # Query the index for suggested terms - customize options and display name:
                # =========================================================================

                suggestions = (
                    session
                    # Query the index
                    .query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder.by_field("product_name", "chokolade")
                        # Customize suggestions options
                        .with_options(
                            SuggestionOptions(
                                accuracy=0.3,
                                page_size=5,
                                distance=StringDistanceTypes.N_GRAM,
                                sort_mode=SuggestionSortMode.POPULARITY,
                            )
                        )
                        # Customize display name for results
                        .with_display_name("SomeCustomName")
                    ).execute()
                )
                # endregion
                # region suggestions_20
                # Define the suggestion request
                suggestion_request = SuggestionWithTerm("product_name")
                # Looking for terms from index-field 'ProductName' that are similar to 'chokolade'
                suggestion_request.term = "chokolade"
                # Customize options
                suggestion_request.options = SuggestionOptions(
                    accuracy=0.3,
                    page_size=5,
                    distance=StringDistanceTypes.N_GRAM,
                    sort_mode=SuggestionSortMode.POPULARITY,
                )
                # Customize display name
                suggestion_request.display_field = "SomeCustomName"

                # Query the index for suggestions
                suggestions = (
                    session.query_index_type(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using' - pass the suggestion request
                    .suggest_using(suggestion_request).execute()
                )
                # endregion
                # region suggestions_21
                # Query the index for suggested terms - customize options and display name:
                # =========================================================================

                suggestions = (
                    session.advanced
                    # Query the index
                    .document_query(Products_ByName, Products_ByName.IndexEntry)
                    # Call 'suggest_using'
                    .suggest_using(
                        lambda builder: builder.by_field("product_name", "chokolade")
                        # Customize suggestions options
                        .with_options(
                            SuggestionOptions(
                                accuracy=0.3,
                                page_size=5,
                                distance=StringDistanceTypes.N_GRAM,
                                sort_mode=SuggestionSortMode.POPULARITY,
                            )
                        )
                        # Customize display name for results
                        .with_display_name("SomeCustomName")
                    ).execute()
                )
                # endregion

                # region suggestions_22
                # The resulting suggested terms:
                # ==============================

                print("Suggested terms:")
                # Results are available under the custom name entry
                for suggested_term in suggestions["SomeCustomName"].suggestions:
                    print(f"\t{suggested_term}")

                # Suggested terms:
                #     chocolade
                #     schokolade
                #     chocolate
                #     chowder
                #     marmalade
                # endregion
