# Cost Cutting on the Cloud with a NoSQL Document Database<br/><small>by <a href="mailto:oren@hibernatingrhinos.com">Oren Eini</a></small>

![The right NoSQL Document Database can make several server requests register as one, lowering your cloud costs tremendously.](images/cost-cutting-on-the-cloud-with-a-nosql-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

The emergence of cloud computing has transformed today’s business and information technology landscape. Businesses are offered significant advantages, including the ability to provide device and location independent services. Resources can be shared by a large variety of users and monitored from centralized infrastructure, increasing organizational agility.<br/>
The current “cloud computing race” has vendors offering Service Level Agreements (SLA) which exceed 99.99% availability. Cloud computing allows simpler management of IT infrastructure, giving businesses the opportunity to focus on key business operational activities and reducing time needed to market new products.

## But What about Cost?

Cloud providers maintain that cloud computing offers cost reductions. A company that uses a cloud-based model translates capital expenditure (CapEx) properties like physical hardware to operational expenditure (OpEx), gaining improved technical services like an infrastructure easily adjustable for one-off or irregular computing tasks.<br/>
<a href="https://www.statista.com/statistics/545977/worldwide-cloud-computing-spending/">Available figures</a> on global cloud computing expenditure in the Asia Pacific, EMEA, and North America regions reveal that contemporary organizations are using the cloud for operational and business needs more than ever before.

![The right NoSQL Document Database can unify multiple server requests, lowering your own cloud expenditures.](images/chart.jpg)

<p style="text-align: center"><em>Source: <a href="https://www.statista.com/statistics/545977/worldwide-cloud-computing-spending/">www.statista.com</a></em></p>

In spite of the advantages cloud-service providers offer businesses, organizations (especially startups) need to be aware of potential cost drawbacks when choosing to adopt cloud platforms.

## Steering Clear of Unnecessary Overheads

<div class="pull-left margin-right quote-textbox-left">"By 2023, 3 out of every 4 databases will be on a cloud platform.” </div>
<p style="text-align:justify">According to Gartner, “more than 70% of new application deployments will leverage the cloud for at least one-use case. By 2023, 3 out of every 4 databases will be on a cloud platform.” This evident shift emphasizes the need to avoid unnecessary overheads or costs incurred via poor application-design strategy or by wrong resources selected for the job.</p>

It is imperative for businesses and development teams that design cloud-served applications, to use design and deployment models that guarantee availability, optimize performance, and in due course reduce costs.

Application componentization for instance is a popular approach to software design, that allows development teams to create easily-scalable, distributed applications and services. 
It is possible to overdo it though, breaking an application to too many modules that require separate hosting-platforms and cost additional fees.

It is also advisable to check your cloud-platform characteristics carefully before equipping your application with new services and deploying them. Your cloud provider may for instance be capable of handling only a certain volume of file transfers, or introduce higher fees for transmissions to an external hosting platform.

## A NoSQL Document Database: Make the Right Call

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

As you refactor your applications to provide new services, improve performance and handle data better, you should know that selecting the right database can result in huge savings on your cloud platform. 

Here is a simple illustration: Let’s assume we have a simple application that uses a database, with a modest 200-page views per second. Each page-view for a particular page sends the database 20 queries, bringing us to 4,000 requests per second.<br/>
Going by current pricing models where cloud instances charge by client requests, a conservative cost estimation would be $250 per month for a single page. 
<div class="pull-right margin-left quote-textbox-right">By using a database suitable for the job, you can significantly lower the number of requests.</div>
<p style="text-align: justify">When such a page sends 100 queries per page-view, it may drive the costs up to about $1,100 or more.<br/>
By using a database suitable for the job, you can significantly lower the number of requests. Assuming the queries drop from 100 to 10 with each page view, you can save about 90% of your cloud costs. Certain databases, such as RavenDB, are designed for such circumstances and can reduce the number of remote calls per page even further.</p>

Developers are progressively considering and adopting the use of open source NoSQL document databases that offer efficient horizontal scalability, thus aiding performance.
Some of the best open source NoSQL document databases, like RavenDB, are equipped with efficient organization and storage of data and with queries optimization.

RavenDB’s smart queries-optimizer prepares indexes automatically in advance to serve reoccurring queries, and accelerates the retrieval of results by automatically utilizing and refining existing indexes.<br/>
Advanced features like <strong>lazy queries</strong> help you reduce overhead, decrease the page load time and eliminate the majority of network waits.

## More Ways to Cut Cloud Costs with Your Database

<div class="pull-left margin-right quote-textbox-left">RavenDB optimizes database requests by utilizing <span class="nobr">‘lazy requests’</span> and ‘includes’.</div>
<p style="text-align: justify">An application may lower traffic costs by minimizing the number of times it communicates with its database server.
RavenDB optimizes database requests by utilizing ‘lazy requests’ and ‘includes’.</p>

A <strong>lazy request</strong> is a request that isn’t actually sent to the server the minute a user makes it. Instead, lazy requests are collected until the server’s response to one of them is actually needed, and then sent to the server in a unified transmission. The server responds with a single transmission as well.

RavenDB’s '<strong>include</strong>' functionality pre-loads data that is likely to be needed, stores it in advance in the current session, and delivers it if requested. This way related data, e.g. a product page and various pages detailing the product’s components, can be retrieved in a single call. <strong>Including</strong> is much more efficient than an SQL <strong>join</strong> operation in regard to paging and sorting, and is not prone to vulnerabilities like join’s tendency to retrieve cartesian-product results and their impact on query performance.

## The Best Offense is a Good Defense

Applications failure to economize data requests and retrieval, is translated to waste of money, time and other resources. Here are some established principles to consider when hosting applications or services on the cloud.

<ul>
    <li>Do not spread your data across too many cloud platforms.</li>
Prefer hosting database instances in a common data store to reduce costs.</li> 
    <li>Have a good grasp of what your cloud vendor is providing.</li>
Be sure to realize whether they can accommodate your growing needs.
    <li>Trigger notifications and alerts to track your expenses on your cloud platform.</li>
    <li>Continuously check your cloud services` efficiency.</li>
Identify and handle costly performance bottlenecks and other issues. 
    <li>Suspend unrequired services and resources.</li>
</ul>

<hr style="border-color: grey">
<div class="bottom-line">
    <strong>About RavenDB</strong><br/>
Mentioned in both Gartner and Forrester research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of customers from Startups to Fortune 100 Large Enterprises.<br/>
    <strong><a href="https://ravendb.net/buy">RavenDB Features</a></strong> include:
    <ul>
<li><strong>Easy to Use</strong><br/> RQL Query language is based on SQL</li>
<li><strong>Comfortably corresponds with Relational and Document databases</strong><br/> ETL, Data migration</li>
<li><strong>Multiplatform</strong><br/> C#, Node.js, Java, Python, Ruby, Go</li>
<li><strong>Multisystem</strong><br/> Windows, Linux, Mac OS, Docker, Raspberry Pi</li>
<li><strong>Multi-model</strong><br/> Document, Key-Value, Graph, Counters, Attachments</li>
<li><strong>Low demands</strong><br/> Works efficiently on older machines and smaller devices</li>
<li><strong>Authentication and Data Encryption</strong><br/> Secures your data in rest and transit to prevent theft and misuse</li>
<li><strong>Advanced built-in features</strong><br/> Full-Text Search, MapReduce, and Storage Engine</li>
<li><strong>Schema Free</strong></li><br>
</ul>
    <strong><a href="https://ravendb.net/#play-video">Watch our intro video here!</a></strong><br/>
    <strong><a href="https://ravendb.net/downloads#server/dev">Grab RavenDB for free</a></strong> and get:
<ul>
<li>3 cores</li>
<li>our state of the art GUI interface</li>
<li>Connectivity secured with TLS 1.2 and X.509 certificates</li>
<li>6 gigabyte RAM database with up to 3 nodes cluster</li>
<li>Easy compatibility with cloud solutions like AWS, Azure, and more</li>
</ul>
</div>
    
