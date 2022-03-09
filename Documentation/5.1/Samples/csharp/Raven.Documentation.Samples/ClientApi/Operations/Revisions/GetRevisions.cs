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
        public async Task Foo() {

            using (var documentStore = new DocumentStore())
            {
                var company = new Company { Name = "Company Name" };
                #region getAllRevisions
                var revisionsResult = await documentStore.Operations.SendAsync(
                        // get all the revisions for the company document
                        new GetRevisionsOperation<Company>(company.Id));
                #endregion
            }

            using (var documentStore = new DocumentStore())
            {
                var company = new Company { Name = "Company Name" };
                #region getRevisionsWithPaging
                RevisionsResult<Company> revisionsResult = await documentStore.Operations.SendAsync(
                        // skip 50 revisions, retrieve revisions 10 at a time
                        new GetRevisionsOperation<Company>(company.Id, start: 50, pageSize: 10));

                // The retrieved revisions
                List<Company> companiesRevisions = revisionsResult.Results;

                // The number of revisions for this document
                int numberOfRevisions = companiesRevisions.Count;
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
        public List<Contact> Contacts { get; set; }
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
