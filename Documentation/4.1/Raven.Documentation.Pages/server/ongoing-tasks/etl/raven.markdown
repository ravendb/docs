# Ongoing Tasks: RavenDB ETL 
---

{NOTE: }

* **RavenDB ETL Task** creates an [ETL](../../../server/ongoing-tasks/etl/basics) process for a given database when the destination is another RavenDB database.  

* It can be defined in code or using the [Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task) by creating a `RavenDB ETL` task under `Settings -> Manage Ongoing Tasks`.  

* One RavenDB ETL task can have multiple transformation scripts.  

* Each script can be defined on a single collection, multiple selected collections or be applied to **all** documents regardless of the associated collection.  

* The script is executed per document once the document is created or modified.  

* In this page:  
  * [Transformation Script Options](../../../server/ongoing-tasks/etl/raven#transformation-script-options)  
  * [Empty Script](../../../server/ongoing-tasks/etl/raven#empty-script)  
  * [Attachments](../../../server/ongoing-tasks/etl/raven#attachments)  
  * [Revisions](../../../server/ongoing-tasks/etl/raven#revisions)  
  * [Deletions](../../../server/ongoing-tasks/etl/raven#deletions)  
  * [Example](../../../server/ongoing-tasks/etl/raven#example)  
{NOTE/}

![Figure 1. Configure RavenDB ETL task](images/raven-etl-setup.png "RavenDB ETL in Studio")

{PANEL: Transformation Script Options}

{NOTE: Loading Documents}

* In order to load data to the destination database you must call the `loadTo<CollectionName>()` method and pass a JS object.  

* Indicating the collection name in the load method is a convention.  

* The objects passed to the `loadTo` method will be sent to the target database in the last `Load` stage.  

* All results created in a single ETL run will be sent in a single batch and processed transactionally on the destination.

* For example, if you want to write data to `Employees` collection you need to call the following method in the script body:

{CODE-BLOCK:javascript}
loadToEmployees({ ... });
{CODE-BLOCK/}

* The method parameter must be a JS object. You can create it as follows:

{CODE-BLOCK:javascript}
loadToEmployees({
    Name: this.FirstName + " " + this.LastName
});
{CODE-BLOCK/}

* Or simply transform the current document object and pass it:

{CODE-BLOCK:javascript}
this.Name = this.FirstName + " " + this.LastName;

delete this.Address;
delete this.FirstName;
delete this.LastName;

loadToEmployees(this);
{CODE-BLOCK/}
{NOTE/}

{NOTE: Documents Identifiers}

* The documents generated in the destination database are given an id according to the collection name specified in the `loadTo` method.  

* If the specified collection is the _same_ as the original one then the document is loaded to the _same_ collection and the original identifier is preserved.  

* For example, the following ETL script defined on `Employees` collection will keep the same identifiers in the target database:  

{CODE-BLOCK:javascript}
// original identifier will be preserved
loadToEmployees({ ... });
{CODE-BLOCK/}

* If the 'loadTo' method indicates a _different_ target collection, e.g. `People`,  
  then the employee documents will get new identifiers that combine the original ID and the new collection name in the destination database.  

{CODE-BLOCK:javascript}
// new identifier will be generated
loadToPeople({ ... });
{CODE-BLOCK/}

* In addition, ETL appends the symbol `/` to the requested id so that the target database will [generate identifiers on its side](../../../client-api/document-identifiers/working-with-document-identifiers#server-side-generated-ids).  
  As a result, documents in the `People` collection in the target database will have identifiers such as: `employees/1-A/people/0000000000000000001-A`.

{NOTE/}

{NOTE: Filtering}

* Documents can be filtered from the ETL by calling the `loadTo` method only for documents that match some condition:

{CODE-BLOCK:javascript}
if (this.Active) {
    // load only active users
    loadToEmployees({ ... });
}
{CODE-BLOCK/}
{NOTE/}

{NOTE: Loading Data from Other Documents}

* The `load` method loads a document with a specified ID during script execution.

{CODE-BLOCK:javascript}
// this.ReportsTo has some document ID
var manager = load(this.ReportsTo);
{CODE-BLOCK/}
{NOTE/}

{NOTE: Accessing Metadata}

* The metadata can be accessed in the following way:

{CODE-BLOCK:javascript}
var value = this['@metadata']['custom-metadata-key'];
{CODE-BLOCK/}
{NOTE/}

{NOTE: Creating Multiple Documents from a Single Document}

* The `loadTo` method can be called multiple times in a single script.  
  That allows you to split a single source document into multiple documents on the destination database:

{CODE-BLOCK:javascript}

// documents will be created in `Addresses` collection
loadToAddresses({
    City: this.Address.City ,
    Country: this.Address.Country ,
    Address: this.Address.Line1
});

delete this.Address;

// documents will be created in the `Employees` collection
loadToEmployees(this);
{CODE-BLOCK/}
{NOTE/}
{PANEL/}

{PANEL: Empty Script}

* An ETL task can be created with an empty script.  
* The documents will be transferred _without_ any modifications to the _same_ collection as the source document.  
{PANEL/}

{PANEL: Attachments}

* Attachments are sent automatically when you send a _full_ collection to the destination using an _empty_ script.  
* If you use a script you can indicate that an attachment should also be sent by using dedicated functions:

   - `loadAttachment(name)` returns a reference to an attachment that is meant be passed to `addAttachment()`
   - `<doc>.addAttachment([name,] attachmentRef)` adds an attachment to a document that will be sent in the process, `<doc>` is a reference returned by `loadTo<CollectionName>()`

{NOTE: Sending attachments together with documents}

* Attachment is sent along with a transformed document if it's explicitly defined in the script by using `addAttachment()` method. By default attachment name is preserved.
* The below script sends _all_ attachments of a current document by taking advantage of `getAttachments()` function, loads each of them during transformation and adds to
a document that will be sent to 'Users' collection on destination side

{CODE-BLOCK:javascript}
var doc = loadToUsers(this);

var attachments = getAttachments();

for (var i = 0; i < attachments.length; i++) {
    doc.addAttachment(loadAttachment(attachments[i].Name));
}
{CODE-BLOCK/}
{NOTE/}

{NOTE: Changing attachment name}

* If `addAttachment()` is called with two arguments, the first one can indicate a new name for an attachment. In the below example attachment `photo`
will be sent and stored under `picture` name.
* In order to check the existence of an attachment `hasAttachment()` function is used

{CODE-BLOCK:javascript}
var employee = loadToEmployees({
    Name: this.FirstName + " " + this.LastName
});

if (hasAttachment('photo')) {
  employee.addAttachment('picture', loadAttachment('photo'));
}
{CODE-BLOCK/}
{NOTE/}

{NOTE: Loading non existing attachment}

* Function `loadAttachment()` returns `null` if a document doesn't have an attachment with a given name. Passing such reference to `addAttachment()` will be no-op and no error will be thrown.

{NOTE/}

{NOTE: Accessing attachments from metadata}

* The collection of attachments of the currently transformed document can be accessed either by `getAttachments()` helper function or directly from document metadata:

{CODE-BLOCK:javascript}
var attachments = this['@metadata']['@attachments'];
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

{PANEL: Revisions}

* Revisions are _not_ sent by the ETL process.  
  But, if revisions are configured on the destination database, then when the target document is overwritten by the ETL process a revision will be created as expected.  
{PANEL/}

{PANEL: Deletions}

* As described above, the identifiers created for the sent documents can be different from the original source documents identifiers.  
  The source isn't aware of the new IDs created so documents deletion requires a special approach.  

* In order to remove the matching documents on the destination side, the deletion of a document sends a command to remove documents that have an ID with a _well-known prefix_.  

* For example: 
  1. Document `employees/1-A` is processed by ETL and put into `People` collection with ID:  
     `employees/1-A/people/0000000000000000001-A`.  
  2. Deletion of the `employees/1-A` document on the source side triggers sending a command that deletes documents having the following prefix in their ID: `employees/1-A/people/`.  

* If you output multiple documents from a single document then multiple delete commands will be sent, one for each prefix containing the destination collection name.  

* When documents are sent to the same collection and IDs don't change then deletion on the source results in sending a single delete command for a given ID.  
{PANEL/}

{PANEL: Example}

* The following is an example of a RavenDB ETL script processing documents from `Employees` collection:

{CODE-BLOCK:javascript}

var managerName = null;

if (this.ReportsTo !== null)
{
    var manager = load(this.ReportsTo);
    managerName = manager.FirstName + " " + manager.LastName;
}

// load documents to `Employees` collection in the destination
loadToEmployees({
    // the loaded documents will have these fields:
    Name: this.FirstName + " " + this.LastName,
    Title: this.Title,
    BornOn: new Date(this.Birthday).getFullYear(),
    Manager: managerName
});
{CODE-BLOCK/}
{PANEL/}

## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
- [Define SQL ETL Task in Studio](../../../todo-update-me-later)
