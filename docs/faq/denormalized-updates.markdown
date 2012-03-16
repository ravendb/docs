# Updating denormalized data

## See: [Set-based operations](../client-api/set-based-operations)

Like most NoSQL solutions, RavenDB best practices favor denormalization over composition / joins for complex documents. A typical example would be storing the Artist name and id along with the Track document:

    {
          "Name" : "Fame",
          "Artist": { "Id": "artists/294", "Name": "Irene Cara" }
    }
    
That decision make reading the data very fast and easy, but it present a challenge when we need to update the Artist name. In general, it isn't recommended to denormalize data that frequently changes, but even rarely changing properties will change occasionally.

In order to solve this exact problem RavenDB offers Set Based Operations. Here is how we can use them from the client API:

    documentStore.DatabaseCommands.UpdateByIndex("Tracks/ByArtistId",
                                         new IndexQuery
                                         {
                                             Query = "Artist:artists/294"
                                         }, new[]
                                         {
                                             new PatchRequest
                                             {
                                                 Type = "Modify",
                                                 Name = "Artist",
                                                 Nested = new[]
                                                 {
                                                     new PatchRequest
                                                     {
                                                         Type = "Set",
                                                         Name = "Name",
                                                         Value = JValue.CreateString("Fame Soundtrack")
                                                     },
                                                 }
                                             }
                                         },
                                         allowStale: false);
    
This will update the Artist.Name property on all the documents where the Tracks/ByArtistId index matched to the id artists/294.
