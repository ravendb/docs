# How to integrate with Excel?

A very common use case for many application is to expose data to users as an Excel file. RavenDB has a dedicated support that allows to directly consume data stored in a database by Excel application. 
The integration of Excel with the data store is achieved by a designated query streaming endpoint that outputs a steam in a format acceptible by `Excel`, Comma Separated Values (CSV).

In order to take advantage of this feature you need to specify [an RQL]() that you want to query.

The generic HTTP request will have the following address:

{CODE-BLOCK:plain}
http://localhost:8080/databases/[db_name]streams/queries?query=[query]
{CODE-BLOCK/}

In order to include only specific properties in the CSV output you can use the `field` parameter like so:

{CODE-BLOCK:plain}
http://localhost:8080/databases/[db_name]/streams/queries?query=[query]&field=[field-1]&field=[field-2]...&field=[field-N]
{CODE-BLOCK/}

## Example

Firstly lets create a database, Northwind, and import the [sample data](..\..\studio\database\tasks\create-sample-data.markdown) into it.
Now let's query the product collection include the category document and project some of its properties using the below RQL

{CODE-BLOCK:plain}
from Products as p
load p.Category as c
select 
{
    Name: p.Name,
    Category: c.Name,
}
{CODE-BLOCK/}

In order to execute the above query we will need to use the following url:   

{CODE-BLOCK:plain}
http://localhost:8080/databases/Northwind/streams/queries?query=from%20Products%20as%20p%0Aload%20p.Category%20as%20c%0Aselect%20%0A%7B%0A%20%20%20%20Name%3A%20p.Name%2C%0A%20%20%20%20Category%3A%20c.Name%2C%0A%7D
{CODE-BLOCK/}

Going to the above address in a web browser will download you an export.csv file containing following results:

{CODE-BLOCK:plain}
Name,Category
Chang,Beverages
Aniseed Syrup,Condiments
Chef Anton's Cajun Seasoning,Condiments
Chef Anton's Gumbo Mix,Condiments
Grandma's Boysenberry Spread,Condiments
Uncle Bob's Organic Dried Pears,Produce
Northwoods Cranberry Sauce,Condiments
Mishi Kobe Niku,Meat/Poultry
Ikura,Seafood
Queso Cabrales,Dairy Products
Queso Manchego La Pastora,Dairy Products
Konbu,Seafood
Tofu,Produce
Genen Shouyu,Condiments
Pavlova,Confections
Alice Mutton,Meat/Poultry
Carnarvon Tigers,Seafood
{CODE-BLOCK/}

Now to push them to Excel we need to create new spreadsheet and import data `From Text`:

![Importing data from text in Excel](images\excel_from_text.png)

Then in a Open File Dialog we paste our querying url:

![Open File Dialog](images\excel_from_text_dialog.png)

Next, the Import Wizard will show up where we can adjust our import settings (don't forget to check `Comma` as a desired delimiter):

![Import Wizard Step 1](images\excel_from_text_wizard_1.png)

![Import Wizard Step 2](images\excel_from_text_wizard_2.png)

![Import Wizard Step 3](images\excel_from_text_wizard_3.png)

Finally we need to select where we would like to place the imported data:

![Select where to put the data](images\excel_from_text_select.png)

As a result of previous actions, the spreadsheet data should look like:

![Excel results](images\excel_from_text_results.png)

Now we must tell Excel to to refresh data. Click on `Connections` in `Data` panel:

![Excel connections](images\excel_connections.png)

You will see something like that:

![Excel connections dialog](images\excel_connections_dialog_1.png)

Go to Properties and:   
1. **uncheck** `Prompt for file name on refresh`.   
2. **check** `Refresh data when opening the file`.   

![Excel connection properties](images\excel_connections_dialog_2.png)

Finally you can close the file, change something in the database and reopen it. You will see new values.
