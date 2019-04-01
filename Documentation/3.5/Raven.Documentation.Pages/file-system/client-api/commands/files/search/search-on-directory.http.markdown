#Commands: SearchOnDirectory

In order to look for files located in a given directory use the standard [search method](./search) with `__directoryName` and `__level` [terms](../../../../indexing)
specified in the query parameter.

## Example

Download all files located in `/movies` directory which names end with `.avi`:

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/search?query=__directory:/movies%20AND%20__level:2%20AND%20__rfileName:iva.*
< HTTP/1.1 200 OK
{
    "Files":[
        {
            "Metadata":{
                // ...
            },
            "OriginalMetadata":{
                // ...
            },
            "TotalSize":1338,
            "UploadedSize":1338,
            "LastModified":"2015-04-14T08:08:22.8144527+00:00",
            "CreationDate":"2015-04-14T08:08:22.6054409+00:00",
            "Etag":"00000000-0000-0001-0000-000000000008",
            "FullPath":"/movies/intro.avi",
            "Name":"intro.avi",
            "Extension":".avi",
            "Directory":"/movies",
            "IsTombstone":false,
            "HumaneTotalSize":"1.31 KBytes"
        }
    ],
    "FileCount":1,
    "Start":0,
    "PageSize":25
}
{CODE-BLOCK/}
