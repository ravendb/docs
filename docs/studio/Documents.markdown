## Documents
The data here is relevant to every page that have documents in it.

In all the pages that deal with documents at the top right there are some options:  
![Base Fig 5](/Work/docs/docs/studio/Images/Base5.PNG)

With the paging buttons you can go back and forth over the documents  
![Base Fig 6](/Work/docs/docs/studio/Images/Base6.PNG)

Next to it there is a button that if you press on it you can scale the size of each document from compact that only shows the collection name and the document id to Expanded view that will show the data of the document  
![Base Fig 7](/Work/docs/docs/studio/Images/Base7.PNG)

The button next to it allows you to add a new document
For details on the add new document page go to the Documents section **TODO link**  
![Base Fig 8](/Work/docs/docs/studio/Images/Base8.PNG)

The last button allows you to edit a document by ID after pressing it a message will pop up for you to enter the requested ID, if such ID does not exist you will be redirected to the Documents tab, if it does the Document edit page will load with the requested document (on Document edit page go to the Document section **TODO link**)  
![Base Fig 9](/Work/docs/docs/studio/Images/Base9.PNG)

In the body of the page you have a list of the documents  
![Document Fig 1](/Work/docs/docs/studio/Images/Documents1.PNG)

If you have a database other then the default one, while looking at the default database, you will see a special document titled "Sys Doc" with the name of the non-default database  
If you delete this document you will lose access to the database but the database is not deleted so the data will not be lost.  
![Document Fig 2](/Work/docs/docs/studio/Images/Documents2.PNG)

If you keep the curser over a document for a few moments a tooltip will pop with the data inside of the document  
![Document Fig 3](/Work/docs/docs/studio/Images/Documents3.PNG)


You can enter the document edit page by double clicking on it or pressing on the pencil icon.

### New Document or Edit Document page
The layout for both of these pages is the same.  
On a new document the Raven-entity-Name is set by the text before the '/' (with first letter in capital) so the "albums/626" Raven-entity-Name will be "Albums"  
![Document Fig 4](/Work/docs/docs/studio/Images/Documents4.PNG)

On the top right you have several buttons:

- Save
- Reformat Document: will remove empty line and fix indentations 
- Delete Document: will load a conformation message before deleting
- Refresh: will reload the data from the server

At the top you can enter the Document ID  
![Document Fig 5](/Work/docs/docs/studio/Images/Documents5.PNG)

In the data tab you can add or edit the items in the document (Json syntax)  
![Document Fig 6](/Work/docs/docs/studio/Images/Documents6.PNG)

In the metadata tab you can add or edit the items in the document metadata (Json syntax)  
![Document Fig 7](/Work/docs/docs/studio/Images/Documents7.PNG)

On the right side you have the meta of the document and a list of reference in it.  
If you click on a reference the edit page of that document will load (if available in your database)  
![Document Fig 8](/Work/docs/docs/studio/Images/Documents8.PNG)
