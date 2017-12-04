using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToUseSearch
    {
        public interface IFoo<T>
        {
            #region search_1
            IRavenQueryable<T> Search<T>(
                    Expression<Func<T, object>> fieldSelector,
                    string searchTerms,
                    decimal boost = 1,
                    SearchOptions options = SearchOptions.Guess)
            #endregion
                ;
        }

        private class User
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public byte Age { get; set; }

            public ICollection<string> Hobbies { get; set; }
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_4
                    List<User> users = session
                        .Query<User>()
                        .Search(u => u.Name, "a*")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_4_async
                    List<User> users = await asyncSession
                        .Query<User>()
                        .Search(u => u.Name, "a*")
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_2
                    List<User> users = session
                        .Query<User, Users_ByNameAndHobbies>()
                        .Search(x => x.Name, "Adam")
                        .Search(x => x.Hobbies, "sport")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_2_async
                    List<User> users = await asyncSession
                        .Query<User, Users_ByNameAndHobbies>()
                        .Search(x => x.Name, "Adam")
                        .Search(x => x.Hobbies, "sport")
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_3
                    List<User> users = session
                        .Query<User>("Users/ByHobbies")
                        .Search(x => x.Hobbies, "I love sport", boost: 10)
                        .Search(x => x.Hobbies, "but also like reading books", boost: 5)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_3_async
                    List<User> users = await asyncSession
                        .Query<User>("Users/ByHobbies")
                        .Search(x => x.Hobbies, "I love sport", boost: 10)
                        .Search(x => x.Hobbies, "but also like reading books", boost: 5)
                        .ToListAsync();
                    #endregion
                }
            }
        }

        private class Users_ByNameAndHobbies : AbstractIndexCreationTask<User>
        {

        }
    }
}
