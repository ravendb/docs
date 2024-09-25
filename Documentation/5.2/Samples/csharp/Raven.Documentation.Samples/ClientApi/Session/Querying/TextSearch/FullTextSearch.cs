using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class FullTextSearch
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                // Search for single term
                // ======================
                using (var session = store.OpenSession())
                {
                    #region fts_1
                    List<Employee> employees = session
                         // Make a dynamic query on Employees collection
                        .Query<Employee>()
                         // * Call 'Search' to make a Full-Text search
                         // * Search is case-insensitive
                         // * Look for documents containing the term 'University' within their 'Notes' field
                        .Search(x => x.Notes, "University")
                        .ToList();
                    
                    // Results will contain Employee documents that have
                    // any case variation of the term 'university' in their 'Notes' field.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_2
                    List<Employee> employees = await asyncSession
                         // Make a dynamic query on Employees collection
                        .Query<Employee>()
                         // * Call 'Search' to make a Full-Text search
                         // * Search is case-insensitive
                         // * Look for documents containing the term 'University' within their 'Notes' field
                        .Search(x => x.Notes, "University")
                        .ToListAsync();
                    
                    // Results will contain Employee documents that have
                    // any case variation of the term 'university' in their 'Notes' field.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_3
                    List<Employee> employees = session.Advanced
                         // Make a dynamic DocumentQuery on Employees collection
                        .DocumentQuery<Employee>()
                         // * Call 'Search' to make a Full-Text search
                         // * Search is case-insensitive
                         // * Look for documents containing the term 'University' within their 'Notes' field
                        .Search(x => x.Notes, "University")
                        .ToList();
                    
                    // Results will contain Employee documents that have
                    // any case variation of the term 'university' in their 'Notes' field.
                    #endregion
                }
                
                // Search for multiple terms - string
                // ==================================
                using (var session = store.OpenSession())
                {
                    #region fts_4
                    List<Employee> employees = session
                        .Query<Employee>()
                         // * Pass multiple terms in a single string, separated by spaces.
                         // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                         //   within their 'Notes' field
                        .Search(x => x.Notes, "University Sales Japanese")
                        .ToList();
                    
                    // * Results will contain Employee documents that have at least one of the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_5
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                         // * Pass multiple terms in a single string, separated by spaces.
                         // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                         //   within their 'Notes' field
                        .Search(x => x.Notes, "University Sales Japanese")
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have at least one of the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_6
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // * Pass multiple terms in a single string, separated by spaces.
                         // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                         //   within their 'Notes' field
                        .Search(x => x.Notes, "University Sales Japanese")
                        .ToList();
                    
                    // * Results will contain Employee documents that have at least one of the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }
                
                // Search for multiple terms - list
                // ==================================
                using (var session = store.OpenSession())
                {
                    #region fts_7
                    List<Employee> employees = session
                        .Query<Employee>()
                         // * Pass terms in IEnumerable<string>.
                         // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                         //   within their 'Notes' field
                        .Search(x => x.Notes, new[] { "University", "Sales", "Japanese" })
                        .ToList();
                    
                    // * Results will contain Employee documents that have at least one of the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_8
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                         // * Pass terms in IEnumerable<string>.
                         // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                         //   within their 'Notes' field
                        .Search(x => x.Notes, new[] { "University", "Sales", "Japanese" })
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have at least one of the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }
                
                // Search in multiple fields
                // =========================
                using (var session = store.OpenSession())
                {
                    #region fts_9
                    List<Employee> employees = session
                        .Query<Employee>()
                         // * Look for documents containing:
                         //   'French' in their 'Notes' field OR 'President' in their 'Title' field
                        .Search(x => x.Notes, "French")
                        .Search(x => x.Title, "President")
                        .ToList();
                    
                    // * Results will contain Employee documents that have
                    //   at least one of the specified fields with the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_10
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                         // * Look for documents containing:
                         //   'French' in their 'Notes' field OR 'President' in their 'Title' field
                        .Search(x => x.Notes, "French")
                        .Search(x => x.Title, "President")
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have
                    //   at least one of the specified fields with the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_11
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // * Look for documents containing:
                         //   'French' in their 'Notes' field OR 'President' in their 'Title' field
                        .Search(x => x.Notes, "French")
                        .Search(x => x.Title, "President")
                        .ToList();
                    
                    // * Results will contain Employee documents that have
                    //   at least one of the specified fields with the specified terms.
                    // * Search is case-insensitive.
                    #endregion
                }
                
                // Search in complex object
                // ========================
                using (var session = store.OpenSession())
                {
                    #region fts_12
                    List<Company> companies = session
                        .Query<Company>()
                         // * Look for documents that contain:
                         //   the term 'USA' OR 'London' in any field within the complex 'Address' object
                        .Search(x => x.Address, "USA London")
                        .ToList();
                    
                    // * Results will contain Company documents that are located either in 'USA' OR in 'London'.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_13
                    List<Company> companies = await asyncSession
                        .Query<Company>()
                         // * Look for documents that contain:
                         //   the term 'USA' OR 'London' in any field within the complex 'Address' object
                        .Search(x => x.Address, "USA London")
                        .ToListAsync();
                    
                    // * Results will contain Company documents that are located either in 'USA' OR in 'London'.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_14
                    List<Company> companies = session.Advanced
                        .DocumentQuery<Company>()
                         // * Look for documents that contain:
                         //   the term 'USA' OR 'London' in any field within the complex 'Address' object
                        .Search(x => x.Address, "USA London")
                        .ToList();
                    
                    // * Results will contain Company documents that are located either in 'USA' OR in 'London'.
                    // * Search is case-insensitive.
                    #endregion
                }
                
                // Search operators - AND
                // ======================
                using (var session = store.OpenSession())
                {
                    #region fts_15
                    List<Employee> employees = session
                        .Query<Employee>()
                         // * Pass `@operator` with 'SearchOperator.And'
                        .Search(x => x.Notes, "College German", @operator: SearchOperator.And)
                        .ToList();
                    
                    // * Results will contain Employee documents that have BOTH 'College' AND 'German'
                    //   in their 'Notes' field.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_16
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                         // * Pass `@operator` with 'SearchOperator.And'
                        .Search(x => x.Notes, "College German", @operator: SearchOperator.And)
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have BOTH 'College' AND 'German'
                    //   in their 'Notes' field.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_17
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // * Pass `@operator` with 'SearchOperator.And'
                        .Search(x => x.Notes, "College German", @operator: SearchOperator.And)
                        .ToList();
                    
                    // * Results will contain Employee documents that have BOTH 'College' AND 'German'
                    //   in their 'Notes' field.
                    // * Search is case-insensitive.
                    #endregion
                }
                
                // Search operators - OR
                // =====================
                using (var session = store.OpenSession())
                {
                    #region fts_18
                    List<Employee> employees = session
                        .Query<Employee>()
                         // * Pass `@operator` with 'SearchOperator.Or' (or don't pass this param at all)
                        .Search(x => x.Notes, "College German", @operator: SearchOperator.Or)
                        .ToList();
                    
                    // * Results will contain Employee documents that have EITHER 'College' OR 'German'
                    //   in their 'Notes' field.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_19
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                         // * Pass `@operator` with 'SearchOperator.Or' (or don't pass this param at all)
                        .Search(x => x.Notes, "College German", @operator: SearchOperator.Or)
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have EITHER 'College' OR 'German'
                    //   in their 'Notes' field.
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_20
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // * Pass `@operator` with 'SearchOperator.Or' (or don't pass this param at all)
                        .Search(x => x.Notes, "College German", @operator: SearchOperator.Or)
                        .ToList();
                    
                    // * Results will contain Employee documents that have EITHER 'College' OR 'German'
                    //   in their 'Notes' field.
                    // * Search is case-insensitive.
                    #endregion
                }
                
                // Search options - Not
                // ====================
                using (var session = store.OpenSession())
                {
                    #region fts_21
                    List<Company> companies = session
                        .Query<Company>()
                         // Pass 'options' with 'SearchOptions.Not'
                        .Search(x => x.Address, "USA", options: SearchOptions.Not)
                        .ToList();
                    
                    // * Results will contain Company documents are NOT located in 'USA'
                    // * Search is case-insensitive
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_22
                    List<Company> companies = await asyncSession
                        .Query<Company>()
                         // Pass 'options' with 'SearchOptions.Not'
                        .Search(x => x.Address, "USA", options: SearchOptions.Not)
                        .ToListAsync();
                    
                    // * Results will contain Company documents are NOT located in 'USA'
                    // * Search is case-insensitive
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_23
                    List<Company> companies = session.Advanced
                        .DocumentQuery<Company>()
                        .OpenSubclause()
                         // Call 'Not' to negate the next search call
                        .Not
                        .Search(x => x.Address, "USA")
                        .CloseSubclause()
                        .ToList();
                    
                    // * Results will contain Company documents are NOT located in 'USA'
                    // * Search is case-insensitive
                    #endregion
                }
                
                // Search options - Default
                // ========================
                using (var session = store.OpenSession())
                {
                    #region fts_24
                    List<Company> companies = session
                        .Query<Company>()
                        .Where(x => x.Contact.Title == "Owner")
                         // Operator AND will be used with previous 'Where' predicate
                        .Search(x => x.Address.Country, "France")
                         // Operator OR will be used between the two 'Search' calls by default
                        .Search(x => x.Name, "Markets")
                        .ToList();
                    
                    // * Results will contain Company documents that have:
                    //   ('Owner' as the 'Contact.Title')
                    //   AND
                    //   (are located in 'France' OR have 'Markets' in their 'Name' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_25
                    List<Company> companies = await asyncSession
                        .Query<Company>()
                        .Where(x => x.Contact.Title == "Owner")
                         // Operator AND will be used with previous 'Where' predicate
                        .Search(x => x.Address.Country, "France")
                         // Operator OR will be used between the two 'Search' calls by default
                        .Search(x => x.Name, "Markets")
                        .ToListAsync();
                    
                    // * Results will contain Company documents that have:
                    //   ('Owner' as the 'Contact.Title')
                    //   AND
                    //   (are located in 'France' OR have 'Markets' in their 'Name' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_26
                    List<Company> companies = session.Advanced
                        .DocumentQuery<Company>()
                        .WhereEquals(x => x.Contact.Title, "Owner")
                         // Operator AND will be used with previous 'Where' predicate
                         // Call 'OpenSubclause' to open predicate block
                        .OpenSubclause()
                        .Search(x => x.Address.Country, "France")
                         // Operator OR will be used between the two 'Search' calls by default
                        .Search(x => x.Name, "Markets")
                         // Call 'CloseSubclause' to close predicate block
                        .CloseSubclause()
                        .ToList();
                    
                    // * Results will contain Company documents that have:
                    //   ('Owner' as the 'Contact.Title')
                    //   AND
                    //   (are located in 'France' OR have 'Markets' in their 'Name' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }
                
                // Search options - AND
                // ====================
                using (var session = store.OpenSession())
                {
                    #region fts_27
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Search(x => x.Notes, "French")
                         // * Pass 'options' with 'SearchOptions.And' to the second 'Search'
                         // * Operator AND will be used with previous the 'Search' call
                        .Search(x => x.Title, "Manager", options: SearchOptions.And)
                        .ToList();
                    
                    // * Results will contain Employee documents that have:
                    //   ('French' in their 'Notes' field)
                    //   AND
                    //   ('Manager' in their 'Title' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_28
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                        .Search(x => x.Notes, "French")
                         // * Pass 'options' with 'SearchOptions.And' to this second 'Search'
                         // * Operator AND will be used with previous the 'Search' call
                        .Search(x => x.Title, "Manager", options: SearchOptions.And)
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have:
                    //   ('French' in their 'Notes' field)
                    //   AND
                    //   ('Manager' in their 'Title' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_29
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                        .Search(x => x.Notes, "French")
                         // Call 'AndAlso' so that operator AND will be used with previous 'Search' call
                        .AndAlso()
                        .Search(x => x.Title, "Manger")
                        .ToList();
                    
                    // * Results will contain Employee documents that have:
                    //   ('French' in their 'Notes' field)
                    //   AND
                    //   ('Manager' in their 'Title' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }
                
                // Search options - Flags
                // ======================
                using (var session = store.OpenSession())
                {
                    #region fts_30
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Search(x => x.Notes, "French")
                         // Pass logical operators as flags in the 'options' parameter
                        .Search(x => x.Title, "Manager", options: SearchOptions.Not | SearchOptions.And)
                        .ToList();
                    
                    // * Results will contain Employee documents that have:
                    //   ('French' in their 'Notes' field)
                    //   AND
                    //   (do NOT have 'Manager' in their 'Title' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_31
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                        .Search(x => x.Notes, "French")
                         // Pass logical operators as flags in the 'options' parameter
                        .Search(x => x.Title, "Manager", options: SearchOptions.Not | SearchOptions.And)
                        .ToListAsync();
                    
                    // * Results will contain Employee documents that have:
                    //   ('French' in their 'Notes' field)
                    //   AND
                    //   (do NOT have 'Manager' in their 'Title' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_32
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                        .Search(x => x.Notes, "French")
                         // Call 'AndAlso' so that operator AND will be used with previous 'Search' call
                        .AndAlso()
                        .OpenSubclause()
                         // Call 'Not' to negate the next search call
                        .Not
                        .Search(x => x.Title, "Manager")
                        .CloseSubclause()
                        .ToList();
                    
                    // * Results will contain Employee documents that have:
                    //   ('French' in their 'Notes' field)
                    //   AND
                    //   (do NOT have 'Manager' in their 'Title' field)
                    //
                    // * Search is case-insensitive
                    #endregion
                }
                
                
                // Using wildcards
                // ===============
                using (var session = store.OpenSession())
                {
                    #region fts_33
                    List<Employee> employees = session
                        .Query<Employee>()
                         // Use '*' to replace one or more characters
                        .Search(x => x.Notes, "art*")
                        .Search(x => x.Notes, "*logy")
                        .Search(x => x.Notes, "*mark*")
                        .ToList();
                    
                    // Results will contain Employee documents that have in their 'Notes' field:
                    // (terms that start with 'art')  OR
                    // (terms that end with 'logy') OR
                    // (terms that have the text 'mark' in the middle) 
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fts_34
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                         // Use '*' to replace one or more characters
                        .Search(x => x.Notes, "art*")
                        .Search(x => x.Notes, "*logy")
                        .Search(x => x.Notes, "*mark*")
                        .ToListAsync();
                    
                    // Results will contain Employee documents that have in their 'Notes' field:
                    // (terms that start with 'art')  OR
                    // (terms that end with 'logy') OR
                    // (terms that have the text 'mark' in the middle) 
                    //
                    // * Search is case-insensitive
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region fts_35
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // Use '*' to replace one or more characters
                        .Search(x => x.Notes, "art*")
                        .Search(x => x.Notes, "*logy")
                        .Search(x => x.Notes, "*mark*")
                        .ToList();
                    
                    // Results will contain Employee documents that have in their 'Notes' field:
                    // (terms that start with 'art')  OR
                    // (terms that end with 'logy') OR
                    // (terms that have the text 'mark' in the middle) 
                    //
                    // * Search is case-insensitive
                    #endregion
                }
            }
        }
        
        public interface IFoo<T>
        {
            #region syntax
            // Query overloads:
            // ================
            
            IRavenQueryable<T> Search<T>(
                Expression<Func<T, object>> fieldSelector,
                string searchTerms,
                decimal boost,
                SearchOptions options,
                SearchOperator @operator);

            IRavenQueryable<T> Search<T>(
                Expression<Func<T, object>> fieldSelector,
                IEnumerable<string> searchTerms,
                decimal boost,
                SearchOptions options,
                SearchOperator @operator);
            
            // DocumentQuery overloads:
            // ========================
            
            IDocumentQueryBase<T> Search(
                string fieldName,
                string searchTerms,
                SearchOperator @operator);
            
            IDocumentQueryBase<T> Search<TValue>(
                Expression<Func<T, TValue>> propertySelector,
                string searchTerms,
                SearchOperator @operator);
            #endregion
        }
    }
}
