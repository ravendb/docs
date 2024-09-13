using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Client.Http;
using Raven.Client.Json;
using Raven.Client.ServerWide;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class CompareExchange
    {
        private interface IFoo
        {
            /*
            #region get_0
            public GetCompareExchangeValueOperation(string key)
            #endregion

            #region get_list_0
            public GetCompareExchangeValuesOperation(string[] keys)
            #endregion

            #region get_list_1
            public GetCompareExchangeValuesOperation(string startWith, int? start = null, int? pageSize = null)
            #endregion

            #region put_0
            public PutCompareExchangeValueOperation(string key, T value, long index)
            #endregion

            #region delete_0
            public DeleteCompareExchangeValueOperation(string key, long index)
            #endregion
            */
        }
        private class Foo
        {
            #region compare_exchange_value
            public class CompareExchangeValue<T>
            {
                public readonly string Key;
                public readonly T Value;
                public readonly long Index;
            }
            #endregion

            #region compare_exchange_result
            public class CompareExchangeResult<T>
            {
                public bool Successful;
                public T Value;
                public long Index;
            }
            #endregion
        }

        public CompareExchange()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_1
                    CompareExchangeValue<long> readResult =
                        store.Operations.Send(new GetCompareExchangeValueOperation<long>("NextClientId"));

                    long value = readResult.Value;
                    #endregion
                }

                {
                    #region get_2
                    CompareExchangeValue<User> readResult =
                        store.Operations.Send(new GetCompareExchangeValueOperation<User>("AdminUser"));

                    User admin = readResult.Value;
                    #endregion
                }

                {
                    #region get_list_2
                    Dictionary<string, CompareExchangeValue<string>> compareExchangeValues
                        = store.Operations.Send(
                            new GetCompareExchangeValuesOperation<string>(new[] { "Key-1", "Key-2" }));
                    #endregion
                }

                {
                    #region get_list_3
                    // Get values for keys that have the common prefix 'users'
                    // Retrieve maximum 20 entries
                    Dictionary<string, CompareExchangeValue<User>> compareExchangeValues
                        = store.Operations.Send(new GetCompareExchangeValuesOperation<User>("users", 0, 20));
                    #endregion
                }

                {
                    #region put_1 
                    CompareExchangeResult<string> compareExchangeResult
                        = store.Operations.Send(
                            new PutCompareExchangeValueOperation<string>("Emails/foo@example.org", "users/123", 0));

                    bool successful = compareExchangeResult.Successful;
                    // If successfull is true: then Key 'foo@example.org' now has the value of "users/123"
                    #endregion
                }

                {
                    #region put_2
                    // Get existing value
                    CompareExchangeValue<User> readResult =
                        store.Operations.Send(
                            new GetCompareExchangeValueOperation<User>("AdminUser"));

                    readResult.Value.Age++;

                    // Update value
                    CompareExchangeResult<User> saveResult
                        = store.Operations.Send(
                            new PutCompareExchangeValueOperation<User>("AdminUser", readResult.Value, readResult.Index));

                    // The save result is successful only if 'index' wasn't changed between the read and write operations
                    bool saveResultSuccessful = saveResult.Successful;
                    #endregion
                }

                {
                    #region put_3
                    // Creating the item will fail because the index is not 0
                    CompareExchangeResult<string> compareExchangeResult
                        = store.Operations.Send(
                            new PutCompareExchangeValueOperation<string>("key", "value", 123));

                    // Creating the item will succeed because the index is 0
                    compareExchangeResult = store.Operations.Send(
                            new PutCompareExchangeValueOperation<string>("key", "value", 0));
                    #endregion
                }

                {
                    #region delete_1
                    // First, get existing value
                    CompareExchangeValue<User> readResult =
                        store.Operations.Send(
                            new GetCompareExchangeValueOperation<User>("AdminUser"));

                    // Delete the key - use the index received from the 'Get' operation
                    CompareExchangeResult<User> deleteResult
                        = store.Operations.Send(
                            new DeleteCompareExchangeValueOperation<User>("AdminUser", readResult.Index));

                    // The delete result is successful only if the index has not changed between the read and delete operations
                    bool deleteResultSuccessful = deleteResult.Successful;
                    #endregion
                }

                #region expiration_0
                using (IAsyncDocumentSession session = store.OpenAsyncSession())
                {
                    // Set a time exactly one week from now
                    DateTime expirationTime = DateTime.UtcNow.AddDays(7);

                    // Create a new compare exchange value
                    var cmpxchgValue = session.Advanced.ClusterTransaction.CreateCompareExchangeValue("key", "value");

                    // Edit Metadata
                    cmpxchgValue.Metadata[Constants.Documents.Metadata.Expires] = expirationTime;

                    // Send to server
                    session.SaveChangesAsync();
                }
                #endregion

                using (IAsyncDocumentSession session = store.OpenAsyncSession())
                {
                    #region expiration_1
                    // Retrieve an existing key
                    CompareExchangeValue<string> cmpxchgValue = store.Operations.Send(
                            new GetCompareExchangeValueOperation<string>("key"));
                
                    // Set time
                    DateTime expirationTime = DateTime.UtcNow.AddDays(7);
                    
                    // Edit Metadata
                    cmpxchgValue.Metadata[Constants.Documents.Metadata.Expires] = expirationTime;
                
                    // Update value. Index must match the index on the server side,
                    // or the operation will fail.
                    CompareExchangeResult<string> result = store.Operations.Send(
                            new PutCompareExchangeValueOperation<string>(
                                cmpxchgValue.Key,
                                cmpxchgValue.Value,
                                cmpxchgValue.Index));
                    #endregion
                }

                #region metadata_0
                using (IAsyncDocumentSession session = store.OpenAsyncSession(
                                                       new SessionOptions { 
                                                           TransactionMode = TransactionMode.ClusterWide 
                                                       }))
                {
                    // Create a new compare exchange value
                    var cmpxchgValue = session.Advanced.ClusterTransaction.CreateCompareExchangeValue("key", "value");

                    // Add a field to the metadata
                    // with a value of type string
                    cmpxchgValue.Metadata["Field name"] = "some value";

                    // Retrieve metadata as a dictionary
                    IDictionary<string, object> cmpxchgMetadata = cmpxchgValue.Metadata;
                }
                #endregion


            }
        }

        public async Task AsyncExample()
        {
            using (var documentStore = new DocumentStore())
            {
                #region metadata_1
                // Create some metadata
                var cmpxchgMetadata = new MetadataAsDictionary {
                                          ["Year"] = "2020" 
                                      };

                // Create/update the compare exchange value "best"
                await documentStore.Operations.SendAsync(
                    new PutCompareExchangeValueOperation<User>(
                        "best", 
                        new User { Name = "Alice" }, 
                        0, 
                        cmpxchgMetadata));
                #endregion

                // -Include examples-

                #region include_load
                using (IDocumentSession session = documentStore.OpenSession())
                {
                    // Load a user document, include the compare exchange value
                    // with the key equal to the user's Name field
                    var user = session.Load<User>("users/1-A", 
                                                  includes => includes.IncludeCompareExchangeValue(
                                                      x => x.Name));

                    // Getting the compare exchange value does not require
                    // another call to the server
                    var value = session.Advanced.ClusterTransaction
                                                .GetCompareExchangeValue<string>(user.Name);
                }
                #endregion

                #region include_load_async
                using (IAsyncDocumentSession session = documentStore.OpenAsyncSession())
                {
                    // Load a user document, include the compare exchange value
                    // with the key equal to the user's Name field
                    var user = await session.LoadAsync<User>("users/1-A", 
                                                             includes => includes.IncludeCompareExchangeValue(
                                                                 x => x.Name));

                    // Getting the compare exchange value does not require
                    // another call to the server
                    var value = await session.Advanced.ClusterTransaction
                                                      .GetCompareExchangeValueAsync<string>(user.Name);
                }
                #endregion

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    #region include_linq_query
                    var users = session.Query<User>()
                        .Include(builder => builder.IncludeCompareExchangeValue(x => x.Name))
                        .ToList();

                    List<CompareExchangeValue<string>> compareExchangeValues = null;

                    // Getting the compare exchange values does not require
                    // additional calls to the server
                    foreach (User u in users){
                        compareExchangeValues.Add(session.Advanced.ClusterTransaction
                                                      .GetCompareExchangeValue<string>(u.Name));
                    }
                    #endregion
                }

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    #region include_raw_query
                    // First method: from body of query
                    var users1 = session.Advanced
                        .RawQuery<User>(@"
                            from Users as u
                            select u
                            include cmpxchg(u.Name)"
                        )
                        .ToList();

                    List<CompareExchangeValue<string>> compareExchangeValues1 = null;

                    // Getting the compare exchange values does not require
                    // additional calls to the server
                    foreach (User u in users1){
                        compareExchangeValues1.Add(session.Advanced.ClusterTransaction
                                                   .GetCompareExchangeValue<string>(u.Name));
                    }


                    // Second method: from a JavaScript function
                    var users2 = session.Advanced
                        .RawQuery<User>(@"
                            declare function includeCEV(user) {
                                includes.cmpxchg(user.Name);
                                return user;
                            }

                            from Users as u
                            select includeCEV(u)"
                        )
                        .ToList();
                    // Note that includeCEV() returns the same User 
                    // entity it received, without modifying it

                    List<CompareExchangeValue<string>> compareExchangeValues2 = null;

                    // Does not require calls to the server
                    foreach (User u in users2){
                        compareExchangeValues2.Add(session.Advanced.ClusterTransaction
                                                   .GetCompareExchangeValue<string>(u.Name));
                    }
                    #endregion
                }
            }
        }

        private class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        #region include_builder
        public interface ICompareExchangeValueIncludeBuilder<T, out TBuilder>
        {
            TBuilder IncludeCompareExchangeValue(string path);

            TBuilder IncludeCompareExchangeValue(Expression<Func<T, string>> path);

            TBuilder IncludeCompareExchangeValue(Expression<Func<T, IEnumerable<string>>> path);
        }
        #endregion
    }
}
