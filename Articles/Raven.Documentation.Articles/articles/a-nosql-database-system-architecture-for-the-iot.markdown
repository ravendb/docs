# A NoSQL Database System Architecture for the IoT<br/><small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![A NoSQL Database System Architecture for the IoT](images/a-nosql-database-system-architecture-for-the-iot.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

<h2><em>Even as the amount and form of your data has changed drastically over the past decade, the fundamentals of your database system architecture doesn’t have to.</em></h2>

<p>As far back as the Sears, Roebuck and Company founded at the end of the 19th century, data was always aggregated at the edge and centralized at the company headquarters.</p>
 
<p>A local retail branch would never send all of its sales receipts back. They would tally sales for the day, week, and month and that’s what they reported to HQ. The individual receipts would be kept in a box, for safekeeping.</p>
 
<p>As the IoT ushers in a new wave of digital transformation, the question becomes: What do I do with the data? Do you need a new database system architecture? In most cases, the best way to handle the new information is to retain the classic data hierarchy.</p>

<h2>Moving up the Hierarchy by Leveraging a Distributed Database Architecture</h2>

<p>At the edge, you collect your local data through a kiosk, an application, a sensor or other IoT endpoint. The local data is then pushed to a “regional office” which is usually a NoSQL Database operating on a small server like an ARM Chip or a Raspberry Pi.</p>

<p>The regional office will filter a lot of the data, leaving most of the “individual receipts” at the edge. It will also aggregate it and send it to a more centralized location. The data hierarchy has been in effect for ages, only now it’s digitalized. The data takes on a different form at every step on the hierarchy. It goes from weekly sales in the Midwest to weekly sales for the United States to weekly sales worldwide.</p>

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

<p>The Internet of Things gives your organization the ability to take in petabytes of data in real time. To keep every bit of data on hand will cost you an arm and a leg – especially if you are storing it on the cloud where they charge you by memory and usage and number of queries!</p>

<p>Filtering, Aggregating, and Processing at the edge saves you time, money, and with the right database, developer resources in setting up the right database architecture.</p>

<h2>Creating Solutions as Clear as Water</h2>

<p>A good example would be smart meters reading daily water usage for each home. You can have 100,000 smart sensors communicating data for just one town. Every sensor is reading the outside environment, measuring the water used in the home, recording changes in usage and environment and transmitting it to one of hundreds of nodes nearby.</p>

<p>They can be transferring this information by the day, the hour, even the minute.</p>

<p>With this new class of detailed information, water companies can plan strategies for customers to open their taps at the most optimal time, even offer incentives for them to conserve at certain hours. This can save customer’s money, reduce expenses for the water company, and preserve a precious resource for all of us.</p>

<p>To do it, you need a database that can handle the workload efficiently and with no problems.</p>

<h2> The Right NoSQL Database to the Rescue</h2> 

<p>That’s where the RavenDB Enterprise NoSQL database comes in.</p>

<p>At the heart of your distributed database architecture, RavenDB can ingest millions of operations in real time, store it, aggregate it, and replicate it to a relational or nonrelational database along the data hierarchy.</p>

<p>If you have hundreds of nodes collecting data from all these IoT end points, connectivity becomes a real challenge. What happens when 10 or 15 of your edge point nodes aren’t able to connect to a regional server? Does the entire system collapse?</p>

<p>Not with RavenDB.</p>

<p>Your local nodes will continue to operate while offline, collecting and processing the data as instructed. Once connectivity is restored, the accumulated data will be pushed to the regional servers and the system will return to 100%. As a no-overhead NoSQL Database, you don’t have to waste additional developer time and resources. With RavenDB it all just works. Edge processing is a snap.</p>

<p>This enables you to get out of the Internet of Things what you are after: Better Data to respond to changing market conditions and be the first to seize new opportunities.</p>

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
