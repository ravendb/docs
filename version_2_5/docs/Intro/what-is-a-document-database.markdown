## What is a Document Database?

A Document Database is a database that, as its name implies, features the storage of _Documents_ as its main data storage mechanism. An individual document is a storage unit for data, where all documents in a Document Database will normally encode data in the same way. Common document encodings include JSON, BSON, XML, YAML, or binary formats.

Unlike in a Relational Database (RDBMS), a NoSQL Document Database will normally not have any required standard schema to which all documents must adhere. Different documents can have different fields being stored, and data within a document can often be multi-dimensional. 

For example, here is a sample document stored encoded in JSON (the format used in RavenDb):

    {CODE-START: json}
    {
		Id: "Cars-29"
	    Manufacturer: "Ford",
	    Model: "S-Max",
	    Year: 2011,
	    Mileage: 43532,
	    ServiceHistory: [
	    	{ 
	    		Date: "2013-02-11T17:09:38.0830000",
	    		Mileage: 39283,
	    		Reason: "Oil Change"
	    	},
	    	{
                Date: "2012-07-11T13:53:23.0492000",
                Mileage: 23718,
                Reason: "Broken Fender"
            }
        ]
    }
    {CODE-END}

The above document could co-exist in the same database as the following document, which uses a completely different schema:

    {CODE-START: json}
    {
		Id: "Person-2"
	    FirstName: "John",
		LastName: "Smith",
		Age: 38,
		Occupation: "SoftwareDeveloper"
		CompanyId: "Companies-84"
    }
    {CODE-END}

Documents in a Document Database can be retrieved in a number of ways. The first way is by document key, which is an identifier that is unique to the given document (in the Documents above, the `Id` field is the document key). 

Since a Document Database does not use any sort of rigid enforcement for Document structure, in order to query and return documents based on search criteria, pre-defined _Indexes_ are often employed. The index definition will provide the database with the means to be able to identify documents that will be associated with the index, allowing for fast retrieval when a specific index is queried.