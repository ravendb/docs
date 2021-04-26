<h1>The Right Database for Record Management Systems</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/health-billing-systems-rely-on-ravendb-for-timely-health-insurance-payments.jpg" alt="A database for record management systems that is ACID, available even if connectivity is disrupted, and works with data the same way insurance companies do." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="margin-top-sm" style="font-size: 24px;">Health Billing Systems Rely on RavenDB for Timely Health Insurance Payments</p>

When you are about to have surgery, the last issue you want to worry about is how your insurance will cover this. Health Billing uses RavenDB to make sure you can put your mind to rest.

Health Billing Systems delivers billing services to anesthesia providers, ensuring that their insurance companies cover patients who incur such expenses. They collect all the necessary forms, determine their appropriate destinations, and work with the patient's insurance company to make sure payments are completed smoothly.

They use RavenDB for their records management system. Why a separate billing system for anesthesiologists?

Most medical staff bill based on the procedure. Whether an hour or three hours, the cost is based on the operation.

For the medical staff responsible for keeping a patient soundly asleep during a procedure, it's time-based. Their mission is to keep the patient adequately medicated at all times so they are at rest without giving them too much which could be dangerous.

Such work requires constant monitoring, which changes the compensation schedule.

### A Database That Maintains Full Service

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">We have been running RavenDB for six years now and we haven't lost a document.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">– Brad Schultz, IT Manager in Health Billing Systems</span>
</div>

Health Billing first looked at SQL and MongoDB.

As a healthcare application, fault tolerance was critical. A database needs to maintain full service even if internet connectivity goes down and the system is offline. Replica sets and monoliths simply won't do. An operating room cannot call a time because of poor bandwidth.

Every point in their database must be full service at all times.

To achieve this, RavenDB employs [master-master replication](https://ravendb.net/features/replication/external-replication). Every node can read and write. If a node suddenly loses internet connectivity, it can continue to accept writes to the system, updating the entire database cluster once connectivity is restored.

Master-master replication gives Health Billing the fault tolerance it needs to be 100% all hours of the day.

As a document database, Health Billing can process the data exactly how the insurance companies request it – in document form.

If a single piece of information is missing, an insurance company can withhold payment. RavenDB has been NoSQL and ACID for over a decade. Health Billing can process data the way it needs at the highest level of data integrity.

<div class="margin-top margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

### The Future of Health Care: Scale-Out, AI, and the Cloud

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">We have things we haven't touched for years because they are stable.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">– Brad Schultz, IT Manager in Health Billing Systems</span>
</div>

Just like every organization, Health Billing is taking in a lot more data. Their data is expanding at a rapid rate, creating new requirements to keep growing:

**1. Cloud Adoption**

They plan to migrate to the cloud. RavenDB Cloud, the [Database as a Service](https://ravendb.net/whitepapers/ravendb-cloud-database-as-a-service-dbaas) (DBaaS) is HIPAA compliant on the cloud, available on AWS, Azure, and GCP. They are planning on using this service for their mobile application.

**2. Scaling Out**

The cloud enables immediate scale out of your database to accommodate increased traffic. RavenDB makes it painless, reducing overhead and tasks while keeping costs to a minimum to [expand your database cluster capacity](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/15-production-deployments).

**3. AI Integration**

**Health Billing is storing a lot more data being used for AI.** They are using this data to provide the proper procedure codes and diagnosis codes for the insurance companies. By automating this process, claims processors can move faster to get the insurance companies to pay the anesthesiologists.

RavenDB integrates with AI solutions and enables you to work with AI data and produced by AI solutions.

<div class="margin-top-sm margin-bottom-sm">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule your free live demo presentation"/></a>
</div>