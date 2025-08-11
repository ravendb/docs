using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Raven.Documentation.Samples;

namespace Raven.Documentation.Samples.ClientApi.Operations.Patches
{
    public class PatchRequests
    {
        private interface IFoo
        {
            #region patch_generic_interface_increment
            void Increment<T, U>(T entity, Expression<Func<T, U>> fieldPath, U delta);

            void Increment<T, U>(string id, Expression<Func<T, U>> fieldPath, U delta);
            #endregion

            #region patch_generic_interface_set_value

            void Patch<T, U>(string id, Expression<Func<T, U>> fieldPath, U value);

            void Patch<T, U>(T entity, Expression<Func<T, U>> fieldPath, U value);
            #endregion

            #region patch_generic_interface_array_modification_lambda
            void Patch<T, U>(T entity, Expression<Func<T, IEnumerable<U>>> fieldPath,
                Expression<Func<JavaScriptArray<U>, object>> arrayModificationLambda);

            void Patch<T, U>(string id, Expression<Func<T, IEnumerable<U>>> fieldPath,
                Expression<Func<JavaScriptArray<U>, object>> arrayModificationLambda);
            #endregion

            #region add_or_patch_generic
            void AddOrPatch<T, TU>(string id, T entity, Expression<Func<T, TU>> path, TU value);
            #endregion

            #region add_or_patch_array_generic
            void AddOrPatch<T, TU>(string id, T entity, Expression<Func<T, List<TU>>> path, 
                Expression<Func<JavaScriptArray<TU>, object>> arrayAdder);
            #endregion

            #region add_or_increment_generic
            void AddOrIncrement<T, TU>(string id, T entity, Expression<Func<T, TU>> path, TU valToAdd);
            #endregion

            #region patch_non_generic_interface_in_session
            void Defer(ICommandData[] commands);
            #endregion

            #region patch_non_generic_interface_in_store            
            PatchStatus Send(PatchOperation operation);

            Task<PatchStatus> SendAsync(PatchOperation operation, 
                                        SessionInfo sessionInfo = null, 
                                        CancellationToken token = default(CancellationToken));

            #endregion
        }

        public PatchRequests()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region patch_firstName_generic
                // change FirstName to Robert                
                session.Advanced.Patch<Employee, string>(
                    "employees/1",
                    x => x.FirstName, "Robert");

                session.SaveChanges();
                #endregion

