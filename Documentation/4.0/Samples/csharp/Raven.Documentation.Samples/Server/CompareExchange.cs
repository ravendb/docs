using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Linq;

namespace Raven.Documentation.Samples.Server
{
    public class CompareExchange
    {
        private DocumentStore store;

        private class User
        {
            public string Email { get; set; }
            public string Id { get; set; }
            public int Age { get; set; }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region email
                string email = "user@example.com";

                User user = new User
                {
                    Email = email
                };

                using (IDocumentSession session = store.OpenSession())
                {
                    session.Store(user);
                    // At this point, the user document has an Id assigned

                    // Try to reserve a new user email 
                    // Note: This operation takes place outside of the session transaction, 
                    //       It is a cluster-wide reservation
                    CompareExchangeResult<string> cmpXchgResult
                        = store.Operations.Send(
                            new PutCompareExchangeValueOperation<string>("emails/" + email, user.Id, 0));

                    if (cmpXchgResult.Successful == false)
                        throw new Exception("Email is already in use");

                    // At this point we managed to reserve/save the user email -
                    // The document can be saved in SaveChanges
                    session.SaveChanges();
                }
                #endregion

                #region query_cmpxchg
                using (IDocumentSession session = store.OpenSession())
                {
                    var query = from u in session.Query<User>()
                                where u.Email == RavenQuery.CmpXchg<string>("emails/ayende@ayende.com")
                                select u;

                    var q = session.Advanced
                        .DocumentQuery<User>()
                        .WhereEquals("Email", CmpXchg.Value("emails/ayende@ayende.com"));
                }
                #endregion
            }
        }

        #region shared_resource
        private class SharedResource
        {
            public DateTime? ReservedUntil { get; set; }
        }

        public void PrintWork() 
        {
            // Try to get hold of the printer resource
            long reservationIndex = LockResource(store, "Printer/First-Floor", TimeSpan.FromMinutes(20));

            try
            {
                // Do some work for the duration that was set.
                // Don't exceed the duration, otherwise resource is available for someone else.
            }
            finally
            {
                ReleaseResource(store, "Printer/First-Floor", reservationIndex);
            }
        }

        public long LockResource(IDocumentStore store, string resourceName, TimeSpan duration)
        {
            while (true)
            {
                DateTime now = DateTime.UtcNow;

                SharedResource resource = new SharedResource
                {
                    ReservedUntil = now.Add(duration)
                };

                CompareExchangeResult<SharedResource> saveResult = store.Operations.Send(
                        new PutCompareExchangeValueOperation<SharedResource>(resourceName, resource, 0));

                if (saveResult.Successful)
                {
                    // resourceName wasn't present - we managed to reserve
                    return saveResult.Index;
                }

                // At this point, Put operation failed - someone else owns the lock or lock time expired
                if (saveResult.Value.ReservedUntil < now)
                {
                    // Time expired - Update the existing key with the new value
                    CompareExchangeResult<SharedResource> takeLockWithTimeoutResult = store.Operations.Send(
                        new PutCompareExchangeValueOperation<SharedResource>(resourceName, resource, saveResult.Index));

                    if (takeLockWithTimeoutResult.Successful)
                    {
                        return takeLockWithTimeoutResult.Index;
                    }
                }

                // Wait a little bit and retry
                Thread.Sleep(20);
            }
        }

        public void ReleaseResource(IDocumentStore store, string resourceName, long index)
        {
            CompareExchangeResult<SharedResource> deleteResult
                = store.Operations.Send(new DeleteCompareExchangeValueOperation<SharedResource>(resourceName, index));

            // We have 2 options here:
            // deleteResult.Successful is true - we managed to release resource
            // deleteResult.Successful is false - someone else took the lock due to timeout 
        }

        #endregion
    }
}
