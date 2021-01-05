<h1>Document-oriented Database to Streamline Legal Process</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/legal-discovery-platform-turns-to-document-oriented-database-for-fast-case-preparation.jpg" alt="Legal Discovery platforms are turning to document oriented databases like RavenDB to prepare case files fast by indexing and searching Microsoft Word documents." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

## Open Discover eDiscovery Platform Chooses RavenDB to Streamline Legal Process

***Could we be paying lawyers less in the future?***

Ever try to find a needle in a haystack?

How about the hard copy of three real estate transactions on August 10, 1992, in Topeka, Kansas?

What about an affidavit given 30 years ago to testify to the legitimate benefactors of a life insurance policy or a will?

Legal professionals use eDiscovery software to find the needle in record time. Most legal documents are not merely records that are stored inside rows and columns of unlimited data. They are word documents, emails, physical forms written out by hand that gets scanned into a computer.

Today's data goes in and out of the database as a document. It takes a powerful solution to take in these files, establish indexes for them in less than a day, and enable users to search on them like they would any piece of digital data.

The goal is to shorten the discovery process and cut down on document review costs, early case assessment, and all discovery-related tasks.

That is why the **Open Discover eDiscovery Platform** chose the RavenDB NoSQL Document Database to process millions of forms, most being Microsoft Word documents, to index them and execute a series of queries on them, specifically full-text search queries.

To ensure that this was possible, they tested their system by downloading the entire email library of Enron Corp. It consisted of over 1.2 million emails totaling 53 GB of information.

### Using RavenDB NoSQL Document Database to Sort and Comb Over a Million Emails

RavenDB [document-oriented database](https://ravendb.net/articles/document-oriented-databases-can-conquer-costs-to-you-and-your-users) was tasked with producing aggregates for documents by file size and file type. It also took the metadata for each email or document and create counts by its SortDate, or the date the document was last modified.

Aggregates were also designed to give a summary of the number of documents for *each language*.

To ensure privacy and the protection of all parties, sensitive data like passwords, credit card information, bank account numbers had to be identified and filtered to enable processors to know where this information was.

RavenDB's native full-text search was put to good use in pouring over these documents for specific search terms in record time.

The goal was to produce a general application that leads to powerful, fast, and easy-to-use full-text search eDiscovery of legal documents and documents related to a legal proceeding to cut down the legal process's overhead.

### Summary

If this case study had used a relational database instead of a document-oriented database such as RavenDB, it would have taken months of database schema design and store procedure development and not the two weeks in time it took to develop this.

RavenDB processed the 53 GB 1.2 million document data set in a half an hour on a 16-core Windows server. The processing rate was over 100 GB per hour. 31 indexes queried the document store holding 37.7 million metadata properties, mostly email metadata, and extracted text.

RavenDB proved ideal at the back end of a legal documents platform to serve the following functions:

- [Powerful full-text search](https://ravendb.net/features/querying/full-text-search)
- Information governance
- Data breach analysis
- Enterprise search and content management
- Identifying documents with sensitive information
- Filtering for documents that are redundant, obsolete, or trivial

<p>For the full technical case study, see <a href="https://github.com/dotfurther/OpenDiscoverPlatformCaseStudy#case-study-use-of-open-discover-platform-and-ravendb-document-store-in-ediscovery-early-case-assessment-eca" target="_blank" rel="nofollow">github.com/opendiscoverplatformcasestudy</a>.</p>