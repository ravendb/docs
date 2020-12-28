# CQRS and Event Sourcing made easy with RavenDB

Using CQRS enables us to meet a wide range of architectural challenges, such as achieving scalability, managing complexity and managing changing business rules. Storing data using the Event Sourcing technique is the only way never to lose data.

In this article, we will discuss some alternatives to adopt CQRS and Event Sourcing using RavenDB. Although we present some basic concepts, dealing with the fundamentals of CQRS and Event Sourcing is out of scope.

## What is CQRS (a quick review)?

Back in 1980s, Bertrand Meyer introduced the term Command and Query Separation (CQS) to describe the principle that an object's methods should be either commands or queries:  

- A command changes the state of the object and DOES NOT return any data.
- A query returns data but DOES NOT alter the state of the object.

CQS is simple to explain and understand, yet it has a dramatic impact on the way we write software. CQRS – short for Command Query Responsibility Segregation – takes this principle a step further to define a pattern. CQRS is the CQS principle applied in the architecture level.
 
![Figure 1](images/cqrs1.jpg)  

CQRS pattern splits the system into a read stack (queries) and a write stack (commands). The commands involve all the business logic to ensure that the system writes consistent data to the data store.  The queries are often much simpler than the commands – their mission is to consolidate data to provide information “ready to use” to the UI.

The separation of the command stack and the query stack enables us to create more efficient models, improving maintainability and usually the system performance.

Observing the traditional DDD-inspired layered architecture with CQRS in mind, we can see that only the “command side” uses a domain model. The “query side” is made of plain data access and uses DTOs to bring data up to the presentation layer.

Frequently, when implementing CQRS, systems use separate data stores for the “queries” and “commands”. Each data store optimized for the use cases it supports. Some background synchronization processes and strategies are adopted to ensure (frequently) eventual consistence.

![Figure 2](images/cqrs2.jpg)  

## Using RavenDB for implementing CQRS

Supporting schema-free document databases, RavenDB is an excellent option to be used as Query Stack data store tool. A document could easily consolidate all the necessary data needed by the presentation layer in an elegant and organized way.

Even when using a single database for the “command stack” and “query stack”, RavenDB databases are a good option. Indexes and transformers are excellent alternatives to provide data for the Query Stack. Meanwhile, the documents by their ACID nature are perfect for Command Stack.

Storing and loading documents in RavenDB is very easy. It is only necessary to create a session to send and retrieve data from the database.

{CODE article_cqrs_1@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeRepository.cs /}

Another good advantage of using a schema-free database is the “built-in” support for polymorphism. For example, an address is a collection of information, presented in a mostly fixed format, used for describing the location of a building, apartment, etc. Some countries uses different information and different formats and the domain model should support it.

{CODE article_cqrs_2@cqrses-demo\scenario_01\src\Payroll.Domain\Model\BrazilianAddress.cs /}

With RavenDB, the serialization process facilitates the persistence of polymorphic objects.

{CODE article_cqrs_3@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeRepository.cs /}

The serializer adds meta-attributes that ensure that when loading the object, the appropriate type will be used. 

In the previous example, the Patch command was used to perform partial updates without having to load, modify, and save a full document. This is usually useful for updating denormalized data in entities. RavenDB also supports running scripts at server to update documents.

{CODE article_cqrs_4@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeRepository.cs /}

## Welcome Events!

When designing the Command Stack it is common to use the commands, events and messages abstractions.

Commands are imperatives; they are requests for the system to perform a task or action.

{CODE article_cqrs_5@cqrses-demo\scenario_02\src\Payroll.Domain\Commands\RaiseSalaryCommand.cs /}

Events are notifications; they report something that has already happened to other interested parties.  Events happen in the past and or immutable. For example, “the employee salary was raised”, “the employee home address was changed”.  Because events happen in the past, they cannot be changed or undone. However, subsequent events may alter or negate the effects of earlier events.

