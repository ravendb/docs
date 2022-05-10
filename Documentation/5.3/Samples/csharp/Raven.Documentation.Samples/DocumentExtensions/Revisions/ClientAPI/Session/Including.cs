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
                // Include a single revision by Time
                TBuilder IncludeRevisions(DateTime before);
                // Include a single revision by Change Vector
                TBuilder IncludeRevisions(Expression<Func<T, string>> path);
                // Include an array of revisions by Change Vectors
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
                var creationTime = DateTime.Now.ToLocalTime();
                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_2_LoadByTime
                    // Pass `IncludeRevisions` a `DateTime` value
                    var query = session.Load<User>(id, builder => builder
                        .IncludeRevisions(creationTime.ToUniversalTime()));

                    // The revision has been included by its creation time,
                    // and is now retrieved locally from the session
                    var revision = session
                        .Advanced.Revisions.Get<User>(id, creationTime.ToUniversalTime());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_4_LoadByChangeVector
                    var sn = session.Load<Contract>(id, builder => builder
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

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_5_QueryByTime
                    // Pass `IncludeRevisions` a `DateTime` value
                    var query = session.Query<User>()
                        .Include(builder => builder
                            .IncludeRevisions(creationTime.ToUniversalTime()));

                    // The revision has been included by its creation time,
                    // and is now retrieved locally from the session
                    var revision = session
                        .Advanced.Revisions.Get<User>(id, creationTime.ToUniversalTime());
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_6_QueryByChangeVectors
                    // Include the revision whose change vector is stored in
                    // "ContractRev_2_ChangeVector",
                    // and the revisions whose change vectors are stored in
                    // "ContractRevChangeVectors"
                    var query = session.Load<Contract>(id, builder => builder
                        .IncludeRevisions(creationTime)
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
                    #region IncludeRevisions_7_RawQueryByTime
                    // Pass the `include revision` command a `DateTime` value
                    var query = session.Advanced
                        .RawQuery<User>("from Users include revisions($p0)")
                        .AddParameter("p0", creationTime.ToUniversalTime())
                        .ToList();

                    // The revision has been included by its creation time,
                    // and is now retrieved locally from the session
                    var revision = session
                        .Advanced.Revisions.Get<User>(id, creationTime.ToUniversalTime());

                    #endregion
                }

                string changeVector;
                using (var session = store.OpenSession())
                {
                    var metadatas = session.Advanced.Revisions.GetMetadataFor(id);

                    changeVector = metadatas.First()
                        .GetString(Constants.Documents.Metadata.ChangeVector);

                    session.Advanced
                        .Patch<Contract, string>(id, x => x.ContractRev_1_ChangeVector, changeVector);
                    session.SaveChanges();
                }
                
                using (var session = store.OpenSession())
                {
                    #region IncludeRevisions_8_RawQueryByChangeVector
                    // Pass the `include revision` command a `path`
                    // to a document property that holds change vector/s,
                    var query = session.Advanced
                        .RawQuery<Contract>("from Users where Name = 'JohnDoe' include revisions($p0)")
                        .AddParameter("p0", "ContractRev_1_ChangeVector")
                        .ToList();

                    // The revision has been included by change vector,
                    // and is now retrieved locally from the session
                    var revision = session.Advanced.Revisions.Get<Contract>(changeVector);

                    #endregion
                }
            }
        }

        #region IncludeRevisions_3_UserDefinition
        private class Contract
        {
            public string Name { get; set; }
            
            // A single revision's Change Vector
            public string ContractRev_1_ChangeVector { get; set; }

            // A single revision's Change Vector
            public string ContractRev_2_ChangeVector { get; set; }

            // An array of revision Change Vectors
            public List<string> ContractRevChangeVectors { get; set; }
        }
        #endregion

        private class User
        {
            public string Name { get; internal set; }
        }

    }
}
