# The Best Document Database Got Better: RavenDB 4’s 21 Improvements<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![The Best Document Database Got Better: RavenDB 4’s 21 Improvements](images/21-improvements-to-our-nosql-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="text-center">
<button id="podcast-play-button" class="play-button" style=""><i class="icon-play" style="margin-right:20px"></i>Play Podcast</button>
</p>

<audio id="podcast-audio" controls="" style="width: 100%">
  <source src="https://s3-us-west-2.amazonaws.com/static.ravendb.net/20190820--21-improvements-from-RavenDB-3.5-to-RavenDB+4.2.ogg" type="audio/ogg">
  <source src="https://s3-us-west-2.amazonaws.com/static.ravendb.net/20190820--21-improvements-from-RavenDB-3.5-to-RavenDB+4.2.mp3" type="audio/mpeg">
  Your browser does not support the audio element.
</audio>

<br/>
The past 5 years presented huge opportunities for databases to give you more power, speed, and flexibility while using less of your own resources. We decided to make a major overhaul of our database in releasing RavenDB 4.0 to give you the chance to capitalize on these developments. Here are 21 new and improved features that are now part of your RavenDB database and RavenDB Cloud Service.

Due to the continuous release of newer versions of RavenDB, I will use the term RavenDB 4 to refer to all versions of this exciting release. Here are the 21 changes we made to revolutionize your database:

<br/>
#### One: New and Stronger Security
RavenDB 4 has beefed up its security to protect your application from unnecessary attacks that have ravaged other data stores. Using certificates, your data is safe over the wire and at rest. We rearranged protocols to make sure that once your application is ready to go beyond its developer machine and onto the public internet, you will be reminded to make sure security is enabled, and given a range of options to make that happen.

<br/>
#### Two: RavenDB 4 Gives you Ramped Up Performance over the 3.5 version
For RavenDB 3.5, you could get 30 thousand reads per second and 15 thousand writes per second. RavenDB CEO Oren Eini issued a challenge to his team: To increase performance by a factor of 10. They delivered. RavenDB 4 runs at 1 million reads per second and 150 thousand writes per second. You can enjoy both speed and reliability of data as RavenDB 4 gives you this performance while maintaining fully transactional ACID guarantees.

<br/>
#### Three: A New Setup Wizard
Now you can get started in minutes using an on-premise license, and in even less time with the RavenDB Cloud Managed Service. You can quickly setup and secure your RavenDB database cluster without any manual work.

<br/>
#### Four: RavenDB 4 Comes with a new "Database as a Service" Managed Cloud Service
RavenDB Cloud is RavenDB’s Managed Cloud Service. 

