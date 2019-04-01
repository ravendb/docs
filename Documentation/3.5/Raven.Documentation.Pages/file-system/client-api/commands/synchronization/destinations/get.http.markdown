#Commands: GetDestinations

The list of destination file systems are stored as the configuration under `Raven/Synchronization/Destinations`. You can retrieve it by executing [the appropriate GET](../../configurations/get-key) method.

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/config?name=Raven/Synchronization/Destinations
< HTTP/1.1 200 OK

{
    "Destinations":
    [
        {
            "ServerUrl":"http://localhost:8081",
            "Username":null,
            "Password":null,
            "Domain":null,
            "ApiKey":null,
            "FileSystem":"SlaveNorthwindFS",
            "Enabled":true
        }
    ]
}
{CODE-BLOCK/}

