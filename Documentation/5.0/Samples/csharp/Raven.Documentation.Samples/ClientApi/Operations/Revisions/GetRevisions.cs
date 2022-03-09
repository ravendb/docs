using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations.Revisions;

namespace Raven.Documentation.Samples.ClientApi.Operations.Revisions
{
    public class HowtoConfigureRevisions
    {
        public async Task Foo()
        {

            using (var documentStore = new DocumentStore())
            {
                var company = new Company { Name = "Company Name" };
                #region getAllRevisions
                RevisionsResult<Company> revisions = await documentStore.Operations.SendAsync(
                        // get all the revisions for this document
                        new GetRevisionsOperation<Company>(company.Id));

                List<Company> retrievedRevisions = revisions.Results;
                int revisionsCount = revisions.TotalResults;

                #endregion
            }


            using (var documentStore = new DocumentStore())
            {
                var company = new Company { Name = "Company Name" };
                #region getRevisionsWithPaging
                var start = 0;
                var pageSize = 100;
                while (true)
                {
                    RevisionsResult<Company> revisions = await documentStore.Operations.SendAsync(
                        new GetRevisionsOperation<Company>(company.Id, start, pageSize));
                    {
                        // process the retrieved revisions here
                    }
                    if (revisions.Results.Count < pageSize)
                        break; // no more revisions to retrieve

                    // increment 'start' by page-size, to get the "next page" in next iteration
                    start += pageSize;
                }
                #endregion
            }

            using (var documentStore = new DocumentStore())
            {
                var company = new Company { Name = "Company Name" };
                #region getRevisionsWithPaging_wrappedObject
                var parameters = new GetRevisionsOperation<Company>.Parameters
                {
                    Id = company.Id,
                    Start = 0,
                    PageSize = 100
                };

                RevisionsResult<Company> revisions = await documentStore.Operations.SendAsync(
                    new GetRevisionsOperation<Company>(parameters));
                #endregion
            }


        }
    }

    public class Company
    {
        public decimal AccountsReceivable { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int Phone { get; set; }
        public CompanyType Type { get; set; }
        public List<string> EmployeesIds { get; set; }

        public enum CompanyType
        {
            Public,
            Private
        }
    }



}
