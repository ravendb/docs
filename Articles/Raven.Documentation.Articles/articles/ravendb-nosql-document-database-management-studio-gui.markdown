# RavenDB NoSQL Management Studio GUI
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/ravendb-nosql-document-database-management-studio-gui.jpg" alt="RavenDB NoSQL Management Studio GUI" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<br/>

# RavenDB Document Database Management Studio

<a href="https://ravendb.net/">
    <img class="img-responsive m-0-auto" alt="RavenDB GUI Management Studio aims to make your Document Database experience easy to use." src="images/orens-quote-2.png" />
</a>

<strong>The RavenDB NoSQL Document Database Management Studio</strong> is the <strong>Apple of Databases</strong>, and you don’t need to pay extra or waste time and effort attaching it as an addon. It’s a native part of RavenDB that comes free with every license: <a href="https://ravendb.net/download">Community</a>, Professional, and <a href="https://ravendb.net/buy">Enterprise</a>.

The Management Studio is a fully loaded graphical user interface (GUI) that monitors both operational and performance metrics of your database. You can perform most functions without online commands - just point and click. Monitor in real time how much disk space you are utilizing, disk I/O, disk memory, computer memory, and network bandwidth to see whether or not you are getting 100% out of the resources you paid for either on premises or in the cloud.

Easily pinpoint bottlenecks to keep the database layer of your application in peak performance at all times. RavenDB maintains a live log of all your events to isolate any potential issues - making troubleshooting <em>a snap</em>.

You can also create just about anything you need through the studio. You can <a href="https://ravendb.net/docs/article-page/4.1/csharp/studio/server/databases/create-new-database/general-flow">set up databases</a>, nodes, collections, and even create and edit documents at the interface level. Connect remotely to your database from <em>any browser anywhere</em> and do <em>whatever you need</em> to maintain your data. Even if you are new to RavenDB or database admin, the Management Studio lets you navigate like a pro with its easy to use dashboard.

Create code, set up queries, monitor indexes and MapReduce aggregates in our all-in-one interface.

<div style="background-color: #2b333b; text-align: center; color: #97a7b7">
    <p style="padding-top: 15px"><strong>The RavenDB NoSQL Document Database Management Studio is:</strong></p>
    <ul style="list-style-type: none; color: #97a7b7">
        <li><em>FREE with every license,</em></li>
        <li><em>Stunning & intuitive UI,</em></li>
        <li><em>Live monitoring,</em></li>
        <li><em>Effortless configuration,</em></li>
        <li><em>Comprehensive performance statistics,</em></li>
        <li><em>Deep insight into database internals,</em></li>
        <li><em>Extensive logging & incident reporting</em></li>
    </ul>
    <a href="https://ravendb.net/download">
        <img class="img-responsive m-0-auto" style= "padding-bottom: 20px" alt="Try it out" src="images/management-banner.jpg" />
    </a>
</div>

## What you can Monitor with the Management Studio

<a href="http://live-test.ravendb.net/studio/index.html"><img class="img-responsive m-0-auto" alt="Monitor performance internals with our NoSQL Database GUI" src="images/management-screen-1.jpg" /></a>

## Monitor Your Database Topology

<a href="http://live-test.ravendb.net/studio/index.html"><img class="img-responsive m-0-auto" alt="A distributed database needs a good graphical interface for its topography" src="images/management-screen-2.jpg" /></a>

For a distributed database, your database topology consists of databases and nodes that make up your cluster. At any moment, you can see which nodes are carrying what databases. You can also see which server locations the nodes are hosted on. Monitor what tasks each are performing, who are the lead nodes, and to where ETL processes are replicating to.

If a node goes down or a new node becomes the leader of the cluster, you will be <em>the first to know</em>.

## Follow Extensive Logging and Incident Reporting

The Studio will also track operation logs, taking records of all the actions going on in your database. It also records <a href="https://ravendb.net/docs/article-page/4.1/csharp/indexes/troubleshooting/debugging-index-errors">debug information</a> about what is currently going on the server, providing you insights about the functionality of the server itself.

If you have slow reads, it will show in the log files. Things like small batch files, or any type of issue will appear. This gives you the data you need to break open bottlenecks and resolve issues the moment they appear.

From the studio you can also download debug logs and send it to RavenDB support engineers who will isolate areas for improvement even faster.

## Indexes

Using the Management Studio, you can see both static-indexes, those you set up, and auto-indexes – indexes RavenDB will set up automatically to make your queries run faster. RavenDB will only perform queries based on an index to maximize performance. When a query is made, it will look for an index, improve upon one, or create one itself to best fulfill the request. Using the studio, you can see what your database made to <a href="https://ravendb.net/docs/article-page/4.1/csharp/client-api/cluster/speed-test">speed up your application</a>.

You can see which indexes are applied to which databases, how they are performing, and what steps need to be taken, if any, to further optimize how you cull your data.

## MapReduce Helps Sort and Analyze Big Data

Monitor Big Data with our MapReduce Visualizer that lets you track the progress of data aggregations step by step. It graphs the relations between your documents and aggregate results.

<a href="http://live-test.ravendb.net/studio/index.html"><img class="img-responsive m-0-auto" alt="Visualize your MapReduce for Document Database Aggregations" src="images/management-screen-3.jpg" /></a>

You can see the documents that went into each aggregation individually or segmented. You can also monitor how long each step in the <a href="https://ravendb.net/docs/article-page/4.1/csharp/indexes/map-reduce-indexes">Map and Reduce</a> process took to determine performance bumps that can be quickly smoothed.

## The Studio Lets You See the RavenDB Document Database in Action

