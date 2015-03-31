#Loading files

There are two overloads of the `LoadFileAsync` method used to load a single or multiple files in a single call.

##Syntax

{CODE load_file_0@FileSystem\ClientApi\Session\LoadingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full file path to load |

| Return Value | |
| ------------- | ------------- |
| **Task&lt;FileHeader&gt;** | The file instance represented by the [`FileHeader`]() object or `null` if a file does not exist. |

<br />

{CODE load_file_1@FileSystem\ClientApi\Session\LoadingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **paths** | IEnumerable&lt;string&gt; | The collection of the file paths to load |

| Return Value | |
| ------------- | ------------- |
| **Task&lt;FileHeader[]&gt;** | The array of file instances, each represented by the[`FileHeader`]() object or `null` if a file does not exist. |

{INFO: File headers}
Note that the load method does not download file content. It fetches only the header, which is a basic session entity object.
{INFO/}


##Example I

{CODE load_file_2@FileSystem\ClientApi\Session\LoadingFiles.cs /}

##Example II

If you pass multiple paths, the returned array contains headers in exactly the same order as  the given paths. 
If a file does not exist, the value at the appropriate position in the array will be null.

{CODE load_file_3@FileSystem\ClientApi\Session\LoadingFiles.cs /}