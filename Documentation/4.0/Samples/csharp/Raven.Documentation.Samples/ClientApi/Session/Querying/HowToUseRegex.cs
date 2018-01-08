using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToUseRegex
    {
        private class User
        {
            public string FirstName { get; set; }
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region regex_1
                    // loads all users, which first name
                    // starts with 'A' or 'J'
                    List<User> users = session
                        .Query<User>()
                        .Where(x => Regex.IsMatch(x.FirstName, "^[AJ]"))
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region regex_1_async
                    // loads all users, which first name
                    // starts with 'A' or 'J'
                    List<User> users = await asyncSession
                        .Query<User>()
                        .Where(x => Regex.IsMatch(x.FirstName, "^[AJ]"))
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
