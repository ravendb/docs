# Ongoing Tasks: RavenDB ETL 
---

{NOTE: }

* **RavenDB ETL Task** creates an [ETL](../../../server/ongoing-tasks/etl/basics) 
  process for a given database when the destination is another RavenDB database.  

* The script is executed per document whenever the document is created, modified, and/or deleted.  

* It can be defined in code or using the [Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task).  

* One RavenDB ETL task can have multiple transformation scripts and each script can load to a different collection.  

* Each script can be defined on the source database to trigger ETL from a single collection, 
  multiple selected collections or be applied to **all** documents regardless of the associated collection(s).  

* In secure servers, for the destination cluster to trust the source, you must: 
    1. Download/export the server certificate from the source server.  
    2. Upload/import its client certificate (.pfx) into the destination.

* In this page:  
  * [Transformation Script Options](../../../server/ongoing-tasks/etl/raven#transformation-script-options)  
  * [Empty Script](../../../server/ongoing-tasks/etl/raven#empty-script)  
  * [Attachments](../../../server/ongoing-tasks/etl/raven#attachments) 
  * [Counters](../../../server/ongoing-tasks/etl/raven#counters)  
  * [Revisions](../../../server/ongoing-tasks/etl/raven#revisions)  
  * [Deletions](../../../server/ongoing-tasks/etl/raven#deletions)  

{NOTE/}

---

![Figure 1. Configure RavenDB ETL task](images/raven-etl-setup.png "RavenDB ETL in Studio")

{PANEL: Transformation Script Options}

* [Loading Documents](../../../server/ongoing-tasks/etl/raven#loading-documents)
* [Documents Identifiers](../../../server/ongoing-tasks/etl/raven#documents-identifiers)
* [Filtering](../../../server/ongoing-tasks/etl/raven#filtering)
* [Loading Data from Other Documents](../../../server/ongoing-tasks/etl/raven#loading-data-from-other-documents)
* [Accessing Metadata](../../../server/ongoing-tasks/etl/raven#accessing-metadata)
* [Creating Multiple Documents from a Single Document](../../../server/ongoing-tasks/etl/raven#creating-multiple-documents-from-a-single-document)

---

### Loading Documents


* To load data to the destination database you must call the `loadTo<CollectionName>()` method and pass a JS object.  

* Indicating the collection name in the load method is a convention.  

* The objects passed to the `loadTo` method will be sent to the target database in the last stage - `Load`.  

* All results created in a single ETL run will be sent in a single batch and processed transactionally in the destination.

   * For example, if you want to write data to the `Employees` collection you need to call the following method in the script body:

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

#### Example: loadTo Method

The following is an example of a RavenDB ETL script processing documents from the `Employees` collection:

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

---

### Documents Identifiers

* The documents generated in the destination database are given an ID according to the collection name specified in the `loadTo` method.  

* If the specified collection is the _same_ as the original one then the document is loaded to the _same_ collection and the original identifier is preserved.  
   * For example, the following ETL script defined in the `Employees` collection will keep the same identifiers in the target database:  

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

---

### Filtering

Documents can be filtered from the ETL by calling the `loadTo` method only for documents that match some condition:

{CODE-BLOCK:javascript}
if (this.Active) {
    // load only active users
    loadToEmployees({ ... });
}
{CODE-BLOCK/}

---

### Loading Data from Other Documents

The `load` method loads a document with the specified ID into the script context so it can be transformed.  

{CODE-BLOCK:javascript}
// this.ReportsTo has some document ID
var manager = load(this.ReportsTo);
{CODE-BLOCK/}

---

### Accessing Metadata

The metadata can be accessed in the following way:

{CODE-BLOCK:javascript}
var value = this['@metadata']['custom-metadata-key'];
{CODE-BLOCK/}

---

### Creating Multiple Documents from a Single Document

The `loadTo` method can be called multiple times in a single script.  
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

---

* [Sending attachments together with documents](../../../server/ongoing-tasks/etl/raven#sending-attachments-together-with-documents)
* [Changing attachment name](../../../server/ongoing-tasks/etl/raven#changing-attachment-name)
* [Loading non-existent attachment](../../../server/ongoing-tasks/etl/raven#loading-non-existent-attachment)
* [Accessing attachments from metadata](../../../server/ongoing-tasks/etl/raven#accessing-attachments-from-metadata)

---

### Sending attachments together with documents


* Attachment is sent along with a transformed document if it's explicitly defined in the script by using `addAttachment()` method. By default, the attachment name is preserved.
* The script below sends _all_ attachments of a current document by taking advantage of `getAttachments()` function, loads each of them during transformation, and adds them to
a document that will be sent to the 'Users' collection on the destination database.

{CODE-BLOCK:javascript}
var doc = loadToUsers(this);

var attachments = getAttachments();

for (var i = 0; i < attachments.length; i++) {
    doc.addAttachment(loadAttachment(attachments[i].Name));
}
{CODE-BLOCK/}

---

### Changing attachment name

* If `addAttachment()` is called with two arguments, the first one can indicate a new name for an attachment. In the example below, attachment `photo`
will be sent and stored under the `picture` name.
* To check the existence of an attachment `hasAttachment()` function is used

{CODE-BLOCK:javascript}
var employee = loadToEmployees({
    Name: this.FirstName + " " + this.LastName
});

if (hasAttachment('photo')) {
  employee.addAttachment('picture', loadAttachment('photo'));
}
{CODE-BLOCK/}

---

### Loading non-existent attachment

Function `loadAttachment()` returns `null` if a document doesn't have an attachment with a given name. Passing such reference to `addAttachment()` will be no-op and no error will be thrown.


---

### Accessing attachments from metadata

The collection of attachments of the currently transformed document can be accessed either by `getAttachments()` helper function or directly from document metadata:



{CODE-BLOCK:javascript}
var attachments = this['@metadata']['@attachments'];
{CODE-BLOCK/}

{PANEL/}

{PANEL: Counters}

* Counters are sent automatically when you send a _full_ collection to the destination using an _empty_ script.
* If a script is defined RavenDB doesn't send counters by default.
* To indicate that a counter should also be sent, the [behavior function](../../../server/ongoing-tasks/etl/raven#counter-behavior-function) 
  needs to be defined in the script which decides if the counter should be sent if it's modified
  (e.g. by increment operation). If the relevant function doesn't exist, a counter isn't loaded.
* The reason that counters require special functions is that incrementing a counter _doesn't_ modify the change vector of a related document so the document _isn't_ processed
  by ETL on a change in the counter.
* Another option of sending a counter is to explicitly add it in a script to a loaded document.


---

* [Counter behavior function](../../../server/ongoing-tasks/etl/raven#counter-behavior-function)
* [Adding counter explicitly in a script](../../../server/ongoing-tasks/etl/raven#adding-counter-explicitly-in-a-script)

---

### Counter behavior function

* Every time a counter of a document from a collection that ETL script is defined on is modified then the behavior function is called to check
if the counter should be loaded to a destination database.

{INFO: Important}

The counter behavior function can be defined _only_ for counters of documents from collections that are ETLed to _the same_ collections e.g.: 
a script is defined on `Products` collection and it loads documents to `Products` collection in a destination database using `loadToProducts()` method. 

{INFO/}

The function is defined in the script and should have the following signature:

{CODE-BLOCK:javascript}
function loadCountersOf<CollectionName>Behavior(docId, counterName) {
   return [true | false]; 
}
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **docId** | `string` | The identifier of a deleted document. |
| **<CollectionName>** | `string` | The collection that the ETL script is working on. |
| **counterName** | `string` | The name of the modified counter for that doc. |

| Return | Description |
| - | - |
| **bool** | If the function returns `true` then a change value is propagated to a destination. |

#### Example: Modifying a Counter Named "downloads"

* The following script is defined on the `Products` collection:

{CODE-BLOCK:javascript}

if (this.Category == 'software') {
    loadToProducts({
        ProductName: this.Name
    });
}

function loadCountersOfProductsBehavior(docId, counterName) {
   var doc = load(docId);

   if (doc.Category == 'software' && counterName = 'downloads')
        return true;
}

{CODE-BLOCK/}

---

###Adding counter explicitly in a script

Counter behavior functions typically handle counters of documents 
  that are loaded to the same collection. If a transformation script for `Employees`
  collection specifies that they are loaded to the `People` collection in a target database, 
  then due to document ID generation strategy by ETL process (see [Documents Identifiers](../../../server/ongoing-tasks/etl/raven#documents-identifiers)),
  the counters won't be sent because the final ID of a loaded document isn't known on the source side.  
  
  You can use special functions in the script code to deal with counters on documents that are loaded into different collections:

{CODE-BLOCK:javascript}
var person = loadToPeople({ Name: this.Name + ' ' + this.LastName });

person.addCounter(loadCounter('likes'));
{CODE-BLOCK/}

* The above example indicates that the `likes` counter will be sent together with a document. It uses the following functions to accomplish that:
  - `loadCounter(name)` returns a reference to a counter that is meant be passed to `addCounter()`
  - `<doc>.addCounter(counterRef)` adds a counter to a document that will be sent in the process, `<doc>` is a reference returned by `loadTo<CollectionName>()`

{INFO: Important}

As the transformation script is run on a document update then counters added explicitly (`addCounter()`) will be loaded along with documents _only_ if the document is changed.
It means that incremented counter value won't be sent until a document is modified and the ETL process will run the transformation for it.

{INFO/}


{NOTE: Counter value override by ETL}

Counters sent by the ETL process always _override_ the existing value on the destination. ETL doesn't send an `increment` counter command 
but it **sets the value using a** `put` command.

{NOTE/}

{PANEL/}

{PANEL: Revisions}

Revisions are _not_ sent by the ETL process.  

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

* Deletions can be filtered by defining deletion behavior functions in the script.
   * [Collection specific function deletion handling](../../../server/ongoing-tasks/etl/raven#collection-specific-function-deletion-handling)
   * [Generic function for deletion handling](../../../server/ongoing-tasks/etl/raven#generic-function-for-deletion-handling)

---

### Collection specific function deletion handling

In order to define deletion handling for a specific collection use the following signature:

{CODE-BLOCK:javascript}
function deleteDocumentsOf<CollectionName>Behavior(docId) {
   return [true | false]; 
}
{CODE-BLOCK/}

   - `<CollectionName>` needs to be substituted by a real collection name that ETL script is working on (same convention as for `loadTo` method)
   - The first parameter is the identifier of a deleted document.


---

### Generic function for deletion handling

Another option is the usage of a generic function for deletion handling:

{CODE-BLOCK:javascript}
function deleteDocumentsBehavior(docId, collection) {
   return [true | false]; 
}
{CODE-BLOCK/}

   - The first parameter is the identifier of a deleted document.
   - The second parameter is the name of a collection.

   - A document deletion is propagated to a destination only if the function returns `true`.


## Deletions: Filtering deletions in the destination database

You can further specify the desired deletion behavior by adding filters.

By the time an ETL process runs a delete behavior function, the original document is already deleted from the source. 
It is no longer available. 
You may want the ETL to set up an archive of documents that were deleted from the source, 
or save a part of deleted documents in a separate document for later use.

Following are three examples of ways to save documents for later use when they are deleted from the source database:  

--- 

#### Example - preventing deletions in the destination database

{CODE-BLOCK:javascript}
loadToUsers(this);

function deleteDocumentsOfUsersBehavior(docId) {
    return false;
}
{CODE-BLOCK/}

---

#### Example - storing deletion info in an additional auxiliary document

When you delete a document you can store a deletion marker document that will prevent propagating the deletion by ETL.  

* In the below example if `LocalOnlyDeletions/{docId}` exists then we skip this deletion during ETL.  
* You can add `@expires` tag to the metadata when storing the marker document, so it would be automatically cleaned up after a certain time
  by [the expiration extension](../../../server/extensions/expiration#setting-the-document-expiration-time).

{CODE-BLOCK:javascript}
loadToUsers(this);

function deleteDocumentsOfUsersBehavior(docId) {
    var localOnlyDeletion = load('LocalOnlyDeletions/' + docId);

    return !localOnlyDeletion;
}
{CODE-BLOCK/}

#### Example - filtering deletions using the generic function

If you define ETL for all documents, regardless a collection they belong to, then the generic function can be used to filter out deletions by specifying a collection name

{CODE-BLOCK:javascript}
function deleteDocumentsBehavior(docId, collection) {
    return 'Users' != collection;
}
{CODE-BLOCK/}

{PANEL/}


## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

### Client API

- [How to Add ETL](../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
