#Debugging indexes

RavenDB offers an ability to debug indexes. You can look into internal indexing structures to figure out how the index is built and see what is actually going on in the index.

Let's create sample indexes that we will use in describing particular debugging options below. First let's put some documents in database named *DB*: 

{CODE-START:json /}
   > curl -X PUT http://localhost:8080/databases/DB/docs/users/1 -d "{ 'FirstName':'Daniel', 'LastName':'Johnson', 'Age':25, '@metadata' : {'Raven-Entity-Name': 'Users'} }" --header "Raven-Entity-Name:Users"
   > curl -X PUT http://localhost:8080/databases/DB/docs/users/2 -d "{ 'FirstName':'David', 'LastName':'Williams', 'Age':20, '@metadata' : {'Raven-Entity-Name': 'Users'} }" --header "Raven-Entity-Name:Users"
   > curl -X PUT http://localhost:8080/databases/DB/docs/users/3 -d "{ 'FirstName':'Daniel', 'LastName':'Brown', 'Age':35, '@metadata' : {'Raven-Entity-Name': 'Users'} }" --header "Raven-Entity-Name:Users"
   > curl -X PUT http://localhost:8080/databases/DB/docs/users/4 -d "{ 'FirstName':'David', 'LastName':'Davis', 'Age':45, '@metadata' : {'Raven-Entity-Name': 'Users'} }" --header "Raven-Entity-Name:Users"
   > curl -X PUT http://localhost:8080/databases/DB/docs/users/5 -d "{ 'FirstName':'Bob', 'LastName':'Davis', 'Age':20, '@metadata' : {'Raven-Entity-Name': 'Users'} }" --header "Raven-Entity-Name:Users"
{CODE-END /}

and create two indexes:

Map only

{CODE-START:json /}
   > curl -X PUT http://localhost:8080/databases/DB/indexes/UsersByFirstName 
		-d "{ Map: 'from user in docs.Users select new {user.FirstName}' }"
{CODE-END /}

Map Reduce

{CODE-START:json /}
   > curl -X PUT http://localhost:8080/databases/DB/indexes/AvgUsersAge 
	-d "{ Map: 'from user in docs.Users select new {user.FirstName, user.Age}', Reduce: 'from result in results group result by new { result.FirstName } into nameGroup select new { nameGroup.Key.FirstName, Age = nameGroup.Average(x => x.Age) } '}"
{CODE-END /}

##Stats

To retrieve basic info about the index you can get its statistics. Accomplish it by creating the HTTP GET request with *debug=stats* parameter:

{CODE-START:json /}
   > curl -X GET http://localhost:8080/databases/DB/indexes/UsersByFirstName?debug=stats
{CODE-END /}

The output will provide you with the following info:

{CODE-START:json /}
{
	"Name":"UsersByFirstName",
	"IndexingAttempts":5,
	"IndexingSuccesses":5,
	"IndexingErrors":0,
	"LastIndexedEtag":"00000000-0000-0100-0000-00000000000a",
	"LastIndexedTimestamp":"2012-11-30T08:32:12.1060000",
	"LastQueryTimestamp":"2012-11-30T09:04:26.3136843Z",
	"TouchCount":0,
	"ReduceIndexingAttempts":null,
	"ReduceIndexingSuccesses":null,
	"ReduceIndexingErrors":null,
	"LastReducedEtag":null,
	"LastReducedTimestamp":null,
	"Performance":[]
}
{CODE-END /}

##Indexing entries

By default querying indexes will result in documents that match the criteria. For the query:

{CODE-START:json /}
   > curl -X GET 
		http://localhost:8080/databases/DB/indexes/UsersByFirstName?query=FirstName:Daniel
{CODE-END /}

Raven will return the results:

