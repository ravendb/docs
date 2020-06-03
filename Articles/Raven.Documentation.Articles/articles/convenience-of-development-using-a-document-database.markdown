# Convenience of Development with a Document Database <br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![Convenience of Development with a Document Database](images/convenience-of-development-using-a-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>
Using a Document Database for your application is like installing tailor-made software.

Applications manage their data in the form of objects. Objects have sets of keys and values where your values can be strings, Booleans, URLs, even word files, or email attachments. In some cases, a property inside an object will be another object, creating a hierarchy of values.

Documents within a database work essentially the same way. They are composed of JSON documents, all with keys and values, often containing other documents as values.

This eliminates the problem of impedance mismatch and opens up the world of nonrelational data modeling to you.

## A Document Database is More Effective than a Relational Database

Using a [document database](https://ravendb.net/learn) to manage your application's data is like taking a bike out of the basement and giving it to your kid. Using a relational database is like taking out a box with 250 bike parts and an instructions manual.

To satisfy an application query using a relational database, you need to round out your data's edges before it can travel the length of your application to satisfy your users. That costs latency, complexity, money, and time.

You can spare yourself all these hassles with the right data model.

## Advantages of No Impudence Mismatch for Document Databases

**Performance:** When you code an object, you can capture an entire document. In addition, database Documents and Application objects also co-exist in memory as efficiently as they do on disk. This is another boon to performance.

In an application using a relational database, this object will call data from multiple tables, all of which need to be put together for every query. A single page can require hundreds of trips to the server, making the additional step of putting tables together to fit inside your object cumbersome throughout your app.

## How Your RavenDB Resources Meet the New Challenges

Setting up an IoT Big Data infrastructure requires a lot of your database. Here's what it takes to make it happen:

**It works well on older and smaller machines:** Smaller machines are a given, but if you set up an IoT System where you have 500,000 car doors with small chips processing data, you won't be able to replace those chips in a hardware update. Your software must be able to work on limited hardware with little expectation of added capacity throughout the life of the project. We built RavenDB to accommodate older machines with less computing power, knowing that at the later stages of multi-year projects, nobody will replace the hardware - but you will still need cutting edge performance. <br/>
<br/>
<div>
    <a href="https://cloud.ravendb.net/"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="RavenDB Cloud"/></a>
</div>
<br/>

**DevOps:** When you make a new version of your application, you have schema constraints. If you change the data model in your application objects, you must change the schema in the database, which is a nontrivial task. Creating new classes that add properties, even if you are inheriting everything from the previous version, also requires fundamental changes to your database layer.

RavenDB is schemaless, so when you make a new version of your application, you don't have to worry about breaking the database or stalling your next release. New properties will automatically be inside each document.

Document modeling fits well with RavenDB's domain-driven design. It enhances the speed at which you develop code, making it a lot easier to code an application to take in data the way you want it. New releases are a snap because the coding time for that is also truncated.

**Operation overhead:** If you have different schemas on different versions of your application, running them concurrently on a single relational database can get very hairy, costing you time, complexity, and money. Using a document database like RavenDB lets you [run several versions of your application on a single instance](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/2-zero-to-ravendb), keeping everything simple and less costly.

**Scaling out:** The smooth flow of objects taking in documents also enables smoother scaling out. As your traffic increases, and the data you are taking in expands, taking on more nodes is a lot easier and with fewer speedbumps. Data migration is a non-issue. When scaling out, cloud platforms often require you to migrate your data to the new scaled out database they provisioned for you, forcing you to pay for two databases while they are migrating, and for the migration itself.

## Benefits and Costs of Adding NoSQL to Your Skillset

Most application developers don't capitalize on these efficiency gains because the investment in learning an entirely new way to model data appears too daunting to be worth the benefits.

There are two reasons why the task is not as challenging as you may think, and the returns are greater than originally imagined.

1. The time you invest in learning document databases and nonrelational data modeling is a *one-time* investment. The returns are *ongoing* for every application that includes a document database.

2. The learning path is made smooth and easy by a wealth of resources that guide you at each step. RavenDB offers [webinars](https://ravendb.net/learn/webinars), easy to understand [documentation](https://ravendb.net/learn/docs-guide), a [Bootcamp](https://ravendb.net/learn/bootcamp), and even an [owner's manual](https://ravendb.net/learn/inside-ravendb-book) authored by RavenDB CEO Oren Eini.


It's as easy as that, and the benefits will fit into your application, *and your bank account*, as smoothly as a document modeled data will fit into your next application object.<br/>
<br/>
<div>
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner.jpg" class="img-responsive m-0-auto" alt="Schedule a free one-on-one live RavenDB Demo"/></a>
</div>
<br/>

