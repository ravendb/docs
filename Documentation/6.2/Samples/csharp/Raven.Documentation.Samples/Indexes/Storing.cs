using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class StoringDataInIndex
    {
        #region index_1
        public class QuantityOrdered_ByCompany :
            AbstractIndexCreationTask<Order, QuantityOrdered_ByCompany.IndexEntry>
        {
            // The index-entry:
            public class IndexEntry
            {
                // The index-fields:
                public string Company { get; set; }
                public string CompanyName { get; set; }
                public int TotalItemsOrdered { get; set; }
            }
            
            public QuantityOrdered_ByCompany()
            {
                Map = orders => from order in orders
                    select new IndexEntry
                    {
                        // 'Company' is a SIMPLE index-field,
                        // its value is taken directly from the Order document:
                        Company = order.Company,
                        
                        // 'CompanyName' is also considered a simple index-field,
                        // its value is taken from the related Company document:
                        CompanyName = LoadDocument<Company>(order.Company).Name,
                        
                        // 'TotalItemsOrdered' is a COMPUTED index-field:
                        // (the total quantity of items ordered in an Order document)
                        TotalItemsOrdered = order.Lines.Sum(orderLine => orderLine.Quantity) 
                    };
                
                // Store the calculated 'TotalItemsOrdered' index-field in the index:
                // ==================================================================
                Stores.Add(x => x.TotalItemsOrdered, FieldStorage.Yes);
                
                // You can use an analyzer to tokenize the 'CompanyName' index-field for full-text search:
                // =======================================================================================
                Analyzers.Add(x => x.CompanyName, "SimpleAnalyzer");
                
                // Store the original value of `CompanyName` in the index (BEFORE tokenization):
                // =============================================================================
                Stores.Add(x => x.CompanyName, FieldStorage.Yes);
            }
        }
        #endregion
        
        #region index_2
        public class QuantityOrdered_ByCompany_JS : AbstractJavaScriptIndexCreationTask
        {
            public QuantityOrdered_ByCompany_JS()
            {
                Maps = new HashSet<string>()
                {
                    @"map('orders', function(order) {
                        let company = load(order.Company, 'Companies')
                        return {
                            Company: order.Company,
                            CompanyName: company.Name,
                            TotalItemsOrdered: order.Lines.reduce(function(total, line) {
                                return total + line.Quantity;
                            }, 0)
                        };
                    })" 
                };

                Fields = new Dictionary<string, IndexFieldOptions>
                {
                    {
                        "TotalItemsOrdered", new IndexFieldOptions
                        {
                            Storage = FieldStorage.Yes
                        }
                    },
                    {
                        "CompanyName", new IndexFieldOptions
                        {
                            Storage = FieldStorage.Yes,
                            Analyzer = "SimpleAnalyzer"
                        }
                    }
                };
            }
        }
        #endregion

        public async void Storing()
        {
            using (var store = new DocumentStore())
            {
                #region index_3
                var indexDefinition = new IndexDefinition
                {
                    Name = "QuantityOrdered/ByCompany",
                    
                    Maps =
                    {
                        @"from order in docs.Orders
                          select new
                          {
                              Company = order.Company,
                              CompanyName = LoadDocument(order.Company, ""Companies"").Name,
                              TotalItemsOrdered = order.Lines.Sum(orderLine => orderLine.Quantity) 
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>
                    {
                        {
                            "TotalItemsOrdered", new IndexFieldOptions
                            {
                                Storage = FieldStorage.Yes
                            }
                        },
                        {
                            "CompanyName", new IndexFieldOptions
                            {
                                Storage = FieldStorage.Yes,
                                Analyzer = "SimpleAnalyzer"
                            }
                        }
                    }
                };
                
                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region query_1
                using (var session = store.OpenSession())
                {
                    List<NumberOfItemsOrdered> itemsOrdered = session
                        .Query<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .Where(order => order.Company == "companies/90-A")
                         // Project results into a custom class:
                        .ProjectInto<NumberOfItemsOrdered>()
                        .ToList();
                } 
                #endregion
                
                #region query_1_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<NumberOfItemsOrdered> itemsOrdered = await asyncSession
                        .Query<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .Where(order => order.Company == "companies/90-A")
                        .ProjectInto<NumberOfItemsOrdered>()
                        .ToListAsync();
                } 
                #endregion
                
                #region query_1_docQuery
                using (var session = store.OpenSession())
                {
                    List<NumberOfItemsOrdered> itemsOrdered = session.Advanced
                        .DocumentQuery<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .WhereEquals(order => order.Company, "companies/90-A")
                        .SelectFields<NumberOfItemsOrdered>()
                        .ToList();
                } 
                #endregion
                
                #region query_1_docQuery_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<NumberOfItemsOrdered> itemsOrdered = await asyncSession.Advanced
                        .AsyncDocumentQuery<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .WhereEquals(order => order.Company, "companies/90-A")
                        .SelectFields<NumberOfItemsOrdered>()
                        .ToListAsync();
                } 
                #endregion
                
                #region query_2
                using (var session = store.OpenSession())
                {
                    List<ProjectedDetails> orders = session
                        .Query<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .Where(order => order.Company == "companies/90-A")
                         // Project results into a custom class:
                        .ProjectInto<ProjectedDetails>()
                        .ToList();
                } 
                #endregion
                
                #region query_2_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<ProjectedDetails> orders = await asyncSession
                        .Query<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .Where(order => order.Company == "companies/90-A")
                        .ProjectInto<ProjectedDetails>()
                        .ToListAsync();
                } 
                #endregion
                
                #region query_2_docQuery
                using (var session = store.OpenSession())
                {
                    List<ProjectedDetails> orders = session.Advanced
                        .DocumentQuery<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .WhereEquals(order => order.Company, "companies/90-A")
                        .SelectFields<ProjectedDetails>()
                        .ToList();
                } 
                #endregion
                
                #region query_2_docQuery_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<ProjectedDetails> orders = await asyncSession.Advanced
                        .AsyncDocumentQuery<QuantityOrdered_ByCompany.IndexEntry, QuantityOrdered_ByCompany>()
                        .WhereEquals(order => order.Company, "companies/90-A")
                        .SelectFields<ProjectedDetails>()
                        .ToListAsync();
                } 
                #endregion
            }
        }
        
        #region details_to_project_1
        public class NumberOfItemsOrdered
        {
            // This field was stored in the index definition
            public int TotalItemsOrdered { get; set; }
        }
        #endregion
        
        #region details_to_project_2
        public class ProjectedDetails
        {
            // This field was Not stored in the index definition
            public string Company { get; set; }
            // This field was Not stored in the index definition
            public DateTime OrderedAt { get; set; }
            // This field was stored in the index definition
            public int TotalItemsOrdered { get; set; }
        }
        #endregion
    }
}
