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
                using (var store = new DocumentStore
                {
                    #region find_projected_prop_usage
                    Conventions =
                    {
                        FindProjectedPropertyNameForIndex = (indexedType, indexName, path, prop) => path + prop
                    }
                    #endregion
                })
                {
                    using (var session = store.OpenSession())
                    {
                        #region find_projected_prop_query
                        var query = session.Query<User, UsersIndex>()
                            .Where(u => u.School.Id != null)
                            .Select(u => u.School.Id)
                            .ToList();
                        #endregion
                    }
                }
            }
        }
    }
}
