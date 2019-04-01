# Query: Reporting View

This view allows to perform report queries on results of existing indexes. Under the hood it works based on [Dynamic aggregation](../../../indexes/querying/dynamic-aggregation) feature.

{PANEL:Action Bar}

Action Bar consists of the following:

- `Select the index` - first you need to choose an index you want to report on,
- `Run the report` - runs the report on the selected index for a given aggregation values,
- `Edit the index` - enables to edit a definition of the selected index by navigating to [Index Edit View](../indexes/index-edit-view),
- `Export to CSV` - Export the report to CSV file,
- `Cache` - Enable/Disable caching option.
![Figure 1. Studio. Reporting View.](images/reporting_view_1.png)


{PANEL/}

{PANEL:Aggregation criteria}

- `Group By` - name of a field that the report will aggregate by the index results,
- `Values` - names of fields that will be processed by report
- `Filter` - filtering criteria

![Figure 2. Studio. Reporting View. Criteria](images/reporting_view_2.png)

{PANEL/}


{PANEL:Issuing reporting query - example}

The first step is selecting the appropriate index:

![Figure 3. Studio. Reporting View. Select the index.](images/reporting_view_3.png)

Next fill the aggregation criteria:

![Figure 4. Studio. Reporting View. Set critetia.](images/reporting_view_4.png)

Then run the report. You will see the results:

![Figure 5. Studio. Reporting View. Results.](images/reporting_view_5.png)

This is equivalent of doing:

{CODE-BLOCK:csharp}
select EmployeeID, sum(tot.Total) Total from Orders o join 
    (
        select sum((Quantity * UnitPrice) * (1- Discount)) Total, OrderId from [Order Details]
        group by OrderID
    ) tot
    on o.OrderID = tot.OrderID
where o.CustomerID = @CustomerId
group by EmployeeID
{CODE-BLOCK/}

In code, this would look something like this:

{CODE sample_csharp@Studio\Overview\Query\ReportingView.cs /}

{PANEL/}

