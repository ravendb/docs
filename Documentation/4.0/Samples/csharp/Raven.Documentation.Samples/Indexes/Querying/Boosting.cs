using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Boosting
    {
        public class User
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public byte Age { get; set; }

            public ICollection<string> Hobbies { get; set; }
        }

        public Boosting()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region boosting_1_0
                    IList<User> users = session
                        .Query<User>()
                        .Search(x => x.Hobbies, "I love sport", boost: 10)
                        .Search(x => x.Hobbies, "but also like reading books", boost: 5)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region boosting_1_1
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

                using (var session = store.OpenSession())
                {
                    #region boosting_2_1
                    IList<User> users = session
                        .Advanced
                        .DocumentQuery<User>()
                        .WhereStartsWith(x => x.Name, "G")
                        .Boost(10)
                        .WhereStartsWith(x => x.Name, "A")
                        .Boost(5)
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
