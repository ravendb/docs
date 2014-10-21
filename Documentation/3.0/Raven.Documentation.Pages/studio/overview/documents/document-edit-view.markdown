# Documents : Document Edit View

Whenever you want to edit a document (e.g. by click on Id in [Documents View](../../../studio/overview/documents/documents-view)) you will be redirected to `Document Edit View` which allows you to view or edit document content and it's metadata.

## Action Bar

Action Bar contains following buttons:

- `Save` - save document on server (enabled when changes are detected),
- `Refresh` - loads document from server,
- `Format` - adjusts JSON formatting,
- `New Line Mode` - toggles new line visibility,
- `Auto-Collapse` - toggles if nested complex properties should be collapsed,
- `Delete` - deletes document from server,
- `Pager` - pages through documents

![Figure 1. Studio. Document Edit View. Action Bar.](images/document-edit-view-action-bar.png)  

## Document Key

Under the `Action Bar` there is a textbox with key under which document is stored on server.

![Figure 2. Studio. Document Edit View. Document Key.](images/document-edit-view-document-key.png)  

{INFO:Key generation}

- When only key prefix is typed e.g. `products/` then during save next available key will be assigned for that prefix.
- If key is left blank, then during save the document will have a GUID assigned as key.

{INFO/}

{WARNING Changing document key to a different one and saving does not overwrite existing document. It creates a new one (or overwrites document found under new key). /}

## Document Editor

Document data and associated metadata (e.g. collection association - `Raven-Entity-Name`) can be manipulated in editor, by switching to appropriate tab.

![Figure 3. Studio. Document Edit View. Data tab.](images/document-edit-view-data-tab.png)  

![Figure 4. Studio. Document Edit View. Metadata tab.](images/document-edit-view-metadata-tab.png)  

## Metadata

Metadata section can be found on the right side of the view, next to editor. You can found all metadata associated with the document along with its current etag, last modified date and size in KB. If document contains any related documents, their keys are displayed there and if it is not the first viewed document, the list of recent ones is available too.

![Figure 5. Studio. Document Edit View. Metadata.](images/document-edit-view-metadata.png)  