{CODE article_cqrs_6@cqrses-demo\scenario_02\src\Payroll.Domain\Events\EmployeeSalaryRaisedEvent.cs /}

Both commands and events are types of message that are used to exchange data between objects or contexts. This exchange generally occurs using some kind of messaging system.

The UI of systems adopting CQRS are commonly designed to send commands to the command stack (domain model). After processing the commands, events are generated and raised.

## What is Event Sourcing (a quick review)?

Event Sourcing is a technique that ensures that all changes to application state are stored as a sequence of the events. This technique allows us to recover the status of any entity/aggregate simply "replaying" the changes recorded in the events. The big gain is that we can know the status of any entity/aggregate over time. 

![Figure 3](images/cqrs3.jpg)  

This is because using Event Sourcing we have not only the current state of the entities, but also we are able to recover the state of the objects in the past. 

## Using RavenDB to implement Event Sourcing

Implementing ES with RavenDB is simple and easy. If the system already raise events, we could just need to listen and save the event objects in a dedicated collection.

{CODE article_cqrs_7@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeEventStore.cs /}

Another approach would be to maintain a list of events generated in each entity. Then use the Repository Service as a Message Dispatcher. 

{CODE article_cqrs_8@cqrses-demo/scenario_02/src/Infrastructure.EventSourcing.RavenDB/RavenDBESEmployeeRepository.cs /}

In this example, one document will be created for each entity (Employee) with an array containing the related events.

## Querying a stream of events (using MapReduce)

Storing events in Command Stack opens up possibilities and interesting technical challenges. How to produce ViewModels for Query Stack? If we are using RavenDB, we could use Map Reduce indexes.

Indexes are functions that are performed on the server side and determine which fields (and which values) can be used in searches. These functions often create structures similar to materialized views that accelerate the search results.

The indexing process happens in the background on the server. It is triggered whenever a data is added or modified. This approach allows the server to respond quickly even when large portions of data have been modified avoiding slow searches. The problem, if you see it that way, is that there is no guarantee that the search results are updated.

{CODE article_cqrs_9@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeEventStore.cs /}

What this index takes into account is the total number of events associated with each entity Employee . It does this by applying the Map Reduce technique.

Let's understand how it works...

First, the "map" converts all stored events to a common representation.
 
![Figure 4](images/cqrs4.jpg)  

Second, all the mapped objects are grouped by some criteria.

![Figure 5](images/cqrs5.jpg)  

Third and finally, reduction:
 
![Figure 6](images/cqrs6.jpg)  

Following the same idea, the next example shows how to compute the names and salaries of employees using an event stream collection.

{CODE article_cqrs_10@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeEventStore.cs /}

Note that during the "map" we use Initial Salary from EmployeeRegisteredEvent and Amount from EmployeeRegisteredEvent.

Following the idea of delivering "ready-to-use" data, we have a FullName property.

The Reduce is almost self-explanatory. I am adding salaries and taking the first produced name.

With this index it is easy to know the top paid employees.

{CODE article_cqrs_11@cqrses-demo/scenario_01/src/Payroll.Infrastructure.RavenDbEmployeeRepository/EmployeeEventStore.cs /}

## Querying a stream of events (using Compilation Extensions)

RavenDB is extensible. It is easy to create plugins to add some domain specific capabilities to the database.

In the next example, we define a Compilation Extension with two basic helper functions. 

{CODE article_cqrs_12@cqrses-demo\scenario_02\src\Payroll.RavenDB.Plugins\Employee.cs /}

These helper functions, after properly deployed, can be used on the server side code like in transforms.

## Last words...

Phew!
In this article we learned how easy and natural it is using RavenDB to implement CQRS and Event Sourcing.

Schema-free documents are perfect to store "ready-to-use" data for the Query Stack. Providing transactions, RavenDB is solid enough to be used as a Command Stack database as well.

RavenDB is really fast and flexible, so, it's easily used as an Event Store.  The built-in indexing feature Map/Reduce is excellent for creating projections and snapshots. 

That's it.