#Is the file format stable?

Do I need to take any special steps when upgrading RavenDB to migrate the stored data?

The short answer for that is: No, you don't have to do anything when you upgrade RavenDB.

The long answer is that sometimes we perform changes which require changing the file format. RavenDB include support for perform those kind of migrations automatically on startup if it finds that the stored database is using an old format.

Please note, however, that those migrations are only one way. If you want to move backward and a change to the file format have occurred, RavenDB will fail to start (with a detailed error message). You can move data between different versions using the [import/export](http://ravendb.net/docs/server/administration/export-import) tool, which works across all versions of RavenDB.