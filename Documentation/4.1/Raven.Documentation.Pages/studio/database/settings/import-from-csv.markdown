# Import from CSV

## What is CSV

A comma-separated values (CSV) file is a delimited text file that uses a comma to separate values ( [from wikipedia](https://en.wikipedia.org/wiki/Comma-separated_values)).

## How should i format my documents as CSV

RavenDB is using JSON format for storing documents thus the CSV lines representing documents should have a specific format to them. 
There are three types of properties for JSON: 

- Primitive: numbers, strings and boolean. 
- Nested Objects: Where the value of the property is a JSON object. 
- Array: An array of values that can either be primitive, nested objects or arrays. 

Lets look at a sample JSON document 
{CODE-BLOCK:json}
{
    "Name": "Import from CSV",
    "NestedObject": {
        "Name": "Inner Object"
    },
    "ArrayObject": [
        1,
        2,
        3,
        4
    ],
    "@metadata": {
        "@collection": "Samples"
    }
}
{CODE-BLOCK/}

The `Name` property is a primitive and should appear unescaped in the CSV like so: 

{CODE-BLOCK:json}
Name
Import from CSV
{CODE-BLOCK/}

The `NestedObject` property is a nested JSON and as such should be decomposed into multiple properties, one for each nested property. 
The decomposition rules goes as follow:  
[`the name of the parent property`].[`name of the inner object property`]  
like so:  

{CODE-BLOCK:json}
NestedObject.Name
Inner Object
{CODE-BLOCK/}

The import process will combine properties with the same prefix back into a JSON object. 

The "ArrayObject" property is an array and as such contains multiple values and should be escaped as string like so: 

{CODE-BLOCK:json}
ArrayObject
"[1,2,3,4]"
{CODE-BLOCK/}

Last thing we need inorder for an import to work is to add the `collection` property otherwise the name of the collection will derive from the CSV file name.

{CODE-BLOCK:json}
@metadata.@collection
Samples
{CODE-BLOCK/}

If we want to import the document with a specific `id` we need to include an `@id` property too. 

{CODE-BLOCK:json}
@id
Samples/1-A
{CODE-BLOCK/} 

The end result should look like so:

{CODE-BLOCK:json}
@id,Name,NestedObject.Name,ArrayObject,@metadata.@collection
Samples/1-A,Import from CSV,Inner Object,"[1,2,3,4]",Samples
{CODE-BLOCK/} 

Now that we got a valid CSV file we can import it to RavenDB by selecting  
[`Target database`]->[`Settings`]->[`Import Data`]->[`From CSV file`] like so:  
![Figure 1. Import CSV file](images/csv-import.JPG "Import CSV file")

The end result should look like the document above
