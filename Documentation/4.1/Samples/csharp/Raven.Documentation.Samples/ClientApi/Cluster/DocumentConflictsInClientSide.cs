using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Documentation.Samples.Indexes.Querying;
using Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Backup;

namespace Raven.Documentation.Samples.ClientApi.Cluster
{
    public class DocumentConflictsInClientSide
    {
        public async Task CodeSamples()
        {
            using (var store = new DocumentStore
            {

            })
            {
                #region PUT_Sample

                using (var session = store.OpenSession())
                {
                    session.Store(new User {Name = "John Doe"}, "users/123"); // users/123 is a conflicted document
                    session.SaveChanges(); //when this request is finished, the conflict for users/132 is resolved.
                }

                #endregion

                #region DELETE_Sample

                using (var session = store.OpenSession())
                {
                    session.Delete("users/123"); // users/123 is a conflicted document
                    session.SaveChanges(); //when this request is finished, the conflict for users/132 is resolved.
                }                

                #endregion

                #region Modify_conflict_resolution_sample

                using (var documentStore = new DocumentStore
                {
                    Urls = new []{ "http://<url of a database>" },
                    Database = "<database name>"
                })
                {
                    var resolveByCollection = new Dictionary<string, ScriptResolver>
                    {
                        {
                            "ShoppingCarts", new ScriptResolver //specify conflict resolution for collection
                            {
                                // conflict resolution script is written in javascript
                                Script = @"
                                var final = docs[0];
                                for(var i = 1; i < docs.length; i++)
                                {
                                    var currentCart = docs[i];
                                    for(var j = 0; j < currentCart.Items.length; j++)
                                    {
                                        var item = currentCart.Items[j];
                                        var match = final.Items
                                                         .find( i => i.ProductId == item.ProductId);
                                        if(!match)
                                        {
                                            // not in cart, add
                                            final.Items.push(item);
                                        }
                                        else
                                        {
                                            match.Quantity = Math.max(
                                                        item.Quantity ,
                                                        match.Quantity);
                                        }
                                    }
                                }
                                return final; // the conflict will be resolved to this variant
                                "
                            }
                        }
                    };

                    var op = new ModifyConflictSolverOperation(
                        documentStore.Database,
                        resolveByCollection,    //we specify conflict resolution scripts by document collection 
                        resolveToLatest: true); // if true, RavenDB will resolve conflict to the latest
                                                // if there is no resolver defined for a given collection or
                                                // the script returns null

                    await documentStore.Maintenance.Server.SendAsync(op);
                }                

                #endregion
            }
        }
    }
}
