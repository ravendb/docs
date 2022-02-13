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
                // Incude a single revision by date
                TBuilder IncludeRevisions(DateTime before);
                // Include a single revision by changevector
                TBuilder IncludeRevisions(Expression<Func<T, string>> path);
                // Include an array of revisions by changevectors
                TBuilder IncludeRevisions(Expression<Func<T, IEnumerable<string>>> path);
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
                    // Pass `IncludeRevisions` a `DateTime` value
                    var query = session.Load<User>(id, builder => builder
                        .IncludeRevisions(dateTime.ToUniversalTime()));

                    // The revision has been included by date,
                    // and is now retrieved locally from the session
                    var revision = session
                        .Advanced.Revisions.Get<User>(id, dateTime.ToUniversalTime());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_4_LoadByChangeVector
                    var sn = session.Load<UserDefinedClass>(id, builder => builder
                        // Include a single revision 
                        .IncludeRevisions(x => x.ContractRev_1_ChangeVector)
                        // Include a group of revisions
                        .IncludeRevisions(x => x.ContractRevChangeVectors));

                    // The revisions have been included by change vectors,
                    // and are now retrieved locally from the session
                    var revision = sn.ContractRev_1_ChangeVector;
                    var revisions = sn.ContractRevChangeVectors;
                    #endregion
                }

                var beforeDateTime = DateTime.UtcNow;
                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_5_QueryByDate
                    // Pass `IncludeRevisions` a `DateTime` value
                    var query = session.Query<User>()
                        .Customize(x => x.WaitForNonStaleResults())
                        .Include(builder => builder
                            .IncludeRevisions(beforeDateTime.ToUniversalTime()));

                    // The revision has been included by date,
                    // and is now retrieved locally from the session
                    var revision = session
                        .Advanced.Revisions.Get<User>(id, dateTime.ToUniversalTime());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_6_QueryByChangeVectors
                    // Include the revision whose change vector is stored in
                    // "ContractRev_2_ChangeVector",
                    // and the revisions whose change vectors are stored in
                    // "ContractRevChangeVectors"
                    var query = session.Load<UserDefinedClass>(id, builder => builder
                        .IncludeRevisions(dateTime)
                        .IncludeRevisions(x => x.ContractRev_2_ChangeVector)
                        .IncludeRevisions(x => x.ContractRevChangeVectors));

                    // The revisions have been included by change vectors,
                    // and are now retrieved locally from the session
                    var revision = query.ContractRev_2_ChangeVector;
                    var revisions = query.ContractRevChangeVectors;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    var beforeGivenDate = DateTime.UtcNow;
                    #region IncludeRevisions_7_RawQueryByDate
                    // Pass the `include revision` command a `DateTime` value
                    var query = session.Advanced
                        .RawQuery<User>("from Users include revisions($p0)")
                        .AddParameter("p0", beforeGivenDate.ToUniversalTime())
                        .WaitForNonStaleResults()
                        .ToList();

                    // The revision has been included by date,
                    // and is now retrieved locally from the session
                    var revision = session
                        .Advanced.Revisions.Get<User>(id, dateTime.ToUniversalTime());

                    #endregion
                }

                string changeVector;
                using (var session = store.OpenSession())
                {
                    var metadatas = session.Advanced.Revisions.GetMetadataFor(id);

                    changeVector = metadatas.First()
                        .GetString(Constants.Documents.Metadata.ChangeVector);

                    session.Advanced
                        .Patch<UserDefinedClass, string>(id, x => x.ContractRev_1_ChangeVector, changeVector);
                    session.SaveChanges();
                }
                
                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_8_RawQueryByChangeVector
                    // Pass the `include revision` command a `path`
                    // to a document property that holds change vector/s,
                    var query = session.Advanced
                        .RawQuery<UserDefinedClass>("from Users where Name = 'JohnDoe' include revisions($p0)")
                        .AddParameter("p0", "ContractRev_1_ChangeVector")
                        .WaitForNonStaleResults()
                        .ToList();

                    // The revision has been included by change vector,
                    // and is now retrieved locally from the session
                    var revision = session.Advanced.Revisions.Get<UserDefinedClass>(changeVector);

                    #endregion
                }
            }
        }

        #region IncludeRevisions_3_UserDefinition
        private class UserDefinedClass
        {
            public string Name { get; set; }
            
            // A single revision's Change Vector
            public string ContractRev_1_ChangeVector { get; set; }
            
            public string ContractRev_2_ChangeVector { get; set; }
            
            // An array of revision Change Vectors
            public List<string> ContractRevChangeVectors { get; set; }
        }
        #endregion

    }
}
