# A NoSQL Database For More Demanding Workloads
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![A NoSQL Database For More Demanding Workloads](images/nosql-database-creates-personal-experience-for-every-user.jpg)

{SOCIAL-MEDIA-LIKE/}

Most websites and landing pages still try to be all things to all people — putting the onus on users to find what they want.

RavenDB newest client, <a href="https://view.do" target="_blank" rel="nofollow">view.DO</a> uses Web 5.0 technology to digitally converse with each customer and prospect to give them immediate and specific answers, content, and follow-ups that relate to their particular needs.

view.DO allows you to provide users exactly what they are looking for. Their Digital Experience Platform (DXP) uses real-time data to deliver hyper-personalized, interactive experiences, much like a modern 'choose your own adventure' for your content.

For years, view.DO has been operating in a Windows environment using core RDBMS (a <a href="https://en.wikipedia.org/wiki/Relational_database" target="_blank" rel="nofollow">relational database</a> system) along with reporting tools. As customer requirements grew, they needed to innovate their platform. Primarily, there was a need to support new features like serving up dynamic, personalized content rapidly. Their SQL platform made schema changes hard.

To create radically new ways of presenting tailored information to their users, a fundamentally new approach to system design was needed.

<div class="margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

## Seizing the Opportunity with RavenDB NoSQL Database

A personalized experience requires lots of data. Every user brings demographic, contextual, behavioral information, and a lot more. The more data that's available, the more personalized the experience.

view.DO's entire business model is based on collecting, sorting, and processing as much information as it can get in as little time as possible.

Their relational databases became too overwhelmed by the volume of data required for their personalization features.

A distributed [NoSQL database](https://ravendb.net/articles/ravendb-best-nosql-database-example-for-startups) enables them to scale effectively to meet the most demanding workloads. The right document database lets them build and update visitor profiles on the fly to deliver the low latency required for real-time engagement with their customers. As a flexible and performant database, RavenDB serves view.DO as the foundational layer for managing their engagement life cycle.

As their range of products, services, users, and visitors continues to expand, so does the strain on their relational databases. There is a need to fragment customer data as different applications work with different customer data.

NoSQL document databases use a flexible data model that enables multiple applications to access the same customer data as well as add new attributes without affecting other applications.

Data Flow for General User Experience Platform
![Data Flow for General User Experience Platform](images/data-flow-for-general-user-experience-platform.jpg)
<small>Destination data rendered on Mobile and Web depends on a data store that can support hyper-personalization and profile management capabilities.</small>

## Why RavenDB and not Other NoSQL Database Options
view.DO needed a database that could do the following:
<ul>
    <li>Update multiple documents in a single transaction</li>
    <li class="margin-top-sm">Perform fast optimistic consistency</li>
    <li class="margin-top-sm">Allow for smooth SQL data migration to a NoSQL database</li>
    <li class="margin-top-sm">Tune the database easily for highly concurrent updates to match workload on demand</li>
    <li class="margin-top-sm">A dedicated team of professionals committed to product excellence</li>
    <li class="margin-top-sm">Google Cloud Platform Readiness</li>
</ul>

MongoDB was initially considered. They quickly found that MongoDB did not support the key features most important to them. MongoDB did offer relative schema flexibility to a relational database, but it still cost resources to tune these NoSQL databases.

view.DO performed the same tests on RavenDB as they did for MongoDB and Cassandra. They were pleased with RavenDB's ground-up approach that met their core requirements.

RavenDB executed a multi-document update in a single transaction without any performance degradation. The customer was able to tune the database on rare occasions with negligible downtime.

This customer uses C# to develop web API logic. It optimizes database operations exploiting async parallel constructs of C#, so the performance of the **Experience Web** applications on RavenDB was fast as well.

view.DO was delighted with the open-source nature of RavenDB. During platform development, they were able to contact the RavenDB community and get help.

## New Platform

Thanks to RavenDB, the customer now has a platform that is reliable, adaptable, secure, and fast at delivering the right personalized content in the proper context every time. Delays and sluggishness were removed, and end-users can now enjoy a far better user experience. The platform is enabled on the Google Cloud Platform, offering reliability and speed.

## Reducing the complexity of Platform Deployments

view.DO intends to leverage RavenDB's SQL [ETL Services](https://ravendb.net/why-ravendb/integration-with-relational-databases), Revisions, and Attachments features to eventually replace custom code and CQRS microservices, reducing the complexity of their platform deployments.

view.DO is on a hi-growth path, and they have the confidence that RavenDB will be a reliable document storage solution to further enable that growth.

<div>
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>

