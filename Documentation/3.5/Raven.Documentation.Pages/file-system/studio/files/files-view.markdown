#Files view

The main view in the file system studio is a file explorer that allows you to browse and modify your files. It has the standard layout where
the directory structure is visible on the left side while a list files of the selected directory occupies right side of the view. The name of the file system
in the directory structure means the root directory.

![Figure 1. Studio. Files View. Explorer](images/files-view-explorer.png)  

{NOTE: Note}

$revisions folder is only visible when versioning bundle is enabled and it contain all revisions files.

{NOTE/}

##Creating a folder

Directories in RavenFS are created automatically based on file names which are always full paths. The studio provides a feature to create
folders virtually in order to build the appropriate structure before a file upload. Use `New Folder` button located above the directory 
structure to create a new folder (it will become a subfolder of the currently selected one):

![Figure 2. Studio. Files View. Create folder](images/files-view-create-folder.png)  

The folder remains virtual as long as it is empty. If you switch between studio pages without uploading any file there then it will disappear.

![Figure 3. Studio. Files View. Virtual folder](images/files-view-empty-folder.png)  

To *persist* this folder you need to add a file there.

##Deleting a folder

Choose which folder you want to delete and click the delete button

![Figure 4. Studio. Files View. Delete a folder](images/files-view-delete-folder.png)

##Uploading a file

To upload a file you have to select the folder where it should to be placed and click the upload button:

![Figure 5. Studio. Files View. File upload](images/files-view-upload.png)

A status of the upload operation is tracked by `Upload Queue` panel. If it finishes successfully it will be visible on the file list.

![Figure 6. Studio. Files View. File uploaded](images/files-view-file-uploaded.png)

Upload queue status colors:

<style type="text/css">
.rect_legend
{
	width: 15px;
	height: 15px;
	border: solid 1px black;
}
</style>

| Color | Description|
| ------------- | ----- |
| <div class="rect_legend" style="background-color: #772953" /> | No operation was made |
| <div class="rect_legend" style="background-color:#28b62c" /> | Upload was successful |
| <div class="rect_legend" style="background-color: #DF382C" /> | Upload failed |

##Downloading a file

Choose which file you want to get and click the download button:

![Figure 7. Studio. Files View. Download a file](images/files-view-download-file.png)

##Deleting files

You can select multiple files and delete them at once:

![Figure 8. Studio. Files View. Delete a file](images/files-view-delete-files.png)

##Editing a file

Select a single document and click the edit button or click on its name to navigate you to [File Edit View](./file-edit-view).

![Figure 9. Studio. Files View. Edit a file](images/files-view-edit-file.png)

##Renaming a file

Select a single document and click the rename button.

![Figure 10. Studio. Files View. Rename a file](images/files-view-rename-file.png)

{NOTE: Note}

Multiple files can be selected using shift

{NOTE/}
