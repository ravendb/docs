# RavenDB NoSQL Management Studio GUI<br/><small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![RavenDB NoSQL Management Studio GUI](images/ravendb-nosql-document-database-management-studio-gui.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

<p>The RavenDB NoSQL Document Database Management Studio is the Apple of Databases, and you don’t need to pay extra or waste time and effort attaching it as an addon. It’s a native part of RavenDB that comes free with every license: Community, Professional, and Enterprise.</p>

<p>The Management Studio is a fully loaded graphical user interface (GUI) that monitors both operational and performance metrics of your database. You can perform most functions without online commands -- just point and click. Monitor in real time how much disk space you are utilizing, disk i/o, disk memory, computer memory, and network bandwidth to see whether or not you are getting 100% out of the resources you paid for either on premises or in the cloud.</p>

<p>Easily pinpoint bottlenecks to keep the database layer of your application in peak performance at all times. RavenDB maintains a live log of all your events to isolate any potential issues - making troubleshooting a snap.</p>

<p>You can also create just about anything you need through the studio. You can set up databases, nodes, collections, and even create and edit documents at the interface level. Connect remotely to your database from any browser anywhere and do whatever you need to maintain your data. Even if you are new to RavenDB or database admin, the Management Studio lets you navigate like a pro with its easy to use dashboard.</p>

<p>Create code, set up queries, monitor indexes and MapReduce aggregates in our all-in-one interface.</p>

<div style="color: #97a7b7;">
The RavenDB NoSQL Document Database Management Studio is:
<ul>
<li>FREE with every license</li>
<li>Stunning & intuitive UI</li>
<li>Live monitoring</li>
<li>Effortless configuration</li>
<li>Comprehensive performance statistics</li>
<li>Deep insight into database internals</li>
<li>Extensive logging & incident reporting</li>
</ul>
<a href="https://ravendb.net/download"><img alt="Try it out" src="images/management-banner.jpg" /></a>
</div>

<h2>What you can Monitor with the Management Studio</h2>
<a href="http://live-test.ravendb.net/studio/index.html"><img alt="Monitor performance internals with our NoSQL Database GUI" src="images/management-screen-1.jpg" /></a>

<h2>Monitor Your Database Topology</h2>
<a href="http://live-test.ravendb.net/studio/index.html"><img alt="A distributed database needs a good graphical interface for its topography" src="images/management-screen-2.jpg" /></a>

<p>For a distributed database, your database topology consists of databases and nodes that make up your cluster. At any moment, you can see which nodes are carrying what databases. You can also see which server locations the nodes are hosted on. Monitor what tasks each are performing, who are the lead nodes, and to where ETL processes are replicating to. </p>
<p>If a node goes down or a new node becomes the leader of the cluster, you will be the first to know. </p>

<h2>Follow Extensive Logging and Incident Reporting</h2>
<p>The Studio will also track operation logs, taking records of all the actions going on in your database. It also records debug information about what is currently going on the server, providing you insights about the functionality of the server itself.</p>

<p>If you have slow reads, it will show in the log files. Things like small batch files, or any type of issue will appear. This gives you the data you need to break open bottlenecks and resolve issues the moment they appear.</p>

<p>From the studio you can also download debug logs and send it to RavenDB support engineers who will isolate areas for improvement even faster.</p>

<h2>Indexes </h2>
<p>Using the Management Studio, you can see both static-indexes, those you set up, and auto-indexes – indexes RavenDB will set up automatically to make your queries run faster. RavenDB will only perform queries based on an index to maximize performance. When a query is made, it will look for an index, improve upon one, or create one itself to best fulfill the request. Using the studio, you can see what your database made to <a href="https://ravendb.net/docs/article-page/4.1/csharp/client-api/cluster/speed-test">speed up your application</a>. </p>

<p>You can see which indexes are applied to which databases, how they are performing, and what steps need to be taken, if any, to further optimize how you cull your data.</p>

<h2>MapReduce Helps Sort and Analyze Big Data</h2>
<p>Monitor Big Data with our MapReduce Visualizer that lets you track the progress of data aggregations step by step. It graphs the relations between your documents and aggregate results.</p>
<a href="http://live-test.ravendb.net/studio/index.html"><img alt="Visualize your MapReduce for Document Database Aggregations" src="images/management-screen-3.jpg" /></a>
<p>You can see the documents that went into each aggregation individually or segmented. You can also monitor how long each step in the <a href="https://ravendb.net/docs/article-page/4.1/csharp/indexes/map-reduce-indexes">Map and Reduce</a> process took to determine performance bumps that can be quickly smoothed.</p>

