using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples.ClientApi.Operations.Common
{
    public class Person
    {
        public string Name;
        public int Age;
    }

    public class Person_ByAge : AbstractIndexCreationTask<Person>
    {
        public Person_ByAge()
        {
            Map = persons => from person in persons
                             select new
                             {
                                 Age = person.Age
                             };
        }
    }

    public class DeleteByQuery
    {
        private interface IFoo
        {
            #region delete_by_query

            DeleteByQueryOperation DeleteByQueryOperation<TEntity, TIndexCreator>(
                Expression<Func<TEntity, bool>> expression,
                QueryOperationOptions options = null)
                where TIndexCreator : AbstractIndexCreationTask, new();

            DeleteByQueryOperation DeleteByQueryOperation<TEntity>(
                string indexName, Expression<Func<TEntity, bool>> expression,
                QueryOperationOptions options = null);

            DeleteByQueryOperation DeleteByQueryOperation(
                IndexQuery queryToDelete, QueryOperationOptions options = null);

            #endregion
        }

        public DeleteByQuery()
        {
            using (var store = new DocumentStore())
            {
                #region delete_by_query1
                // remove all documents from the server where Name == Bob using Person/ByName index
                var operation = store
                    .Operations
                    .Send(new DeleteByQueryOperation<Person>("Person/ByName", x => x.Name == "Bob"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query2
                // remove all documents from the server where Age > 35 using Person/ByAge index
                var operation = store
                    .Operations
                    .Send(new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query3
                // delete multiple docs with specific ids in a single run without loading them into the session
                var operation = store
                    .Operations
                    .Send(new DeleteByQueryOperation(new IndexQuery
                    {
                        Query = "from People u where id(u) in ('people/1-A', 'people/3-A')"
                    }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query_wait_for_completion
                // remove all document from server where Name == Bob and Age >= 29 using People collection
                var operation = store
                    .Operations
                    .Send(new DeleteByQueryOperation(new IndexQuery
                    {
                        Query = "from People where Name = 'Bob' and Age >= 29"
                    }));

                operation.WaitForCompletion(TimeSpan.FromSeconds(15));
                #endregion
            }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region delete_by_query1_async

                // remove all documents from the server where Name == Bob using Person/ByName index
                var operation = await store
                    .Operations
                    .SendAsync(new DeleteByQueryOperation<Person>("Person/ByName", x => x.Name == "Bob"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query2_async

                //remove all documents from the server where Age > 35 using Person/ByAge index
                var operation = await store
                    .Operations
                    .SendAsync(new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query3_async

                // delete multiple docs with specific ids in a single run without loading them into the session
                var operation = await store
                    .Operations
                    .SendAsync(new DeleteByQueryOperation(new IndexQuery
                    {
                        Query = "from People u where id(u) in ('people/1-A', 'people/3-A')"
                    }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_by_query_wait_for_completion_async

                // remove all document from server where Name == Bob and Age >= 29 using People collection
                var operation = await store
                    .Operations
                    .SendAsync(new DeleteByQueryOperation(new IndexQuery
                    {
                        Query = "from People where Name = 'Bob' and Age >= 29"
                    }));

                await operation.WaitForCompletionAsync(TimeSpan.FromSeconds(15));

                #endregion
            }
        }
    }
}
