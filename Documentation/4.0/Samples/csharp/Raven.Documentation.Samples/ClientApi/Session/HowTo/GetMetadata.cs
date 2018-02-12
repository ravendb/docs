using System;
using System.Collections.Generic;
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

                    var user = new User
                    {
                        Name = "Idan"
                    };

                    session.Store(user);
                    var userMetadata = session.Advanced.GetMetadataFor(user);
                    userMetadata.Add("Permissions", "ReadOnly");
                    session.SaveChanges();

                    #endregion
                }

                #region modify_metadata_2

                using (var session = store.OpenSession())
                {
                    var user = session.Load<User>("users/1-A");
                    var metadata = session.Advanced.GetMetadataFor(user);

                    if (metadata.ContainsKey("Permissions"))
                        metadata["Permissions"] = "ReadAndWrite";
                    else
                        metadata.Add("Permissions", "ReadAndWrite");

                    session.SaveChanges();
                }

                #endregion
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
