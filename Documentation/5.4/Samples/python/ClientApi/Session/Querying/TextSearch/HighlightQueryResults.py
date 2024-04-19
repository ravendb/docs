import os
from typing import Optional, Callable, TypeVar, List, Dict, Set

from ravendb import QueryData, DocumentQuery
from ravendb.documents.queries.highlighting import Highlightings, HighlightingOptions

from examples_base import ExampleBase, Employee

_T = TypeVar("_T")


class HighlightQueryResults(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_example(self):
        with self.embedded_server.get_document_store("HighlightQueryResult") as store:
            with store.open_session() as session:
                # region highlight_1
                # Make a full-text search dynamic query:
                # ======================================

                # Define a callback that takes highlightings as an argument
                sales_highlightings: Optional[Highlightings] = None

                def _sales_highlights(highlightings: Highlightings):
                    # You may use the results (highlightings) here in any way desired
                    sales_highlightings = highlightings

                employees_result = list(  # Execute the query inside the parenthesis
                    session
                    # Make a query on 'Employees' collection
                    .query(object_type=Employee)
                    # Search for documents containing the term 'sales' in their 'Notes' field
                    .search("Notes", "sales")
                    # Request to highlight the searched term by calling 'Highlight'
                    .highlight(
                        "Notes",  # The document-field name in which we search
                        35,  # Max length of each text fragment
                        4,  # Max number of fragments to return per document
                        _sales_highlights,  # An out param for getting the highlighted text fragments
                    )
                )
                # endregion

                # region fragments_1
                # Process results:
                # ================

                # 'employees_results' contains all Employee DOCUMENTS that have 'sales' in their 'Notes' field.
                # 'sales_highlights' contains the text FRAGMENTS that highlight the 'sales' term.

                builder = ["<ul>", {os.linesep}]
                for employee in employees_result:
                    # Call 'get_fragments' to get all fragments for the specified employee Id
                    fragments = sales_highlightings.get_fragments(employee.Id)
                    for fragment in fragments:
                        builder.append(f"{os.linesep}<li>Doc: {employee.Id} Fragment: {fragment}</li>")

                fragments_html = builder.append(f"{os.linesep}</ul>")

                # The resulting fragments_html:
                # ============================

                # <ul>
                #   <li>Doc: employees/2-A Fragment: company as a <b style="background:yellow">sales</b></li>
                #   <li>Doc: employees/2-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
                #   <li>Doc: employees/2-A Fragment: president of <b style="background:yellow">sales</b> in March 1993</li>
                #   <li>Doc: employees/2-A Fragment: member of the <b style="background:yellow">Sales</b> Management</li>
                #   <li>Doc: employees/3-A Fragment: hired as a <b style="background:yellow">sales</b> associate in</li>
                #   <li>Doc: employees/3-A Fragment: promoted to <b style="background:yellow">sales</b> representativ</li>
                #   <li>Doc: employees/5-A Fragment: company as a <b style="background:yellow">sales</b> representativ</li>
                #   <li>Doc: employees/5-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
                #   <li>Doc: employees/5-A Fragment: <b style="background:yellow">Sales</b> Management." </li>
                #   <li>Doc: employees/6-A Fragment: for the <b style="background:yellow">Sales</b> Professional.</li>
                #  </ul>

                # endregion

            with store.open_session() as session:
                # region highlight_4
                # Define customized tags to use for highlighting the searched terms
                # =================================================================
                tags_to_use = HighlightingOptions(
                    # Provide strings of your choice to 'PreTags' & 'PostTags', e.g.:
                    # the first term searched for will be wrapped with '+++'
                    # the second term searched for will be wrapped with '<<<' & '>>>'
                    pre_tags=["+++", "<<<"],
                    post_tags=["+++", ">>>"],
                )

                # Define a callback that takes highlightings as an argument
                manager_highlightings: Optional[Highlightings] = None

                def _manager_highlights(highlightings: Highlightings):
                    # You may use the results (highlightings) here in any way desired
                    manager_highlightings = highlightings

                # Make a full-text search dynamic query:
                # ======================================
                employees_result = list(
                    session.query(object_type=Employee)
                    # Search for:
                    #  * documents containing the term 'sales' in their 'Notes' field
                    #  * OR for documents containing the term 'manager' in their 'Title' field
                    .search("Notes", "sales")
                    .search("Title", "manager")
                    # Call 'Highlight' for each field searched
                    # Pass 'tagsToUse' to OVERRIDE the default tags used
                    .highlight("Notes", 35, 1, _sales_highlights)
                    .highlight("Title", 35, 1, tags_to_use, _manager_highlights)
                )
                # endregion

                # region fragments_2
                # The resulting salesHighlights fragments:
                # ========================================
                #
                # "for the +++Sales+++ Professional."
                # "hired as a +++sales+++ associate in"
                # "company as a +++sales+++"
                # "company as a +++sales+++ representative"
                #
                # The resulting managerHighlights fragments:
                # ==========================================
                #
                # "Sales <<<Manager>>>"
                # endregion
            with store.open_session() as session:
                # region highlight_6
                # Make a full-text search dynamic query & project results:
                # ========================================================

                # Define a callback that takes highlightings as an argument
                terms_highlightings: Optional[Highlightings] = None

                def _terms_highlights(highlightings: Highlightings):
                    # You may use the results (highlightings) here in any way desired
                    terms_highlightings = highlightings

                employees_projected = list(
                    session.query(object_type=Employee)
                    .search("Notes", "manager german")
                    .highlight("Notes", 35, 2, _terms_highlights)
                    .select_fields_query_data(
                        QueryData.custom_function("o", "{ Name: o.FirstName + ' ' + o.LastName, Title: o.Title }"),
                    )
                )

                # todo reeb & gracjan: lets implement it after 5.4 release
                #  i have a perfect ticket for that
                #  https://issues.hibernatingrhinos.com/issue/RDBC-820#focus=Comments-67-1050834.0-0

                # endregion

                # region fragments_3
                # The resulting fragments from termsHighlights:
                # =============================================
                #
                # "to sales <b style=\"background:yellow\">manager</b> in March"
                # "and reads <b style=\"background:lawngreen\">German</b>.  He joined"
                # "to sales <b style=\"background:yellow\">manager</b> in January"
                # "in French and <b style=\"background:lawngreen\">German</b>."
                #
                # NOTE: each search term is wrapped with a different color
                # 'manager' is wrapped with yellow
                # 'german' is wrapped with lawngreen
                # endregion

            class Foo:
                # region syntax_1

                def highlight(
                    self,
                    field_name: str,
                    fragment_length: int,
                    fragment_count: int,
                    highlightings_callback: Callable[[Highlightings], None],
                    options: Optional[HighlightingOptions] = None,
                ) -> DocumentQuery[_T]: ...

                # endregion

                class HighlightingOptionsClass:
                    # region syntax_2
                    def __init__(self, group_key: str = None, pre_tags: List[str] = None, post_tags: List[str] = None):
                        self.group_key = group_key
                        self.pre_tags = pre_tags
                        self.post_tags = post_tags

                    # endregion

                class HighlightingsClass:
                    # region syntax_3
                    def __init__(self, field_name: str):
                        self.field_name = field_name
                        ...

                    @property
                    def result_indents(self) -> Set[str]: ...

                    # endregion

                class Foo2:
                    # region syntax_4
                    def get_fragments(self, key: str) -> List[str]: ...

                    # endregion