<h2>The Studio Let’s You See the RavenDB Document Database in Action</h2>
<p>Once you execute your requests, you can see all of the data returned right on the screen. It can be data sets, aggregations results, performance results for indexes and reads or writes per second, even memory utilized during the operations.</p>
<p>Once the data set appears, you can click on the collections or documents and drill down. For each document, you can see the requested columns, or the document itself. You can also click on the attachments to see which files were added to the document.</p>
<p>This can be a health care application where you want X-rays and test results as part of the patient’s info, or it can be an advertising platform where you want the actual graphics to accompany sales information for a campaign. The possibilities are endless.</p>

<h2>What You Can Create in Your Studio</h2>
<a href="https://www.gartner.com/reviews/review/view/556267"><img alt="The RavenDB NoSQL Document Database makes development frictionless, reducing your software release cycle" src="images/perfect-fit-for-release-cycles.jpg" /></a>
<p>The Management Studio is more than monitoring. You can run your entire data system off it. With a point and a click you can set up a new regular or encrypted database. You can create new documents and assign them to a specific collection. If the documents you need to make are very similar to those in a particular collection, you can clone new documents that are duplicates of other documents and edit from there.</p>
<p>Using the studio, you can perform queries, create indexes, patch bundles of documents, and when it is all done, you can see the results and export them to a CSV file.</p>
<a href="http://live-test.ravendb.net/studio/index.html"><img alt="Perform queries, create indexes, export your data results to a CSV file with RavenDB Management Studio." src="images/management-screen-4.jpg" /></a>
<p>For aggregations, you can create MapReduce indexes where you see the results and the segments or even documents that comprise them.</p>
<h2>Distributed Database Clusterwide Capabilities</h2>
<p>As a distributed database, you need functionality to manage your database cluster. That includes creating new nodes to your cluster and assigning the right databases to them. It also means connecting the node to the right server and assigning its status as a leader, mentor, or watcher. To guarantee each node has the memory it needs to function at peak performance, you can even decide how many cores to assign them. You also have the ability to remove a node.</p>
<p>You also have the option to configure a database independently from the cluster, giving it a custom-made environment with which to operate.</p>
<p>With your <a href="https://ravendb.net/download">NoSQL Document Database</a> Management Studio you control everything. You can perform these operations without a command line.</p>

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

<h2>Keep Everyone Busy with Tasks</h2>
<p>In the Studio, working with SQL databases and other nonrelational applications is a breeze. You can migrate your data from SQL into RavenDB thereby converting it into a Document format. At the click of a button you can upgrade your data assets from pen and paper technology to the 21st century.</p>
<p>You can also create an ongoing ETL to push RavenDB data to an SQL database. This is ideal for analytics, taking in data as a document and pushing it where it can be optimally visualized.</p>
<p>Define work tasks to be done by your database on an ongoing basis. It can be external replication to another cluster, an on-prem server or in the cloud. It can be to a relational or another nonrelational database. Data replication can go anywhere you send it.</p>
<p>You can set the task and set the node responsible for the task. If that node were to go offline, RavenDB will automatically reassign those tasks to functioning nodes.</p>
<p>With the studio, you can schedule a backup or snapshot of the database and determine when and how often these backups will take place. You can also set up data subscriptions, enabling other machines to listen in on your data operations and perform actions when data that meets their criteria enters your system.</p>

<a href="https://ravendb.net/docs/article-page/4.1/csharp/studio/database/tasks/ongoing-tasks/external-replication-task#external-replication---details-in-tasks-list-view"><h2>External Replication - Details in Tasks List View</h2></a>
<a href="http://live-test.ravendb.net/studio/index.html"><img alt="ETL Replication to backups, relational SQL databases, the cloud, and more with your NoSQL RavenDB Document Database" src="images/management-screen-5.jpg" /></a>

<h2>It’s All Here</h2>
<p>The RavenDB NoSQL Document Database Management Studio is geared to making your developers more productive, and to bring your applications to production faster. Even without experience in coding databases, you can quickly manage our NoSQL Document Database like an expert with our point and click GUI that requires no additional investment or integration.</p>

<div class="bottom-line">
    <p><strong>About RavenDB</strong><br/>
Mentioned in both Gartner and Forrester research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of customers from Startups to Fortune 100 Large Enterprises.</p>
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