{CODE-START:json /}
{
	"Results":[
	{
		"FirstName":"Daniel",
		"LastName":"Johnson",
		"Age":25,
		"@metadata":{ // .. // }
	},
	{
		"FirstName":"Daniel",
		"LastName":"Brown",
		"Age":35,
		"@metadata":{ // .. // }
	}],
	// .. index info go here .. //
}
{CODE-END /}
Instead of that you can show raw index entries. To achieve that add *debug=entries* parameter to the query string:

{CODE-START:json /}
   > curl -X GET 
		http://localhost:8080/databases/DB/indexes/UsersByFirstName?query=FirstName:Daniel"&"debug=entries
{CODE-END /}

Now you will get the following output:

{CODE-START:json /}
{
	"Count":2,
	"Results":[
	{
		"FirstName":"daniel",
		"__document_id":"users/1"
	},
	{
		"FirstName":"daniel",
		"__document_id":"users/3"
	}],
	// .. index info go here .. //
}
{CODE-END /}

Note that each single result has only two positions: indexed field and document reference.

##Map results

You can see the results of documents mapping according to the map definition of the index. Add *debug=map* and *key* parameter that determine the mapped field to the query string:

{CODE-START:json /}
   > curl -X GET -g 'http://localhost:8080/databases/DB/indexes/AvgUsersAge?debug=map&key={\"FirstName\":\"David\"}'
{CODE-END /}

In result you will get raw, mapped items of the index:
{CODE-START:json /}
{
	"Count":2,
	"Results":[
	{
		"ReduceKey":"{\"FirstName\":\"David\"}",
		"Timestamp":"2012-11-30T08:34:27.8040000",
		"Etag":"00000000-0000-0100-0000-00000000000f",
		"Data":{
			"FirstName":"David",
			"Age":45.0
		},
		"Bucket":159818,
		"Source":"users/4"
	},
	{
		"ReduceKey":"{\"FirstName\":\"David\"}",
		"Timestamp":"2012-11-30T08:34:27.8040000",
		"Etag":"00000000-0000-0100-0000-00000000000d",
		"Data":{
			"FirstName":"David",
			"Age":20.0
		},
		"Bucket":814440,
		"Source":"users/2"
	}]
}
{CODE-END /}

If you miss the *key* parameter the response message will produce the list of available keys, e.g.:

{CODE-START:json /}
{
	"Error":"Query string argument 'key' is required",
	"Keys":[
		"{\"FirstName\":\"Bob\"}",
		"{\"FirstName\":\"Daniel\"}",
		"{\"FirstName\":\"David\"}"
	]
}
{CODE-END /}

##Reduce results

The same way you are able to see reduced index results. You have to use *debug=reduce* option and apart from *key* additionally you need to pass *level* to say what step of reducing process you are interested in.

{CODE-START:json /}
   > curl -X GET -g 'http://localhost:8080/databases/DB/indexes/AvgUsersAge?debug=reduce&key={\"FirstName\":\"David\"}&level=2'
{CODE-END /}

The output:

{CODE-START:json /}
{
	"Count":2,
	"Results":[
	{
		"ReduceKey":"{\"FirstName\":\"David\"}",
		"Timestamp":"2012-11-30T08:34:27.9290000",
		"Etag":"00000000-0000-0100-0000-000000000026",
		"Data":
		{
			"FirstName":"David",
			"Age":45.0
		},
		"Bucket":0,
		"Source":"156"
	},
	{
		"ReduceKey":"{\"FirstName\":\"David\"}",
		"Timestamp":"2012-11-30T08:34:27.9290000",
		"Etag":"00000000-0000-0100-0000-000000000027",
		"Data":
		{
			"FirstName":"David",
			"Age":20.0
		},
		"Bucket":0,
		"Source":"795"
	}]
}
{CODE-END /}

##Skipping projections

RavenDB offers a feature called [live projections](../../client-api/querying/handling-document-relationships#live-projections) which allows you to transform index result on server side.
Sometimes you might want to see index result without applying transformation. Then you need to use *skipTransformResults=true* option:

{CODE-START:json /}
   > curl -X GET 
	http://localhost:8080/databases/DB/indexes/IndexWithTransformation?skipTransformResults=true
{CODE-END /}