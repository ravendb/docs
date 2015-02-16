#Synchronization types

The synchronization is triggered by a file modification or executed as a periodic task. In order to minimize amount of data needed to synchronize a file, 
RavenFS determines kind of file differences to figure out what type of work needs to be done. It is implemented by comparing file metadata from both file systems, 
what means that the source needs to retrieve metadata from the destination before it starts to push any bytes. There are four types of synchronizations:

* [Content update](#content-update)
* [Metadata update](#metadata-update)
* [Rename](#rename)
* [Delete](#delete)


##Content update

There are two types of actions that are considered as content updates:

* a new file upload,
* an upload of an existing file with changed content.

In the first case is simple, we have to transfer the entire file. The second scenario is much more interesting and the implemented approach is explained below.

###How to detect that content has changed?

Behind the scenes RavenFS calculates a file hash based on its content every time you upload it. The result is stored under `Content-MD5` key in file metadata.
The source file system is able to determine whether content of a local file and a remote one are different just by comparing these metadata records.

###Remote Differential Compression

In order to synchronize large files in an efficient way and minimizing the amount of data transferred between source and destination file systems RavenFS uses Remote Differential Compression (RDC).
It is the built-in Windows feature. Below there are presented just basics of RDC algorithms that will allow you to understand how a file content synchronization works.
To explore RDC in details take a look [here](https://msdn.microsoft.com/en-us/library/dd357428%28v=prot.20%29.aspx).

####Overview

Remote Differential Compression takes care of detection of file parts where content is different or the same between two files. The result of RDC detection is a list
called *a needs list*, which contains items that describe how to construct the target file on a destination system by using parts sent by the source and chunks
of the file already existing there. Each element of the needs list consists of the accurate byte range and information which version of the file it concerns (*source* or *seed*).
Based on the needs list the source server pushes chunks of the file marked as *source* to the destination. The transferred data and the existing file chunks
(marked as *seed* in the list) are combined according to the order they appear on the needs list to create the synchronized file.

RDC is able to calculate the mentioned byte ranges very precisely what allows to reduce the amount of sent data, especially when a small part of file is changed
(it doesn't matter if it happens at the beginning / end of the file or in the middle of it).

####Signatures

RDC divides a file to chunks. Each chunk has an assigned hash value that together with the chunk size creates a signature. A collection of file signatures 
has full information about a file content. The source retrieves information about the file content existing on the destination by downloading its signatures.
By comparing its own signatures with the downloaded ones, the source is able to generate the needs list.

{INFO: Signatures synchronization}
A really large file can have very big signatures. Therefore they are now directly downloaded. RavenFS internally also synchronizes the signatures to speed up the entire operation and reduce the amount of exchanged data (the signature is smaller than its related file).
{INFO/}

###HTTP request format

An upload of missing file chunks between the source and destination file systems is performed by using HTTP multipart POST message. 
The destination server exposes `/synchronization/MultipartProceed` endpoint which accepts only RavenFS specific formatted MIME multipart content. 
Below there is a sample synchronization request sent by the server (`Content-Type` request header has to be set as `multipart/form-data; boundary=syncing`).

<code>
--syncing   
Content-Disposition: form-data; Syncing-need-type=seed; Syncing-range-from=0; Syncing-range-to=407029   
Content-Type: plain/text   

--syncing   
Content-Disposition: file; Syncing-need-type=source; Syncing-range-from=407030; Syncing-range-to=412242   
Content-Type: application/octet-stream   
   
[... data from byte 407030 to 412242 goes here...]   
   
--syncing--
</code>

Note that first seed part is empty because it involves the destination file chunk, it contains only information about byte range that needs to be copied from the existing file.
The second one is *source* part and it contains a range of file content bytes.

If the file does not exist on the destination file system, then the synchronization request consists of one source part that contains entire file data.

###Temporary sync file

RavenFS assumes that an error during the synchronization operation may happen at any time. To avoid the scenario that the failed synchronization damaged the synced file on the destination system
we build the target file under `[FILENAME].downloading` name. If the synchronization finishes without any error, then `[FILENAME]` file will be deleted and `[FILENAME].downloading` renamed to `[FILENAME]`.


##Metadata update

If differences between the source file and the destination one is only the metadata, then the source system creates a POST request just with file metadata placed as request headers.
The destination server has the dedicated endpoint for that: `/synchronization/updateMetadata`. The sent metadata will override the existing one while `ETag` and `Last-Modified` 
will get new values (as usual after every successful synchronization).

##Rename

RavenFS is able to recognize that a file was renamed and in order to reflect that change on destinations it doesn't need to transfer the file content at all. 
Synchronizing the rename operation is forcing destination file systems to renames the file according to a given name. The appropriate HTTP endpoint on a destination server is `/synchronization/rename`.

##Delete

To deal with synchronization of deletions the file system keeps tombstones with a delete marker for each deleted file. Then it is able to determine that the destination node has files that were already deleted on the source.
The delete synchronization is sending a POST message to destination server to `/synchronization/delete` endpoint.
 