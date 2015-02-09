#Indexing

The file system allows you to search files by using [Lucene query syntax](http://lucene.apache.org/core/old_versioned_docs/versions/3_0_0/queryparsersyntax.html). You can look for a file by using:

* name,
* size,
* directory,
* date of modification,
* any user defined metadata.

The more files and corresponded metadata you add the more search terms you can use to build your search query. All available search fields you can find by using [Client API](TODO arek). Below there is an explanation of built-in search fields: 

Let's assume that we have a file `documents/pictures/wallpaper.jpg`, then default search terms would have the values:

* `__key` - the full name of the file: `documents/pictures/wallpaper.jpg`,
* `__fileName` - the last part of file path `wallpaper.jpg`,
* `__rfileName` - the *reversed* version of `__fileName` (to support queries that ends with the wildcard): `gpj.repapllaw`,
* `__directory` - the full directory path: `/documents/pictures`,
* `__rdirectory` - the *reversed* directory path (to support queries that ends with the wildcard): `serutcip/stnemucod/`
* `__directoryName` - the list of directories associated with the file: `/documents/pictures`, `/documents`, `/`,
* `__rdirectoryName` - the list of *reversed* paths of directories associated with the file (to support queries that ends with the wildcard): `serutcip/stnemucod/`, `stnemucod/`, `/`,
* `__level` - the nesting level: `3`,
* `__modified` - the date of file indexing (the date index format is *yyyy-MM-dd_HH-mm-ss*),
* `__size` - the file length (in bytes) stored as string (format D20 used),
* `__size_numeric` - the file length (in bytes) stored as numeric fields, what allows to search by range.

A sample query to find all files under `/documents` directory (or nested) that name ends with `.jpg` and size is greater or equal than 1MB:

`__directoryName:/documents AND __rfileName:gpj.*  AND __size_numeric:[1048576 TO *]`

The easiest way to search for files from the code is to use either [Client API](../client-api/indexTODO arek) methods.

Searching is also supported by studio, where you will find useful predefined search filters:

![Figure 1: Search filters](images\indexing_studio.png)