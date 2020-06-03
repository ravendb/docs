# NoSQL ACID Database for Sparkling Logic <br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![Document database indexing in RavenDB compared to MongoDB, another NoSQL document database, as well as the relational database, PostgreSQL. Indexing document properties.](images/how-sparkling-logic-uses-ravendb-nosql-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

By using RavenDB's transactional NoSQL ACID database, Sparkling Logic can now build robust solutions for the decision management marketplace that features extensive flexibility and faster performance.

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <img src="images/sparkling-logic.svg" style="max-width:240px" alt="Sparkling Logic"/>
    <small class="margin-top-sm">Using a database in a way you’ve never seen before.</small>
</div>

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">"Effective Decision Management needs transaction support for ever-evolving documents. It needs versioning. It needs powerful and intuitive queries. Millions of dollars and critical decisions go through our platform every day, this is a mission critical platform."</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Carlos Serrano, CTO at Sparkling Logic, Inc.</span>
</div>

Sparkling Logic offers a platform for customers who want to make automated decisions that involve a fair amount of risk by enabling them to establish a set of logic (decision flows, business rules, predictive analytics models) to come to the optimal conclusion.

Customers include banks, insurance companies, credit institutions, financial services, and fraud detection for credit and insurance fraud. Customers also include IoT solutions like data collected from sensors on heavy machinery to determine what needs to be maintained, fixed, or even repositioned. 

Take, for example, a bank that issues credit cards. They develop applications that will take in the necessary information for people applying for a credit card. When a client asks for credit, the bank assesses the risk, decides whether or not to issue the credit card, and determines the terms like the credit limit, minimum payment, interest on the outstanding balance, and so on.

In a day, thousands of people can request a credit card. To process all of them promptly, the Sparkling Logic platform offers a place to put together all the logic, rules, predictive models, and configurations so the bank can make decisions on the spot.

RavenDB serves as the repository for the logic, rules, predictive models, and configurations that power these decisions.

The Sparkling Logic system enables users to update their logic, change rules, implement or adjust predictive models, delete obsolete policies, and create new configurations to meet evolving needs.

RavenDB will adapt immediately to ensure that risk is managed by enabling companies to apply their most updated logic and standards to come to the right decisions.

<div class="margin-top margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="RavenDB Cloud"/></a>
</div>

### Using a NoSQL ACID Database

Sparkling Logic employs RavenDB as a fully embedded database. All the predictive models, the rules, decision flows, and meta information gets stored in the NoSQL document database.

This decision logic is a key business asset that changes frequently. Some customers update their rules hundreds of times per day. When a customer wants to persist a change, the embedded database is entrusted with the safe update of the updates to the business critical asset.

Sparkling Logic started using RavenDB 1.0 in 2011. Currently, they use the 4.2 version. Customers use Sparkling Logic both on-prem and on the cloud.

### Why RavenDB?

Sparkling Logic first looked into using a relational database. While it worked with previous generations of this type of technology, it became too difficult to support the evolving requirements of their customers. There is a lot of work that goes into the object layer on top of the database. A document database was the natural evolution for their platform.

RavenDB provides the key combination of features Sparkling Logic was after. As a platform working with business critical assets such financial instruments, a [transactional ACID database](https://ravendb.net/why-ravendb/acid-transactions) was a must. Schemaless structure is also essential for clients entering in varying logics and rules for their decision modeling. To take complicated modeling further, Sparkling Logic needs to manage very complicated relationships using simple queries. Availability is vital, so is extensibility. Using the database's versioning also enabled the company to jettison plugins.

### Benefits of RavenDB

Sparkling Logic has leveraged the following to enhance the value of their platform:

**Productivity.** An easy to use NoSQL transactional ACID database, reducing development time.

**Performance.** Performing complex logic calculations using simple queries while maintaining the overall performance.

**Replication.** Customers need to be able to have disaster recovery and automatic fallback if their primary data store is offline for some reason. Having the ability to replicate their decision-making logic in real-time for [high-availability](https://ravendb.net/why-ravendb/high-availability) is a big plus.

**Adaptability.** What the database did not provide, it made possible, something complicated to achieve with other technologies. If a feature was missing, RavenDB was adaptable enough for Sparkling Logic to build it themselves. In many cases, future versions of RavenDB would have what they were looking for, and the upgrades were seamless.

The powerful **querying system is very intuitive and very natural to use**. In the Sparkling Logic customers world, it is necessary to follow complex relationships between parts of the decision logic, as well as between parts of the decision logic and data models. These relations are at the heart of the effective design, implementation and optimization of decisions.

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">"A document database without querying power would have become a bottleneck. RavenDB gave us the querying power we needed without sacrificing the flexibility in the documents."</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Carlos Serrano, CTO at Sparkling Logic, Inc.</span>
</div>

RavenDB makes querying relationships easy. Sparkling Logic can build robust solutions for the decision management marketplace that includes extensive flexibility and fast performance *a lot sooner*. Their release cycle is shortened by RavenDB's ease of use, especially with complex queries.

<div class="margin-top margin-bottom-xs">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>