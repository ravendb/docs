# Navigating the New Frontier of Information <br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![Navigating the New Frontier of Information: Big Data in IoT](images/nosql-databases-navigating-the-new-frontier-of-information-in-big-data-and-iot.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>
What drives Big Data and the Internet of things is the unprecedented ability to move massive amounts of computing power to the edge on really cheap machines. It's possible to set up a 3G connection on a device that is less than $5. A Raspberry Pi, which is a full-blown computer, is cheap enough that you can treat it as a disposable unit.

What was science fiction just a decade ago is now a fantastic reality.

Data systems now have hundreds of thousands of endpoints, all processing events in microseconds, demanding storage engines process petabytes of data in real-time.

## Getting to this Point

It all began when you had a single mainframe connected to workstation computers. By the 90s, you had personal computers, where a computer at every desk linked to an individual or a handful of servers. There was always a strict divide between servers and desktops.

Today's world has hundreds of thousands of edge devices collecting data to servers right at the edge. One example is a Fitbit device attached to your wrist, measuring your heart rate and sending it to your Smartphone. The smartphone can send data to a server in the cloud which processes the data into aggregates that you can see on a website.

Servers and Clients intersect everywhere up and down the data journey.

## Extraordinary Possibilities

Today you can collect more precise data, make better decisions, and create superior applications.

For example, let's say that you want to ring a doorbell that connects to a camera which uses image recognition. When someone you know goes to your door, the camera will recognize the person and automatically open it and let him in.

What about tracking inventory? Once upon a time, you closed up shop to take inventory. Now you have scanners everywhere telling you the exact amount of product you have on hand at any moment.

There are dozens of examples.

You can collect massive amounts of data to detect patterns to a dizzying level of precision, and with that data set an algorithm that can determine what course of action comes next.

For example, a diabetes application can measure your insulin levels every hour for years. Based on the information it keeps, it can project when you will eat, what you will eat, and how much insulin you should take at any given moment before your blood sugar rises.

## How Your RavenDB Resources Meet the New Challenges

Setting up an IoT Big Data infrastructure requires a lot of your database. Here's what it takes to make it happen:

**It works well on older and smaller machines:** Smaller machines are a given, but if you set up an IoT System where you have 500,000 car doors with small chips processing data, you won't be able to replace those chips in a hardware update. Your software must be able to work on limited hardware with little expectation of added capacity throughout the life of the project. We built RavenDB to accommodate older machines with less computing power, knowing that at the later stages of multi-year projects, nobody will replace the hardware - but you will still need cutting edge performance.

<div class="pull-left margin-right">
    <div class="quote-textbox-left">
		We give you more with each release while keeping RavenDB as lightweight as possible.
    </div>
</div>
<p style="text-align:justify">
<strong>No Surprises:</strong> If the capacity of the hardware isn't going to change, then the promises your database makes in terms of lightweight memory and disk space usage <em>must be kept over multiple version upgrades</em>. Fitting into lean edge devices, your database is putting on tight jeans. It cannot afford to gain any significant amount of weight. Over the last bundle of new versions, especially the great leap into RavenDB 4.0, we required no additional computing resources. We give you more with each release while keeping RavenDB as lightweight as possible.</p>
<br/>
<div class="margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>
<br/>
**Working Offline:** With so many edge points, parts of your system will inevitably go offline. One example is a Fitbit device that collected data and was put in a drawer for six months before it could upload to a server. Once the user decides to get back into the fitness program and put it on his wrist, your database needs to transmit the older data even after being off the network for half a year. Another example can be when users go into an elevator, a train, or take a 2-week hike in the Everglades. RavenDB will work with nodes or any data collection point offline, collecting data even when the database is disconnected from the network. Once your connection is restored, RavenDB will update the entire database system with the new data. The ability to keep working under network partition is vital for systems like health care, banking, and point of sale where your infrastructure must be available at all times and information can never be lost.

**Revision History:** What happens when data suddenly uploaded from an edge point that was offline for months changes your aggregates? What happens when you have an application that monitors your blood pressure every 30 minutes, and based on the average reading for certain times of the day, the model your doctor uses recommends 50mg of a particular medication? He gives you the prescription and sends you on your way. What if data, just received but collected half a year ago, changes the average BP reading, indicating that for the past six months you should have been taking 75mg of another medication?
<div class="pull-right margin-left">
    <div class="quote-textbox-right">
		RavenDB's point in time feature lets you see what your database looked like at any time in the past.
    </div>
</div>
<p style="text-align:justify">The doctor will need to prove what his data looked like six months ago and today. He will need a database that can present this information to you, auditors, and regulators. RavenDB's point in time feature lets you see what your database looked like at any time in the past. You can see the history of any document to determine what a measurement looked like at the moment you acted on it.</p>

**High Performance at Massive Scale:** You need a system that can move petabytes of data in real-time and maintain that performance as the scale of your data keeps expanding. You might also need a database that can deliver ACID guarantees while keeping performance robust. RavenDB uses automatic indexes, self-optimizing queries, and bundled transactions as part of its arsenal of speed to give you InMemory performance for a persistent database that also guarantees you ACID Data Consistency.

**Pull and Push Replication Up and Down Your Data Journey:** One example is when the company headquarters decides to offer a "buy one get one free" sale on all purchases of Sammy's Frosted Cookies. The central server at HQ will have to communicate this to their thousands of point of sale registers worldwide, so the next time Jimmy gets his way, the computer will tell the cashier to tell his mommy she can go and get another package free of charge. RavenDB supports edge processing both to and from the edge, enabling a fast-moving and efficient IoT database system.<br/>
<br/>
<div>
    <a href="https://ravendb.net/whitepapers/ravendb-point-of-sale-database"><img src="images/point-of-sale-database.jpg" class="img-responsive m-0-auto" alt="RavenDB Point of Sale Database Whitepaper"/></a>
</div>
<br/>