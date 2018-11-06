using System;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{
    public class Querying
    {
        #region users_index
        private class UsersIndex : AbstractIndexCreationTask<User>
        {
            public UsersIndex()
            {
                Map = users => from user in users
                    select new
                    {
                        SchoolId = user.School.Id
                    };
            }
        }
        #endregion

        private class User
        {
            public Reference School { get; set; }
        }

        private class Reference
        {
            public string Id { get; set; }

            #region find_projected_prop_name
            public Func<Type, string, string, string, string> FindProjectedPropertyNameForIndex
            #endregion
            ;
        }

        public Querying()
        {
            {
                var store = new DocumentStore()
                {
                    Conventions =
                    {
                        #region find_prop_name
                        FindPropertyNameForIndex = (indexedType, indexName, path, prop) =>
                            (path + prop).Replace(",", "_").Replace(".", "_"),

                        FindPropertyNameForDynamicIndex = (indexedType, indexName, path, prop) => path + prop,
                        #endregion

                        #region throw_if_query_page_is_not_set
                        ThrowIfQueryPageSizeIsNotSet = true
                        #endregion
                    }
                };
            }
            {
                #region find_projected_prop_exmple1
                using (var store = new DocumentStore
                {
                    Conventions =
                    {
                        FindProjectedPropertyNameForIndex = (indexedType, indexName, path, prop) => path + prop
                    }
                })
                {
                    using (var session = store.OpenSession())
                    {
                        var query1 = session.Query<User, UsersIndex>()
                            .Select(u => u.School.Id).ToList();

                        // Generated RQL : "from index 'UsersIndex' select School.Id"

                        var query2 = session.Query<User, UsersIndex>()
                            .Where(u => u.School.Id != null).ToList();

                        // Generated RQL : "from index 'UsersIndex' where School_Id != null"

                    }
                }
                #endregion
            }
            {
                #region find_projected_prop_exmple2
                using (var store = new DocumentStore())
                {
                    using (var session = store.OpenSession())
                    {
                        // FindProjectedPropertyNameForIndex is null, FindPropertyNameForIndex will be used instead

                        var query = session.Query<User, UsersIndex>()
                            .Select(u => u.School.Id).ToList();

                        // Generated RQL : "from index 'UsersIndex' select School_Id"
                    }
                }
                #endregion
            }
        }
    }
}
