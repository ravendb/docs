# Excel integration

To integrate with Excel application we will need following items:   
1. Some data (built-in Northwind sample data in this example).   
2. [Index](../client-api/querying/static-indexes/defining-static-index) that we will query against (in this example we will use default `Raven/DocumentsByEntityName` index).   
3. Transformer (optional - just to shape up the results).   

!['Products/ForExcel' transformer](images\excel_transformer.jpg)

To query a database for `Product` using our `Raven/DocumentsByEntityName` index then transforming the results with `Products/ForExcel` transformer and formating them to excel we need to visit following url:   

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/streams/query/Raven/DocumentsByEntityName?query=Tag:Products&resultsTransformer=Products/ForExcel&format=excel
{CODE-END /}

![Query results](images\excel_result.jpg)

Now to push it to Excel we need to create new spreadsheet and import data `From Text`.

![Importing data from text in Excel](images\excel_from_text.jpg)

Then in a Open File Dialog we paste our querying URL.

![Open File Dialog](images\excel_from_text_dialog.jpg)

Next, the Import Wizard will show up where we can adjust our import settings (don't forget to check 'Comma' as a desired delimiter).

![Import Wizard Step 1](images\excel_from_text_wizard_1.jpg)

![Import Wizard Step 2](images\excel_from_text_wizard_2.jpg)

Finally we need to select where we would like the imported data to go:

![Select where to put the data](images\excel_from_text_select.jpg)

As a result of previous actions, the spreadsheet data should look like:

![Select where to put the data](images\excel_from_text_results.jpg)

Now we must tell Excel to to refresh data, to do it click on Connections in Data tab:

![Select where to put the data](images\excel_connections.jpg)

And you will see something like this:

![Select where to put the data](images\excel_connections_dialog.jpg)

Now go to Properties and:   
1. **uncheck** Prompt for file name on refresh.   
2. **check** Refresh data when opening the file.   

Now you can close the file, change something in the database and reopen it and you will see new values.