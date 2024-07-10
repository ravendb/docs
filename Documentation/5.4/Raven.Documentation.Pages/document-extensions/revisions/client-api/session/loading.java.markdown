# Get Revisions

There are a few methods that allow you to retrieve revisions from a database:   

- **session.advanced().revisions().getFor** 
    - can be used to return all previous revisions for a specified document   
- **session.advanced().revisions().getMetadataFor**
    - can be used to return metadata of all previous revisions for a specified document  
- **session.advanced().revisions().get**
    - can be used to retrieve a revision(s) using a change vector(s)  

{PANEL:getFor}

### Syntax

{CODE:java syntax_1@DocumentExtensions\Revisions\ClientAPI\Session\Loading.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE:java example_1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.java /}

{PANEL/}

{PANEL:getMetadataFor}

### Syntax

{CODE:java syntax_2@DocumentExtensions\Revisions\ClientAPI\Session\Loading.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE:java example_2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.java /}

{PANEL/}

{PANEL:get}

### Syntax

{CODE:java syntax_3@DocumentExtensions\Revisions\ClientAPI\Session\Loading.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **changeVector** or **changeVectors**| `String` or `String[]` | one or many revision change vectors |

### Example

{CODE:java example_3_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.java /}

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
