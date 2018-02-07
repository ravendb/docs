using System;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Indexes.Querying;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetMetadata
    {
        public GetMetadata()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region get_metadata_2

                    var employee = session.Load<Employee>("employees/1-A");
                    var metadata = session.Advanced.GetMetadataFor(employee);

                    #endregion

                    #region modify_metadata_1

                    var user = session.Load<User>("users/1-A");
                    var userMetadata = session.Advanced.GetMetadataFor(user);
                    if (userMetadata.ContainsKey(Constants.Documents.Metadata.LastModified))
                        userMetadata[Constants.Documents.Metadata.LastModified] = DateTime.UtcNow;
                    session.SaveChanges();

                    #endregion
                }
            }
        }

        private interface IFoo
        {
            #region get_metadata_1

            IMetadataDictionary GetMetadataFor<T>(T instance);

            #endregion
        }
    }
}
