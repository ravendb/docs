﻿# Ongoing Tasks: RavenDB ETL  
---

{NOTE: }

* **RavenDB ETL Task** creates an [ETL](../../../server/ongoing-tasks/etl/basics) 
  process for a given database when the destination is another RavenDB database.  

* The script is executed per document whenever the document is created, modified, and/or deleted.  

* It can be defined in code or using the [Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task).  

* One RavenDB ETL task can have multiple transformation scripts and each script can load to a different collection.  

* Each script can be defined on the source database to trigger ETL from a single collection, 
  multiple selected collections or be applied to **all** documents regardless of the associated collection(s).  

* For the destination cluster to trust the source, you must [pass the .pfx certificate from the source to the destination cluster](../../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates).

* In this page:  
  * [Transformation Script Options](../../../server/ongoing-tasks/etl/raven#transformation-script-options)  
  * [Empty Script](../../../server/ongoing-tasks/etl/raven#empty-script)  
  * [Attachments](../../../server/ongoing-tasks/etl/raven#attachments) 
  * [Counters](../../../server/ongoing-tasks/etl/raven#counters)  
  * [Time Series](../../../server/ongoing-tasks/etl/raven#time-series)  
  * [Revisions](../../../server/ongoing-tasks/etl/raven#revisions)  
  * [Deletions](../../../server/ongoing-tasks/etl/raven#deletions)  

{NOTE/}

---

![Figure 1. Configure RavenDB ETL task](images/raven-etl-setup.png "RavenDB ETL in Studio")

{PANEL: Transformation Script Options}

{NOTE: Loading Documents}

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
{NOTE/}

{NOTE: }

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

* The `load` method loads a document with the specified ID into the script context so it can be transformed.  

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
{NOTE/}

{NOTE: Changing attachment name}

* If `addAttachment()` is called with two arguments, the first one can indicate a new name for an attachment. In the below example attachment `photo`
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
{NOTE/}

{NOTE: Loading non-existent attachment}

* Function `loadAttachment()` returns `null` if a document doesn't have an attachment with a given name. Passing such reference to `addAttachment()` will be no-op and no error will be thrown.

{NOTE/}

{NOTE: Accessing attachments from metadata}

* The collection of attachments of the currently transformed document can be accessed either by `getAttachments()` helper function or directly from document metadata:

{CODE-BLOCK:javascript}
var attachments = this['@metadata']['@attachments'];
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

{PANEL: Counters}

* Counters are sent automatically when you send a _full_ collection to the destination using an _empty_ script.
* If a script is defined RavenDB doesn't send counters by default.
* To indicate that a counter should also be sent, the behavior function needs to be defined in the script which decides if the counter should be sent if it's modified
(e.g. by increment operation). If the relevant function doesn't exist, a counter isn't loaded.
* The reason that counters require special functions is that incrementing a counter _doesn't_ modify the change vector of a related document so the document _isn't_ processed
by ETL on a change in the counter.
* Another option of sending a counter is to explicitly add it in a script to a loaded document.

{NOTE: Counter behavior function}

* Every time a counter of a document from a collection that ETL script is defined on is modified then the behavior function is called to check
if the counter should be loaded to a destination database.

{INFO: Important}

The counter behavior function can be defined _only_ for counters of documents from collections that are ETLed to _the same_ collections e.g.: 
a script is defined on `Products` collection and it loads documents to `Products` collection in a destination database using `loadToProducts()` method. 

{INFO/}

* The function is defined in the script and should have the following signature:

{CODE-BLOCK:javascript}
function loadCountersOf<CollectionName>Behavior(docId, counterName) {
   return [true | false]; 
}
{CODE-BLOCK/}

* `<CollectionName>` needs to be substituted by a real collection name that the ETL script is working on (same convention as for the `loadTo` method)

* The first parameter is the identifier of a document. The second one is the name of a modified counter for that doc.

* If the function returns `true` then a change value is propagated to a destination.

#### Example

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
{NOTE/}

{NOTE: }
####Adding counter explicitly in a script

* The usage of counter behavior functions is limited to dealing with counters of documents 
  that are loaded to the same collection. If a transformation script for `Employees`
  collection specifies that they are loaded to the `People` collection in a target database, 
  then due to document ID generation strategy by ETL process (see [Documents Identifiers](../../../server/ongoing-tasks/etl/raven#documents-identifiers)),
  the counters won't be sent as the final ID of a loaded document isn't known on the source side.  
  
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


{NOTE/}

{NOTE: Counter value override by ETL}

Counters sent by the ETL process always _override_ the existing value on the destination. ETL doesn't send an _increment_ counter command 
but it sets the value using a _put_ command.

{NOTE/}

{PANEL/}

{PANEL: Time Series}

* If the transformation script is empty, time series are transferred along with their 
documents by default.  
* When the script is not empty, ETL can be set for time series using either:  
  * The **time series load behavior function**.  
  * `loadTimeSeries()` and `addTimeSeries()`.  

### Time Series Load Behavior Function

* The time-series behavior function is defined in the script to set the conditions under 
which time-series data is loaded.  
* The load behavior function evaluates each [time-series segment](../../../document-extensions/timeseries/design#segmentation) 
and decides whether to load it to the destination database. ETL only updates the data 
that has changed: if only one time-series entry is modified, only the segment that 
entry belongs to is evaluated.  
* Changes to time-series trigger ETL on both the time-series itself and on the document 
it extends.  
* The function returns either a boolean or an object with two `Date` values that 
specify the range of time-series entries to load.  
* The time-series behavior function can _only_ be applied to time-series whose source 
collection and target collection have the same name. Loading a time-series from an 
Employees collection on the server-side to a Users collection at the target database 
is not possible using the load behavior function.  
* The function should be defined with the following signature:  

{CODE-BLOCK:java}
function loadTimeSeriesOf<collection name>Behavior(docId, timeSeriesName) {
   return [ true | false | <span of time> ];
}
//"span of time" refers to this type: { string?: from, string?: to }
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **\<collection name>** | A part of the function's name | Determines which collection's documents this behavior function applies to. A function named `loadTimeSeriesOfEmployeesBehavior` will apply on all time-series in the collection `Employees` |
| **docId** | `string` | This parameter is used inside the function to refer to the documents' ID |
| **timeSeriesName** | `string` | This parameter is used inside the function to refer to the time series' name |

| Return Value | Description |
| - | - |
| `true` | If the behavior function returns `true`, the given time series segment is loaded. |
| `false` | The given time series segment is not loaded |
| **\<span of time>** | An object with two optional `Date` values: `from` and `to`. If this is the return value, the script loads the time series entries between these two times. If you leave `from` or `to` undefined they default to the start or end of the time series respectively. |

#### Example

* The following script is defined in the `Companies` collection. The behavior function loads 
each document in the collection into the script context using `load(docId)`, then filters 
by the document's `Address.Country` property as well as the time series' name. This 
sends only stock price data for French companies.  

{CODE-BLOCK:javascript}
loadToCompanies(this);

function loadTimeSeriesOfCompaniesBehavior(docId, timeSeriesName) {
   var company = load(docId);

   if (company.Address.Country == 'France' && timeSeriesName = 'StockPrices')
        return true;
}
{CODE-BLOCK/}

### Adding Time Series to Documents

* Time series can be loaded into the script context using `loadTimeSeries()`.  
* Once a time series is loaded into the script, it can be added to a document using 
`AddTimeSeries()`.  

{CODE-BLOCK:javascript}
var employee = loadToEmployees({
    Name: this.Name + ' ' + this.LastName
});

employee.addTimeSeries(loadTimeSeries('StockPrices'));
{CODE-BLOCK/}

{WARNING: }
When using `addTimeSeries`, `addAttachment`, and\or `addCounter`, ETL deletes and 
replaces the existing documents at the destination database, including all time 
series, counters, and attachments.  
{WARNING/}

{INFO: }
Since the transformation script is run on document update, time series added to 
documents using `addTimeSeries()` will be loaded _only_ when the document they 
extend has changed.  
{INFO/}

#### Filtering by start and end date

Both the behavior function and `loadTimeSeries()` accept a start and end date as 
second and third parameters. If these are set, only time-series data within this 
time span is loaded to the destination database.  

{CODE-BLOCK:javascript}
company.addTimeSeries(loadTimeSeries('StockPrices', new Date(2020, 3, 26), new Date(2020, 3, 28)));
{CODE-BLOCK/}

{CODE-BLOCK:javascript}
function loadTimeSeriesOfUsersBehavior(doc, ts)
{
    return {
        from: new Date(2020, 3, 26),
        to: new Date(2020, 3, 28)
    };
};
{CODE-BLOCK/}

{PANEL/}

{PANEL: Revisions}

* Revisions are _not_ sent by the ETL process.  
  But, if revisions are configured on the destination database, then when the target document is overwritten by the ETL process a revision will be created as expected.  
{PANEL/}

{PANEL: Deletions}

Upon source document modifications, ETL is set to delete and replace the destination documents by default.  

If you want to control the way deletions are handled in the destination database, 
you can change the default settings with the configurable functions described in this section.

* [Why documents are deleted by default](../../../server/ongoing-tasks/etl/raven#deletions-why-documents-are-deleted-by-default-in-the-destination-database)
* [Collection specific function](../../../server/ongoing-tasks/etl/raven#deletions-collection-specific-function)
* [Generic function](../../../server/ongoing-tasks/etl/raven#deletions-generic-function)
* [Filtering deletions in the destination database](../../../server/ongoing-tasks/etl/raven#deletions-filtering-deletions-in-the-destination-database)
* [Deletions Example: ETL script with deletion behavior defined](../../../server/ongoing-tasks/etl/raven#deletions-example-etl-script-with-deletion-behavior-defined)

## Deletions: Why documents are deleted by default in the destination database

### Preventing duplication

To prevent duplication, we delete the documents in the destination by default before loading the 
updated documents that replace the deleted ones.  
If the document is deleted in the source, RavenDB also deletes it in the destination by default.

Some developers prefer to control the deletes so that, for example, a delete in the source will not cause a delete in the destination, 
or to preserve a history of the document in the destination.  

The functions in this section were created to allow developers this control. 

### Identifiers change when destination collections are different

If we load a document to the same collection as the source, then the ID is preserved and no special approach is needed. Deletion in the source results in 
sending a single delete command in the destination for a given ID.  
  
But, when we load documents to different collections, then the [ID](../../../server/ongoing-tasks/etl/raven#documents-identifiers) is different.  
The source isn't aware of the new IDs created. This forces us to load new documents with incremented IDs instead of overwriting the fields in existing documents.  

* Each updated version of the document gets a [server generated ID](../../../client-api/document-identifiers/working-with-document-identifiers#server-side-generated-ids)
  in which the number at the end is incremented with each version.  

    For example: 
    `"...profile/0000000000000000019-B"` will become `".../profile/0000000000000000020-B"`  
    The word before the number is the collection name and the letter after the number is the node.  
    In this case, the document's collection is "Profile", which is in a database in node "B", 
    and which has been updated via ETL 20 times.

* If the ETL is defined to load the documents to more than one collection, 
  by default it will delete, and if it's not deleted in the source, it will replace all of the documents with the same prefix.  

     For example:  
     Document `employees/1-A` is processed by ETL and put into the `People` and `Sales` collections with IDs:  
     `employees/1-A/people/0000000000000000001-A` and `employees/1-A/sales/0000000000000000001-A`.  
     Deletion or modification of the `employees/1-A` document on the source side triggers sending a command that deletes **all** documents 
     having the following prefix in their ID: `employees/1-A`.  

---

Deletions can be controlled by defining deletion behavior functions in the ETL script.

* See a [sample ETL script with deletion behavior defined](../../../server/ongoing-tasks/etl/raven#deletions-example-etl-script-with-deletion-behavior-defined).

## Deletions: Collection specific function

To define deletion handling when the source and destination collections are the same, use the following sample. 

{CODE-BLOCK:javascript}
function deleteDocumentsOf<CollectionName>Behavior(docId, deleted) {
    // If any document in the specified source collection is modified but is not deleted,  
    // then the ETL will not send a delete command to the destination, thus saving a history of it.  
    if (deleted == false) 
    return false; 
    // If the source document was deleted, the destination will also be deleted 
    // including all of it's version history. 
    else return true;
}
{CODE-BLOCK/}

`<CollectionName>` needs to be substituted by a real collection name that the ETL script is working on (same convention as for [loadTo](../../../server/ongoing-tasks/etl/raven#transformation-script-options) 
method).  
e.g. `function deleteDocumentsOfOrdersBehavior(docId, deleted) {return false;}`  

| Parameter | Type | Description | Notes |
| - | - | - | - |
| **docId** | `string` | The identifier of a deleted document. |  |
| **deleted** | `bool` | If you don't include the `deleted` parameter, RavenDB will execute the function without checking if the document was deleted from the source database. <br/> If you include `deleted`, RavenDB will check if the document was indeed deleted or just updated. | Optional |

| Return Value | Description |
| - | - |
| **true** | The document will be deleted from the destination database. |
| **false** | The document will not be deleted from the destination database. |

Leaving out the `deleted` parameter will not allow you to check if the source document was deleted and will 
trigger the command regardless of whether the source document was deleted.  

* This means that if the method returns `true` and  
  if a source document was updated, but not deleted,  
  it will be deleted in the destination before the updated document with a new ID is loaded to replace the previous one.  
   * If the source document was deleted, the destination will also be deleted.
* If the method returns `false`, a 'historical' set of the document will accumulate in the destination collection every time 
  the source document is updated.  
  The number at the end of the [ID will automatically increment](../../../server/ongoing-tasks/etl/raven#identifiers-change-when-destination-collections-are-different) with every new version. 
   * If the document was deleted from the source, the set of old versions of the document will remain in the destination 
     because the delete behavior returns `false`.


## Deletions: Generic function

There is also a generic function to control deletion on different collections.

{CODE-BLOCK:javascript}
function deleteDocumentsBehavior(docId, collection, deleted) {
    // If any document in the specified source collection is modified but is not deleted,  
    // then the ETL will not send a delete command to the destination collection "Users"
    // (the old document versions will remain and a new version will be stored with an incremented ID)
    if (collection == "Users" && deleted == false) 
        return false; 
    // If the source document was deleted, delete the entire set of versions from the destination.
    else return true;
}
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **docId** | `string` | The identifier of a deleted document. |
| **collection** | `string` | The name of a collection. |
| **deleted** | `bool` | Optional and therefore doesn't affect existing code. If you don't include the `deleted` parameter, RavenDB will execute the function without checking if the document was deleted from the source database. <br/> If you include `deleted`, RavenDB will check if the document was indeed deleted or just updated. 

| Return Value | Description |
| - | - |
| **true** | The document will be deleted from the destination database. |
| **false** | The document will not be deleted from the destination database. |


Leaving out the `deleted` parameter will not allow you to check if the source document was deleted and will 
trigger the command regardless of whether the source document was deleted.  

* This means that if the method returns `true` and  
  if a source document was updated, but not deleted,  
  it will be deleted in the destination before the updated document with an incremented ID is loaded to replace the previous one.  
   * If the source document was deleted, the destination will also be deleted.
* If the method returns `false`, a 'historical' set of the document will accumulate in the destination collection every time 
  the source document is updated.  
  The number at the end of the ID will automatically increment with every new version. 
   * If the document was deleted from the source, the set of old versions of the document will remain in the destination 
     because the delete behavior returns `false`.

## Deletions: Filtering deletions in the destination database

You can further specify the desired deletion behavior by adding filters.

By the time an ETL process runs a delete behavior function, a document is already deleted. 
If you want to filter deletions, you need some way to store that information
to be able to determine if a document should be deleted in the delete behavior function.

#### Filtering out all deletions:

{CODE-BLOCK:javascript}
loadToUsers(this);

function deleteDocumentsOfUsersBehavior(docId) {
    return false;
}
{CODE-BLOCK/}

#### Storing deletion info in an additional document:

When you delete a document you can store a deletion marker document that will prevent the deletion by ETL. 
In the below example, if `LocalOnlyDeletions/{docId}` exists then we skip this deletion during ETL. 
You can add the `@expires` tag to the metadata when storing the marker document, 
so it would be automatically cleaned up after a certain time
by [the expiration extension](../../../server/extensions/expiration).

{CODE-BLOCK:javascript}
loadToUsers(this);

function deleteDocumentsOfUsersBehavior(docId) {
    var localOnlyDeletion = load('LocalOnlyDeletions/' + docId);

    return !localOnlyDeletion;
}
{CODE-BLOCK/}

#### When ETL is set on the entire database, but you want to filter deletions by certain collections:

If you define ETL for all documents, regardless of the collection they belong to, then the 
generic function can filter deletions by collection name.

{CODE-BLOCK:javascript}
function deleteDocumentsBehavior(docId, collection, deleted) {
    return 'Users' != collection;
}
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **docId** | `string` | The identifier of a deleted document. |
| **collection** | `string` | The name of a collection. |
| **deleted** | `bool` | Optional and therefore doesn't affect existing code. If you don't include the `deleted` parameter, RavenDB will execute the function without checking if the document was deleted from the source database. <br/> If you include `deleted`, RavenDB will check if the document was indeed deleted or just updated.  If you set `return true` and the document was deleted in the source database, it will also delete the document in the destination database.  |

| Return | Description |
| - | - |
| **bool** | If the returned value is `true`, the document will be deleted. |


## Deletions Example: ETL script with deletion behavior defined

The following example will check if the source document was deleted or just updated before 
loading the transformed document. This function can be used if the destination collection is the same or different from the source.  

Any modification to a source document where an ETL is defined on its collection will trigger an ETL operation.

{CODE-BLOCK:javascript}

// Define ETL to destination collection "productsHistory".
// Defined to update only the name of the item, a link to order the product, and the supplier's phone number
loadToproductsHistory ({
    Name: this.Name + "updated data.."
    SupplierOrderLink: this.SupplierOrderLink + "updated data.."
    SupplierPhone: this.SupplierPhone + "updated data.."
});

function deleteDocumentsBehavior(docId, collection, deleted) {
    // Prevents document deletions from destination collection "productsHistory" if source document is deleted.
    // If the source document information is updated (NOT deleted), the script will delete then
    // replace the destination document (with an incremented ID) to keep it current.
    if (collection = "productsHistory" && deleted = true)
        return false; 
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

### Document Extensions

- [Attachments](../../../document-extensions/attachments/what-are-attachments)
- [Counters](../../../document-extensions/counters/overview)
- [Time Series](../../../document-extensions/timeseries/overview)
- [Revisions](../../../document-extensions/revisions/overview)

