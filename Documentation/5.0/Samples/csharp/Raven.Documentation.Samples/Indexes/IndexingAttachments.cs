using System.Collections.Generic;
using System.Linq;
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

        #region index
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
