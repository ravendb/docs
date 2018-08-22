using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;


namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class WhatIsChangesApi
    {
        private interface IFoo
        {
            #region changes_1
            IDatabaseChanges Changes(string database = null);
            #endregion
        }

        public async Task AsyncWhatIsChangesApi()
        {
            using (var store = new DocumentStore())
            {
                #region changes_2
                IDatabaseChanges subscription = store.Changes();
                await subscription.EnsureConnectedNow();
                subscription.ForAllDocuments().Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));

                try
                {
                    // application code here
                }
                finally
                {
                    if (subscription != null)
                        subscription.Dispose();
                }
                #endregion
            }
        }
    }
}
