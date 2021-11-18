# Import from CSV

## What is CSV

A Comma-Separated Values (CSV) file is a delimited text file that uses a comma to separate values ([from Wikipedia](https://en.wikipedia.org/wiki/Comma-separated_values)).  

## How should I format my documents as CSV

RavenDB uses JSON format for storing documents, thus the CSV lines representing documents should have a specific format. 
There are three types of properties in JSON: 

- Primitive: values that are numbers, strings, or booleans  
- Nested Object: where the value of the property is a JSON object  
- Array: an array of values that can either be primitives, nested objects, or arrays  

Let's look at a sample JSON document:
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

The `NestedObject` property is a nested JSON object and as such should be decomposed into multiple properties - one for each nested property.  
The decomposition rule goes as follows:  
[`the name of the parent property`].[`name of the inner object property`]  
like so:  

{CODE-BLOCK:json}
NestedObject.Name
Inner Object
{CODE-BLOCK/}

The import process will combine properties with the same prefix back into one JSON object.  

The `ArrayObject` property is an array and as such contains multiple values. These should be escaped as a string like so:  

{CODE-BLOCK:json}
ArrayObject
"[1,2,3,4]"
{CODE-BLOCK/}

Last thing we need in order for an import to work is to add the `collection` property. If we don't, the name of the collection will derive from the CSV file name.

{CODE-BLOCK:json}
@metadata.@collection
Samples
{CODE-BLOCK/}

If we want to import the document with a specific `id` we need to include an `@id` property too. 

{CODE-BLOCK:json}
@id
Samples/1-A
{CODE-BLOCK/} 

The complete CSV line should look like this:  

{CODE-BLOCK:json}
@id,Name,NestedObject.Name,ArrayObject,@metadata.@collection
Samples/1-A,Import from CSV,Inner Object,"[1,2,3,4]",Samples
{CODE-BLOCK/} 

Now that we've got a valid CSV file we can import it to RavenDB.  

![Figure 1. Import CSV file](images/import-from-CSV-studio-view.png "Import CSV file")
1. Select `Tasks` tab.  
2. Select `Import Data`.  
3. Make sure that you are importing into the correct database.  
4. Select `From CSV File`.  
5. Select the file to import.  
6. You can name the [collection](../../../../client-api/faq/what-is-a-collection) where the file will be imported.  When running an [RQL](../../../../indexes/querying/what-is-rql) query, the code `from` refers to the collection that contains the desired document.  
7. Define CSV options.  
8. Select `Import Collection`  
  
After importing the CSV file, the resulting document should look like the document 
[above](../../../../studio/database/tasks/import-data/import-from-csv#how-should-i-format-my-documents-as-csv).  
