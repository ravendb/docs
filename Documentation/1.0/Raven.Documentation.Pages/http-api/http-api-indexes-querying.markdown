# Querying indexes
Raven's indexes support very rich querying capabilities. 

* Querying the index
* Fetching results from the index
* Sorting results
* Paging results
* Storing / indexing options

We will start with the following documents in the database:

{CODE-START:plain /}
    > curl -X PUT http://localhost:8080/docs/bob -d "{ Name: 'Bob', HomeState: 'Maryland', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/sarah -d "{ Name: 'Sarah', HomeState: 'Illinois', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/paul -d "{ Name: 'Paul', HomeState: 'Maryland', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/mary -d "{ Name: 'Mary', HomeState: 'Maryland', ObjectType: 'User' }"
    > curl -X PUT http://localhost:8080/docs/george -d "{ Name: 'George', HomeState: 'California', ObjectType: 'User' }"
{CODE-END /}

And we will define the following index:

{CODE-START:plain /}
    > curl -X PUT http://localhost:8080/indexes/usersByHomeState 
              -d "{ Map:'from doc in docs\r\nwhere doc.ObjectType==\"User\"\r\nselect new { doc.HomeState, doc.Name }' }"
{CODE-END /}

## Querying the index
Given this setup work, we can now query using:

{CODE-START:json /}
    > curl http://localhost:8080/indexes/usersByHomeState?query=HomeState:California
    
    {
          "Results":[
                {
                      "Name":"George",
                      "HomeState":"California",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"george",
                            "@etag":"17b3f1fd-4c79-11df-8ec2-001fd08ec235"
                      }
                }
          ],
          "IsStale":false,
          "TotalResults":1
    }
{CODE-END /}

This gives us all the documents where the home state is California. The same query for Maryland will return three results:

{CODE-START:json /}
    > curl http://localhost:8080/indexes/usersByHomeState?query=HomeState:Maryland

    {
          "Results":[
                {
                      "Name":"Bob",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"bob",
                            "@etag":"17b3f1f9-4c79-11df-8ec2-01fd08ec235"
                      }
                },
                {
                      "Name":"Paul",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"paul",
                            "@etag":"17b3f1fb-4c79-11df-8ec2-001fd08ec235"
                      }
                },
                {
                      "Name":"Mary",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"mary",
                            "@etag":"17b3f1fc-4c79-11df-8ec2-001fd08ec235"
                      }
                }
          ],
          "IsStale":false,
          "TotalResults":3
    }
{CODE-END /}

The query for Raven is the one used by Lucene. You can pass any valid Lucene query using the query parameter on the query string. You can read more on [the query syntax on the Lucene documentation](http://lucene.apache.org/core/3_0_3/queryparsersyntax.html).

##Fetching results from the index
But while just getting the documents matching a particular query is useful, we can do better than that. Instead of getting the documents themselves, I want to get the values directly from the index, without getting the full document. So let us say that I just want to display the names by home state, we can do that by utilizing the fetch query string parameter.

{CODE-START:json /}
    > curl "http://localhost:8080/indexes/usersByHomeState?query=HomeState:Maryland&amp;fetch=HomeState&amp;fetch=Name"
    
    {
          "Results":[
                {
                      "HomeState":"Maryland",
                      "Name":"Bob",
                      "__document_id":"bob"
                },
                {
                      "HomeState":"Maryland",
                      "Name":"Paul",
                      "__document_id":"paul"
                },
                {
                      "HomeState":"Maryland",
                      "Name":"Mary",
                      "__document_id":"mary"
                }
          ],
          "IsStale":false,
          "TotalResults":3
    }
{CODE-END /}

This gives us just the data that we need,  Note:

* We can't fetch any data that isn't already on the index, that is why we had to index the Name
* The field must be stored in the index using the Store option "Yes", outlined at the bottom of this page
* We get the document id for the document responsible for a particular value.

## Sorting Results
Next up, we would like to get the results sorted by the name. We can use the sort parameter to do so.

{CODE-START:json /}
    > curl "http://localhost:8080/indexes/usersByHomeState?query=HomeState:Maryland&amp;fetch=HomeState&amp;fetch=Name&amp;sort=Name"
    
    {
          "Results":[
                {
                      "HomeState":"Maryland",
                      "Name":"Bob",
                      "__document_id":"bob"
                },
                {
                      "HomeState":"Maryland",
                      "Name":"Mary",
                      "__document_id":"mary"
                },
                {
                      "HomeState":"Maryland",
                      "Name":"Paul",
                      "__document_id":"paul"
                }
          ],
          "IsStale":false,
          "TotalResults":3
    }
{CODE-END /}

As you can see, we are now sorting by Name ascending. We can reverse the sort order by prefixing - in front of the sort field, like thus:

{CODE-START:json /}
    > curl "http://localhost:8080/indexes/usersByHomeState?query=HomeState:Maryland&amp;fetch=HomeState&amp;fetch=Name&amp;sort=-Name"
    
    {
          "Results":[
                {
                      "HomeState":"Maryland",
                      "Name":"Paul",
                      "__document_id":"paul"
                },
                {
                      "HomeState":"Maryland",
                      "Name":"Mary",
                      "__document_id":"mary"
                },
                {
                      "HomeState":"Maryland",
                      "Name":"Bob",
                      "__document_id":"bob"
                }
          ],
          "IsStale":false,
          "TotalResults":3
    }
{CODE-END /}

Sorting results works without fetching, as well:

{CODE-START:json /}
    > curl "http://localhost:8080/indexes/usersByHomeState?query=HomeState:Maryland&amp;sort=Name"

          "Results":[
                {
                      "Name":"Bob",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"bob",
                            "@etag":"17b3f1f9-4c79-11df-8ec2-001fd08ec235"
                      }
                },
                {
                      "Name":"Mary",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"mary",
                            "@etag":"17b3f1fc-4c79-11df-8ec2-001fd08ec235"
                      }
                },
                {
                      "Name":"Paul",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"paul",
                            "@etag":"17b3f1fb-4c79-11df-8ec2-001fd08ec235"
                      }
                }
          ],
          "IsStale":false,
          "TotalResults":3
    }
{CODE-END /}

