using System.Collections.Generic;
using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
    public class GetUserPermission
    {
        private interface IFoo
        {
            #region get_user_permission_1
            UserPermission GetUserPermission(string database, bool readOnly);
            #endregion
        }

        public GetUserPermission()
        {
            using (var store = new DocumentStore {Url = "http:localhost:8080",DefaultDatabase = "Foo"})
            {
                store.Initialize();
                #region get_user_permission_2
                var per = store.DatabaseCommands.GetUserPermission("Foo", false);
                var isGrant = per.IsGranted;
                var res = per.Reason;

                #endregion
            }
        }
    }
}