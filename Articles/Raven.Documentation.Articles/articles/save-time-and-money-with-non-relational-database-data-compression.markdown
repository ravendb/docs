# Save Time and Money with Non Relational Database Data Compression
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![Using Non Relational Database Data Compression, you get the benefits of a document database with the storage savings and performance of an SQL legacy model."](images/save-time-and-money-with-non-relational-database-data-compression.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">Get the Benefits of a Document Database with the Storage Savings and Performance of an SQL Legacy Model</p>

There is a lot they don't tell you about relational databases. How quickly we forget that we were using this technology even before ABBA came out with its first hit song:
<br/>
<br/>
<div class="text-center"><iframe width="560" height="315" src="https://www.youtube.com/embed/Sj_9CiNkkn4" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe></div>
<br/>

Requiring you to scale the Mount Everest of complexity with a massive labyrinth of tables and joins were also buried somewhere on page 8, paragraph 3 of the standard SQL user agreement.

Alas, there is also something we don't like to talk about when it comes to Document Databases: Storage.

Every JSON document has two sides to it, the key property and the value.

Let's take a look at two entries to my customer ID file:

<pre>
    <code style="background:transparent;">
    { 
        “LastModifiedDate”: “2020-05-31T12:30:00.000Z”,
        “ForgotPasswordToken”: null
        “ForgotPasswordTokenExpirty”: null,
        “LastSuccessfullChargeDate”: “2019-12-31T12:30:00.000Z”
        ”RegistrationDate”: “2011-02-03T12:30:00.000Z”
        “CustomerFirstName”: “Oren”,
        “CustomerLastName”: “Eini”,
        “Address1”: “100 Main Street”,
        “City”: “Fon Du Lac”,
        “State”: “Wisconsin”,
        “Zip”: “54935”,
        “PaymentMethod”: “AMEX Gold Card”
    }
    </code>
</pre>

Notice how the key properties appear in *both* documents.

<pre>
    <code style="background:transparent;">
    {
        “LastModifiedDate”: “2020-05-31T12:12:00.000Z”,
        “ForgotPasswordToken”: null
        “ForgotPasswordTokenExpirty”: null,
        “LastSuccessfullChargeDate”: “2019-12-31T12:30:00.000Z”
        ”RegistrationDate”: “2016-07-09T12:30:00.000Z”
        “CustomerFirstName”: “Sabrina”,
        “CustomerLastName”: “Globus”,
        “Address1”: “200 Main Street”,
        “City”: “White Pigeon”,
        “State”: “Michigan”,
        “Zip”: “49099”,
        “PaymentMethod”: “Visa”
    }
    </code>
</pre>

In a relational database, the schema lets you state your property names just once before you enter in all the values. That helps a lot in reducing storage and increasing performance.

The problem is that the schema will cost you in your next version. One change to the schema is like a minor change to the foundation of a 30-story building. It will take a lot of resources.

RavenDB gives you a schemaless database, which is excellent for DevOps and initial setup. But for some documents, the property names can be 70% of your data. If you have millions of documents in your database or a time series model with billions, the storage becomes a severe cost - especially on the cloud.

We have been working hard to get you out from beyond that rock and the hard place.

### Non Relational Database Document Compression

RavenDB 5.0 is a [non relational database](https://ravendb.net/articles/acid-cluster-distributed-nonrelational-database) that comes with adaptive learning document compression. You can train RavenDB on the structure of your documents, and then you create the right compression dictionary to reach the best compression available.

This is a classic RavenDB feature in that **it just works**.

Just tell RavenDB which fields you need to compress, and your database does the rest.

Operations such as indexing extensive collections are reduced in time because you need to read much less data from the disk. At the same time, the typical sets of operations like reading and writing documents see no significant slowdown.

Storage is expensive, especially in the cloud. RavenDB Document Compression can save you over 50% in your cloud storage costs and 10% on your total cloud database costs.

<div class="margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

### RavenDB has a Special Sauce You Can't Get Anywhere Else

We eliminated the need for workarounds and have taken this further.

The automatic compression feature allows for RavenDB to learn from your data to further compress over time. It's not machine learning, but RavenDB uses adaptive learning to compress your data better.

Documents Compression applies not only to your keys but also to any repeated *values* in your documents. In the example above, "2020-05-31" repeats in the `"LastModifiedDate"` property in both documents. Data compression eliminates that redundancy as well. RavenDB *learns* about the structure of your documents and will emit only the data that is different between documents.

Enjoy a webinar on-demand!

<div class="margin-bottom">
    <a href="https://ravendb.net/learn/webinars/little-known-features-in-ravendb-document-oriented-database"><img src="images/little-known-features-in-ravendb-document-oriented-database.png" class="img-responsive m-0-auto" alt="Little Known Features in the RavenDB 5.0 Document Oriented Database"/></a>
</div>