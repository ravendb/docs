using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.Highlighting;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HighlightQueryResults
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region highlight_1
                    // Make a full-text search dynamic query:
                    // ======================================
                    List<Employee> employeesResults = session
                         // Make a dynamic query on 'Employees' collection
                        .Query<Employee>()
                         // Search for documents containing the term 'sales' in their 'Notes' field
                        .Search(x => x.Notes, "sales")
                         // Request to highlight the searched term by calling 'Highlight'. Pass:
                         //   * The document-field name in which we search (e.g. 'Notes') 
                         //   * Max length of each string fragment (e.g. 35)
                         //   * Max number of fragments to return per document (e.g. 4)
                         //   * An out param for getting the highlighted text fragments (e.g. 'salesHighlights')
                        .Highlight(x => x.Notes, 35, 4, out Highlightings salesHighlights)
                         // Execute the query
                        .ToList();
                    #endregion

                    #region fragments_1
                    // Process results:
                    // ================
                    
                    // 'employeesResults' contains all Employee DOCUMENTS that have 'sales' in their 'Notes' field.
                    // 'salesHighlights' contains the text FRAGMENTS that highlight the 'sales' string.
                    
                    StringBuilder builder = new StringBuilder().AppendLine("<ul>");

                    foreach (var employee in employeesResults)
                    {
                        // Call 'GetFragments' to get all fragments for the specified employee id
                        string[] fragments = salesHighlights.GetFragments(employee.Id);
                        foreach (var fragment in fragments)
                        {
                            builder.AppendLine(
                                $"<li>Doc: {employee.Id} Fragment: {fragment}</li>");
                        }
                    }

                    string fragmentsHtml = builder.AppendLine("</ul>").ToString();
                    
                    // The resulting fragmentsHtml:
                    // ============================
                    
                    // <ul>
                    //   <li>Doc: employees/2-A Fragment: company as a <b style="background:yellow">sales</b></li>
                    //   <li>Doc: employees/2-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
                    //   <li>Doc: employees/2-A Fragment: president of <b style="background:yellow">sales</b> in March 1993</li>
                    //   <li>Doc: employees/2-A Fragment: member of the <b style="background:yellow">Sales</b> Management</li>
                    //   <li>Doc: employees/3-A Fragment: hired as a <b style="background:yellow">sales</b> associate in</li>
                    //   <li>Doc: employees/3-A Fragment: promoted to <b style="background:yellow">sales</b> representativ</li>
                    //   <li>Doc: employees/5-A Fragment: company as a <b style="background:yellow">sales</b> representativ</li>
                    //   <li>Doc: employees/5-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
                    //   <li>Doc: employees/5-A Fragment: <b style="background:yellow">Sales</b> Management." </li>
                    //   <li>Doc: employees/6-A Fragment: for the <b style="background:yellow">Sales</b> Professional.</li>
                    // </ul>
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region highlight_2
                    // Make a full-text search dynamic query:
                    // ======================================
                    List<Employee> employeesResults = await asyncSession
                         // Make a dynamic query on 'Employees' collection
                        .Query<Employee>()
                         // Search for documents containing the term 'sales' in their 'Notes' field
                        .Search(x => x.Notes, "sales")
                         // Request to highlight the searched term by calling 'Highlight'. Pass:
                         //   * The document-field name in which we search (e.g. 'Notes') 
                         //   * Max length of each string fragment (e.g. 35)
                         //   * Max number of fragments to return per document (e.g. 4)
                         //   * An out param for getting the highlighted text fragments (e.g. 'salesHighlights')
                        .Highlight(x => x.Notes, 35, 4, out Highlightings salesHighlights)
                         // Execute the query
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_3
                    // Make a full-text search dynamic DocumentQuery:
                    // ==============================================
                    List<Employee> employeesResults = session.Advanced
                         // Make a dynamic documentQuery on 'Employees' collection
                        .DocumentQuery<Employee>()
                         // Search for documents containing the term 'sales' in their 'Notes' field
                        .Search(x => x.Notes, "sales")
                         // Request to highlight the searched term by calling 'Highlight'. Pass:
                         //   * The document-field name in which we search (e.g. 'Notes') 
                         //   * Max length of each string fragment (e.g. 35)
                         //   * Max number of fragments to return per document (e.g. 4)
                         //   * An out param for getting the highlighted text fragments (e.g. 'salesHighlights')
                        .Highlight(x => x.Notes, 35, 4, out Highlightings salesHighlights)
                         // Execute the documentQuery
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_4
                    // Define customized tags to use for highlighting the searched terms
                    // =================================================================
                    HighlightingOptions tagsToUse = new HighlightingOptions
                    {
                        // Provide strings of your choice to 'PreTags' & 'PostTags', e.g.:
                        // the first term searched for will be wrapped with '+++'
                        // the second term searched for will be wrapped with '<<<' & '>>>'
                        PreTags = new[] { "+++", "<<<" },
                        PostTags = new[] { "+++", ">>>" }
                    };
                    
                    // Make a full-text search dynamic query:
                    // ======================================
                    List<Employee> employeesResults = session
                        .Query<Employee>()
                         // Search for:
                         //   * documents containing the term 'sales' in their 'Notes' field
                         //   * OR for documents containing the term 'manager' in their 'Title' field
                        .Search(x => x.Notes, "sales")
                        .Search(x => x.Title, "manager")
                         // Call 'Highlight' for each field searched
                         // Pass 'tagsToUse' to OVERRIDE the default tags used 
                        .Highlight(x => x.Notes, 35, 1, tagsToUse, out Highlightings salesHighlights)
                        .Highlight(x => x.Title, 35, 1, tagsToUse, out Highlightings managerHighlights)
                        .ToList();
                    #endregion
                    
                    #region fragments_2
                    // The resulting salesHighlights fragments:
                    // ========================================
                    
                    // "for the +++Sales+++ Professional."
                    // "hired as a +++sales+++ associate in"
                    // "company as a +++sales+++"
                    // "company as a +++sales+++ representativ"
                    
                    // The resulting managerHighlights fragments:
                    // ==========================================
                    
                    // "Sales <<<Manager>>>"
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region highlight_5
                    // Define customized tags to use for highlighting the searched terms
                    // =================================================================
                    HighlightingOptions tagsToUse = new HighlightingOptions
                    {
                        // The first term searched for will be wrapped with '+++'
                        // the second term searched for will be wrapped with '<<<' & '>>>'
                        PreTags = new[] { "+++", "<<<" },
                        PostTags = new[] { "+++", ">>>" }
                    };
                    
                    // Make a full-text search dynamic query:
                    // ======================================
                    List<Employee> employeesResults = await asyncSession
                        .Query<Employee>()
                         // Search for:
                         //   * documents containing the term 'sales' in their 'Notes' field
                         //   * OR for documents containing the term 'manager' in their 'Title' field
                        .Search(x => x.Notes, "sales")
                        .Search(x => x.Title, "manager")
                         // Call 'Highlight' for each field searched
                         // Pass 'tagsToUse' to OVERRIDE the default tags used 
                        .Highlight(x => x.Notes, 35, 1, tagsToUse, out Highlightings salesHighlights)
                        .Highlight(x => x.Title, 35, 1, tagsToUse, out Highlightings managerHighlights)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region highlight_6
                    // Make a full-text search dynamic query & project results:
                    // ========================================================
                    var employeesProjectedResults = session
                        .Query<Employee>()
                         // Search for documents containing 'sales' or 'german' in their 'Notes' field
                        .Search(x => x.Notes, "manager german")
                         // Request to highlight the searched terms from the 'Notes' field 
                        .Highlight(x => x.Notes, 35, 2, out Highlightings termsHighlights)
                         // Define the projection
                        .Select(x => new
                        {
                            // These fields will be returned instead of the whole document
                            // Note: it is Not mandatory to return the field in which we search for the highlights 
                            Name = $"{x.FirstName} {x.LastName}",
                            x.Title
                        })
                        .ToList();
                    #endregion
                    
                    #region fragments_3
                    // The resulting fragments from termsHighlights:
                    // =============================================
                    
                    // "to sales <b style=\"background:yellow\">manager</b> in March"
                    // "and reads <b style=\"background:lawngreen\">German</b>.  He joined"
                    // "to sales <b style=\"background:yellow\">manager</b> in January"
                    // "in French and <b style=\"background:lawngreen\">German</b>."

                    // Note: each search term is wrapped with a different color
                    // 'manager' is wrapped with yellow
                    // 'german' is wrapped with lawngreen
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region highlight_7
                    // Make a full-text search dynamic query & project results:
                    // ========================================================
                    var employeesProjectedResults = await asyncSession
                        .Query<Employee>()
                         // Search for documents containing 'sales' or 'german' in their 'Notes' field
                        .Search(x => x.Notes, "manager german")
                         // Request to highlight the searched terms from the 'Notes' field 
                        .Highlight(x => x.Notes, 35, 2, out Highlightings termsHighlights)
                         // Define the projection
                        .Select(x => new
                        {
                            // These fields will be returned instead of the whole document
                            // Note: it is Not mandatory to return the field in which we search for the highlights 
                            Name = $"{x.FirstName} {x.LastName}",
                            x.Title
                        })
                        .ToListAsync();
                    #endregion
                }
            }
        }
        
        private interface IFoo<T>
        {
            #region syntax_1
            IRavenQueryable<T> Highlight(
                string fieldName,
                int fragmentLength,
                int fragmentCount,
                out Highlightings highlightings);

            IRavenQueryable<T> Highlight(
                string fieldName,
                int fragmentLength,
                int fragmentCount,
                HighlightingOptions options,
                out Highlightings highlightings);

            IRavenQueryable<T> Highlight(
                Expression<Func<T, object>> path,
                int fragmentLength,
                int fragmentCount,
                out Highlightings highlightings);

            IRavenQueryable<T> Highlight(
                Expression<Func<T, object>> path,
                int fragmentLength,
                int fragmentCount,
                HighlightingOptions options,
                out Highlightings highlightings);
            #endregion
        }
        
        public class HighlightingOptionsClass
        {
            #region syntax_2
            public string GroupKey { get; set; }
            public string[] PreTags { get; set; }
            public string[] PostTags { get; set; }
            #endregion
        }
        
        public class HighlightingsClass
        {
            #region syntax_3
            public string FieldName { get; }
            public IEnumerable<string> ResultIndents;
            #endregion
        }

        public interface IFoo2
        {
            #region syntax_4
            public string[] GetFragments(string key);
            #endregion
        }
    }
}
