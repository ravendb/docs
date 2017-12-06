using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples.ClientApi.Operations
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

            #region delete_by_query1

            DeleteByQuery DeleteByQueryOperation<TEntity, TIndexCreator>(Expression<Func<TEntity, bool>> expression,
                QueryOperationOptions options = null)
                where TIndexCreator : AbstractIndexCreationTask, new();

            DeleteByQuery DeleteByQueryOperation<TEntity>(string indexName, Expression<Func<TEntity, bool>> expression,
                QueryOperationOptions options = null);

            DeleteByQuery DeleteByQueryOperation(IndexQuery queryToDelete, QueryOperationOptions options = null);

            #endregion
        }

        public DeleteByQuery()
        {

            using (var store = new DocumentStore())
            {
                store.Initialize();

                using (var session = store.OpenSession())
                {
                    #region delete_by_query2

                    // remove all documents from the server where Name == Bob using Person/ByName index
                    var operation1 =
                        store.Operations.Send(
                            new DeleteByQueryOperation<Person>("Person/ByName", x => x.Name == "Bob"));
                    operation1.WaitForCompletion(TimeSpan.FromSeconds(15));

                    //remove all documents from the server where Age > 35 using Person/ByAge index
                    var operation2 =
                        store.Operations.Send(
                            new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                    operation2.WaitForCompletion(TimeSpan.FromSeconds(15));

                    // remove all document form Pepole collection in the server 
                    var operation3 = store.Operations.Send(new DeleteByQueryOperation(new IndexQuery
                    {
                        Query = "from People"
                    }));
                    operation3.WaitForCompletion();

                    #endregion
                }

            }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                store.Initialize();

                using (var session = store.OpenSession())
                {
                    #region delete_by_query3

                    // remove all documents from the server where Name == Bob using Person/ByName index
                    var operation1 =
                        await store.Operations.SendAsync(
                            new DeleteByQueryOperation<Person>("Person/ByName", x => x.Name == "Bob"));
                    operation1.WaitForCompletion(TimeSpan.FromSeconds(15));

                    //remove all documents from the server where Age > 35 using Person/ByAge index
                    var operation2 =
                        await store.Operations.SendAsync(
                            new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                    operation2.WaitForCompletion(TimeSpan.FromSeconds(15));

                    // remove all document form Pepole collection in the server 
                    var operation3 = await store.Operations.SendAsync(new DeleteByQueryOperation(new IndexQuery
                    {
                        Query = "from People"
                    }));
                    operation3.WaitForCompletion();

                    #endregion
                }
            }
        }
    }
}
