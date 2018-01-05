using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Indexes.Querying;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Basics
    {
        public Basics()
        {
            using (var store = new DocumentStore())
            {
                #region basics_1_2
                store.Conventions.ThrowIfQueryPageSizeIsNotSet = true;
                #endregion

                using (var session = store.OpenSession())
                {
                    #region basics_1_0
                    List<Employee> employees = session
                        .Query<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_1_1
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Take(128)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_1_3
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }

                /*
                #region basics_1_5
                var tagName = session
                                .Advanced
                                .DocumentStore
                                .Conventions
                                .GetTypeTagName(typeof(User));

                session.Advanced.DocumentStore.DatabaseCommands
                    .DeleteByIndex(Constants.DocumentsByEntityNameIndex, 
                        new IndexQuery()
                        {
                            Query = $"Tag:{ tagName }"
                        });
                #endregion
                */

                using (var session = store.OpenSession())
                {
                    #region basics_1_6
                    var operation = store.Operations.Send(
                        new DeleteByQueryOperation(new IndexQuery()
                        {
                            Query = session.Query<User>().ToString() // "from Users"
                        }));
                    #endregion
                }
            }
        }
    }
}
