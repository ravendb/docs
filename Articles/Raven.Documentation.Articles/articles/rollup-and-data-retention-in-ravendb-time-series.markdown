# NoSQL Data Retention and Rollups
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![Track data in small time intervals while commanding data rollup to larger aggregates and dictating data retention policy using RavenDB Time Series."](images/rollup-and-data-retention-in-ravendb-time-series.jpg)

{SOCIAL-MEDIA-LIKE/}

## Rollup and Data Retention in RavenDB NoSQL Time Series

Time series allows you to record data over time, usually in super small increments to enable you to see patterns in the data. By measuring the direction of your data with such micro precision, you are able to conduct better analysis.

It can be a really valuable tool in reducing costs, improving strategy, and of course, driving sales growth.

The challenge is that the amount of data you are registering is going to be very high.

Take, for example, a heart rate monitor. Assume you have an application that tracks data in 1-second intervals year over year.

In just 1 day, your database will have 86,400 data entries. In a year, you have to manage over 31.5 million entries. While the movement of the data is important, the details may not be as crucial. It's valuable information to see that my heart rate rises every Monday night, I can attribute that to the football game.

If you are managing the application for the heart rate monitor and you have 5,000 users transferring their data, that can result in over 150 billion data points.

However, I don't care what my heart rate was on the first Monday in December at 8:43:06 PM - unless we scored a touchdown!

Rollups and aggregation allow you to take the data and package it into larger increments. Once you have the second-by-second raw data for say, 30 days, you can command your database to roll up those 86,400 points into 1-minute intervals.

You can produce great insights like *what was the first value in that minute, what was the last value, the sum, the maximum value, the minimum value, the average value* and lots of other information. In the process, you are reducing your data set from 2.6 million data points for the month to 43,200.

After 3 months, you may not care to hold the data with the granularity of one minute. You can direct your database to "roll up the 1-minute intervals to 10-minute intervals".

You start from **taking in** data every second, then **aggregating it** in one-minute intervals, then **rolling it up** into larger intervals.

<div class="margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

### Data Retention Options in RavenDB

But what about the raw data once it has been rolled up? Is it simply stashed in some massive time-series junkyard? That's going to cost money, especially on the cloud.

Using RavenDB Time Series, you are in control.

You define the retention policy. You determine how long you hold onto the raw data.

You can retain or discard the raw data. You can also determine how the data gets purged. You also control [data retention](https://ravendb.net/features/time-series/distributed-time-series) configuration.

You can even separate your rollups and retention where you roll up the data as it comes in without holding it, or you keep the data and decide what to do with it at a later time.

We are pleased to roll out rollups in order to put time on your side!

Learn more about RavenDB Time Series in this on-demand webinar:

<div class="margin-bottom">
    <a href="https://ravendb.net/learn/webinars/working-with-time-series-data-in-ravendb"><img src="images/working-with-time-series-data-in-ravendb.png" class="img-responsive m-0-auto" alt="Working with Time Series Data in RavenDB"/></a>
</div>