                #region patch_firstName_non_generic_session
                // change FirstName to Robert                
                session.Advanced.Defer(new PatchCommandData(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.FirstName = args.FirstName;",
                        Values =
                        {
                            {"FirstName", "Robert"}
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region patch_firstName_non_generic_store
                // change FirstName to Robert                
                store.Operations.Send(new PatchOperation(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.FirstName = args.FirstName;",
                        Values =
                        {
                            {"FirstName", "Robert"}
                        }
                    },
                    patchIfMissing: null));
                #endregion

                #region patch_firstName_and_lastName_generic
                // Modify FirstName to Robert and LastName to Carter in single request
                // ===================================================================
                
                // The two Patch operations below are sent via 'SaveChanges()' which complete transactionally,
                // as this call generates a single HTTP request to the database.
                // Either both will succeed or both will be rolled back since they are applied within the same transaction.
                // However, on the server side, the two Patch operations are still executed separately.
                // To achieve atomicity at the level of a single server-side operation, use 'Defer' or the operations syntax.
                
                session.Advanced.Patch<Employee, string>("employees/1", x => x.FirstName, "Robert");
                session.Advanced.Patch<Employee, string>("employees/1", x => x.LastName, "Carter");

                session.SaveChanges();
                #endregion

                #region pathc_firstName_and_lastName_non_generic_session
                // Change FirstName to Robert and LastName to Carter in single request
                // Note that here we do maintain the atomicity of the operation
                session.Advanced.Defer(new PatchCommandData(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"
                                this.FirstName = args.UserName.FirstName;
                                this.LastName = args.UserName.LastName;",
                        Values =
                        {
                            {
                                "UserName", new
                                {
                                    FirstName = "Robert",
                                    LastName = "Carter"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region pathc_firstName_and_lastName_store
                // Change FirstName to Robert and LastName to Carter in single request
                // Note that here we do maintain the atomicity of the operation
                store.Operations.Send(new PatchOperation(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"
                                this.FirstName = args.UserName.FirstName;
                                this.LastName = args.UserName.LastName;",
                        Values =
                        {
                            {
                                "UserName", new
                                {
                                    FirstName = "Robert",
                                    LastName = "Carter"
                                }
                            }
                        }
                    }, patchIfMissing: null));
                #endregion

                #region increment_age_generic
                // increment UnitsInStock property value by 10
                session.Advanced.Increment<Product, int>("products/1-A", x => x.UnitsInStock, 10);

                session.SaveChanges();
                #endregion

                #region increment_age_non_generic_session
                session.Advanced.Defer(new PatchCommandData(
                    id: "products/1-A",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.UnitsInStock += args.UnitsToAdd;",
                        Values =
                        {
                            {"UnitsToAdd", 10}
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region increment_age_non_generic_store

                store.Operations.Send(new PatchOperation(
                    id: "products/1-A",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.UnitsInStock += args.UnitsToAdd;",
                        Values =
                        {
                            {"UnitsToAdd", 10}
                        }
                    },
                    patchIfMissing: null));
                #endregion

                #region remove_property_age_non_generic_session
                // remove property Age
                session.Advanced.Defer(new PatchCommandData(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"delete this.Age;"
                    },
                    patchIfMissing: null));
                session.SaveChanges();
                #endregion

                #region remove_property_age_store
                // remove property Age
                store.Operations.Send(new PatchOperation(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"delete this.Age;"
                    },
                    patchIfMissing: null));
                #endregion

                #region rename_property_age_non_generic_session
                // rename FirstName to Name
                session.Advanced.Defer(new PatchCommandData(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"var firstName = this[args.Rename.Old];
                                delete this[args.Rename.Old];
                                this[args.Rename.New] = firstName;",
                        Values =
                        {
                            {
                                "Rename", new
                                {
                                    Old = "FirstName",
                                    New = "Name"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region rename_property_age_store

                store.Operations.Send(new PatchOperation(
                    id: "employees/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"var firstName = this[args.Rename.Old];
                                delete this[args.Rename.Old];
                                this[args.Rename.New] = firstName;",
                        Values =
                        {
                            {
                                "Rename", new
                                {
                                    Old = "FirstName",
                                    New = "Name"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));
                #endregion

                #region add_new_comment_to_comments_generic_session
                // add a new comment to Comments
                session.Advanced.Patch<BlogPost, BlogComment>("blogposts/1",
                    x => x.Comments,
                    comments => comments.Add(new BlogComment
                    {
                        Content = "Lore ipsum",
                        Title = "Some title"
                    }));

                session.SaveChanges();
                #endregion

                #region add_new_comment_to_comments_non_generic_session
                // add a new comment to Comments
                session.Advanced.Defer(new PatchCommandData(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Comments.push(args.Comment);",
                        Values =
                        {
                            {
                                "Comment", new BlogComment
                                {
                                    Content = "Lore ipsum",
                                    Title = "Some title"
                                }
                            }
                        }

                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion 

                #region add_new_comment_to_comments_store
                // add a new comment to Comments
                store.Operations.Send(new PatchOperation(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Comments.push(args.Comment);",
                        Values =
                        {
                            {
                                "Comment", new BlogComment
                                {
                                    Content = "Lore ipsum",
                                    Title = "Some title"
                                }
                            }
                        }

                    },
                    patchIfMissing: null));
                #endregion

                #region insert_new_comment_at_position_1_session
                // insert a new comment at position 1 to Comments
                session.Advanced.Defer(new PatchCommandData(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Comments.splice(1, 0, args.Comment);",
                        Values =
                        {
                            {
                                "Comment", new BlogComment
                                {
                                    Content = "Lore ipsum",
                                    Title = "Some title"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region insert_new_comment_at_position_1_store
                store.Operations.Send(new PatchOperation(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Comments.splice(1, 0, args.Comment);",
                        Values =
                        {
                            {
                                "Comment", new BlogComment
                                {
                                    Content = "Lore ipsum",
                                    Title = "Some title"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));
                #endregion

                #region modify_a_comment_at_position_3_in_comments_session
                // modify a comment at position 3 in Comments
                session.Advanced.Defer(new PatchCommandData(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Comments.splice(3, 1, args.Comment);",
                        Values =
                        {
                            {
                                "Comment", new BlogComment
                                {
                                    Content = "Lore ipsum",
                                    Title = "Some title"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region modify_a_comment_at_position_3_in_comments_store
                // modify a comment at position 3 in Comments
                store.Operations.Send(new PatchOperation(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = "this.Comments.splice(3, 1, args.Comment);",
                        Values =
                        {
                            {
                                "Comment", new BlogComment
                                {
                                    Content = "Lore ipsum",
                                    Title = "Some title"
                                }
                            }
                        }
                    },
                    patchIfMissing: null));
                #endregion

                #region filter_items_from_array_session
                // filter out all comments of a blogpost which contains the word "wrong" in their contents 
                session.Advanced.Defer(new PatchCommandData(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.Comments = this.Comments.filter(comment=>
                                 !comment.Content.includes(args.Text));",
                        Values =
                        {
                            {"Text", "wrong"}
                        }
                    },
                    patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region filter_items_from_array_session_generic
                // filter out all comments of a blogpost which contains the word "wrong" in their contents 
                session.Advanced.Patch<BlogPost, BlogComment>("blogposts/1",
                    x => x.Comments,
                    comments => comments.RemoveAll(y => y.Content.Contains("wrong")));

                session.SaveChanges();
                #endregion

                #region filter_items_from_array_store
                // filter out all comments of a blogpost which contains the word "wrong" in their contents
                store.Operations.Send(new PatchOperation(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.Comments = this.Comments.filter(comment=>
                                 !comment.Content.includes(args.Text));",
                        Values =
                        {
                            {"Text", "wrong"}
                        }
                    },
                    patchIfMissing: null));
                #endregion

                #region update_product_name_in_order_session
                // update product names in order, according to loaded product documents
                session.Advanced.Defer(new PatchCommandData(
                    id: "orders/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.Lines.forEach(line=> { 
                                var productDoc = load(line.Product);
                                line.ProductName = productDoc.Name;
                                });"
                    }, patchIfMissing: null));

                session.SaveChanges();
                #endregion

                #region update_product_name_in_order_store
                // update product names in order, according to loaded product documents
                store.Operations.Send(new PatchOperation(
                    id: "blogposts/1",
                    changeVector: null,
                    patch: new PatchRequest
                    {
                        Script = @"this.Lines.forEach(line=> { 
                                var productDoc = load(line.Product);
                                line.ProductName = productDoc.Name;
                                });"
                    },
                    patchIfMissing: null));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region update_value_in_whole_collection
                // increase by 10 Freight field in all orders
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(@"from Orders as o
                                                      update
                                                      {
                                                          o.Freight +=10;
                                                      }"));
                // Wait for the operation to be complete on the server side.
                // Not waiting for completion will not harm the patch process and it will continue running to completion.
                operation.WaitForCompletion();
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region update-value-by-dynamic-query
                // set discount to all orders that was processed by a specific employee
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(@"from Orders as o
                                                      where o.Employee = 'employees/4-A'
                                                      update
                                                      {
                                                          o.Lines.forEach(line=> line.Discount = 0.3);
                                                      }"));
                operation.WaitForCompletion();

                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region update-value-by-index-query
                // switch all products with supplier 'suppliers/12-A' with 'suppliers/13-A'
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from index 'Product/Search' as p
                                  where p.Supplier = 'suppliers/12-A'
                                  update
                                  {
                                      p.Supplier = 'suppliers/13-A'
                                  }"
                    }));

                operation.WaitForCompletion();
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region update-on-stale-results
                // patch on stale results
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from Orders as o
                                  where o.Company = 'companies/12-A'
                                  update
                                  {
                                      o.Company = 'companies/13-A'
                                  }"
                    },
                    new QueryOperationOptions
                    {
                        AllowStale = true
                    }));

                operation.WaitForCompletion();
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region report_progress_on_patch
                // report progress during patch processing
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from Orders as o
                                  where o.Company = 'companies/12-A'
                                  update
                                  {
                                      o.Company = 'companies/13-A'
                                  }"
                    },
                    new QueryOperationOptions
                    {
                        AllowStale = true
                    }));
                
                operation.OnProgressChanged += (sender, x) =>
                {
                    var det = (DeterminateProgress)x;
                    Console.WriteLine($"Processed: {det.Processed}; Total: {det.Total}");
                };

                operation.WaitForCompletion();
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region patch-request-with-details     
                // perform patch and create summary of processing statuses
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from Orders as o
                                  where o.Company = 'companies/12-A'
                                  update
                                  {
                                      o.Company = 'companies/13-A'
                                  }"
                    },
                    new QueryOperationOptions
                    {
                        RetrieveDetails = true
                    }));

                var result = operation.WaitForCompletion<BulkOperationResult>();
                var formattedResults =
                    result.Details
                    .Select(x => (BulkOperationResult.PatchDetails)x)
                    .GroupBy(x => x.Status)
                    .Select(x => $"{x.Key}: {x.Count()}").ToList();

                formattedResults.ForEach(Console.WriteLine);
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region change-collection-name   

                // delete the document before recreating it with a different collection name
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from Orders as c
                                  update
                                  {
                                      del(id(c));
                                      this[""@metadata""][""@collection""] = ""New_Orders"";
                                      put(id(c), this);
                                  }"
                    }));

                operation.WaitForCompletion();

                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region change-all-documents   

                // perform a patch on all documents using @all_docs keyword
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from @all_docs
                                  update
                                  {
                                      this.Updated = true;
                                  }"
                    }));

                operation.WaitForCompletion();

                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region patch-by-id

                // perform a patch by document ID
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        Query = @"from @all_docs as d
                                  where id() in ('orders/1-A', 'companies/1-A')
                                  update
                                  {
                                      d.Updated = true;
                                  }"
                    }));

                operation.WaitForCompletion();

                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region patch-by-id-using-parameters 

                // perform a patch by document ID
                var operation = store
                    .Operations
                    .Send(new PatchByQueryOperation(new IndexQuery
                    {
                        QueryParameters = new Parameters
                        {
                            {"ids", new[] {"orders/1-A", "companies/1-A"}}
                        },
                        Query = @"from @all_docs as d
                                  where id() in ($ids)
                                  update
                                  {
                                      d.Updated = true;
                                  }"
                    }));

                operation.WaitForCompletion();

                #endregion
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region add_document_session
                    session.Advanced.Defer(new PatchCommandData("employees/1-A", null,
                        new PatchRequest
                        {
                            Script = "put('orders/', { Employee: id(this) });",
                        }, null));

                    session.SaveChanges();
                    #endregion

                    #region clone_document_session
                    session.Advanced.Defer(new PatchCommandData("employees/1-A", null,
                        new PatchRequest
                        {
                            Script = "put('employees/', this);",
                        }, null));

                    session.SaveChanges();
                    #endregion
                }

                #region add_document_store
                store.Operations.Send(new PatchOperation("employees/1-A", null, new PatchRequest
                {
                    Script = "put('orders/', { Employee: id(this) });",
                }));
                #endregion

                #region clone_document_store
                store.Operations.Send(new PatchOperation("employees/1-A", null, new PatchRequest
                {
                    Script = "put('employees/', this);",
                }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    var id = "users/1-A";
                    #region Add_Or_Patch_Sample
                    // While running AddOrPatch specify <entity type, field type>
                    session.Advanced.AddOrPatch<User, DateTime>(

                    // Specify document id and entity on which the operation should be performed.
                        id,
                        new User
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            LastLogin = DateTime.Now
                        },
                        // The path to the field and value to set.
                        x => x.LastLogin, new DateTime(2021, 9, 12));

                    session.SaveChanges();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {   
                var id = "users/1-A";
                #region Add_Or_Patch_Array_Sample
                // While running AddOrPatch specify <entity type, field type>
                session.Advanced.AddOrPatch<User, DateTime>(

                    // Specify document id and entity on which the operation should be performed.
                    id,
                    new User
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        LoginTimes =
                        new List<DateTime>
                        {
                                DateTime.UtcNow
                        }
                    },
                    // The path to the field
                    x => x.LoginTimes,
                    // Modifies the array
                    u => u.Add(new DateTime(1993, 09, 12), new DateTime(2000, 01, 01)));

                session.SaveChanges();
                #endregion
            }


            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            { 
                var id = "users/1-A";

            #region Add_Or_Increment_Sample
            // While running AddOrIncrement specify <entity type, field type>
            session.Advanced.AddOrIncrement<User, int>(

                    // Specify document id and entity on which the operation should be performed.
                    id,
                    new User
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        LoginCount = 1

                    // The path to the field and value to be added.
                    }, x => x.LoginCount, 1);

                session.SaveChanges();
            #endregion
        }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    {
                        #region increment_counter_by_document_reference_generic_session
                        var order = session.Load<Order>("orders/1-A");
                        session.CountersFor(order).Increment("Likes", 1);
                        session.SaveChanges();
                        #endregion
                    }

                    #region increment_counter_by_document_id_non_generic_session
                    session.Advanced.Defer(new PatchCommandData("orders/1-A", null,
                        new PatchRequest
                        {
                            Script = "incrementCounter(this, args.name, args.val);",
                            Values =
                            {
                                { "name", "Likes" },
                                { "val", 20 }
                            }
                        }, null));
                    session.SaveChanges();
                    #endregion

                    #region delete_counter_by_document_id_generic_session
                    session.CountersFor("orders/1-A").Delete("Likes");
                    session.SaveChanges();
                    #endregion

                    #region delete_counter_by_document_refference_non_generic_session
                    session.Advanced.Defer(new PatchCommandData("products/1-A", null,
                        new PatchRequest
                        {
                            Script = "deleteCounter(this, args.name);",
                            Values =
                            {
                                { "name", "Likes" },
                            }
                        }, null));
                    session.SaveChanges();
                    #endregion

                    {
                        #region get_counter_by_document_id_generic_session
                        var order = session.Load<Order>("orders/1-A");
                        var counters = session.Advanced.GetCountersFor(order);
                        #endregion
                    }

                    #region get_counter_by_document_id_non_generic_session
                    session.Advanced.Defer(new PatchCommandData("orders/1-A", null,
                        new PatchRequest
                        {
                            Script = @"var likes = counter(this.Company, args.name);
                                       put('result/', {company: this.Company, likes: likes});",
                            Values =
                            {
                                { "name", "Likes" },
                            }
                        }, null));
                    session.SaveChanges();
                    #endregion
                }

