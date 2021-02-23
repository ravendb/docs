<h1>NoSQL Database for Remote Working</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/cronos-sky-use-ravendb-nosql-database-for-remote-working-solutions.jpg" alt="Cronos uses the RavenDB NoSQL Document Database to provide their remote workforce of over 4,000 employees laptops, tables, chairs, and more." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">Cronos, Sky2, chose RavenDB NoSQL database to make remote working feel cozy.</p>

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <a href="https://cronos-groep.be/" target="_blank" rel="nofollow"><img src="images/cronos.png" class="img-responsive m-0-auto" alt="Cronos"/></a>
</div>

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">MapReduce, indexes, document compression, subscriptions, expiration, revisions, refresh... we use RavenDB for everything!</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Steve Hansen - CTO, Cronos</span>
</div>

By March 2020, thousands of people suddenly found themselves working from home. While Cronos effectively handled the IT challenges to adapt, there was one problem nobody could anticipate.

Comfort.

Who would expect an employee to be less comfortable working in their own home than at the office?

There was a productivity gap in employees using the same chairs to perform their duties where they binged watched reruns of Grey's Anatomy. They encouraged employees to purchase new desks and chairs that created a more focused work environment.

They also offered laptops, stationery, and all the basics one needs to turn a piece of their home into their private workspace.

Their data scaled to the point that they quickly reaching the limits of their relational database.

### Immediate Needs to Scale and Simplify

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">Using SQL, we got to the point where we couldn't add anything to our application without breaking something.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Steve Hansen - CTO, Cronos</span>
</div>

The **make your own office** concept was novel. The challenge was people moving from branch to branch, even from daughter company to daughter company.

This made things hairy.

One employee would purchase a computer, table, and chair, making their home office a fixed location. They would move from project to project, working for different legal entities within the same company.

They are moving locations while staying in the same place.

Their paperwork has to go from one space in the system to another.

They already had over 500 tables to track employee names, addresses, IDs, department, desk type, chair type, etc. Moving from one project to another meant creating new IDs, moving the inventory of the office desk, chairs, and computers from one business to another, and reassigning increases and decreases in local budgets to accommodate new employees.

This risked creating even more tables while making things too complicated with employees using different IDs existing as different people in different tables.

Data systems kept scaling out in the form of more tables, fields, and volume of data. Whenever anything new was added, it broke a piece of the system.

Something had to be done to allow comfortable remote working.

### Moving from an SQL Monolith to a Document Driven Distributed Architecture Resulted in Massive Performance Gains

**To store everything about an employee in a single place, all the data was migrated to the RavenDB NoSQL Database.**

Using RavenDB and the [RQL query language](https://ravendb.net/features/querying/rql) was easy. There were plenty of webinars, use cases, and documentation that was simple to read. The RavenDB setup wizard got them up and running in minutes.

RavenDB's [automatic index](https://ravendb.net/features/indexes/auto-indexes) feature made sure everything had its own index. Documents made the data easier to find, and RavenDBs native full-text search reduced the need for external addons.

When someone made a data query, their legacy SQL systems took up to 30 seconds to return the answer. In many cases, the user got timed out.

Once they moved to RavenDB, query times dropped to one second or less.

The effort involved in changing over to a document model paid out in spades.

The development team ware experts in SQL, using the old school technology for decades. The biggest challenge was to retrain their minds to think in document data modeling.

As a [schemaless database](https://ravendb.net/articles/save-time-and-money-with-non-relational-database-data-compression), RavenDB enabled them to perform all the trial and error necessary to get to a point where they were experts in document data models.

The cost of exceptions was minimal.

After using the new system for two months, they became very comfortable with the new way of organizing data.

### RavenDB Features Made a Huge Difference

Some of the features of RavenDB gave this project an additional boost in ROI:

Data expiration. Local regulations demand certain data be kept for four weeks. *They use this feature to clean out parts of their database to comply with local ordinance.* To maintain GDPR European guidelines, other data can be held for years.

Attachments. Other regulations required employees to declare that their work computers being used at home were used exclusively for work. If there were any personal use on these machines, they would get taxed. Employees had to sign statements that they were using their work resources for work. RavenDB was able to store these attachments inside an employees' document.

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">RavenDB is a real developer database and not just a simple data store.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Steve Hansen - CTO, Cronos</span>
</div>

<div class="margin-top-md">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>