using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class IndexingAttachments
    {
        private interface IFoo
        {
            #region syntax
            IEnumerable<AttachmentName> AttachmentsFor(object doc);
            #endregion

            #region syntax_2
            public IAttachmentObject LoadAttachment(object doc, string name);
            public IEnumerable<IAttachmentObject> LoadAttachments(object doc);
            #endregion

        }

        private class Foo
        {
            #region result
            public string Name;
            public string Hash;
            public string ContentType;
            public long Size;
            #endregion
        }

        #region AttFor_index_LINQ
        public class Employees_ByAttachmentNames : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public string[] AttachmentNames { get; set; }
            }

            public Employees_ByAttachmentNames()
            {
                Map = employees => from e in employees
                                   let attachments = AttachmentsFor(e)
                                   select new Result
                                   {
                                       AttachmentNames = attachments.Select(x => x.Name).ToArray()
                                   };
            }
        }
        #endregion

        #region AttFor_index_JS
        public class Employees_ByAttachmentNames_JS : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string[] AttachmentNames { get; set; }
            }

            public Employees_ByAttachmentNames_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (e) {
                        var attachments = attachmentsFor(e);
                        return {
                            AttachmentNames: attachments.map(
                                function(attachment) {
                                    return attachment.Name;
                                }
                        };
                    })"
                };
            }
        }
        #endregion

        #region LoadAtt_index_LINQ
        private class Companies_With_Attachments : AbstractIndexCreationTask<Company>
        {

            public Companies_With_Attachments()
            {
                Map = companies => from company in companies
                                   let attachment = LoadAttachment(company, company.ExternalId)
                                   select new
                                   {
                                       CompanyName = company.Name,
                                       AttachmentName = attachment.Name,
                                       AttachmentContentType = attachment.ContentType,
                                       AttachmentHash = attachment.Hash,
                                       AttachmentSize = attachment.Size,
                                       AttachmentContent = attachment.GetContentAsString(Encoding.UTF8),
                                   };
            }
        }
        #endregion

        #region LoadAtt_index_JS
        private class Companies_With_Attachments_JavaScript : AbstractJavaScriptIndexCreationTask
        {
            public Companies_With_Attachments_JavaScript()
            {
                Maps = new HashSet<string>
                {
                    @"map('Companies', function (company) {
                        var attachment = loadAttachment(company, company.ExternalId);
                        return {
                            CompanyName: company.Name,
                            AttachmentName: attachment.Name,
                            AttachmentContentType: attachment.ContentType,
                            AttachmentHash: attachment.Hash,
                            AttachmentSize: attachment.Size,
                            AttachmentContent: attachment.getContentAsString('utf8')
                        };
                    })"
                };
            }
        }
        #endregion

        #region LoadAtts_index_LINQ
        private class Companies_With_All_Attachments : AbstractIndexCreationTask<Company>
        {
            public Companies_With_All_Attachments()
            {
                Map = companies => from company in companies
                                   let attachments = LoadAttachments(company)
                                   from attachment in attachments
                                   select new
                                   {
                                       AttachmentName = attachment.Name,
                                       AttachmentContent = attachment.GetContentAsString(Encoding.UTF8)
                                   };
            }
        }
        #endregion

        #region LoadAtts_index_JS
        private class Companies_With_All_Attachments_JS : AbstractJavaScriptIndexCreationTask
        {
            public Companies_With_All_Attachments_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Companies', function (company) {
                        var attachments = loadAttachments(company);
                        return attachments.map(attachment => ({
                            AttachmentName: attachment.Name,
                            AttachmentContent: attachment.getContentAsString('utf8')
                        }));
                    })"
                };
            }
        }
        #endregion

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query1
                    //return all employees that have an attachment called "cv.pdf"
                    List<Employee> employees = session
                        .Query<Employees_ByAttachmentNames.Result, Employees_ByAttachmentNames>()
                        .Where(x => x.AttachmentNames.Contains("cv.pdf"))
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query2
                    //return all employees that have an attachment called "cv.pdf"
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByAttachmentNames.Result, Employees_ByAttachmentNames>()
                        .Where(x => x.AttachmentNames.Contains("cv.pdf"))
                        .OfType<Employee>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query3
                    //return all employees that have an attachment called "cv.pdf"
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee, Employees_ByAttachmentNames>()
                        .ContainsAny("AttachmentNames", new[] { "cv.pdf" })
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