                #region increment_counter_by_document_id_store
                store.Operations.Send(new PatchOperation("orders/1-A", null, new PatchRequest
                {
                    Script = "incrementCounter(this, args.name, args.val);",
                    Values =
                    {
                        { "name", "Likes" },
                        { "val", -1 }
                    }
                }));
                #endregion

                #region delete_counter_by_document_refference_store
                store.Operations.Send(new PatchOperation("products/1-A", null, new PatchRequest
                {
                    Script = "deleteCounter(this, args.name);",
                    Values =
                    {
                        { "name", "Likes" },
                    }
                }));
                #endregion

                #region get_counter_by_document_id_store
                store.Operations.Send(new PatchOperation("orders/1-A", null, new PatchRequest
                {
                    Script = @"var likes = counter(this.Company, args.name);
                               put('result/', {company: this.Company, likes: likes});",
                    Values =
                    {
                        { "name", "Likes" },
                    }
                }));
                #endregion

                using (var session = store.OpenSession())
                {
                    #region patch_using_inline_compilation_with_function
                    // Modify value using inline string compilation
                    // ============================================
                    
                    session.Advanced.Defer(new PatchCommandData(
                        id: "products/1-A",
                        changeVector: null,
                        patch: new PatchRequest
                        {
                            Script = @"
                                // Give a discount if the product is low in stock:
                                const functionBody = 'return doc.UnitsInStock < lowStock ? ' + 
                                    'doc.PricePerUnit * discount :' + 
                                    'doc.PricePerUnit;';
                            
                                // Define a function that processes the document and returns the price:
                                const calcPrice = new Function('doc', 'lowStock', 'discount', functionBody);
                            
                                // Update the product's PricePerUnit based on the function:
                                this.PricePerUnit = calcPrice(this, args.LowStock, args.Discount);",
                            
                            Values = {
                                {"LowStock", "10"},
                                {"Discount", "0.8"}
                            }
                        },
                        patchIfMissing: null));

                    session.SaveChanges();
                    
                    // The same can be applied using the 'operations' syntax.
                    #endregion
                }
                
                #region patch_using_inline_compilation_with_eval
                // Modify value using inline string compilation
                // ============================================

                store.Operations.Send(new PatchOperation("products/1-A", null, new PatchRequest
                {
                    Script = @"
                        // Give a discount if the product is low in stock:
                        const discountExpression = 'this.UnitsInStock < args.LowStock ? ' + 
                            'this.PricePerUnit * args.Discount :' + 
                            'this.PricePerUnit';
                        
                        // Call 'eval', pass the string expression that contains your logic:
                        const price = eval(discountExpression);
                        
                        // Update the product's PricePerUnit:
                        this.PricePerUnit = price;",
                    
                    Values = {
                        {"LowStock", "10"},
                        {"Discount", "0.8"}
                    }
                }));
                
                // The same can be applied using the 'session defer' syntax.
                #endregion
            }
        }
    }
        
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DateTime> LoginTimes { get; set; }
        public DateTime LastLogin { get; set; }
        public int LoginCount { get; set; }
        public string Supplies { get; set; }
    }
    
    public class BlogPost
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public DateTime PublishedAt { get; set; }

        public string[] Tags { get; set; }

        public BlogComment[] Comments { get; set; }
    }
    
    public class BlogComment
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
