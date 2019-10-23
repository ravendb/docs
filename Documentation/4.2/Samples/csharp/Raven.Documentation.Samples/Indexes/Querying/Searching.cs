using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Searching
    {
        #region search_20_2
        public class Users_ByName : AbstractIndexCreationTask<User>
        {
            public Users_ByName()
            {
                Map = users => from user in users
                               select new
                               {
                                   Name = user.Name
                               };

                Index(x => x.Name, FieldIndexing.Search);
            }
        }
        #endregion

        #region search_21_2
        public class Users_Search : AbstractIndexCreationTask<User, Users_Search.Result>
        {
            public class Result
            {
                public string Query;
            }

            public Users_Search()
            {
                Map = users => from user in users
                               select new
                               {
                                   Query = new object[]
                                   {
                                        user.Name,
                                        user.Hobbies,
                                        user.Age
                                   }
                               };

                Index("Query", FieldIndexing.Search);
            }
        }
        #endregion

        #region linq_extensions_search_user_class
        public class User
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public byte Age { get; set; }

            public ICollection<string> Hobbies { get; set; }
        }
        #endregion

        public Searching()
        {

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_3_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "John Steve")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_3_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Name, "John Steve")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_4_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Hobbies, "looking for someone who likes sport books computers")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_4_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Hobbies, "looking for someone who likes sport books computers")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_5_0
                    List<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "Steve")
                        .Search(x => x.Hobbies, "sport")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_5_1
                    List<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Name, "Steve")
                        .Search(x => x.Hobbies, "sport")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_6_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Hobbies, "I love sport", boost: 10)
                        .Search(x => x.Hobbies, "but also like reading books", boost: 5)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_6_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Hobbies, "I love sport")
                        .Boost(10)
                        .Search(x => x.Hobbies, "but also like reading books")
                        .Boost(5)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_7_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Hobbies, "computers")
                        .Search(x => x.Name, "James")
                        .Where(x => x.Age == 20)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_7_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Hobbies, "computers")
                        .Search(x => x.Name, "James")
                        .WhereEquals(x => x.Age, 20)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_8_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "Steve")
                        .Search(x => x.Hobbies, "sport", options: SearchOptions.And)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_8_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Name, "Steve")
                        .AndAlso()
                        .Search(x => x.Hobbies, "sport")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_9_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "James", options: SearchOptions.Not)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_9_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Not
                        .Search(x => x.Name, "James")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_10_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "Steve")
                        .Search(x => x.Hobbies, "sport", options: SearchOptions.Not | SearchOptions.And)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_10_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search(x => x.Name, "Steve")
                        .AndAlso()
                        .Not
                        .Search(x => x.Hobbies, "sport")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_11_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "Jo* Ad*")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_11_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search("Name", "Jo* Ad*")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_12_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "*oh* *da*")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_12_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search("Name", "*oh* *da*")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_20_0
                    IList<User> users = session
                        .Query<User, Users_ByName>()
                        .Search(x => x.Name, "John")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_20_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User, Users_ByName>()
                        .Search("Name", "John")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_21_0
                    var users = session
                        .Query<Users_Search.Result, Users_Search>()
                        .Search(x => x.Query, "John")
                        .As<User>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_21_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User, Users_Search>()
                        .Search("Query", "John")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_22_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "John Steve", @operator: SearchOperator.Or)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_22_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search("Name", "John Steve", @operator: SearchOperator.Or)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_22_2
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Name, "John Steve", @operator: SearchOperator.And)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region search_22_3
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .Search("Name", "John Steve", @operator: SearchOperator.And)

                        .ToList();
                    #endregion
                }
            }
        }
    }
}
