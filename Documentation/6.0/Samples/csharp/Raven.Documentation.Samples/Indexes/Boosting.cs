using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq.Indexing;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    #region index_1
    public class Orders_ByCountries_BoostByField : AbstractIndexCreationTask<Order>
    {
        public class IndexEntry
        {
            // Index-field 'ShipToCountry' will be boosted in the map definition below
            public string ShipToCountry { get; set; }
            public string CompanyCountry { get; set; }
        }
        
        public Orders_ByCountries_BoostByField()
        {
            Map = orders => from order in orders
                let company = LoadDocument<Company>(order.Company)
                
                // Note: with current server version,
                // use 'select new' instead of 'select new IndexEntry' for compilation
                select new 
                {
                    // Boost index-field 'ShipToCountry':
                    // * Use method 'Boost', pass a numeric value to boost by 
                    // * Documents that match the query criteria for this field will rank higher
                    ShipToCountry = order.ShipTo.Country.Boost(10), 
                    CompanyCountry = company.Address.Country
                };
        }
    }
    #endregion
    
    #region index_1_js
    public class Orders_ByCountries_BoostByField_JS : AbstractJavaScriptIndexCreationTask
    {
        public Orders_ByCountries_BoostByField_JS()
        {
            Maps = new HashSet<string>()
            {
                @"map('orders', function(order) {
                    let company = load(order.Company, 'Companies')
                    return {
                        ShipToCountry: boost(order.ShipTo.Country, 10),
                        CompanyCountry: company.Address.Country
                    };
                })"
            };
        }
    }
    #endregion
    
    #region index_2
    public class Orders_ByCountries_BoostByIndexEntry : AbstractIndexCreationTask<Order>
    {
        public class IndexEntry
        {
            public string ShipToCountry { get; set; }
            public string CompanyCountry { get; set; }
        }
        
        public Orders_ByCountries_BoostByIndexEntry()
        {
            Map = orders => from order in orders
                let company = LoadDocument<Company>(order.Company)
                
                select new IndexEntry()
                {
                    ShipToCountry = order.ShipTo.Country,
                    CompanyCountry = company.Address.Country
                }
                // Boost the whole index-entry:
                // * Use method 'Boost'
                // * Pass a document-field that will set the boost level dynamically per document indexed.  
                // * The boost level will vary from one document to another based on the value of this field.
               .Boost((float) order.Freight);
        }
    }
    #endregion
    
    #region index_2_js
    public class Orders_ByCountries_BoostByIndexEntry_JS : AbstractJavaScriptIndexCreationTask
    {
        public Orders_ByCountries_BoostByIndexEntry_JS()
        {
            Maps = new HashSet<string>()
            {
                @"map('orders', function(order) {
                    let company = load(order.Company, 'Companies')
                    return boost({
                        ShipToCountry: order.ShipTo.Country,
                        CompanyCountry: company.Address.Country
                    }, order.Freight)
                })"
            };
        }
    }
    #endregion

    public class Boosting
    {
        public async void Queries()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1
                    List<Order> orders = session
                         // Query the index    
                        .Query<Orders_ByCountries_BoostByField.IndexEntry, Orders_ByCountries_BoostByField>()
                        .Where(x => x.ShipToCountry == "Poland" || x.CompanyCountry == "Portugal")
                        .OfType<Order>()
                        .ToList();
                    
                    // Because index-field 'ShipToCountry' was boosted (inside the index definition),
                    // then documents containing 'Poland' in their 'ShipTo.Country' field will get a higher score than
                    // documents containing a company that is located in 'Portugal'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2
                    List<Order> orders = await asyncSession
                         // Query the index    
                        .Query<Orders_ByCountries_BoostByField.IndexEntry, Orders_ByCountries_BoostByField>()
                        .Where(x => x.ShipToCountry == "Poland" || x.CompanyCountry == "Portugal")
                        .OfType<Order>()
                        .ToListAsync();
                    
                    // Because index-field 'ShipToCountry' was boosted (inside the index definition),
                    // then documents containing 'Poland' in their 'ShipTo.Country' field will get a higher score than
                    // documents containing a company that is located in 'Portugal'.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_3
                    List<Order> orders = session.Advanced
                         // Query the index 
                        .DocumentQuery<Orders_ByCountries_BoostByField.IndexEntry, Orders_ByCountries_BoostByField>()
                        .WhereEquals(x => x.ShipToCountry, "Poland")
                        .OrElse()
                        .WhereEquals(x => x.CompanyCountry, "Portugal")
                        .OfType<Order>()
                        .ToList();
                    
                    // Because index-field 'ShipToCountry' was boosted (inside the index definition),
                    // then documents containing 'Poland' in their 'ShipTo.Country' field will get a higher score than
                    // documents containing a company that is located in 'Portugal'.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_4
                    List<Order> orders = session
                         // Query the index  
                        .Query<Orders_ByCountries_BoostByIndexEntry.IndexEntry, Orders_ByCountries_BoostByIndexEntry>()
                        .Where(x => x.ShipToCountry == "Poland" || x.CompanyCountry == "Portugal")
                        .OfType<Order>()
                        .ToList();
                    
                    // The resulting score per matching document is affected by the value of the document-field 'Freight'. 
                    // Documents with a higher 'Freight' value will rank higher.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_5
                    List<Order> orders = await asyncSession
                         // Query the index  
                        .Query<Orders_ByCountries_BoostByIndexEntry.IndexEntry, Orders_ByCountries_BoostByIndexEntry>()
                        .Where(x => x.ShipToCountry == "Poland" || x.CompanyCountry == "Portugal")
                        .OfType<Order>()
                        .ToListAsync();
                    
                    // The resulting score per matching document is affected by the value of the document-field 'Freight'. 
                    // Documents with a higher 'Freight' value will rank higher.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_6
                    List<Order> orders = session.Advanced
                         // Query the index  
                        .DocumentQuery<Orders_ByCountries_BoostByIndexEntry.IndexEntry, Orders_ByCountries_BoostByIndexEntry>()
                        .WhereEquals(x => x.ShipToCountry, "Poland")
                        .OrElse()
                        .WhereEquals(x => x.CompanyCountry, "Portugal")
                        .OfType<Order>()
                        .ToList();
                    
                    // The resulting score per matching document is affected by the value of the document-field 'Freight'. 
                    // Documents with a higher 'Freight' value will rank higher.
                    #endregion
                }
            }
        }
    }
}
