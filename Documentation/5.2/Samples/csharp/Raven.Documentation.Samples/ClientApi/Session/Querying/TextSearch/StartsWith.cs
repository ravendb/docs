﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class StartsWith
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region startsWith_1
                    List<Product> products = session
                        .Query<Product>()
                         // Call 'StartsWith' on the field
                         // Pass the prefix to search by
                        .Where(x => x.Name.StartsWith("Ch"))
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that starts with any case variation of 'ch'
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region startsWith_2
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Call 'StartsWith' on the field
                         // Pass the prefix to search by
                        .Where(x => x.Name.StartsWith("Ch"))
                        .ToListAsync();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that starts with any case variation of 'ch'
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region startsWith_3
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'WhereStartsWith'
                         // Pass the document field and the prefix to search by
                        .WhereStartsWith(x => x.Name, "Ch")
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that starts with any case variation of 'ch'
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region startsWith_4
                    List<Product> products = session
                        .Query<Product>()
                         // Pass 'exact: true' to search for an EXACT prefix match
                        .Where(x => x.Name.StartsWith("Ch"), exact: true)
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that starts with 'Ch'
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region startsWith_5
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Pass 'exact: true' to search for an EXACT prefix match
                        .Where(x => x.Name.StartsWith("Ch"), exact: true)
                        .ToListAsync();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that starts with 'Ch'
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region startsWith_6
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'WhereStartsWith'
                         // Pass 'exact: true' to search for an EXACT prefix match
                        .WhereStartsWith(x => x.Name, "Ch", exact: true)
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that starts with 'Ch'
                    #endregion
                }                using (var session = store.OpenSession())
                {
                    #region startsWith_7
                    List<Product> products = session
                        .Query<Product>()
                         // Call 'StartsWith' on the field
                         // Pass the prefix to search by
                        .Where(x => x.Name.StartsWith("Ch") == false)
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that does NOT start with 'ch' or any other case variations of it
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region startsWith_8
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Call 'StartsWith' on the field
                         // Pass the prefix to search by
                        .Where(x => x.Name.StartsWith("Ch") == false)
                        .ToListAsync();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that does NOT start with 'ch' or any other case variations of it
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region startsWith_9
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'Not' to negate the next predicate
                        .Not
                         // Call 'WhereStartsWith'
                         // Pass the document field and the prefix to search by
                        .WhereStartsWith(x => x.Name, "Ch")
                        .ToList();
                    
                    // Results will contain only Product documents having a 'Name' field
                    // that does NOT start with 'ch' or any other case variations of it
                    #endregion
                }
            }
        }
    }
}
