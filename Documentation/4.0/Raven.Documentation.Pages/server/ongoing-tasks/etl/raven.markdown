#Ongoing Tasks: RavenDB ETL 

RavenDB ETL is a task that creates [ETL](../../../server/ongoing-tasks/etl/basics) process for a given database where a destination is another RavenDB database.


It can be defined using the Studio by creating `RavenDB ETL` task in `Settings -> Manage Ongoing Tasks`.

![Figure 1. Configure RavenDB ETL task](images/raven-etl-setup.png)

{PANEL:Transformation Scripts}

One task can have multiple transformations. Each of them can be defined on single collection or multiple ones. There is also an option to indicate that the script should be applied
to **all** documents regardless the collection they belong to.

### `loadTo` Method

The script is executed per document once it is created or modified. In order to load data to the destination database you need to call `loadTo<CollectionName>()` method and pass JS object.
It is a convention that a collection name that a document will be put into on the other side is indicated in the load method name. 

For example, if you want to write data to `Employees` collection you need to call the following method in the script body:

{CODE-BLOCK:javascript}
loadToEmployees({ ... });
{CODE-BLOCK/}

The method parameter must be a JS object. You can create it as follows:

{CODE-BLOCK:javascript}
loadToEmployees({
    Name: this.FirstName + " " + this.LastName
});
{CODE-BLOCK/}

or simply transform current document object you have and pass it:

{CODE-BLOCK:javascript}
this.Name = this.FirstName + " " + this.LastName;

delete this.Address;
delete this.FirstName;
delete this.LastName;

loadToEmployees(this);
{CODE-BLOCK/}

### Filtering

If you want to filter some documents out from the ETL you simply omit `loadTo` call:

{CODE-BLOCK:javascript}
if (this.Active) {
    // load only active users
    loadToEmployees({ ... });
}
{CODE-BLOCK/}

### Loading Other Documents

The `load` method loads a document with a specified ID during script execution.

{CODE-BLOCK:javascript}
var manager = load(this.ReportsTo);
{CODE-BLOCK/}

### Accessing Metadata

You can access metadata in the following way:

{CODE-BLOCK:javascript}
var value = this['@metadata']['custom-metadata-key'];
{CODE-BLOCK/}

### Multiple Documents from Single One

The `loadTo` method can be called multiple time in a single script. That allows you for example to split a single document into a multiple ones:

{CODE-BLOCK:javascript}

loadToAddresses({
    City: this.Address.City ,
    Country: this.Address.Country ,
    Address: this.Address.Line1
});

delete this.Address;

loadToEmployees(this);
{CODE-BLOCK/}

### Empty Script

The empty script means transferring documents without any transformation to the same collection.

{PANEL/}

{PANEL:Loading Documents}

The objects indicated to be sent to the target database by `loadTo` call are pushed in the last stage. All results created in a single ETL run will be sent in a single
batch and processed transactionally on the destination.

### Documents Identifiers

Depending on the destination collection that documents are loaded into, they might have the same identifiers or get different ones.

If documents are loaded to the same collection they will preserve their original identifiers. For example, ETL on `Employees` collection loading documents using:

{CODE-BLOCK:javascript}
loadToEmployees({ ... });
{CODE-BLOCK/}

will keep the same identifiers in the target database. 

If the load method indicates different target collection, e.g. `People`:

{CODE-BLOCK:javascript}
loadToPeople({ ... });
{CODE-BLOCK/}

the employee documents will get the identifiers that combine the original ID and new collection name. In addition to that, ETL appends `/` prefix to
ask the target database to [generate identifiers on its side](../../../client-api/document-identifiers/working-with-document-identifiers#server-side-generated-ids). In result 
documents in `People` collection on the other side will have identifiers like `employees/1-A/people/0000000000000000001-A`.

### Attachments

Attachments are sent automatically over the wire when you send a full collection to the destination (empty script). If you use a script, there is currently no way to indicate
that attachments should also be sent.

### Deletions

As described above, the identifiers of sent documents are typically different than on the source side. The source isn't aware of them so deletions require 
a special approach. In order to remove the matching documents on the destination side the deletion of a document sends a command that will remove documents which IDs have a well-known prefix.

For example, if document `employees/1-A` that was processed by ETL, put into `People` collection and got `employees/1-A/people/0000000000000000001-A` ID is deleted then the deletion 
is propagated by sending the command that deletes documents having the following prefix in ID: `employees/1-A/people/`. 

If you output multiple documents from a single one then multiple delete commands will be sent, one for each prefix containing destination collection name.

When documents are sent to the same collection and IDs don't change then deletion on the source results in sending a single delete command for a given ID.

{PANEL/}
