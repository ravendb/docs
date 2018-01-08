using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Documentation.Samples.Indexes.Querying;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session
{
    public class Basics
    {
        private class ProjectedUser
        {
            public string FullName { get; set; }
        }

        public Basics()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    int id = 1;

                    /*
                    #region load_1_0
                    User user = session.Load<User>(id);
                    #endregion
                    */

                    #region load_1_1
                    User user = session.Load<User>("users/" + id);
                    #endregion

                    /*
                    #region load_2_0
                    User[] users = session.Load<User>(new[] 
                                        {
                                            "users/1", "users/2" 
                                        });
                    #endregion
                    */

                    #region load_2_1
                    Dictionary<string, User> users = session.Load<User>(new[]
                                                        {
                                                            "users/1", "users/2"
                                                        });
                    #endregion

                    /*
                    #region delete_1_0
                    session.Delete<User>(id);
                    #endregion
                    */

                    #region delete_2_0
                    session.Delete(user);
                    session.Delete("users/" + id);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    /*
                    #region load_3_0
                    var users = session.Load<Transformer, ProjectedUser>("users/1");
                    #endregion
                    */

                    #region load_3_1
                    var users = session.Query<User>().Where(x => x.Id == "users/1")
                                                     .Select(x => new ProjectedUser
                                                     {
                                                         FullName = x.Name,
                                                     }).First();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    /*
                    #region delete_by_index_1_7
                    Operation operation = 
                        session
                            .Advanced
                            .DeleteByIndex<Person, Person_ByAge>(x => x.Age < 35);
                    #endregion
                    */
                }

                #region delete_by_index_1_8
                Operation operation =
                    store
                        .Operations
                        .Send(new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                #endregion
            }
        }

        private class Person
        {
            public int Age { get; set; }
        }

        private class Person_ByAge : AbstractIndexCreationTask<Person>
        {

        }
    }
}
