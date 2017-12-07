﻿
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using static Raven.Client.Documents.Operations.PatchOperation;

namespace Raven.Documentation.Samples.ClientApi.Commands.Patches
{
	public class PatchRequests
    {
		private interface IFoo
		{
            #region patch_generic_interfact
            void Increment<T, U>(T entity, Expression<Func<T, U>> path, U valToAdd);

            void Increment<T, U>(string id, Expression<Func<T, U>> path, U valToAdd);

            void Patch<T, U>(string id, Expression<Func<T, U>> path, U value);

            void Patch<T, U>(T entity, Expression<Func<T, U>> path, U value);

            void Patch<T, U>(T entity, Expression<Func<T, IEnumerable<U>>> path,
                Expression<Func<JavaScriptArray<U>, object>> arrayAdder);

            void Patch<T, U>(string id, Expression<Func<T, IEnumerable<U>>> path,
                Expression<Func<JavaScriptArray<U>, object>> arrayAdder);
            #endregion

            #region patch_non_generic_interface_in_session
            void Defer(ICommandData command, params ICommandData[] commands);
            #endregion

            #region patch_non_generic_interface_in_store
            PatchStatus Send(PatchOperation operation);
            #endregion
        }
        
        public PatchRequests()
        {
            using (var store = new DocumentStore().Initialize())
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
                session.Advanced.Defer(new PatchCommandData("employees/1", null, new PatchRequest
                {
                    Script = @"this.FirstName = args.FirstName;",
                    Values =
                    {
                        {"FirstName","Robert" }
                    }
                }, null));
                session.SaveChanges();
                #endregion

                #region patch_firstName_non_generic_store
                // change FirstName to Robert                
                store.Operations.Send(new PatchOperation("employees/1", null, new PatchRequest
                {
                    Script = @"this.FirstName = args.FirstName;",
                    Values =
                    {
                        {"FirstName","Robert" }
                    }
                }, null));
                #endregion

                #region patch_firstName_and_lastName_generic
                // change FirstName to Robert and LastName to Carter in single request
                // note that in this case, we create single request, but two seperate batch operations
                // in order to achieve atomicity, please use the non generic APIs
                session.Advanced.Patch<Employee, string>("employees/1", x => x.FirstName, "Robert");
                session.Advanced.Patch<Employee, string>("employees/1", x => x.LastName, "Carter");
                session.SaveChanges();
                #endregion

                #region pathc_firstName_and_lastName_non_generic_session
                // change FirstName to Robert and LastName to Carter in single request
                // note that here we do maintain the atomicity of the operation
                session.Advanced.Defer(new PatchCommandData("employees/1",null, new PatchRequest
                {
                    Script = @"
                                this.FirstName = args.UserName.FirstName
                                this.LastName = args.UserName.LastName",
                    Values =
                    {
                        {"UserName", new { FirstName = "Robert", LastName = "Carter" }}                        
                    }
                },null));
                session.SaveChanges();
                #endregion

                #region pathc_firstName_and_lastName_store
                // change FirstName to Robert and LastName to Carter in single request
                // note that here we do maintain the atomicity of the operation
                store.Operations.Send(new PatchOperation("employees/1", null, new PatchRequest
                {
                    Script = @"
                                this.FirstName = args.UserName.FirstName
                                this.LastName = args.UserName.LastName",
                    Values =
                    {
                        {"UserName", new { FirstName = "Robert", LastName = "Carter" }}
                    }
                }, null));
                #endregion

                #region increment_age_generic
                // increment Age property value by 10
                session.Advanced.Increment<Employee, int>("employees/1", x => x.Age, 10);
                session.SaveChanges();
                #endregion

                #region increment_age_non_generic_session
                session.Advanced.Defer(new PatchCommandData("employees/1", null, new PatchRequest
                {
                    Script = @"this.Age += args.AgeToAdd",
                    Values =
                    {
                        {"AgeToAdd", 10 }
                    }
                },null));
                session.SaveChanges();
                #endregion

                #region increment_age_non_generic_store
                store.Operations.Send(new PatchOperation("products/1",null, new PatchRequest
                {
                    Script = @"this.Age += args.AgeToAdd",
                    Values =
                    {
                        {"AgeToAdd", 10 }
                    }
                }, null));
                #endregion
                                
                #region remove_property_age_non_generic_session
                // remove property Age
                session.Advanced.Defer(new PatchCommandData("employees/1", null, new PatchRequest
                {
                    Script = @"delete this.Age"
                }, null));
                session.SaveChanges();
                #endregion

                #region remove_property_age_store
                // remove property Age
                store.Operations.Send(new PatchOperation("employees/1", null, new PatchRequest
                {
                    Script = @"delete this.Age"
                }, null));
                #endregion

                #region rename_property_age_non_generic_session
                // rename FirstName to First
                session.Advanced.Defer(new PatchCommandData("employees/1", null, new PatchRequest
                {
                    Script = @" var firstName = this[args.Rename.Old];
                                delete this[args.Rename.Old];
                                this[args.Rename.New] = firstName",
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
                }, null));
                session.SaveChanges();
                #endregion
                #region rename_property_age_store
                store.Operations.Send(new PatchOperation("employees/1",null, new PatchRequest
                {
                    Script = @" var firstName = this[args.Rename.Old];
                                delete this[args.Rename.Old];
                                this[args.Rename.New] = firstName",
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
                }, null));
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
                session.Advanced.Defer(new PatchCommandData("blogposts/1", null, new PatchRequest
                {
                    Script = "this.Comments.push(args.Comment)",
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
                    
                }, null));
                session.SaveChanges();
                #endregion 

                #region add_new_comment_to_comments_store
                // add a new comment to Comments
                store.Operations.Send(new PatchOperation("blogposts/1", null, new PatchRequest
                {
                    Script = "this.Comments.push(args.Comment)",
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

                }, null));
                #endregion

                #region insert_new_comment_at_position_1_session
                // insert a new comment at position 1 to Comments
                session.Advanced.Defer(new PatchCommandData("blogposts/1", null, new PatchRequest
                {
                    Script = "this.Comments.splice(1,0,args.Comment)",
                    Values =
                    {
                        {"Comment", new BlogComment
                                    {
                                        Content = "Lore ipsum",
                                        Title = "Some title"
                                    }
                        }
                    }
                }, null));
                session.SaveChanges();
                #endregion

                #region insert_new_comment_at_position_1_store
                store.Operations.Send(new PatchOperation("blogposts/1",null, new PatchRequest
                {
                    Script = "this.Comments.splice(1,0,args.Comment)",
                    Values =
                    {
                        {"Comment", new BlogComment
                                    {
                                        Content = "Lore ipsum",
                                        Title = "Some title"
                                    }
                        }
                    }
                }, null));
                #endregion

                #region modify_a_comment_at_position_3_in_comments_session
                // modify a comment at position 3 in Comments
                session.Advanced.Defer(new PatchCommandData("blogposts/1", null, new PatchRequest
                {
                    Script = "this.Comments.splice(3,1,args.Comment)",
                    Values =
                    {
                        {"Comment", new BlogComment
                                    {
                                        Content = "Lore ipsum",
                                        Title = "Some title"
                                    }
                        }
                    }
                }, null));
                session.SaveChanges();
                #endregion

                #region modify_a_comment_at_position_3_in_comments_store
                // modify a comment at position 3 in Comments
                store.Operations.Send(new PatchOperation("blogposts/1", null, new PatchRequest
                {
                    Script = "this.Comments.splice(1,0,args.Comment)",
                    Values =
                    {
                        {"Comment", new BlogComment
                                    {
                                        Content = "Lore ipsum",
                                        Title = "Some title"
                                    }
                        }
                    }
                }, null));
                #endregion

                #region filter_items_from_array_session
                // filter out all comments of a blogpost which contains the word "wrong" in their contents 
                session.Advanced.Defer(new PatchCommandData("blogposts/1", null, new PatchRequest
                {
                    Script = @"this.Comments = this.Comments.filter(comment=>
                                comment.Content.includes(args.TitleToRemove));",
                    Values =
                    {
                        {"TitleToRemove","wrong" }
                    }
                }, null));
                session.SaveChanges();
                #endregion

                #region filter_items_from_array_store
                // filter out all comments of a blogpost which contains the word "wrong" in their contents
                store.Operations.Send(new PatchOperation("blogposts/1", null, new PatchRequest
                {
                    Script = @"this.Comments = this.Comments.filter(comment=>
                                comment.Content.includes(args.TitleToRemove));",
                    Values =
                    {
                        {"TitleToRemove","wrong" }
                    }
                }, null));                
                #endregion

                #region update_product_name_in_order_session
                // update product names in order, according to loaded product documents
                session.Advanced.Defer(new PatchCommandData("orders/1", null, new PatchRequest
                {
                    Script = @"this.Lines.forEach(line=> { 
                                var productDoc = load(line.Product);
                                line.ProductName = productDoc.Name;
                                });"
                }, null));
                session.SaveChanges();
                #endregion

                #region update_product_name_in_order_store
                // update product names in order, according to loaded product documents
                store.Operations.Send(new PatchOperation("blogposts/1", null, new PatchRequest
                {
                    Script = @"this.Lines.forEach(line=> { 
                                var productDoc = load(line.Product);
                                line.ProductName = productDoc.Name;
                                });"
                }, null));
                #endregion
               
            }
        }
    }
}
