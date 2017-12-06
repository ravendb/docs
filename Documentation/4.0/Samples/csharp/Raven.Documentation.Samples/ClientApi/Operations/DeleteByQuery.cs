﻿using System;
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

            #region delete_by_query

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

                #region delete_by_query1

                // remove all documents from the server where Name == Bob using Person/ByName index
                var operation1 =
                    store.Operations.Send(
                        new DeleteByQueryOperation<Person>("Person/ByName", x => x.Name == "Bob"));
                #endregion

                #region delete_by_query2

                //remove all documents from the server where Age > 35 using Person/ByAge index
                var operation2 =
                    store.Operations.Send(
                        new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                #endregion

                #region delete_by_query3

                // remove all document from People collection in the server 
                var operation3 = store.Operations.Send(new DeleteByQueryOperation(new IndexQuery
                {
                    Query = "from People"
                }));
                #endregion


                #region delete_by_query_wait_for_completion

                // remove all document from server where Name == Bob and Age >= 29 using People collection
                var operation4 = store.Operations.Send(new DeleteByQueryOperation(new IndexQuery
                {
                    Query = "from People where Name = 'Bob' and Age >= 29"
                }));

                operation4.WaitForCompletion(TimeSpan.FromSeconds(15));

                #endregion
            }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                store.Initialize();

                #region delete_by_query1_async

                // remove all documents from the server where Name == Bob using Person/ByName index
                var operation1 =
                    await store.Operations.SendAsync(
                        new DeleteByQueryOperation<Person>("Person/ByName", x => x.Name == "Bob"));
                #endregion

                #region delete_by_query2_async

                //remove all documents from the server where Age > 35 using Person/ByAge index
                var operation2 =
                    await store.Operations.SendAsync(
                        new DeleteByQueryOperation<Person, Person_ByAge>(x => x.Age < 35));
                #endregion

                #region delete_by_query3_async

                // remove all document from People collection in the server 
                var operation3 = await store.Operations.SendAsync(new DeleteByQueryOperation(new IndexQuery
                {
                    Query = "from People"
                }));

                #endregion

                #region delete_by_query_wait_for_completion_async

                // remove all document from server where Name == Bob and Age >= 29 using People collection
                var operation4 = await store.Operations.SendAsync(new DeleteByQueryOperation(new IndexQuery
                {
                    Query = "from People where Name = 'Bob' and Age >= 29"
                }));

                await operation4.WaitForCompletionAsync(TimeSpan.FromSeconds(15));

                #endregion
            }
        }
    }
}