## Paging results
When the number of results gets too big, we need to page through them. This is done using the start and pageSize parameters. Here is a complex query, for all the documents in the states between Illinois and Maryland, sorted by name and paged.

{CODE-START:json /}
    > curl "http://localhost:8080/indexes/usersByHomeState?query=HomeState:\[Illinois%20TO%20Maryland\]&amp;sort=Name&amp;start=0&amp;pageSize=2"
    
    {
          "Results":[
                {
                      "Name":"Bob",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"bob",
                            "@etag":"17b3f1f9-4c79-11df-8ec2-001fd08ec235"
                      }
                },
                {
                      "Name":"Mary",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"mary",
                            "@etag":"17b3f1fc-4c79-11df-8ec2-001fd08ec235"
                      }
                }
          ],
          "IsStale":false,
          "TotalResults":4
    }
{CODE-END /}

Just to make things clear, here are the parameters without encoding:

* query = HomeState:[Illinois TO Maryland]
* sort = Name
* start = 0
* pageSize = 2

You can see that the TotalResults value indicate that we have more results, we can get to them using:

{CODE-START:json /}
    > curl "http://localhost:8080/indexes/usersByHomeState?query=HomeState:\[Illinois%20TO%20Maryland\]&amp;sort=Name&amp;start=2&amp;pageSize=2"
    
    {
          "Results":[
                {
                      "Name":"Paul",
                      "HomeState":"Maryland",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"paul",
                            "@etag":"17b3f1fb-4c79-11df-8ec2-001fd08ec235"
                      }
                },
                {
                      "Name":"Sarah",
                      "HomeState":"Illinois",
                      "ObjectType":"User",
                      "@metadata":{
                            "Content-Type":"application/x-www-form-urlencoded",
                            "@id":"sarah",
                            "@etag":"17b3f1fa-4c79-11df-8ec2-001fd08ec235"
                      }
                }
          ],
          "IsStale":false,
          "TotalResults":4
    }
{CODE-END /}

All in all, this is pretty easy thing to do.

## Storing / Indexing options
If you have worked with Lucene before, you are probably familiar with the need to tweak the index using the Field.Store and Field.Index parameters. Extensive discussion on the meaning of those values are beyond the scope of this document, but   Raven allows you to define them using the following syntax:

{CODE-START:json /}
    {
          "Map":"from  doc  in  docs\r\nwhere  doc.ObjectType==\"User\"\r\nselect  new  {  doc.HomeState,  doc.Name  }",
          "Stores":{
                "HomeState":"Yes"
          },
          "Indexes":{
                "Name":"NoNorms"
          }
    }
{CODE-END /}

The valid values for Stores are:

* Yes
* No
* Compressed

The valid values for Indexes are:

* No
* NotAnalyzedNoNorms
* Analyzed
* NotAnalyzed