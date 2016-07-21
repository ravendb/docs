using System.Collections.Generic;
using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
    public class GetUserInfo
    {
        private interface IFoo
        {
            #region get_user_info_1
            UserInfo GetUserInfo();
            #endregion
        }

        public GetUserInfo()
        {
            using (var store = new DocumentStore())
            {
                store.Initialize();
                #region get_user_info_2
                var info = store.DatabaseCommands.GetUserInfo();
                var isAdmin = info.IsAdminCurrentDb;
                List<DatabaseInfo> databases = info.Databases;

                #endregion
            }
        }
    }
}