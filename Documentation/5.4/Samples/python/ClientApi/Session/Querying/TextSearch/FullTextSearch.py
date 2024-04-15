from typing import TypeVar

from ravendb import SearchOperator, DocumentQuery

from examples_base import ExampleBase, Employee, Company

_T = TypeVar("_T")


class FullTextSearch(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_examples(self):
        with self.embedded_server.get_document_store("FTS") as store:
            # Search for single term
            # ======================
            with store.open_session() as session:
                # region fts_1
                employees = list(
                    session
                    # Make a dynamic query on Employees collection
                    .query(object_type=Employee)
                    # * Call 'Search' to make a Full-Text search
                    # * Search is case-insensitive
                    # * Look for documents containing the term 'University' within their 'Notes' field
                    .search("Notes", "University")
                )
                # Results will contain Employee documents that have
                # any case variation of the term 'university' in their 'Notes' field.
                # endregion

            # Search for multiple terms - string
            # ==================================
            with store.open_session() as session:
                # region fts_4
                employees = list(
                    session.query(object_type=Employee)
                    # * Pass multiple terms in a single string, separated by spaces.
                    # * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                    #   within their 'Notes' field
                    .search("Notes", "University Sales Japanese")
                )

                # * Results will contain Employee documents that have at least one of the specified terms.
                # * Search is case-insensitive.
                # endregion

            # Search for multiple terms - list
            # ==================================
            with store.open_session() as session:
                # region fts_7
                # todo reeb: skip - not implemented to provide a list here
                employees = list(
                    session.query(object_type=Employee)
                    # * Pass terms in IEnumerable<string>.
                    # * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                    #   within their 'Notes' field
                    .search("Notes", "University Sales Japanese")
                )

                # * Results will contain Employee documents that have at least one of the specified terms.
                # * Search is case-insensitive.
                # endregion

            # Search in multiple fields
            # =========================
            with store.open_session() as session:
                # region fts_9
                employees = list(
                    session.query(object_type=Employee)
                    # * Look for documents containing:
                    #   'French' in their 'Notes' field OR 'President' in their 'Title' field
                    .search("Notes", "French").search("Title", "President")
                )
                # * Results will contain Employee documents that have
                #   at least one of the specified fields with the specified terms.
                # * Search is case-insensitive.
                # endregion

            # Search in complex object
            # =========================
            with store.open_session() as session:
                # region fts_12
                companies = list(
                    session.query(object_type=Company)
                    # * Look for documents that contain:
                    #   the term 'USA' OR 'London' in any field within the complex 'Address' object
                    .search("Address", "USA London")
                )
                # endregion

            # Search operators - AND
            # ======================
            with store.open_session() as session:
                # region fts_15
                employees = list(
                    session.query(object_type=Employee)
                    # * Pass `@operator` with 'SearchOperator.AND'
                    .search("Notes", "College German", operator=SearchOperator.AND)
                )
                # * Results will contain Employee documents that have BOTH 'College' AND 'German'
                #   in their 'Notes' field.
                # * Search is case-insensitive.
                # endregion

            # Search operators - OR
            # ======================
            with store.open_session() as session:
                # region fts_18
                employees = list(
                    session.query(object_type=Employee)
                    # * Pass `@operator` with 'SearchOperator.OR' (or don't pass this param at all)
                    .search("Notes", "College German", operator=SearchOperator.OR)
                )
                # * Results will contain Employee documents that have BOTH 'College' OR 'German'
                #   in their 'Notes' field.
                # * Search is case-insensitive.
                # endregion

            # Search options - Not
            # ======================
            with store.open_session() as session:
                # region fts_23
                companies = list(
                    session.query(object_type=Company)
                    .open_subclause()
                    # Call 'Not' to negate the next search call
                    .not_()
                    .search("Address", "USA")
                    .close_subclause()
                )
                # * Results will contain Company documents are NOT located in 'USA'
                # * Search is case-insensitive
                # endregion

            # Search options - Default
            # ========================
            with store.open_session() as session:
                # region fts_24
                companies = list(
                    session.query(object_type=Company).where_equals("Contact.Title", "Owner")
                    # Operator AND will be used with previous 'where_equals' predicate
                    .search("Address.Country", "France")
                    # Operator OR will be used between the two 'search' calls by default
                    .search("Name", "Markets")
                )

                # * Results will contain Company documents that have:
                #   ('Owner' as the 'Contact.Title')
                #   AND
                #   (are located in 'France' OR have 'Markets' in their 'Name' field)
                #
                # * Search is case-insensitive
                # endregion

            with store.open_session() as session:
                companies = list(
                    session.advanced.document_query(object_type=Company)
                    .where_equals("Contact.Title", "Owner")
                    # Operator AND will be used with previous 'where' predicate
                    # Call 'open_subclause' to open predicate block
                    .open_subclause()
                    .search("Address.Country", "France")
                    # Operator OR will be used between the two 'search' calls by default
                    .search("Name", "Markets")
                    # Call 'close_subclause' to close predicatee block
                    .close_subclause()
                )
                # * Results will contain Company documents that have:
                #   ('Owner' as the 'Contact.Title')
                #   AND
                #   (are located in 'France' OR have 'Markets' in their 'Name' field)
                #
                # * Search is case-insensitive
                # endregion

            # Search options - AND # todo: search options not implemented in python API - workaround is to use subclauses or and_also (fts_29)
            # ====================
            with store.open_session() as session:
                # region fts_27
                employees = list(
                    session.query(object_type=Employee).search("Notes", "French")
                    # * Pass 'options' with 'SearchOptions.And' to this second 'Search'
                    # * Operator AND will be used with previous the 'Search' call
                    # todo implement: .search("Title", "Manager", options=SearchOptions.AND))
                )
                # endregion

            with store.open_session() as session:
                # region fts_29
                employees = list(
                    session.advanced.document_query(object_type=Employee)
                    .search("Notes", "French")
                    # Call 'and_also' so that operator AND will be used with previous 'Search' call
                    .and_also()
                    .search("Title", "Manager")
                )

                # * Results will contain Employee documents that have:
                #   ('French' in their 'Notes' field)
                #   AND
                #   ('Manager' in their 'Title' field)
                #
                # * Search is case-insensitive

                # endregion

            # Search options - Flags
            # ======================
            with store.open_session() as session:
                # region fts_32
                employees = list(
                    session.advanced.document_query(object_type=Employee)
                    .search("Notes", "French")
                    # Call 'AndAlso' so that operator AND will be used with previous 'Search' call
                    .and_also()
                    .open_subclause()
                    # Call 'Not' to negate the next search call
                    .not_()
                    .search("Title", "Manager")
                    .close_subclause()
                )

                # * Results will contain Employee documents that have:
                #   ('French' in their 'Notes' field)
                #   AND
                #   (do NOT have 'Manager' in their 'Title' field)
                #
                # * Search is case-insensitive
                # endregion

            # Using wildcards
            # ===============
            with store.open_session() as session:
                # region fts_33
                employees = list(
                    session.query(object_type=Employee)
                    # Use '*' to replace one or more characters
                    .search("Notes", "art*")
                    .search("Notes", "*logy")
                    .search("Notes", "*mark*")
                )

                # Results will contain Employee documents that have in their 'Notes' field:
                # (terms that start with 'art')  OR
                # (terms that end with 'logy') OR
                # (terms that have the text 'mark' in the middle)
                #
                # * Search is case-insensitive
                # endregion

    class Foo:
        # region syntax
        def search(self, field_name: str, search_terms: str, operator: SearchOperator = None) -> DocumentQuery[_T]: ...

        # endregion
