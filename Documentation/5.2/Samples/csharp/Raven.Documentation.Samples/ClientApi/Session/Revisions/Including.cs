using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using System.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.Revisions
{
    public class Including
    {
        private interface IFoo
        {
            public interface IRevisionIncludeBuilder<T, out TBuilder>
            {
                #region IncludeRevisions_1_IncludeRevisions
                TBuilder IncludeRevisions(Expression<Func<T, string>> path);
                TBuilder IncludeRevisions(Expression<Func<T, IEnumerable<string>>> path);
                TBuilder IncludeRevisions(DateTime before);
                #endregion
            }
        }

        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {

                const string id = "users/95";
                string cv;
                var dateTime = DateTime.Now.ToLocalTime();
                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_2_LoadByDate
                    var query = session.Load<User>(id, builder => builder
                        .IncludeRevisions(dateTime));

                    // This revision has been included, and is now loaded from memory
                    var revision = session.Advanced.Revisions.Get<User>(id, dateTime.ToUniversalTime());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_4_LoadByChangeVector
                    var sn = session.Load<UserDefinedClass>(id, builder => builder
                        // Include a single revision 
                        .IncludeRevisions(x => x.Payroll_1_ChangeVector)
                        // Include a group of revisions
                        .IncludeRevisions(x => x.PayrollChangeVectors));

                    var metadatas = session.Advanced.Revisions.GetMetadataFor(id);
                    cv = metadatas.First().GetString(Constants.Documents.Metadata.ChangeVector);

                    // This revision has been included, and is now loaded from memory
                    var revision = session.Advanced.Revisions.Get<User>(cv);
                    #endregion
                }

                var beforeDateTime = DateTime.UtcNow;
                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_5_QueryByDate
                    var query = session.Query<User>()
                        .Customize(x => x.WaitForNonStaleResults())
                        .Include(builder => builder
                            .IncludeRevisions(beforeDateTime));
                    var users = query.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_6_QueryByChangeVectors
                    // Include the revision whose change vector is stored in "Payroll_3_ChangeVector"
                    // and the revisions whose change vectors are stored in "PayrollChangeVectors"
                    var query = session.Load<UserDefinedClass>(id, builder => builder
                        .IncludeRevisions(dateTime)
                        .IncludeRevisions(x => x.Payroll_3_ChangeVector)
                        .IncludeRevisions(x => x.PayrollChangeVectors));
                    #endregion
                }


                using (var session = store.OpenSession())
                {
                    var beforeGivenDate = DateTime.UtcNow;
                    #region IncludeRevisions_7_RawQueryByDate
                    // Include revision preceding the given date
                    var query = session.Advanced
                        .RawQuery<User>("from Users include revisions($p0)")
                        .AddParameter("p0", beforeGivenDate)
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_8_RawQueryByChangeVector
                    // Include revision by change vector
                    var query = session.Advanced
                        .RawQuery<UserDefinedClass>("from Users where Name = 'JohnDoe' include revisions($p0)")
                        .AddParameter("p0", "Payroll_1_ChangeVector")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
            }
        }

        #region IncludeRevisions_3_UserDefinition
        private class UserDefinedClass
        {
            public string Name { get; set; }
            
            // A single revision's Change Vector
            public string Payroll_1_ChangeVector { get; set; }
            
            public string Payroll_2_ChangeVector { get; set; }
            public string Payroll_3_ChangeVector { get; set; }
            
            // An array of revision Change Vectors
            public List<string> PayrollChangeVectors { get; set; }
        }
        #endregion

    }
}
