# Compare Exchange 

Compare exchange feature allows you to perform cluster-wide interlocked distributed operations. 
This gives ability to create unique constraints, because you can to reserve values for particular documents. 

{NOTE: Migration from 3.X}
If you are migrating from RavenDB 3.X unique constraints feature, compare exchange is replacement for this functionality. 
{NOTE/}

## How it works

Compare exchange value can be arbitrary object. When saving value, user has to provide:

| Parameter | Description |
| ------------- | ---- |
| **key** | represents values under which value is saved |
| **value** | any json value is permitted: string, boolean, number, array or object |
| **index** | used for concurrency control |

### Creating values

When creating 'Compare Exchange' value index should be set to `0`. Put will succeed only if, value with given key doesn't exist. 

### Updating values

Updating 'Compare Exchange' values can be divied into 2 phases:

- First we read value by its key (we get index and object value).
- Once value is updated, we provide index obtained during read operation and updated value. Put operation will succeed only, if index on server side matches index from save request, which means value was not updated by someone else between read and write operation. 

{WARNING: Scope of compare exchange operations}
Please notice that compare exchange operations are not using transaction associated with current session. It is caused by the fact,
that atomicity is guaranteed across entire cluster, but session transaction spans single node.
{WARNING/}

{PANEL:Example}
Compare Exchange can be used to maintain uniqness across user account e-mails. In example below we first try to reserve user e-mail, then
we save user account document. 

Implications:

- since `User` object is saved as document, it can be indexed, queried, etc. 
- if `SaveChanges` fails, user reservation is not rolled back

{CODE email@Server\CompareExchange.cs /}

{PANEL/}

{PANEL:Example II}

Instead of making reservation and storing User object, you can save entire value inside compare exchange value. 

- it is fully atomic operation across entire cluster
- since `User` object is not saved as document, it *can not* be indexed, queried, etc.
- no need to handle partial failures

{CODE email2@Server\CompareExchange.cs /}

{PANEL/}


{PANEL:Example III}

In this example we use compare exchange for shared resource reservation system. This reservation is cluster-wide. 
It has build-in protection against clients, who never release resource (i.e. due to failure), by using timeout. 

{CODE shared_resource@Server\CompareExchange.cs /}

{PANEL/}
