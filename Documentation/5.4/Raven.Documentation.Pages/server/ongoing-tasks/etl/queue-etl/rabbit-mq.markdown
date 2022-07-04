# Ongoing Tasks: Queue ETL Overview
---

{NOTE: }

* **RabbitMQ** exchanges are designed to disperse data to multiple queues, 
  making for a flexible data channeling system that can ease the management 
  of complex scenarios.  
  For example, a RabbitMQ exchange can inform different bank departments 
  of the same event via different queues so each department would handle 
  the event from its own perspective of it.  

* A RavenDB RabbitMQ ETL task Extracts data from RavenDB, Transforms it 
  into a new JSON object, and Loads it to a RabbitMQ Exhange using the 
  ClouodEvents library.  

* This page explains how to create a RabbitMQ ETL task using code.  
  [Learn here](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task) 
  how to define a RabbitMQ ETL task using code.  

* In this page:  
  * [](../../../../)  

{NOTE/}

---

{PANEL: Queue ETL}

{PANEL/}

{PANEL: Kafka}

sample script:

{CODE-BLOCK:sql}
var orderData = {
    Id: id(this), // property with RavenDB document ID
    OrderLinesCount: this.Lines.length,
    TotalCost: 0
};

for (var i = 0; i < this.Lines.length; i++) {
    var line = this.Lines[i];
    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
    orderData.TotalCost += cost;
}

loadToOrders(orderData); // load to the 'Orders' topic
{CODE-BLOCK/}

{PANEL/}

{PANEL: Rabbit MQ}

sample script:

{CODE-BLOCK:sql}
var orderData = {
    Id: id(this), // property with RavenDB document ID
    OrderLinesCount: this.Lines.length,
    TotalCost: 0
};

for (var i = 0; i < this.Lines.length; i++) {
    var line = this.Lines[i];
    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
    orderData.TotalCost += cost;
}

loadToOrders(orderData); // load to the 'Orders' queue
{CODE-BLOCK/}

{PANEL/}


## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

### Client API

- [How to Add ETL](../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
