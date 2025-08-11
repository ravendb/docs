using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.Attachments
{
    public class IndexingAttachments
    {
        #region index_1
        public class Employees_ByAttachmentDetails : 
            AbstractIndexCreationTask<Employee, Employees_ByAttachmentDetails.IndexEntry>
        {
            public class IndexEntry
            {
                public string EmployeeName { get; set; }
                
                public string[] AttachmentNames { get; set; }
                public string[] AttachmentContentTypes { get; set; }
                public long[] AttachmentSizes { get; set; }
            }

            public Employees_ByAttachmentDetails()
            {
                Map = employees => from employee in employees
                    
                    // Call 'AttachmentsFor' to get attachments details
                    let attachments = AttachmentsFor(employee)
                    
                    select new IndexEntry()
                    {
                        // Can index info from document properties:
                        EmployeeName = employee.FirstName + " " + employee.LastName,
                        
                        // Index DETAILS of attachments:
                        AttachmentNames = attachments.Select(x => x.Name).ToArray(),
                        AttachmentContentTypes = attachments.Select(x => x.ContentType).ToArray(),
                        AttachmentSizes = attachments.Select(x => x.Size).ToArray()
                    };
            }
        }
        #endregion

        #region index_1_JS
        public class Employees_ByAttachmentDetails_JS : AbstractJavaScriptIndexCreationTask
        {
            public Employees_ByAttachmentDetails_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee) {
                        var attachments = attachmentsFor(employee);

                        return {
                            EmployeeName: employee.FirstName + ' ' + employee.LastName,

                            AttachmentNames: attachments.map(function(attachment) { return attachment.Name; }),
                            AttachmentContentTypes: attachments.map(function(attachment) { return attachment.ContentType; }),
                            AttachmentSizes: attachments.map(function(attachment) { return attachment.Size; })
                        };
                    })"
                };
            }
        }
        #endregion

        #region index_2
        public class Employees_ByAttachment: 
            AbstractIndexCreationTask<Employee, Employees_ByAttachment.IndexEntry>
        {
            public class IndexEntry
            {
                public string AttachmentName { get; set; }
                public string AttachmentContentType { get; set; }
                public long AttachmentSize { get; set; }
                
                public string AttachmentContent { get; set; }
            }
            
            public Employees_ByAttachment()
            {
                Map = employees => 
                    from employee in employees
                    
                    // Call 'LoadAttachment' to get attachment's details and content
                    // pass the attachment name, e.g. "notes.txt"
                    let attachment = LoadAttachment(employee, "notes.txt")
                    
                    select new IndexEntry()
                    {
                        // Index DETAILS of attachment:
                        AttachmentName = attachment.Name,
                        AttachmentContentType = attachment.ContentType,
                        AttachmentSize = attachment.Size,
                        
                        // Index CONTENT of attachment:
                        // Call 'GetContentAsString' to access content
                        AttachmentContent = attachment.GetContentAsString()
                    };
                
                // It can be useful configure Full-Text search on the attachment content index-field
                Index(x => x.AttachmentContent, FieldIndexing.Search);
                
                // Documents with an attachment named 'notes.txt' will be indexed,
                // allowing you to query them by either the attachment's details or its content.
            }
        }
        #endregion

        #region index_2_JS
        public class Employees_ByAttachment_JS : AbstractJavaScriptIndexCreationTask
        {
            public Employees_ByAttachment_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee) {
                          var attachment = loadAttachment(employee, 'notes.txt');
                        
                          return {
                              AttachmentName: attachment.Name,
                              AttachmentContentType: attachment.ContentType,
                              AttachmentSize: attachment.Size,
                              AttachmentContent: attachment.getContentAsString()
                          };
                      })"
                };
                 
                Fields = new Dictionary<string, IndexFieldOptions>
                {
                    {
                        "AttachmentContent", new IndexFieldOptions
                        {
                            Indexing = FieldIndexing.Search
                        }
                    }
                };
            }
        }
        #endregion

        #region index_3
        public class Employees_ByAllAttachments : 
            AbstractIndexCreationTask<Employee, Employees_ByAllAttachments.IndexEntry>
        {
            public class IndexEntry
            {
                public string AttachmentName { get; set; }
                public string AttachmentContentType { get; set; }
                public long AttachmentSize { get; set; }
                public string AttachmentContent { get; set; }
            }

            public Employees_ByAllAttachments()
            {
                Map = employees =>
                    
                    // Call 'LoadAttachments' to get details and content for ALL attachments
                    from employee in employees
                    from attachment in LoadAttachments(employee)
                    
                    // This will be a Fanout index -
                    // the index will generate an index-entry for each attachment per document
                    
                    select new IndexEntry
                    {
                        // Index DETAILS of attachment:
                        AttachmentName = attachment.Name,
                        AttachmentContentType = attachment.ContentType,
                        AttachmentSize = attachment.Size,
                        
                        // Index CONTENT of attachment:
                        // Call 'getContentAsString' to access content
                        AttachmentContent = attachment.GetContentAsString()
                    };

                // It can be useful configure Full-Text search on the attachment content index-field
                Index(x => x.AttachmentContent, FieldIndexing.Search);
            }
        }
        #endregion

        #region index_3_JS
        public class Employees_ByAllAttachments_JS : AbstractJavaScriptIndexCreationTask
        {
            public Employees_ByAllAttachments_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee) {
                          const allAttachments = loadAttachments(employee);
                     
                          return allAttachments.map(function (attachment) {
                              return {
                                  attachmentName: attachment.Name,
                                  attachmentContentType: attachment.ContentType,
                                  attachmentSize: attachment.Size,
                                  attachmentContent: attachment.getContentAsString()
                              };
                          });
                      })"
                };

                Fields = new Dictionary<string, IndexFieldOptions>
                {
                    {
                        "attachmentContent", new IndexFieldOptions
                        {
                            Indexing = FieldIndexing.Search
                        }
                    }
                };
            }
        }
        #endregion

        public void SampleData()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region store_attachments
                    // Create some sample attachments:
                    for (var i = 1; i <= 3; i++)
                    {
                        var id = $"employees/{i}-A";

                        // Load an employee document:
                        var employee = session.Load<Employee>($"employees/{i}-A");
                        if (employee?.Notes == null || employee.Notes.Count == 0)
                            continue;

                        // Store the employee's notes as an attachment on the document:
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(employee.Notes[0]);
                        using (var stream = new MemoryStream(bytes))
                        {
                            session.Advanced.Attachments.Store(
                                $"employees/{i}-A",
                                "notes.txt", stream,
                                "text/plain");
                            
                            session.SaveChanges();
                        }
                    }
                    #endregion
                }
            }
        }

        public async Task QueryExamples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1
                    List<Employee> employees = session
                         // Query the index for matching employees
                        .Query<Employees_ByAttachmentDetails.IndexEntry, Employees_ByAttachmentDetails>()
                         // Filter employee results by their attachments details
                        .Where(x => x.AttachmentNames.Contains("photo.jpg"))
                        .Where(x => x.AttachmentSizes.Any(size => size > 20_000))
                         // Return matching Employee docs
                        .OfType<Employee>()
                        .ToList();
                    
                    // Results:
                    // ========
                    // Running this query on the Northwind sample data,
                    // results will include 'employees/4-A' and 'employees/5-A'.
                    // These 2 documents contain an attachment by name 'photo.jpg' with a matching size.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_async
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByAttachmentDetails.IndexEntry, Employees_ByAttachmentDetails>()
                        .Where(x => x.AttachmentNames.Contains("photo.jpg"))
                        .Where(x => x.AttachmentSizes.Any(size => size > 20_000))
                        .OfType<Employee>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_1_documentQuery
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employees_ByAttachmentDetails.IndexEntry, Employees_ByAttachmentDetails>()
                        .WhereEquals("AttachmentNames", "photo.jpg")
                        .WhereGreaterThan("AttachmentSizes", 20_000)
                        .OfType<Employee>() 
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_2
                    List<Employee> employees = session
                         // Query the index for matching employees
                        .Query<Employees_ByAttachment.IndexEntry, Employees_ByAttachment>()
                         // Can make a full-text search
                         // Looking for employees with an attachment content that contains 'Colorado' OR 'Dallas'
                        .Search(x => x.AttachmentContent, "Colorado Dallas")
                        .OfType<Employee>()
                        .ToList();
                    
                    // Results:
                    // ========
                    // Results will include 'employees/1-A' and 'employees/2-A'.
                    // Only these 2 documents have an attachment by name 'notes.txt'
                    // that contains either 'Colorado' or 'Dallas'.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2_async
                    List<Employee> employees = await asyncSession
                         // Query the index for matching employees
                        .Query<Employees_ByAttachment.IndexEntry, Employees_ByAttachment>()
                         // Can make a full-text search
                         // Looking for employees with an attachment content that contains 'Colorado' OR 'Dallas'
                        .Search(x => x.AttachmentContent, "Colorado Dallas")
                        .OfType<Employee>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2_documentQuery
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employees_ByAttachment.IndexEntry, Employees_ByAttachment>()
                        .Search(x => x.AttachmentContent, "Colorado Dallas")
                        .OfType<Employee>() 
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_3
                    // Query the index for matching employees
                    List<Employee> employees = session
                        .Query<Employees_ByAllAttachments.IndexEntry, Employees_ByAllAttachments>()
                        // Filter employee results by their attachments details and content:
                        // Using 'SearchOptions.Or' combines the full-text search on 'AttachmentContent'
                        // with the following 'Where' condition using OR logic.
                        .Search(x => x.AttachmentContent, "Colorado Dallas", options: SearchOptions.Or)
                        .Where(x => x.AttachmentSize > 20_000)
                        .OfType<Employee>()
                        .ToList();
                    
                    // Results:
                    // ========
                    // Results will include:
                    // 'employees/1-A' and 'employees/2-A' that match the content criteria 
                    // 'employees/4-A' and 'employees/5-A' that match the size criteria
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_3_async
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByAttachment.IndexEntry, Employees_ByAttachment>()
                        .Search(x => x.AttachmentContent, "Colorado Dallas", options: SearchOptions.Or)
                        .Where(x => x.AttachmentSize > 20_000)
                        .OfType<Employee>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_3_documentQuery
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employees_ByAllAttachments.IndexEntry, Employees_ByAllAttachments>()
                        .Search(x => x.AttachmentContent, "Colorado Dallas")
                        .OrElse()
                        .WhereGreaterThan(x => x.AttachmentSize, 20_000)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region syntax_1
            // Returns a list of attachment details for the specified document.
            IEnumerable<AttachmentName> AttachmentsFor(object document);
            #endregion

            #region syntax_2
            // Returns the attachment specified for the given document.
            public IAttachmentObject LoadAttachment(object document, string attachmentName);
            #endregion
            
            #region syntax_3
            // Returns a list of all attachments for the specified document.
            public IEnumerable<IAttachmentObject> LoadAttachments(object doc);
            #endregion
        }

        #region attachment_details
        // AttachmentsFor returns a list containing the following attachment details object:
        public class AttachmentName
        {
            public string Name;
            public string Hash;
            public string ContentType;
            public long Size;
        }
        #endregion

        private interface IFoo2
        {
            #region attachment_object
            public interface IAttachmentObject
            {
                public string Name { get; }
                public string Hash { get; }
                public string ContentType { get; }
                public long Size { get; }
                
                public string GetContentAsString();
                public string GetContentAsString(Encoding encoding);
                public Stream GetContentAsStream();
            }
            #endregion
        }
    }
}
