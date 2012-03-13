#Getting multiple documents in a single round-trip
We can address documents directly by using their key (in the following example, the key is 'users/ayende'):

    > curl http://localhost:8080/docs/users/ayende

    {
      "name": "ayende"
    }

But while that is useful, there are often scenarios where we want to get more than a single document. In order to avoid the common SELECT N+1 issues, RavenDB supports the ability to get multiple documents in a single remote call.
 
We load the database with the following two documents:

     > curl -X PUT http://localhost:8080/docs/users/ayende -d "{ name: 'ayende'}"
    {"Key":"users/ayende","ETag":"7f9cd674-4c6f-11df-8ec2-001fd08ec235"}
    > curl -X PUT http://localhost:8080/docs/users/oren -d "{ name: 'oren'}"
    {"Key":"users/oren","ETag":"7f9cd675-4c6f-11df-8ec2-001fd08ec235"}

And now, in order to get them both in a single query, we use:

    > curl -X POST http://localhost:8080/queries -d "['users/ayende','users/oren']"
    [
       {
                 "name":"ayende",
                "@metadata":{
                    "Content-Type":"application/x-www-form-urlencoded",
                    "@id":"users/ayende",
                    "@etag":"7f9cd674-4c6f-11df-8ec2-001fd08ec235"
                }
       },
       {
                "name":"oren",
                "@metadata":{
                    "Content-Type":"application/x-www-form-urlencoded",
                    "@id":"users/oren",
                    "@etag":"7f9cd675-4c6f-11df-8ec2-001fd08ec235"
                 }
       }
    ]

More formally, we POST a request to '/queries', with the content of a JSON array of document ids. The response for such a request is a JSON array of documents, include the document metadata.

Important: If you request a non existing key, the request is ignored. In other words, the output of this request and the previous ones are identical.Â

    > curl -X POST http://localhost:8080/queries -d "['users/ayende','does not exists', 'users/oren']"
    [
          {
                "name":"ayende",
                "@metadata":{
                       "Content-Type":"application/x-www-form-urlencoded",
                       "@id":"users/ayende",
                       "@etag":"7f9cd674-4c6f-11df-8ec2-001fd08ec235"
                }
          },
          {
                "name":"oren",
                "@metadata":{
                       "Content-Type":"application/x-www-form-urlencoded",
                       "@id":"users/oren",
                       "@etag":"7f9cd675-4c6f-11df-8ec2-001fd08ec235"
               }
          }
    ]

In other words, you cannot rely on the size of the returned array to be the same as the array of ids requested.
Aside from missing documents, which are ignored, the order of the documents in the returned array match the order of ids in the requested array.
