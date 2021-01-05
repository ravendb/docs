<p><small class="series-name">Yet Another Bug Tracker: Start of the Series</small></p>
<h1>Building an enterprise application with the .NET Core and RavenDB NoSQL database</h1>
<small>by <a href="https://alex-klaus.com" target="_blank" rel="nofollow">Alex Klaus</a></small>

<div class="article-img figure text-center">
  <img src="images/building-application-with-net-core-and-ravendb-nosql-database.jpg" alt="Leveraging the .NET Core + RavenDB NoSQL database to build enterprise applications" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

*NoSQL* is not hard, it's different. And to show that, [RavenDB](https://ravendb.net) and I kick off a new series of articles dedicated to building enterprise applications leveraging the *.NET Core + RavenDB* bundle.

We want to give as much of practical knowledge as possible, so let's build a real-ish enterprise solution and explain various interesting aspects in the articles along the way. Of course, making everything open source and under the MIT license. The solution will show not only the database part but also domain services, tests, API and the front-end. The goal is to show how various DB features play out all the way through the API to the UI.

<p>And we will follow the best practices applying the <a href="https://en.wikipedia.org/wiki/Domain-driven_design" target="_blank" rel="nofollow">Domain Driven Design</a> (DDD), the <a href="https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/" target="_blank" rel="nofollow">Onion Architecture</a>, <a href="https://martinfowler.com/bliki/CQRS.html" target="_blank" rel="nofollow">Command Query Responsibility Segregation</a> (CQRS), etc. and more importantly <a href="https://en.wikipedia.org/wiki/Common_sense" target="_blank" rel="nofollow">Common sense</a> 😉 to keep the project simple and pragmatic.</p>

### Case Study
<hr>
<p>We may come from different experiences and backgrounds but all developers are familiar with bug tracking systems, a necessary evil of the contemporary software development. We know the <a href="https://softwareengineering.stackexchange.com/a/134420" target="_blank" rel="nofollow">domain</a>, the <a href="https://martinfowler.com/bliki/UbiquitousLanguage.html" target="_blank" rel="nofollow">ubiquitous language</a> (<em>"backlog"</em>, <em>"sprint"</em>, <em>"project"</em>, etc.), how such systems get consumed by the end-user. Eventually we did not have much of a choice on what our intended application would do 🙂 and here we go – our project is one more solution for a well-known problem and it's called <em>"Yet Another Bug Tracker"</em>.</p>

<p>The source code is available at <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">github.com/ravendb/samples-yabt</a>.

Once again, the solution is not meant to be commercially viable, but rather be a convenient example helping the developers in building other enterprise solutions.

### Domain Model
<hr>
<p>The bug tracker domain is quite popular among the <em>DDD</em> gurus and well-covered by <a href="https://vaughnvernon.co" target="_blank" rel="nofollow">Vaughn Vernon</a> in his books. I will conveniently refer to his book <a href="https://www.amazon.com/Domain-Driven-Design-Distilled-Vaughn-Vernon/dp/0134434420" target="_blank" rel="nofollow">"Domain-Driven Design Distilled"</a> for explanation of the domain model (structuring entities and aggregates).</p>

We start off with a simple model (like on the diagram below) and will add more entities and aggregates later. It will be all about the *Backlog Item* aggregate with various related entities like *Project*, *User/Team*, *Sprint* etc.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt/1.png" class="img-responsive m-0-auto" alt="Diagram"/>
</div>

### Architecture/Software design
<hr>
<p>The architecture of the <em>YABT</em> project is based on the <a href="https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1" target="_blank" rel="nofollow">Onion Architecture</a> to avoid known structural pitfalls, such as undesired dependencies between layers and contamination of the externally facing layer (e.g. API or UI) with business logic.</p>

While the *YABT* has some deviations from the classic *Onion Architecture*, it definitely follows the key tenets:

<ul>
    <li class="margin-top-xs">The application is built around an independent object model.</li>
    <li class="margin-top-xs">Inner layers define interfaces. Outer layers implement interfaces.</li>
    <li class="margin-top-xs">Direction of coupling is toward the center.</li>
    <li class="margin-top-xs">All application core code can be compiled and run separate from infrastructure.</li>
</ul>

Our diagram has some resemblance with the classic Onion diagram:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt/2.png" class="img-responsive m-0-auto" alt="Onion Diagram"/>
</div>

<p>Sure thing, the <em>YABT</em> follows the <a href="https://en.wikipedia.org/wiki/SOLID" target="_blank" rel="nofollow">SOLID principles</a>. It's by default.</p>

<p class="margin-top-xs">Check out the <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">source code</a> of the <em>YABT</em>. Let us know what you think. Stay tuned!</p>

<a href="https://ravendb.net/news/use-cases/yabt-series"><h4 class="margin-top">Read more articles in this series</h4></a>
<div class="series-nav">
    <div class="nav-btn disabled margin-bottom-xs">
        <small>‹ Previous in the series</small>
        <strong class="previous">-</strong>
    </div>
    <a href="https://ravendb.net/articles/nosql-data-model-through-ddd-prism">
        <div class="nav-btn margin-bottom-xs">
            <small>Next in the series ›</small>
            <strong class="next">NoSQL Data Model through the DDD prism</strong>
        </div>
    </a>
</div>