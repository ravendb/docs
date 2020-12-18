# Microservices Architecture – A Little History
<small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/the-first-microservices-architecture.jpg" alt="How RavenDB Uses its Own Database in a Microservices Architecture for all its Data Needs" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

The first microservices architecture didn't come from a server, a virtual machine, or over countless instances on the cloud. The first microservices architecture in modern history didn't even take place on a computer.

It was on the assembly line.

The revolution in productivity the assembly line enabled originated from solving the same problems modern applications have in order to keep scaling your capacity as business expands.

### The Monolith

<div class="text-center" style="margin: 30px"><iframe width="560" height="315" src="https://www.youtube.com/embed/NkQ58I53mjk?start=74" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe></div>

Picture a factory with 10 workers all making lamps. There are 20 pieces to each lamp and every worker is responsible for making 10 lamps per day. One employee is scheduled to work an additional 2 hours to package all of the lamps and put them on the truck for shipping.

In order to run an effective factory, the following must be done:

<ul>
    <li>Each worker must know every part that goes into the lamp. They must be trained on every detail in the process and packaging a completed lamp.</li>
    <li>When a new worker is hired, they must be taught everything.</li>
    <li>If a worker is out for the day, the factory produces 10 less lamps.</li>
    <li>If the worker scheduled to package lamps is out for that day, 100 lamps will be waiting in the warehouse to be wrapped while the driver drinks his 10<sup>th</sup> cup of coffee waiting to ship out.</li>
</ul>

This is the challenge of the monolith architecture. Each part of the architecture has to know the entire system. Training new people takes time and money. Losing experienced people comes at a bigger cost. One absent employee can disrupt, even freeze multiple processes.

### Scaling Out for Flexibility

<div style="margin: 30px">
    <a href="https://cloud.ravendb.net" target="_blank"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="RavenDB Cloud"/></a>
</div>

What happens when we create a factory with 6 work stations? Each station requires 4 specific steps in putting together a lamp and is manned by 3 workers. When a worker at one station finishes his task, he puts it on the conveyor belt for the next worker to perform his mini-process. Two workers are assigned at the end of the line to pack each assembled lamp as it arrives.

<ul>
    <li>Each worker doesn't need to know everything, just their specific task. Training takes less time and losing a worker is less costly.</li>
    <li>If a worker is out for the day, there are 2 others that cover for him.</li>
    <li>Management has the flexibility to scale up in a more targeted manner. If workstations 1, 2, and 4 are producing twice the output, they can hire 3 additional workers to man station 3.</li>
</ul>

This is the bricks and mortar version of microservices. Productivity advantages a factory, software system, or any organization can enjoy are essentially the same.

### Scalable Systems

For the factory to go from monolith to microservices, they had to rearrange their entire work flow and get new machinery. They certainly had to buy the same tools, machines, and quality controls more than once to accommodate multiple services using the same general infrastructure.

To enable your software systems to enjoy the same benefits, you need something similar:

<ul>
    <li>Software layers that are distributed in nature</li>
    <li>Tools that can replicate with ease, letting you create new instances of your models almost immediately</li>
    <li>Software that works smoothly on the cloud</li>
    <li>Easy to set up, use, and learn</li>
    <li>Top level ETL functionality to pass on work to each service, easy to communicate with other systems</li>
</ul>

Over the coming years, complexity of applications must be reined in. Microservices shifts complexity, and compared to monolith, reduces it as you scale out your data.

### About RavenDB

<div style="margin: 30px">
    <a href="https://ravendb.net/whitepapers/mongodb-ravendb-best-nosql-open-source-document-database"><img src="images/ravendb-vs-mongodb.png" class="img-responsive m-0-auto" alt="RavenDB vs MongoDB Whitepaper"></a>
</div>

First developed in 2009, RavenDB is an Open Source [NoSQL Distributed Database](https://ravendb.net) that gives you unprecedented performance while maintaining ACID guarantees. RavenDB does this by being the only database to give you automatic indexes while leveraging the document model to eliminate your need for JOINS and *select N+1* functions that require excessive trips to the server, slow down your application, and squander your cloud budget. [RavenDB](https://ravendb.net) is quick to set up and easy to use. Its' query language, RQL, is 80% SQL, making most developers familiar with it from the start. [RavenDB Cloud](https://cloud.ravendb.net) is the managed cloud service, using automatic caching, burstable instances, and cost-free scaling to keep your cloud costs minimal and predictable, making sure you only pay for resources you are using. You can spin up a DBaaS instance on AWS, Azure, or GCP in minutes.

RavenDB keeps complexity to a minimum by including native MapReduce, Full Text Search, and automatic caching in its database. The Management Studio GUI, also part of the database, monitors the internals of your data, measuring performance for every step in your queries, aggregations, indexes, MapReduce, and more. Find performance bottlenecks easy and resolve them right away. Most of what you can do in the RavenDB API you can do on the Management Studio GUI. Execute functions like making queries, create new databases, new nodes, and much more with greater ease.

Over 3 million users have downloaded RavenDB. Startups and Fortune 100 companies alike are among RavenDB's thousands of clients served, including a quick service restaurant that employs 1.5 million instances of RavenDB throughout their 37,000 branches. RavenDB is mentioned in both Gartner and Forrester research reports.