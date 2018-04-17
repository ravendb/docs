# Document conflict in client-side
  
## What are conflicts?
When two or more changes of a single document are done concurrently in two separate nodes, RavenDB cannot know which one of the changes is the correct one. This is called document conflict.  
{NOTE For more information about conflicts and their resolution, see [article about conflicts](../../server/clustering/replication-conflicts). /}
  
## When is conflict except thrown?
DocumentConflictException will be thrown _only_ if there is a fetch of document by id 
and that document is conflicted. Otherwise conflicted documents are ignored. 
This means during "starts with" queries, streaming, regular queries etc. the conflicted 
documents are not retrieved and no exceptions are thrown.

### Additionally
 Fetching attachments of a conflicted document will throw `InvalidOperationException` on the server.

## How the conflict can be resolved from the client side?
 * PUT of a document with id that belongs to conflicted document will resolve the conflict.

{CODE-BLOCK:csharp}
using (var session = store.OpenSession())
{
    session.Store(new User { Name = "John Doe"}, "users/123"); // users/123 is a conflicted document
    session.SaveChanges(); //when this request is finished, the conflict for users/132 is resolved.
}
{CODE-BLOCK/}

 * DELETE of a conflicted document will resolve its conflict.  

{CODE-BLOCK:csharp}
using (var session = store.OpenSession())
{
    session.Delete("users/123"); // users/123 is a conflicted document
    session.SaveChanges(); //when this request is finished, the conflict for users/132 is resolved.
}
{CODE-BLOCK/}

 * Incoming replication will resolve conflict if the incoming document has a larger [change vector](../../server/clustering/change-vector).

## Modifying conflict resolution from the client-side
In RavenDB we can resolve conflicts either by resolving to latest or by using conflict resolution script to decide which one of the conflicted document variants are the ones that need to be kept. The following is an example how we can set conflict resolution script from the client-side.
{CODE-BLOCK:csharp}
using (var store = new DocumentStore
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
                    return final; //the conflict will be resolved to this variant
                    "
            }
        }
    };

    var op = new ModifyConflictSolverOperation(
        store.Database,
        resolveByCollection, //we specify conflict resolution scripts by document collection 
        resolveToLatest: false); //if true, RavenDB will resolve conflict to the latest.

    await store.Maintenance.Server.SendAsync(op);
}
{CODE-BLOCK/}
