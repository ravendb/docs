# Excel integration

To integrate with Excel application we will need following items:   
1. Some data (built-in Northwind sample data in this example).   
2. [Index](../client-api/querying/static-indexes/defining-static-index) that we will query against (in this example we will use default `Raven/DocumentsByEntityName` index).   
3. [Transformer](../client-api/querying/results-transformation/result-transformers) (optional - just to shape up the results).   

!['Products/ForExcel' transformer](images\excel_transformer.png)

To query a database for `Product` using our `Raven/DocumentsByEntityName` index then transforming the results with `Products/ForExcel` transformer and formating them to excel we need to visit following url:   

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/streams/query/Raven/DocumentsByEntityName?query=Tag:Products&resultsTransformer=Products/ForExcel&format=excel
{CODE-END /}

![Query results](images\excel_result.png)

Now to push it to Excel we need to create new spreadsheet and import data `From Text`.

![Importing data from text in Excel](images\excel_from_text.png)

Then in a Open File Dialog we paste our querying URL.

![Open File Dialog](images\excel_from_text_dialog.png)

Next, the Import Wizard will show up where we can adjust our import settings (don't forget to check 'Comma' as a desired delimiter).

![Import Wizard Step 1](images\excel_from_text_wizard_1.png)

![Import Wizard Step 2](images\excel_from_text_wizard_2.png)

Finally we need to select where we would like the imported data to go:

![Select where to put the data](images\excel_from_text_select.png)

As a result of previous actions, the spreadsheet data should look like:

![Excel results](images\excel_from_text_results.png)

Now we must tell Excel to to refresh data, to do it click on Connections in Data tab:

![Excel connections](images\excel_connections.png)

And you will see something like this:

![Excel connections dialog](images\excel_connections_dialog_1.png)

Go to Properties and:   
1. **uncheck** Prompt for file name on refresh.   
2. **check** Refresh data when opening the file.   

![Excel connection properties](images\excel_connections_dialog_2.png)

Finally you can close the file, change something in the database and reopen it and you will see new values.