Once you execute your requests, you can see all of the data returned <em>right on the screen</em>. It can be data sets, aggregations results, performance results for indexes and reads or writes per second, even memory utilized during the operations.

Once the data set appears, you can click on the collections or documents and drill down. For each document, you can see the requested columns, or the document itself. You can also click on the attachments to see which files were added to the document.

This can be a health care application where you want X-rays and test results as part of the patient’s info, or it can be an advertising platform where you want the actual graphics to accompany sales information for a campaign. The possibilities are endless.

## What You Can Create in Your Studio

<a href="https://www.gartner.com/reviews/review/view/556267" rel="nofollow"><img class="img-responsive m-0-auto" alt="The RavenDB NoSQL Document Database makes development frictionless, reducing your software release cycle" src="images/perfect-fit-for-release-cycles.jpg" /></a>

The Management Studio is more than monitoring. You can run your entire data system off it. With a point and a click you can set up a new regular or encrypted database. You can create new documents and assign them to a specific collection. If the documents you need to make are very similar to those in a particular collection, you can clone new documents that are duplicates of other documents and edit from there.

Using the studio, you can perform queries, create indexes, patch bundles of documents, and when it is all done, you can see the results and export them to a CSV file.

<a href="http://live-test.ravendb.net/studio/index.html"><img class="img-responsive m-0-auto" alt="Perform queries, create indexes, export your data results to a CSV file with RavenDB Management Studio." src="images/management-screen-4.jpg" /></a>

For aggregations, you can create MapReduce indexes where you see the results and the segments or even documents that comprise them.

## Distributed Database Clusterwide Capabilities

As a distributed database user, you need functionality to manage your database cluster. That includes creating new nodes to your cluster and assigning the right databases to them. It also means connecting the node to the right server and assigning its status as a leader, mentor, or watcher. To guarantee each node has the memory it needs to function at peak performance, you can even decide how many cores to assign them. You also have the ability to remove a node.

You also have the option to configure a database independently from the cluster, giving it a custom-made environment with which to operate.

With your <a href="https://ravendb.net/download">NoSQL Document Database</a> Management Studio you control everything. You can perform these operations without a command line.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

## Keep Everyone Busy with Tasks

In the Studio, working with SQL databases and other nonrelational applications is a breeze. You can migrate your data from SQL into RavenDB thereby converting it into a Document format. At the click of a button you can upgrade your data assets from pen and paper technology to the 21st century.

You can also create an ongoing ETL to push RavenDB data to an SQL database. This is ideal for analytics, taking in data as a document and pushing it where it can be optimally visualized.

Define work tasks to be done by your database on an ongoing basis. It can be external replication to another cluster, an on-prem server or in the cloud. It can be to a relational or another nonrelational database. <a href="https://ravendb.net/docs/article-page/4.1/csharp/server/clustering/replication/replication">Data replication</a> can go anywhere you send it.

You can set the task and set the node responsible for the task. If that node were to go offline, RavenDB will automatically reassign those tasks to functioning nodes.

With the studio, you can schedule a backup or snapshot of the database and determine when and how often these backups will take place. You can also set up data subscriptions, enabling other machines to listen in on your data operations and perform actions when data that meets their criteria enters your system.

<a href="https://ravendb.net/docs/article-page/4.1/csharp/studio/database/tasks/ongoing-tasks/external-replication-task#external-replication---details-in-tasks-list-view"><h2>External Replication - Details in Tasks List View</h2></a>

<a href="http://live-test.ravendb.net/studio/index.html"><img class="img-responsive m-0-auto" alt="ETL Replication to backups, relational SQL databases, the cloud, and more with your NoSQL RavenDB Document Database" src="images/management-screen-5.jpg" /></a>

## It’s All Here

The RavenDB NoSQL Document Database Management Studio is geared to making your developers more productive, and to bring your applications to production faster. Even without experience in coding databases, you can quickly manage our NoSQL Document Database like an expert with our point and click GUI that requires <em>no additional investment or integration</em>.

<div class="bottom-line">
    <p><strong>About RavenDB</strong><br/>
Mentioned in both <em>Gartner</em> and <em>Forrester</em> research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of customers from Startups to Fortune 100 Large Enterprises.</p>
    <p><strong><a href="https://ravendb.net/buy">RavenDB Features</a></strong> include:
    <ul>
<li><strong>Easy to Use:</strong> RQL Query language is based on SQL</li>
<li>Works well with existing relational database. ETL feature and migration to Document model available.</li>
<li><strong>Multiplatform:</strong> C#, Node.js, Java, Python, Ruby, Go</li>
<li><strong>Multisystem:</strong> Windows, Linux, Mac OS, Docker, Raspberry Pi</li>
<li><strong>Multi-model:</strong> Document, Key-Value, Graph, Counters, Attachments</li>
<li><strong>Works efficiently</strong> on older machines and smaller devices</li>
<li>Built in Full-Text Search, MapReduce, and Storage Engine</li>
<li>Schema Free</li>
</ul>
    </p>
    <p>
        <strong><a href="https://ravendb.net/#play-video">Watch our intro video here!</a></strong>
    </p>
    <p><strong><a href="https://ravendb.net/downloads#server/dev">Grab RavenDB for free</a></strong> and get:
    <ul>
<li>3 cores</li>
<li>our state-of-the-art GUI interface</li>
<li>Connectivity secured with TLS 1.2 and X.509 certificates</li>
<li>6 gigabyte RAM database with up to a 3-server cluster</li>
<li>Easy compatibility with cloud solutions like AWS, Azure, and more</li>
</ul>
    </p>
</div>
