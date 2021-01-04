# RavenDB Web Analytics Use Case: Big Data Creates Big Revenue

<div class="article-img figure text-center">
  <img src="images/big-data-document-database-etl-replication-ravendb-case-study.jpg" alt="RavenDB Web Analytics Use Case: Big Data Creates Big Revenue" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<br/>

Job boards provide a great service. Along with giving applicants the opportunity to find the job that’s right for them, job posters have the chance to position their vacancy to the most qualified audience. The more precise the metrics available, the better the match for both the applicant and the poster and the sooner someone finds a job.

For a company matching job seekers with job providers, offering richer metrics can produce new revenue streams. The right database can extract and process their new <em>gold mine of Big Data</em>.

## The Client

An England based company provides schools with a faster and more cost-effective way to recruit new staff. Over 7,500 schools and colleges have advertised more than 65,000 jobs to 1.7 million registered candidates.

## The Use Case

Their application software allows candidates to apply quickly to more schools, cutting hours out of the process and facilitating pain-free career moves. Their site takes in personal data on all the registered candidates who make more than 1 million site visits to us each month.

Each registered candidate brings to every page visit a wealth of personalized metadata that takes precise measurements of what he or she is looking for, and their success in finding it over for every visit, page, time span, even event taken on the site.

This Big Data enables the company to create a web analytics solution that is "Google Analytics on steroids". They want to shorten the time someone is looking for work, and better empower educational institutions to fill vacancies <em>faster and more efficiently</em>.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

## Project Objectives

By use of Big Data and Analytics, RavenDB is being used to answer the following:

* How do job candidates run through their site? 
* Which pages give the most value? Which events give the most value?
* Where do they stall? Where do they stop? 
* How can a job posting be measured for it’s accuracy, and effectiveness for a given time, geographic location, or specific audience?
* How do changes to the site impact visitors and visitor flow towards meeting their goals?

The data that is taken in needs to be organized, aggregated, and put to a report that can be <em>instantly accessed</em> by users, job posters, and managers of the company, and developers of the site.

<img class="floating-right img-responsive" alt="Data is the New Oil" src="images/data-is-the-new-oil.jpg" />

By having <em>real time access</em> to this deep a level of data, job posters get more target opportunities which inevitably will cost more. Applicants will get quicker turnaround times, which gives the company the option to offer a "silver" or "gold" membership based on the level of precision the applicant is after.

To provide fast performance and constant availability, they chose to migrate to a NoSQL solution that supports a multi-server data cluster.

By using the right database, Big Data has become <em>a new source of revenue</em> from both consumers and vendors.

## Database Requirements

**ACID.** Once a job is filled, it needs to be taken off the site. In some cases, once a certain number of people apply, the job posting will be taken off the site until the human resources department sorts out the applications and filters out the best ones. In other cases, a headhunter will submit a resume without realizing that the applicant applied to the same position.

ACID consistency prevents a 101th submission once the limit for applicants reaches 100. It makes sure that the same resume isn’t submitted twice by different parties at the same time. If a job has been filled, the moment it is offline all future applications submitted will be given notifications that their information was not sent to the job poster and they need to look elsewhere. The right database will provide the ideal "You might also be interested in ..." options that can be placed in the notification box.

RavenDB supports ACID <em>across the entire data cluster</em>, ensuring all these requirements are met.

**Data Clustering.** To support millions of users, the site must expand from their single server relational solution. A single point of failure for so much traffic can be devastating. By establishing a data cluster, they are able to reduce load, increase performance, and become fault tolerant. If a server goes down, instead of system failure, two other servers will be able to pick up the slack and the user won’t feel any change or disruption in service.

As a distributed database, RavenDB not only enables multiple node data clusters but also gives you the ACID <em>guarantee for data safety and consistency</em> relational databases are known for.

{RAW}
{{INTRO_VIDEO}}
{RAW/}

**Scalability.** During peak times, like late summer when openings must be filled or throughout the winter when there is a demand for substitute teachers, or when there are discounts for membership or posting job opportunities spike traffic, RavenDB is relied upon to enable developers to add nodes to manage load and maintain top performance. Within minutes the site can expand the number of "cashiers" available to serve each user.

**ETL Replication.** Given the legacy databases already in use, they want to maintain their existing schema. A NoSQL document database must support multi-model architecture by replicating data from document form into their legacy relational form.  RavenDB has an ETL replication feature that fits document data into a schema, along with an SQL setup wizard that lets you take relational data and fit it into document form.

**Report tool integration.** RavenDB’s ETL and easy integration to common reporting tools enables them to turn their data into visual metrics seamlessly for job seekers, and job posters, and internal use.

**Replication.** To maintain high-availability and to establish solid fault tolerance, they need replication capabilities. RavenDB can take their data and update the rest of the nodes in the data cluster <em>in real time</em> as the data comes in. In the case of "split brain", where one node in the cluster cannot communicate with the others, that node can continue to take in data. Once the connection is restored, the data will be replicated throughout the cluster.

**Ease of Use.** RavenDB’s C# API and thorough documentation makes going to production a snap for this project, which uses a .NET framework. RavenDB also has API’s for Node.js, Java, Python, Ruby, Go, and more. We  offer 24/7 tech support for all issues, along with consultant options if you need developers to help you <em>code your application to perfection</em>.


<div class="bottom-line">
    <p>
        Would you like to see more?
    </p>
    <p>
        <a href="https://ravendb.net#play-video"><strong>Watch our Explainer Video</strong></a> or <a href="https://ravendb.net/downloads"><strong>Grab RavenDB 4.0 for free</strong></a> and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running quickly for your next project.
    </p>
</div>