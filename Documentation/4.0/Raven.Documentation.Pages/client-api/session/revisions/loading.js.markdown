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

{CODE:nodejs syntax_1@client-api\session\revisions\loading.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **options** | object | |
| &nbsp;&nbsp;*start* | number | used for paging - results start page  |
| &nbsp;&nbsp;*pageSize* | number | used for paging - size of the results page |
| &nbsp;&nbsp;*documentType* | class | type of results |
| **callback** | error-first callback | results callback |

### Example

{CODE:nodejs example_1_sync@client-api\session\revisions\loading.js /}

{PANEL/}

{PANEL:getMetadataFor}

### Syntax

{CODE:nodejs syntax_2@client-api\session\revisions\loading.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **options** | object | |
| &nbsp;&nbsp;*start* | number | used for paging - results start page  |
| &nbsp;&nbsp;*pageSize* | number | used for paging - size of the results page |
| &nbsp;&nbsp;*documentType* | class | type of results |
| **callback** | error-first callback | results callback |

### Example

{CODE:nodejs example_2_sync@client-api\session\revisions\loading.js /}

{PANEL/}

{PANEL:get}

### Syntax

{CODE:nodejs syntax_3@client-api\session\revisions\loading.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **changeVector** or **changeVectors**| string or string[] | one or many revision change vectors |
| **documentType** | class | type of results |
| **callback** | error-first callback | results callback |

### Example

{CODE:nodejs example_3_sync@client-api\session\revisions\loading.js /}

{PANEL/}

## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Revisions in Data Subscriptions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)

### Server

- [Revisions](../../../server/extensions/revisions)
