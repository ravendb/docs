from typing import Optional

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldStorage, FieldIndexing, FieldTermVector
from ravendb.documents.queries.highlighting import Highlightings, HighlightingOptions

from examples_base import ExampleBase, Employee


# region index_1
# Define a Map index:
# ===================
class Employees_ByNotes(AbstractIndexCreationTask):
    # The IndexEntry defines index-field 'employee_notes'
    class IndexEntry:
        def __init__(self, employee_notes: str = None):
            self.employee_notes = employee_notes

    def __init__(self):
        super().__init__()
        self.map = "from employee in docs.Employees select new { employee_notes = employee.notes[0] }"

        # Configure index-field 'employee_notes' for highlighting:
        # ========================================================
        self._store("employee_notes", FieldStorage.YES)
        self._index("employees_notes", FieldIndexing.SEARCH)
        self._term_vector("employee_notes", FieldTermVector.WITH_POSITIONS_AND_OFFSETS)


# endregion


# region index_2
# Define a Map-Reduce index:
# ==========================
class ContactDetailsPerCountry(AbstractIndexCreationTask):
    # The IndexEntry class defines the index-fields
    class IndexEntry:
        def __init__(self, country: str = None, contact_details: str = None):
            self.country = country
            self.contact_details = contact_details

    def __init__(self):
        super().__init__()
        # The 'map' function defines what will be indexed from each document in the collection
        self.map = "from company in docs.Companies select new { country = company.Address.Country, contact_details = company.Contact.Name + ' ' + company.Contact.Title }"

        # The 'reduce' function specifies how data is grouped and aggregated

        # Set 'country' as the group-by key
        # 'contact_details' will be grouped per 'country'

        # Specify the aggregation
        # we'll use string.Join as the aggregation function
        self.reduce = (
            "from result in results group result by result.country into g select new { country = g.key, contact_details = string.Join("
            ", g.Select(x => x.contact_details) )}"
        )

        # Configure index-field 'country' for Highlighting:
        # =================================================
        self._store("country", FieldStorage.YES)

        # Configure index-field 'contact_details' for Highlighting
        self._store("contact_details", FieldStorage.YES)
        self._index("contact_details", FieldIndexing.SEARCH)
        self._term_vector("contact_details", FieldTermVector.WITH_POSITIONS_AND_OFFSETS)


# endregion


