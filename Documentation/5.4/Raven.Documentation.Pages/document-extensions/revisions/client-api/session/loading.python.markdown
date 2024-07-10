# Get Revisions

---

{NOTE: }

* Using the Advanced Session methods you can **retrieve revisions and their metadata**  
  from the database for the specified document.  

* These methods can also be executed lazily, see [get revisions lazily](../../../../client-api/session/how-to/perform-operations-lazily#getRevisions).   
  
* In this page:  
   * [Get all revisions](../../../../document-extensions/revisions/client-api/session/loading#get-all-revisions)  
   * [Get revisions metadata](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-metadata)  
   * [Get revisions by creation time](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-creation-time)  
   * [Get revisions by change vector](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-change-vector)  

{NOTE/}

---

{PANEL: Get all revisions}

Use `get_for` to retrieve all of the revisions currently kept for the specified document.

* **Example**:
  {CODE:python example_1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}
* **Syntax**:
  {CODE:python syntax_1@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}

     | Parameters | Type | Description |
     | - | - |- |
     | **id_** | `str` | Document ID for which to retrieve revisions |
     | **object_type** | `Type[_T]` | The type of the object whose revisions we want to retrieve |
     | **start** | `int` | First revision to retrieve, used for paging |
     | **page_size** | `int` | Number of revisions to retrieve per results page |

{PANEL/}

{PANEL: Get revisions metadata}

Use `get_metadata_for` to retrieve the metadata for all the revisions currently kept for the specified document.

* **Example**:
  {CODE:python example_2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}
* **Syntax**:
  {CODE:python syntax_2@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}

     | Parameters | Type | Description |
     | - | - |- |
     | **id_** | `str` | Document ID for which to retrieve revisions' metadata |
     | **start** | `int` | First revision to retrieve metadata for, used for paging |
     | **page_size** | `int` | Number of revisions to retrieve per results page |

{PANEL/}

{PANEL: Get revisions by creation time}

Use `get_by_before_date` to retrieve a revision by its **creation time**.

* **Example**:
  {CODE:python example_3_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}
* **Syntax**:
  {CODE:python syntax_3@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}

     | Parameter | Type | Description |
     | - | - | - |
     | **id_** | `str` | The ID of the document whose revisions we want to retrieve by creation time |
     | **before_date** | `datetime.datetime` | Revision creation time |
     | **object_type** | `Type[_T]` | The type of the object whose revisions we want to retrieve |


{PANEL/}

{PANEL: Get revisions by change vector}

To retrieve a revision or multiple revisions by **change vectors**, get the 
change vector using `get_metadata_for` and the revisions using `get_by_change_vector`.  

* **Example**:
  {CODE:python example_4_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}
* **Syntax**:
  {CODE:python syntax_4@DocumentExtensions\Revisions\ClientAPI\Session\Loading.py /}

     | Parameter | Type | Description |
     | - | - | - |
     | **change_vectors** | `List[str]` | A list of change vector strings |
     | **object_type** | `Type[_T]` | The types of the objects whose revisions we want to retrieve |

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
