# Licenses Required for New Features

RavenDB has various licenses and tiers to meet different companies' needs of feature sets.  
There are different licenses for local-onsite servers and highly customizable and scalable tiers for cloud-based solutions.  
  
####This article describes the licenses required for features new to version 5.3  
---

In this page:  

* [Incremental Time Series](licenses-required#incremental-time-series)  
* [Concurrent Data Subscriptions](licenses-required#concurrent-data-subscriptions)  
* [Elasticsearch ETL](licenses-required#elasticsearch-etl)  
* [Power BI Support](licenses-required#power-bi-integration)  
* [Intelligent RQL Query and Patch Coding Assistance](licenses-required#intelligent-rql-query-and-patch-coding-assistance)  
* [TCP Data Compression](licenses-required#tcp-data-compression)  
* [Terraform Support](licenses-required#terraform-support)  
  
## Incremental Time Series
{PANEL: } [On-Premise (Free, Professional, and Enterprise licenses)](https://ravendb.net/buy) and [Cloud (Free, Developer, and Production tiers) ](https://cloud.ravendb.net/pricing)  
{PANEL/}
  
Would you like to see how things change over time to identify patterns and analyse the effects of various efforts you've been making?  
  
In RavenDB 5.3, we've introduced enhancements to the RavenDB 5.0, [Distributed Time Series]( ../../document-extensions/timeseries/overview) feature to accurately account for changes made in existing data. This tool enables you to track and visualize data over time, while accounting for edits.  We've enabled users to alter stored time series values, allowing entries to be used as counters that can be incremented and decremented. This provides a useful way to accept and update values in real-time and on an ongoing basis, without the worry of conflicts and performance drops due to expensive synchronization.  
  
As an example, think of an ebook store where you need to collect and update statistics in real-time. For every new hour, you'd create an initial tally of zero sold books, then update this value in real-time with each new sale. Not only that, you can now go back in time and make corrections for any cancellations. All of these operations are guaranteed to be executed concurrently on multiple cluster nodes, without any danger of data loss.  

## Concurrent Data Subscriptions

{PANEL: } [On-Premise (Professional and Enterprise licenses)](https://ravendb.net/buy) and [Cloud (Developer and Production tiers) ](https://cloud.ravendb.net/pricing)  
{PANEL/}
  
Do you have frequently changing data items which you'd like to keep track of?   [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions) allow users to receive frequently changing data continuously from the same ongoing task.  This is vital for dynamic content on websites or dashboards.  
  
RavenDB 5.3 enhances this feature with Concurrent Data Subscriptions. Now you can have more than one "Subscription Worker" consuming documents from the same subscription. RavenDB Servers will provide multiple Workers with batches of documents to process, taking care to distribute different batches to different Workers, and ensuring all of them are under a similar load. Servers will always ensure that none of the data is left unprocessed.  
  
Depending on your needs, Workers can reside on the same machine or run on separate ones. As a result, you can now scale out your processing infrastructure and tremendously increase your capacity for processing documents arriving from subscriptions.  
  
## Elasticsearch ETL
{PANEL: } [On-Premise (Professional and Enterprise licenses)](https://ravendb.net/buy) and [Cloud (Developer and Production tiers) ](https://cloud.ravendb.net/pricing)  
{PANEL/}
  
If you're using Elasticsearch as a solution for full-text search, you can now add RavenDB 5.3, with its ultra-efficient, cost-reducing performance, to the list of applications populating your organization's Elasticsearch database. Through our deft ETL (Extract, Transform, Load) mechanism, you can reshape RavenDB data before pushing it to Elasticsearch and avoid building time-expensive custom solutions.  
  
## Power BI Integration
{PANEL: } [On-Premise (Enterprise license)](https://ravendb.net/buy) and [Cloud (Developer and Production tiers) ](https://cloud.ravendb.net/pricing)  
{PANEL/}
  
Do you want to be able to quickly analyze and visualize your data to gain important insights from it?  
  
From version 5.3, RavenDB is supporting integration with [Power BI](https://powerbi.microsoft.com/en-us/): a powerful business intelligence service by Microsoft. This enables you to push your data to Power BI, where you can analyze, process, transform and remodel it. Using this functionality you can create intelligent reports and gain new insights into your data and business. Additionally, you can now run queries from your BI client to fetch any data from RavenDB that you want to process, analyze and report on.  
  
## Intelligent RQL Query and Patch Coding Assistance
{PANEL: } [On-Premise (Free, Professional, and Enterprise licenses)](https://ravendb.net/buy) and [Cloud (Free, Developer, and Production tiers) ](https://cloud.ravendb.net/pricing)  
{PANEL/}
  
Starting with version 5.3, running queries and patches with [RQL](../indexes/querying/what-is-rql) in the RavenDB Studio will be a much more intuitive experience. As you type, the studio will actively help you with suggestions, offering you a list of possible keywords dynamically filtered based on the current context. RQL queries and patch scripts will be validated before you run them, helping you master RQL quickly.  
  
Features like autocomplete, code hints, context-aware suggestions, and lists of data element names will smooth the learning curve and allow you to focus on your objectives without getting caught up in the syntax of your queries and patch scripts.  
  
## TCP Data Compression
{PANEL: } [On-Premise (Professional and Enterprise licenses)](https://ravendb.net/buy) and [Cloud (Developer and Production tiers) ](https://cloud.ravendb.net/pricing)
{PANEL/}

Do you want to save a lot of money on data transfer?  In addition to existing compression methods for data stored on disk, version 5.3 introduces TCP traffic compression for data in-transport via [subscriptions](licenses-required#concurrent-data-subscriptions).  
  
Simply put, leveraging TCP compression will reduce your bandwidth usage, decrease the amount of data in transit, lower network transfer costs and latency in general, and provide you with a more responsive system. Best of all, no configuration is needed and this compression is completely transparent to the end-user.  
  
## Terraform Support
{PANEL: } [On-Premise (Free, Professional, and Enterprise licenses)](https://ravendb.net/buy) and [Cloud (Free, Developer, and Production tiers) ](https://cloud.ravendb.net/pricing)  
{PANEL/}

  
Want to save a lot of time provisioning your back-end infrastructure?  
  
In RavenDB v.5.3, we provide support for [Terraform by HashiCorp](https://www.terraform.io/), which does a lot of the detailed work of setting up the infrastructure by following your declarative instructions. Basically, you tell it what you want and it knows how to do all of the steps in the correct order.  You can also later make updates to your existing infrastructure using Terraform.  
  

