using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Highlighting;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    #region index_1
    // Define a Map index:
    // ===================
    public class Employees_ByNotes :
        AbstractIndexCreationTask<Employee, Employees_ByNotes.IndexEntry>
    {
        // The IndexEntry class defines index-field 'EmployeeNotes'
        public class IndexEntry
        {
            public string EmployeeNotes { get; set; }
        }
           
        public Employees_ByNotes()
        {
            // The 'Map' function defines the content of index-field 'EmployeeNotes'
            Map = employees => from employee in employees
                select new IndexEntry
                {
                    EmployeeNotes = employee.Notes[0]
                };
            
            // Configure index-field 'EmployeeNotes' for highlighting:
            // =======================================================
            Store(x => x.EmployeeNotes, FieldStorage.Yes);
            Index(x => x.EmployeeNotes, FieldIndexing.Search);
            TermVector(x => x.EmployeeNotes, FieldTermVector.WithPositionsAndOffsets);
        }
    }
    #endregion
    
    #region index_2
    // Define a Map-Reduce index:
    // ==========================
    public class ContactDetailsPerCountry :
        AbstractIndexCreationTask<Company, ContactDetailsPerCountry.IndexEntry>
    {
        // The IndexEntry class defines the index-fields
        public class IndexEntry
        {
            public string Country { get; set; } 
            public string ContactDetails { get; set; }
        }
           
        public ContactDetailsPerCountry()
        {
            // The 'Map' function defines what will be indexed from each document in the collection
            Map = companies => from company in companies
                select new IndexEntry
                {
                    Country = company.Address.Country,
                    ContactDetails = $"{company.Contact.Name} {company.Contact.Title}"
                };
            
            // The 'Reduce' function specifies how data is grouped and aggregated
            Reduce = results => from result in results
                group result by result.Country into g
                select new IndexEntry
                {
                    // Set 'Country' as the group-by key
                    // 'ContactDetails' will be grouped per 'Country'
                    Country = g.Key,
                    
                    // Specify the aggregation
                    // here we use string.Join as the aggregation function
                    ContactDetails = string.Join(" ", g.Select(x => x.ContactDetails))
                };
            
            // Configure index-field 'Country' for Highlighting:
            // =================================================
            Store(x => x.Country, FieldStorage.Yes);
            
            // Configure index-field 'ContactDetails' for Highlighting:
            // ========================================================
            Store(x => x.ContactDetails, FieldStorage.Yes);
            Index(x => x.ContactDetails, FieldIndexing.Search);
            TermVector(x => x.ContactDetails, FieldTermVector.WithPositionsAndOffsets);
        }
    }
    #endregion
    
    public class Highlights
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region highlight_1
                    List<Employee> employeesResults = session
                         // Query the map index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Search for documents containing the term 'manager'
                        .Search(x => x.EmployeeNotes, "manager")
                         // Request to highlight the searched term by calling 'Highlight'
                        .Highlight(x => x.EmployeeNotes, 35, 2, out Highlightings managerHighlights)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region highlight_2
                    List<Employee> employeesResults = await asyncSession
                         // Query the map index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Search for documents containing the term 'manager'
                        .Search(x => x.EmployeeNotes, "manager")
                         // Request to highlight the searched term by calling 'Highlight'
                        .Highlight(x => x.EmployeeNotes, 35, 2, out Highlightings managerHighlights)
                        .OfType<Employee>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_3
                    List<Employee> employeesResults = session.Advanced
                         // Query the map index
                        .DocumentQuery<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Search for documents containing the term 'manager'
                        .Search(x => x.EmployeeNotes, "manager")
                         // Request to highlight the searched term by calling 'Highlight'
                        .Highlight(x => x.EmployeeNotes, 35, 2, out Highlightings managerHighlights)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_4
                    List<Employee> employeesResults = session
                         // Query the map index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Request to highlight the searched term by calling 'Highlight'
                        .Highlight(x => x.EmployeeNotes, 35, 2, out Highlightings managerHighlights)
                         // Search for documents containing the term 'manager'
                        .Where(x => x.EmployeeNotes.Contains("manager"))
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region highlight_5
                    List<Employee> employeesResults = await asyncSession
                         // Query the map index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Request to highlight the searched term by calling 'Highlight'
                        .Highlight(x => x.EmployeeNotes, 35, 2, out Highlightings managerHighlights)
                         // Search for documents containing the term 'manager'
                        .Where(x => x.EmployeeNotes.Contains("manager"))
                        .OfType<Employee>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_6
                    List<Employee> employeesResults = session.Advanced
                         // Query the map index
                        .DocumentQuery<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Request to highlight the searched term by calling 'Highlight'
                        .Highlight(x => x.EmployeeNotes, 35, 2, out Highlightings managerHighlights)
                         // Search for documents containing the term 'manager'
                        .WhereEquals("EmployeeNotes", "manager")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                    
                    #region highlight_7
                    // 'employeesResults' contains all Employee DOCUMENTS that contain the term 'manager'.
                    // 'managerHighlights' contains the text FRAGMENTS that highlight the 'manager' term.
                    
                    StringBuilder builder = new StringBuilder().AppendLine("<ul>");

                    foreach (var employee in employeesResults)
                    {
                        // Call 'GetFragments' to get all fragments for the specified employee Id
                        string[] fragments = managerHighlights.GetFragments(employee.Id);
                        foreach (var fragment in fragments)
                        {
                            builder.AppendLine($"<li>Doc: {employee.Id}</li>");
                            builder.AppendLine($"<li>Fragment: {fragment}</li>");
                            builder.AppendLine($"<li></li>");
                        }
                    }

                    string fragmentsHtml = builder.AppendLine("</ul>").ToString();
                    
                    // The resulting fragmentsHtml:
                    // ============================

                    // <ul>
                    //   <li>Doc: employees/2-A</li>
                    //   <li>Fragment:  to sales <b style="background:yellow">manager</b> in January</li>
                    //   <li>Doc: employees/5-A</li>
                    //   <li>Fragment:  to sales <b style="background:yellow">manager</b> in March</li>
                    //   <li></li>
                    // </ul>
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlight_8
                    // Define the key by which the resulting fragments are grouped:
                    // ============================================================
                    HighlightingOptions options = new HighlightingOptions
                    {
                        // Set 'GroupKey' to be the index's group-by key
                        // The resulting fragments will be grouped per 'Country'
                        GroupKey = "Country"
                    };
                    
                    // Query the map-reduce index:
                    // ===========================
                    List<ContactDetailsPerCountry.IndexEntry> detailsPerCountry = session
                        .Query<ContactDetailsPerCountry.IndexEntry, ContactDetailsPerCountry>()
                         // Search for results containing the term 'agent'
                        .Search(x => x.ContactDetails, "agent")
                         // Request to highlight the searched term by calling 'Highlight'
                         // Pass the defined 'options'
                        .Highlight(x => x.ContactDetails, 35, 2, options, out Highlightings agentHighlights)
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region highlight_9
                    // Define the key by which the resulting fragments are grouped
                    // ===========================================================
                    HighlightingOptions options = new HighlightingOptions
                    {
                        // Set 'GroupKey' to be the index's group-by key
                        // The resulting fragments will be grouped per 'Country'
                        GroupKey = "Country"
                    };
                    
                    // Query the map-reduce index:
                    // ===========================
                    List<ContactDetailsPerCountry.IndexEntry> detailsPerCountry = await asyncSession
                        .Query<ContactDetailsPerCountry.IndexEntry, ContactDetailsPerCountry>()
                         // Search for results containing the term 'agent'
                        .Search(x => x.ContactDetails, "agent")
                         // Request to highlight the searched term by calling 'Highlight'
                         // Pass the defined 'options'
                        .Highlight(x => x.ContactDetails, 35, 2, options, out Highlightings agentHighlights)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_10
                    // Define the key by which the resulting fragments are grouped
                    // ===========================================================
                    HighlightingOptions options = new HighlightingOptions
                    {
                        // Set 'GroupKey' to be the index's group-by key
                        // The resulting fragments will be grouped per 'Country'
                        GroupKey = "Country"
                    };
                    
                    // Query the map-reduce index:
                    // ===========================
                    List<ContactDetailsPerCountry.IndexEntry> detailsPerCountry = session.Advanced
                        .DocumentQuery<ContactDetailsPerCountry.IndexEntry, ContactDetailsPerCountry>()
                         // Search for results containing the term 'agent'
                        .Search(x => x.ContactDetails, "agent")
                         // Request to highlight the searched term by calling 'Highlight'
                         // Pass the defined 'options'
                        .Highlight(x => x.ContactDetails, 35, 2, options, out Highlightings agentHighlights)
                        .ToList();
                    #endregion
                    
                    #region highlight_11
                    // 'detailsPerCountry' contains the contacts details grouped per country.
                    // 'agentHighlights' contains the text FRAGMENTS that highlight the 'agent' term.
                    
                    StringBuilder builder = new StringBuilder().AppendLine("<ul>");

                    foreach (var item in detailsPerCountry)
                    {
                        // Call 'GetFragments' to get all fragments for the specified country key
                        string[] fragments = agentHighlights.GetFragments(item.Country);
                        foreach (var fragment in fragments)
                        {
                            builder.AppendLine($"<li>Country: {item.Country}</li>");
                            builder.AppendLine($"<li>Fragment: {fragment}</li>");
                            builder.AppendLine($"<li></li>");
                        }
                    }

                    string fragmentsHtml = builder.AppendLine("</ul>").ToString();
                    
                    // The resulting fragmentsHtml:
                    // ============================

                    // <ul>
                    //   <li>Country: UK</li>
                    //   <li>Fragment: Devon Sales <b style="background:yellow">Agent</b> Helen Bennett</li>
                    //   <li></li>
                    //   <li>Country: France</li>
                    //   <li>Fragment: Sales <b style="background:yellow">Agent</b> Carine Schmit</li>
                    //   <li></li>
                    //   <li>Country: France</li>
                    //   <li>Fragment: Saveley Sales <b style="background:yellow">Agent</b> Paul Henriot</li>
                    //   <li></li>
                    //   <li>Country: Argentina</li>
                    //   <li>Fragment: Simpson Sales <b style="background:yellow">Agent</b> Yvonne Moncad</li>
                    //   <li></li>
                    //   <li>Country: Argentina</li>
                    //   <li>Fragment: Moncada Sales <b style="background:yellow">Agent</b> Sergio</li>
                    //   <li></li>
                    //   <li>Country: Brazil</li>
                    //   <li>Fragment: Sales <b style="background:yellow">Agent</b> Anabela</li>
                    //   <li></li>
                    //   <li>Country: Belgium</li>
                    //   <li>Fragment: Dewey Sales <b style="background:yellow">Agent</b> Pascale</li>
                    //   <li></li>
                    // </ul>
                    #endregion
                }
            }
        }
    }
}
