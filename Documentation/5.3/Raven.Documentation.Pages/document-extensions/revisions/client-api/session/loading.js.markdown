# Revisions: Loading Revisions

There are a few methods that allow you to download revisions from a database:   

- **session.advanced.revisions.getFor()** 
    - can be used to return all previous revisions for a specified document   
- **session.advanced.revisions.getMetadataFor()**
    - can be used to return metadata of all previous revisions for a specified document  
- **session.advanced.revisions.get()**
    - can be used to retrieve a revision(s) using a change vector(s)  

{PANEL:getFor}

### Syntax

{CODE:nodejs syntax_1@document-extensions\revisions\client-api\session\loading.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **options** | object | |
| &nbsp;&nbsp;*start* | number | used for paging - results start page  |
| &nbsp;&nbsp;*pageSize* | number | used for paging - size of the results page |
| &nbsp;&nbsp;*documentType* | class | type of results |
| **callback** | error-first callback | results callback |

### Example

{CODE:nodejs example_1_sync@document-extensions\revisions\client-api\session\loading.js /}

{PANEL/}

{PANEL:getMetadataFor}

### Syntax

{CODE:nodejs syntax_2@document-extensions\revisions\client-api\session\loading.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **options** | object | |
| &nbsp;&nbsp;*start* | number | used for paging - results start page  |
| &nbsp;&nbsp;*pageSize* | number | used for paging - size of the results page |
| &nbsp;&nbsp;*documentType* | class | type of results |
| **callback** | error-first callback | results callback |

### Example

{CODE:nodejs example_2_sync@document-extensions\revisions\client-api\session\loading.js /}

{PANEL/}

{PANEL:get}

### Syntax

{CODE:nodejs syntax_3@document-extensions\revisions\client-api\session\loading.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **changeVector** or **changeVectors**| string or string[] | one or many revision change vectors |
| **documentType** | class | type of results |
| **callback** | error-first callback | results callback |

### Example

{CODE:nodejs example_3_sync@document-extensions\revisions\client-api\session\loading.js /}

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
