using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.Server
{
    public class CompareExchange
    {
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
                    // at this point user has Id field assigned

                    // try to reserve user e-mail - please notice this operation
                    // takes place outside session transaction
                    // and performing cluster-wide reservation
                    CompareExchangeResult<string> cmpXchgResult
                        = store.Operations.Send(
                            new PutCompareExchangeValueOperation<string>("emails/" + email, user.Id, 0));

                    if (cmpXchgResult.Successful == false)
                        throw new Exception("Name is taken");

                    // we managed to reserve user e-mail - save changes
                    session.SaveChanges();
                }
                #endregion

            }
        }

        public async Task Sample2()
        {
            using (var store = new DocumentStore())
            {
                #region email2
                string email = "user@example.com";

                User user = new User
                {
                    Age = 35,
                    Email = email
                };

                CompareExchangeResult<User> saveResult 
                    = store.Operations.Send(new PutCompareExchangeValueOperation<User>("emails/" + email, user, 0));
                #endregion

            }
        }

        private class SharedResource
        {
            public DateTime? ReservedUntil { get; set; }
        }

        #region shared_resource
        public long TryLockResource(IDocumentStore store, string resourceName, TimeSpan duration)
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
                    // value wasn't present - we managed to reserve
                    return saveResult.Index;
                }

                // put failed - someone else owns the lock or lock expired
                if (saveResult.Value.ReservedUntil < now)
                {
                    CompareExchangeResult<SharedResource> takeLockWithTimeoutResult = store.Operations.Send(
                        new PutCompareExchangeValueOperation<SharedResource>(resourceName, resource, saveResult.Index));

                    if (takeLockWithTimeoutResult.Successful)
                    {
                        return takeLockWithTimeoutResult.Index;
                    }
                }

                // wait a little bit and retry
                Thread.Sleep(20);
            }
        }

        public void ReleaseResource(IDocumentStore store, string resourceName, long index)
        {
            CompareExchangeResult<SharedResource> deleteResult 
                = store.Operations.Send(new DeleteCompareExchangeValueOperation<SharedResource>(resourceName, index));

            // we have 2 options here:
            // deleteResult.Successful is true - we managed to release resource
            // deleteResult.Successful is false - someone else took the look due to timeout 
            //    it is also valid state

        }

        #endregion

    }
}