class Highlights(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_highlights(self):
        with self.embedded_server.get_document_store("IndexHighlights") as store:
            with store.open_session() as session:
                # region highlight_1
                # Define a callback function that takes Highlightings as an argument
                # This function will be called by passing resulting Highlightings
                def __highlight_callback(highlightings: Highlightings):
                    # Process the results here
                    highlightings.get_fragments(...)
                    ...

                employees_results = list(
                    session
                    # Query the map index
                    .query_index_type(Employees_ByNotes, Employees_ByNotes.IndexEntry)
                    # Search for documents containing the term 'manager'
                    .search("employee_notes", "manager")
                    # Request to highlight the searched term by calling 'highlight'
                    # Pass the callback function
                    .highlight("employee_notes", 35, 2, __highlight_callback).of_type(Employee)
                )

                # endregion
                # region highlight_4
                manager_highlights: Optional[Highlightings] = None

                # Define a callback function that takes Highlightings as an argument
                # This function will be called by passing resulting Highlightings
                def __manager_highlights_callback(highlightings: Highlightings):
                    # Process the results here or assign to nonlocal variable to access them later
                    # e.g. highlightings.get_fragments(...)
                    nonlocal manager_highlights
                    manager_highlights = highlightings

                employees_results = list(
                    session
                    # Query the map index
                    .query_index_type(Employees_ByNotes, Employees_ByNotes.IndexEntry)
                    # Request to highlight the searched term by calling 'highlight'
                    .highlight("employees_notes", 35, 2, __manager_highlights_callback)
                    # Search for documents containing the term 'manager'
                    .where_equals("employee_notes", "manager").of_type(Employee)
                )
                # endregion
                # region highlight_7
                # 'employees_results' contains all Employee DOCUMENTS that contain the term 'manager'.
                # 'manager_highlights' contains the text FRAGMENTS that highlight the 'manager' term.
                builder = ["<ul>"]
                for employee in employees_results:
                    # Call 'get_fragments' to get all fragments for the specified employee Id
                    fragments = manager_highlights.get_fragments(employee.Id)
                    for fragment in fragments:
                        builder.append(f"\n<li>Doc: {employee.Id}</li>")
                        builder.append(f"\n<li>Fragment: {fragment}</li>")
                        builder.append("\n<li></li>")

                builder.append("\n</ul>")
                fragments_html = "".join(builder)

                # The resulting fragments_html:
                # =============================

                #  <ul>
                #    <li>Doc: employees/2-A</li>
                #    <li>Fragment:  to sales <b style="background:yellow">manager</b> in January</li>
                #    <li>Doc: employees/5-A</li>
                #    <li>Fragment:  to sales <b style="background:yellow">manager</b> in March</li>
                #    <li></li>
                #  </ul>
                # endregion
                # region highlight_8
                # Define the key by which the resulting fragments are grouped:
                # ============================================================
                options = HighlightingOptions(
                    # Set 'group_key' to be the index's group-by key
                    # The resulting fragments will be grouped per 'country'
                    group_key="Country"
                )

                # Define a callback function that takes Highlightings as an argument:
                # ===================================================================
                agent_highlights: Optional[Highlightings] = None

                def __agent_highlights_callback(highlightings: Highlightings):
                    # Process the results here or assign to nonlocal variable to access them later
                    nonlocal agent_highlights
                    agent_highlights = highlightings

                # Query the map-reduce index:
                # ===========================
                details_per_country = list(
                    session.query_index_type(ContactDetailsPerCountry, ContactDetailsPerCountry.IndexEntry)
                    # Search for results containing the term 'agent'
                    .search("contact_details", "agent")
                    # Request to highlight the searched term by calling 'highlight'
                    # Pass the defined 'options'
                    .highlight("contact_details", 35, 2, __agent_highlights_callback, options)
                )
                # endregion

                # region highlight_11
                # 'details_per_country' contains the contacts details grouped per country.
                # 'agent_highlights' contains the text FRAGMENTS that highlight the 'agent' term.

                builder = ["<ul>"]

                for item in details_per_country:
                    # Call 'get_fragments' to get all fragments for the specified country key
                    fragments = agent_highlights.get_fragments(item.country)
                    for fragment in fragments:
                        builder.append(f"\n<li> Country: {item.country}</li>")
                        builder.append(f"\n<li> Fragment: {fragment}</li>")
                        builder.append(f"\n<li></li>")
                builder.append("\n</ul>")

                fragments_html = "".join(builder)

                #  The resulting fragmentsHtml:
                #  ============================
                #
                #  <ul>
                #   <li>Country: UK</li>
                #   <li>Fragment: Devon Sales <b style="background:yellow">Agent</b> Helen Bennett</li>
                #   <li></li>
                #   <li>Country: France</li>
                #   <li>Fragment: Sales <b style="background:yellow">Agent</b> Carine Schmit</li>
                #   <li></li>
                #   <li>Country: France</li>
                #   <li>Fragment: Saveley Sales <b style="background:yellow">Agent</b> Paul Henriot</li>
                #   <li></li>
                #   <li>Country: Argentina</li>
                #   <li>Fragment: Simpson Sales <b style="background:yellow">Agent</b> Yvonne Moncad</li>
                #   <li></li>
                #   <li>Country: Argentina</li>
                #   <li>Fragment: Moncada Sales <b style="background:yellow">Agent</b> Sergio</li>
                #   <li></li>
                #   <li>Country: Brazil</li>
                #   <li>Fragment: Sales <b style="background:yellow">Agent</b> Anabela</li>
                #   <li></li>
                #   <li>Country: Belgium</li>
                #   <li>Fragment: Dewey Sales <b style="background:yellow">Agent</b> Pascale</li>
                #   <li></li>
                #  </ul>
                # endregion
