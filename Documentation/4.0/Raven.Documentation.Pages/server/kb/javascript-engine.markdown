# Knowledge Base : JavaScript Engine
---

{NOTE: }

Several RavenDB mechanisms incorporate JavaScript scripts:  

* [RQL projections](../../indexes/querying/projections)  
* [Subscriptions](../../client-api/data-subscriptions/creation/examples#create-subscription-with-filtering-and-projection)    
* [ETL](../../server/ongoing-tasks/etl/basics)    
* [Smuggler (data import/export)](../../client-api/smuggler/what-is-smuggler#transformscript)    
* [Single](../../client-api/operations/patching/single-document) or [Set based](../../client-api/operations/patching/set-based) document patches

In order to execute JavaScript code, RavenDB uses [Jint](http://github.com/sebastienros/jint), an open source JavaScript interpreter, supporting ECMAScript 5.1.  

In this page:  
* [How RavenDB uses Jint](../../server/kb/javascript-engine#how-ravendb-uses-jint)  
* [Predefined JavaScript functions](../../server/kb/javascript-engine#predefined-javascript-functions)  


{NOTE/}

---

{PANEL: How RavenDB uses Jint}
As mentioned before, RavenDB uses [Jint](https://github.com/sebastienros/jint) to execute JavaScript code in a variety of operations.  
In order to perform an operation, Jint receives a function to run and a single document to process, therefore, the processing context of Jint is a single document and there is no "long-term" execution context, even when it may look like it, in patch operations.  
Jint engine initialization is an expansive operation, therefore, RavenDB caches Jint instances according to the user defined scripts and reuses them.  


{INFO: Execution limitations}
RavenDB limits the amount of statements that can be performed for each document processing, it's default value is 10,000 and it can be set using the [Patching.MaxStepsForScript](../../server/configuration/patching-configuration#patching.maxstepsforscript) configuration.  
RavenDB limits the amount of cached Jint engines, it's default value is 2048, and it can be set using [Patching.MaxNumberOfCachedScripts](../../server/configuration/patching-configuration#patching.maxstepsforscript).  
RavenDB limits the depth of recursive calls to 64, this value is a constant.  
{INFO /}
{PANEL/}

{PANEL: Predefined JavaScript functions}
RavenDB introduced a set of predefined function, in addition to Jint's ECMAScript.1 implementation.

| Method Signature| Return type | Description |
|--------|:---|-------------| 
| **id(document)** | `string` | Returns document's ID. |
| **load(documentId)** | `object` | Returns the document with the given ID. |
| **loadPath(document, pathString)** | `Task` | Returns the document(s) according to the IDs that can be found in the given `document`, in the path `pathString`. The `pathString` can be of a simple Foo.Bar form, in that case, a single document will be returned. It also be of the form Foo.Bars[].Buzz, in that case it will return an array of documents, answering the path . |
| **cmpxchg(compareExchangeKey)** | `object` | Returns stored  [Compare Exchange](../../client-api/operations/compare-exchange/overview) value for the received key. |
| **getMetadata(document)** | `object` | Returns document's metadata, along with it's `ChangeVector`, `ID` and `LastModified`. |
| **lastModified(document)** | `long` | Returns document's last modified metadata value as total miliseconds of UTC . |
| **include(documentId)** | `Task<string>` | Used for RQL [queries](../../indexes/querying/what-is-rql) in order to include the document with the given ID with the results |
| **del(documentId)** | `void` | Used in patches, deletes the document with the given ID. |
| **put(documentId, document, [optional]changeVectorString)** | `Task` | Used in patches, creates or updates a document with the given ID. In order to generate a new document ID it's possible to use "[collectionPrefix]/" [Server-Side ID](../../server/kb/document-identifier-generation#server-side-id) notation<sup>[[ex]](../../client-api/operations/patching/single-document#add-document)</sup>. <br/>This function can also be used to clone an existing document (Note: Attachments will not be attached to the clone)<sup>[[ex]](../../client-api/operations/patching/single-document#clone-document)</sup>.  |
| **String.prototype.startsWith(searchString, position)** | `bool` | Returns true if at `position` the string starts with `searchString`. |
| **String.prototype.endsWith(searchString, position)** | `bool` | Returns true if at `position` the string end with `searchString`  . |
| **String.prototype.padStart(targetLength, padString)** | `string` | Returns a new string, that padded from it's start by the string `padString`(or white-space by default), until it reaches the length `targetlength`. The function will repeat `padString` if needed. |
| **String.prototype.padEnd(targetLength, padString)** | `string` | Returns a new string, that padded from it's end by the string `padString`(or white-space by default), until it reaches the length `targetlength`. The function will repeat `padString` if needed. |
| **String.prototype.format(arg1, arg2, arg3 ...)** | string | Returns "formatted" string, that replaces all occurrences of `{[number]}` with an argument in the `number`s place (using a zero based index). |
| **startsWith(inputString, prefix)** | `bool` | Returns true if inputString`` starts with prefix``. |
| **endsWith(inputString, suffix)** | `bool` | Returns true if `inputString` ends with `suffix`. |
| **regex(inputString, regex)** | `bool` | Returns true if `inputString` matches `regex` regular expression. |
| **Array.prototype.find(function callback)** | Array's element | Returns the first array element, for which the `callback` function returns `true`. |
| **Object.map(input, function mapFunction, context)** | `Array` | Returns an array containing result of mapFunction process result of all `input`'s properties (items, if it's an array). The `mapFunction`'s signature is `function(itemValue, itemKey)`  |
| **Raven_Min(num1, num2)** | `bool` | Find minimum out of num1 and num2. Parameters can be numbers or strings, but there is no raw number support (see `scalarToRawString` below). Strings will be parsed to double upon processing |
| **Raven_Max(num1, num2)** | `bool` | Find maximum out of num1 and num2. Parameters can be numbers or strings, but there is no raw number support (see `scalarToRawString` below). Strings will be parsed to double upon processing |
| **convertJsTimeToTimeSpanString(ticksNumber)** | `bool` | Returns human readable TimeSpan of the received `ticksNumber`. |
| **scalarToRawString(document, lambdaToField)** | Raw field value (`LazyStringValue` for strings, `LazyNumberValue` for floating point numbers). | Returns raw representation of a field. Useful when working with numbers that exceeds `double`'s numeric or accuracy range. See [Numbers in Jint](../../server/kb/numbers-in-ravendb). Also usefull for better memory consumption when projecting big string values. Note: returned value is immutable |
| **output(message)** | `void` | Used for [single document patches](../../client-api/operations/patching/single-document) debug. |
{PANEL/}

## Related articles

### Patching

- [How to Perform Single Document Patch Operations](../../client-api/operations/patching/single-document)
- [How to Perform Set Based Operations on Documents](../../client-api/operations/patching/set-based)

### Querying

- [Projections](../../indexes/querying/projections)

### Knowledge Base

- [Numbers in RavenDB](../../server/kb/numbers-in-ravendb)
