#Dynamic reporting

In order to explain this feature, let us start with an example. First we are creating the a simple index. The one thing to notice is that we are explicitly setting the _Sort_ 
mode for _Total_ to be `Double`. 

![Figure 1: Create index](images/reporting_1.png)

Now we are going to <em>Query > Reporting</em>:

![Figure 2: Go to reporting](images/reporting_2.png)

And then we can start issue reporting queries:

![Figure 2:Use reporting](images/reporting_3.png)

This is the equivalent of doing:

{CODE-START:csharp /}
select EmployeeID, sum(tot.Total) Total from Orders o join 
    (
        select sum((Quantity * UnitPrice) * (1- Discount)) Total, OrderId from [Order Details]
        group by OrderID
    ) tot
    on o.OrderID = tot.OrderID
where o.CustomerID = @CustomerId
group by EmployeeID
{CODE-END /}

The nice thing about this, and what makes this feature different from standard map/reduce, is that you can filter the input data into the aggregation.
In code, this would look something like this:

{CODE query@Studio\DynamicReporting.cs /}