# Revisions: Loading Revisions

There are a few methods that allow you to download revisions from a database:   

- **session.advanced().revisions().getFor** 
    - can be used to return all previous revisions for a specified document   
- **session.advanced().revisions().getMetadataFor**
    - can be used to return metadata of all previous revisions for a specified document  
- **session.advanced().revisions().get**
    - can be used to retrieve a revision(s) using a change vector(s)  

{PANEL:getFor}

### Syntax

{CODE:java syntax_1@ClientApi\Session\Revisions\Loading.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE:java example_1_sync@ClientApi\Session\Revisions\Loading.java /}

{PANEL/}

{PANEL:getMetadataFor}

### Syntax

{CODE:java syntax_2@ClientApi\Session\Revisions\Loading.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE:java example_2_sync@ClientApi\Session\Revisions\Loading.java /}

{PANEL/}

{PANEL:get}

### Syntax

{CODE:java syntax_3@ClientApi\Session\Revisions\Loading.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **changeVector** or **changeVectors**| `String` or `String[]` | one or many revision change vectors |

### Example

{CODE:java example_3_sync@ClientApi\Session\Revisions\Loading.java /}

{PANEL/}

## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Revisions in Data Subscriptions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)

### Server

- [Revisions](../../../server/extensions/revisions)