You can operate RavenDB instances on AWS, Azure, and the Google Cloud Platform in all regions. Scale up your cluster on demand and hand off to us your day to day chores for installation, configuration, monitoring internals and security, maintaining hardware servers, managing your database’s performance, high-availability, backups, patches, costs and updates to the people who built RavenDB. It’s new and available at [https://cloud.ravendb.net/](https://cloud.ravendb.net/). **RavenDB Cloud is offering a 10% introductory discount for all 2019!**

<br/>
[![Try out RavenDB Cloud for Free](images/ravendb-cloud.png)](https://cloud.ravendb.net/)

<br/>
#### Five: RavenDB 4 expands its support for Microsoft Windows to include Linux, MacOS, Docker Containers, Raspberry Pi, ARM chips and more to give you cross platform ability.

<br/>
#### Six: Along with continued support for C sharp, new clients include C++, node.js, Java, Python, Ruby, Go, with more in development.

<br/>
#### Seven: A New GUI that's the Best in the Business
The RavenDB Management Studio let’s you do just about anything with a point and a click. It’s a FULLY LOADED gui that monitors both operational and performance measurements. You can now perform expanded functions without online commands. Monitor a huge array of performance and functional metrics in real time like disk space, disk i/o, disk memory, computer memory, and network bandwidth to see whether or not you are getting 100% out of the resources in your hardware or what you provisioned in the cloud. Easily pinpoint bottlenecks and keep the database layer of your application at peak performance at all times. This translates into real money as you can carefully monitor the real time return on your investment in the cloud. 

With ease, you can set up databases, nodes, collections, and even create and edit documents using the Studio. Reassign leadership roles for each node in your cluster. Set up tasks like backups, ETL to a relational database, and data subscriptions. Connect remotely to your database from any browser, even from your mobile phone, and do whatever you need to maintain your data.

<br/>
#### Eight: Native, Faster, and Optimized Aggregations
In RavenDB 4, we rewrote MapReduce aggregations to boost performance and give you a complete breakdown of each step in the process. You don’t need Hadoop or any other third party addon to aggregate data effectively, it’s a native component of RavenDB 4. Because we now use certificates to run on https:, you *also* have the comfort of knowing all aggregations are on a secured connection over the network. Aggregations now reduce latency by over 90% from previous versions boosting speed for some of your costliest calculations. 

<br/>
#### Nine: An Automatic Distributed Cluster On Premise and in the Cloud
Whether using RavenDB or RavenDB Cloud’s Database as a Service, you no longer need to do anything to set up a highly available database cluster. It is set up for you by default. Even if you are just using one node, it is a cluster of one, so if you scale out, you don’t need to change configurations. The process is no longer manual. It is part of the RavenDB set-up wizard and takes a matter of minutes to install. 

During peak times, expanding nodes is a simple task, keeping load balanced and performance robust. During off-peak times, retiring nodes is just as quick and easy, enabling you to provision and pay for in the cloud only what you are using. 

<br/>
#### Ten: ACID Guarantees Throughout your Database Cluster
RavenDB 3.5 offered ACID across your entire database. RavenDB 4 takes it to the next step with ACID across your entire database cluster. The choice to enable clusterwide ACID is yours so you can, at will, make your distributed database cluster as fully transactional as your database. 

<br/>
#### Eleven: A Revert Revisions Feature
Imagine you could step back in time and fix a mistake. Revert revisions enable you to see what your database looked like at any time in the past, hours ago, days, weeks, months and make changes. This is great for auditors because even the revision itself is logged as part of the history of a document. You can chronicle the timeline of your database and make changes at any point in time.
 
<br/>
{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

<br/>
#### Twelve: Improved Index Speed and Reliability
Lucene indexes all over the world tend to be corrupted. This could derail your database for DAYS. RavenDB 4 came with a new way to handle indexes that *they will not be corrupted* while also increasing your index speed and reliability. 

RavenDB incorporated its custom made Voron storage engine to store Lucene data. By remolding Voron to work with Lucene, indexes become super reliable. 

<br/>
#### Thirteen: Textsearch is in the box
Unlike other databases that require you to purchase additional databases to enable text search, RavenDB 4 gives it to you as part of your database. Why should you have to pay more for something that is supposed to be part of what you bought? RavenDB 4 serves as both the key-value store and the querying engine for your text search. 

<br/>
#### Fourteen: Master Master Node Distribution
With the new master master node relationships, you can read and write to any node in your database cluster. This is important for availability because it lets you keep working even if your systems go offline. The node you are working with will continue to accept reads and writes and will update the rest of your database cluster once connectivity is restored. 

<br/>
#### Fifteen: Automatic Assignment Failover on the Cluster Level
When it comes to daily tasks for your database, RavenDB 4 will automatically distribute them among the nodes in your cluster to keep load balanced. If one node were to go down, RavenDB will automatically reassign that node’s tasks evenly among the remaining nodes and the backup nodes coming online. Tasks will always get done and as quickly as possible.

<br/>
#### Sixteen: Pull Replication
Pull replication in RavenDB allows you to deploy a new edge node without having any impact on your central database. You can deploy a new location without having to update the central server. Simply define the pull replication definition and that is it. Each edge node will connect to the central location and start pulling all the data from that database. The edge initiates the connection to the central node, not the central to the edge. This is ideal for edge processing, along with replicating data to and from a hybrid on-prem and cloud architecture. 

<br/>
#### Seventeen: A New Graph API Data Model
In RavenDB 4 you can use the Graph API to organize large volumes of data into meaningful patterns to leverage your data to look into the future. RavenDB can track the relationships all your data points have with one another to process, aggregate, and index this new data set in *real time*. The Graph API has been used effectively for trading algorithms in hedge funds, health care applications, fraud detection, and more. 

<br/>
#### Eighteen: The Raven Query Language
**RQL, or the Raven Query Language, is SQL for the Document Database.** It makes it easier to adapt to RavenDB and to get a lot more out of it. *This gives you a **faster ramp up** to learning RavenDB and NoSQL Data Management*.

If you know SQL, you know 80% of RQL. DBAs now have a more intuitive way to approach document databases because the new syntax is really easy.

<br/>
#### Nineteen: Distributed Counters
RavenDB 4 makes it easy to increment anything with superior performance. It can be the number of likes for a product or comment, web analytics metrics, a survey for something, or a poll. As a distributed database, when ten thousand people all increment something at the same time, your application needs to handle the load without freezing. Distributed counters employ multiple servers to handle the aggregate load. Numbers are added without having to lock the system each time it increments. The document itself doesn’t lock because the counter is not technically part of the document. This increases performance by enabling your system to take in all the increments at once.

<br/>
#### Twenty: A Clearer Picture of What's Working Efficiently
You now have greater transparency in knowing how long it takes your database to process every step of your data. A greater breakdown of queries show you where you are spending too much resources. 

**With RavenDB, every step in the process of managing your data is an opportunity to boost your application's performance and reduce your monthly cloud bill.**

**RavenDB 4 will show you** how much time was used for: queries in making an index, reading the documents, reducing the results, writing to disk. You can actually see for yourself the time breakdown of how long each task takes. You will be able to detect if there are problems in I/O, if documents are be too big or other reasons why things aren’t happening the way you want. You can see a CPU usage breakdown, storage breakdown, explanations for queries, and more. RavenDB 4 gives you the tools to diagnose your queries to answer questions like, *Do I need more memory? Can I make changes without provisioning* more memory? What exactly do I need to fix? All of this is aimed at enabling you to maximize performance while minimizing latency and cost. 

<br/>
#### Twenty One: Native Debugging Tools and Quick Access to Report Bugs to RavenDB's Development Team
In RavenDB 4 we developed inhouse debugging tools to work alongside the Windows and Linux debugging and logging tools. This toolset gathers as many details as possible to get you the most accurate answers to resolve problems immediately and quickly. 

**You also have the capacity to reach out to the RavenDB dev team right where you encounter a bug.** If you encounter something that doesn’t look right, you can report it immediately using the RavenDB Management studio and your database will provide all the information the development team at RavenDB needs to investigate. If you get an error message, you can put it on the bug tracker and the development team will personally explain what’s going on and how to fix it.

You can download a free database cluster at ravendb.net or try out a free instance of RavenDB Cloud at [https://cloud.ravendb.net/](https://cloud.ravendb.net/).

{RAW}
<script>

function changeButtonToPlay(button) {
  button.className = "play-button";
  button.innerHTML = "<i class=\"icon-play\" style=\"margin-right:20px\"></i> Play Podcast"
}

function changeButtonToPause(button) {
  button.className = "play-button is-playing";
  button.innerHTML = "<i class=\"icon-pause\" style=\"margin-right:20px\"></i> Pause";
}

var audioElement = document.getElementById('podcast-audio');

audioElement.addEventListener("play", function() {
  var button = document.querySelector("#podcast-play-button");
  changeButtonToPause(button);
});

audioElement.addEventListener("pause", function() {
  var button = document.querySelector("#podcast-play-button");
  changeButtonToPlay(button);
});


document.querySelector("#podcast-play-button").addEventListener("click", function(){
  var audio = document.getElementById('podcast-audio');

  if(this.className === "play-button is-playing"){
    changeButtonToPlay(this);
    audio.pause();
  } else{
    changeButtonToPause(this);
    audio.play();
  }

});

</script>
{RAW/}

<br>
[![Try out RavenDB 4.2 for Free](images/try-out-rdb42.png)](https://ravendb.net/downloads)
