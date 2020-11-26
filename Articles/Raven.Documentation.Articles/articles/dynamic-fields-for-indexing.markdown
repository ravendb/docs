<p><small class="series-name">Yet Another Bug Tracker: Article #3</small></p>
<h1>Dynamic Fields for Indexing</h1>
<small>by <a href="https://alex-klaus.com" target="_blank" rel="nofollow">Alex Klaus</a></small>

![The power of dynamic fields for indexing dictionaries and nested collections in RavenDB](images/dynamic-fields-for-indexing.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">The power of dynamic fields for indexing dictionaries and nested collections in RavenDB</p>

<p><a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">Yet Another Bug Tracker</a> (YABT) we are building in this series needs to be flexible and accommodate custom fields and structures that can be added by the end-user. The custom fields is a widely-adopted concept in bug-trackers to manage additional fields on the ticket per project, team, or ticket type. It exists in <a href="https://confluence.atlassian.com/adminjiraserver/adding-a-custom-field-938847222.html" target="_blank" rel="nofollow">Jira</a>, <a href="https://devblogs.microsoft.com/devops/adding-a-custom-field-to-a-work-item/" target="_blank" rel="nofollow">Azure DevOps</a>, <a href="https://bugzilla.readthedocs.io/en/5.0/administering/custom-fields.html" target="_blank" rel="nofollow">Bugzilla</a> and others.</p>

Let's follow the trend and add to the *Backlog Item* (aka "*ticket*") two related features:

<ol>
    <li class="margin-top-xs">A container for custom fields – additional properties of various types.</li>
    <li class="margin-top-xs">A list of all modifications to keep track of who/when modified the ticket.</li>
</ol>

Here is a mock-up of the *Backlog Item* screen:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt3/1.png" class="img-responsive m-0-auto" alt="Backlog Item screen"/>
</div>

<ul>
    <li class="margin-top-xs">Custom fields are displayed on the right-hand side. Each of them is formatted according to its type.</li>
    <li class="margin-top-xs">The "<em>Created</em>" and "<em>Last Updated</em>" fields (below the custom fields) get resolved from the history of the ticket's modifications (with the rest of the history is available under the "<em>View All</em>"). The name is a link for navigation to the user's profile page.</li>
</ul>

### 1. NoSQL solution
<hr style="border-color:rgba(34,37,43,.15);">

At first glance, the task is trivial. Flexibility of the *[NoSQL](https://ravendb.net)* does not require defining the structure upfront, so any fields can be easily added ad hoc. Though, the developers still need to be aware of the structure for two reasons:

<ul>
    <li class="margin-top-xs">To build the presentation layer (layout and style of the fields).</li>
    <li class="margin-top-xs">To filter and search on dynamically added fields (especially in a strongly-typed language).</li>
</ul>

#### 1.1. DB design overview

We described the Data model in the previous article. Here is the relevant part of the diagram including `CustomField`, `BacklogItem` and `User` aggregates:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt3/2.png" class="img-responsive m-0-auto" alt="NoSQL Diagram"/>
</div>

In `JSON` format a *Backlog Item* record would look like:

<pre>
    <code class="language-json" style="background:transparent;">
    {
        "Title": "Malfunction at the Springfield Nuclear Power Plant",
        "Description": "Some terrible details",
        "ModifiedBy": [{
                "Timestamp": "2020-01-01T00:00:00",
                "Summary": "Created",
                "ActionedBy": {
                    "Id": "users/1-A",
                    "Name": "Homer Simpson"
                }
            },
            {
                "Timestamp": "2020-01-02T00:00:00",
                "Summary": "Modified description",
                "ActionedBy": {
                    "Id": "users/2-A",
                    "Name": "Waylon Smithers"
                }
            }
        ],
        "CustomFields": {
            "CustomFields/1-A": "Mr. Burns",
            "CustomFields/2-A": 10000000000
        }
    }
    </code>
</pre>

Where sample records of custom fields would be:

<pre>
    <code class="language-json" style="background:transparent;">
    {
        "Id": "CustomFields/1-A",
        "Name": "Affected Persona",
        "Type": "Text"
    },
    {
        "Id": "CustomFields/2-A",
        "Name": "Potential Loss",
        "Type": "Currency"
    }
    </code>
</pre>

We are going to focus on two dynamic `BacklogItem` properties:

<ul>
    <li class="margin-top-xs"><code>ModifiedBy</code> – change history, a collection of items with a predefined structure;</li>
    <li class="margin-top-xs"><code>CustomFields</code> – container for custom fields, a more complex structure represented by a dictionary, where each value has variable structure (BTW, any <code>JSON</code> structure can be presented as a dictionary).</li>
</ul>

Taking it one level up from `JSON` to `C#` we get `RavenDB` models for persisting in the database:

<pre>
    <code class="language-csharp" style="background:transparent;">
    // The Backlog Item aggregate persisted in the DB
    public class BacklogItem
    {
        public string Id    { get; private set; }
        public string Title { get; set; }

        // List of all users who/when modified the ticket.
        public IList<ChangedByUserReference> ModifiedBy { get; } = new List<ChangedByUserReference>();

        // Resolve Who/when created & updated the ticket, no need to persist it in the DB
        [JsonIgnore]
        public ChangedByUserReference Created     => ModifiedBy?.OrderBy(m => m.Timestamp).FirstOrDefault();
        [JsonIgnore]
        public ChangedByUserReference LastUpdated => ModifiedBy?.OrderBy(m => m.Timestamp).LastOrDefault();

        // Custom properties of various data types. Stored as { custom field ID, value }
        public IDictionary<string, object> CustomFields { get; set; }
    }

    public class ChangedByUserReference
    {
        // Timestamp of the change
        public DateTime Timestamp { get; set; }
        // Brief summary of the change
        public string Summary { get; set; }
        // The user who made the change
        public UserReference ActionedBy { get; set; }
    }

    public class UserReference
    {
        string Id	{ get; set; }
        string Name	{ get; set; }
    }
    </code>
</pre>

and a separate collection of *Custom Fields* would be:

<pre>
    <code class="language-csharp" style="background:transparent;">
    // The Custom Field entity persisted in the DB
    public class CustomField
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // Type of the custom field determines how to process the associated value
        public CustomFieldType FieldType { get; set; }

        public enum CustomFieldType { Text, Date, Currency }
    }
    </code>
</pre>

#### 1.2. Design justification

Design of a de-normalised database always depends on the main use-cases of your application. Therefore, the structures provided above ought to be justified.

##### References to other entities

While both `ModifiedBy` and `CustomFields` refer to other entities, they do it differently.

Instead of just referencing user IDs, `ActionedBy` property has a bit richer structure – `UserReference` class. It's done to keep often used user's properties handy and avoid excessive `JOIN`s with the `User` collection for presenting auxiliary data requested along with the backlog items. For example, a list of backlog items will show names of users who created/edited them:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt3/3.png" class="img-responsive m-0-auto" alt="Backlog Item list"/>
</div>

But convenience of receiving user names along with backlog items comes with some maintenance responsibility – keeping user names in references in sync with corresponding records in the `Users` collection. It is a trade-off that will be discussed in a separate article.

Alternatively, we can go with a traditional approach of using the `ID` of the referred record and resolving other properties in runtime when querying. The `CustomFields` property is a good fit for that as custom fields are unlikely to be queried along with multiple backlog items (as shown on the mock-up above), so resolving them via a separate request or a `JOIN` would not put too much stress on the DB.

##### ModifiedBy: IList vs IDictionary

`ModifiedBy` could have been presented in `C#` as a dictionary with a timestamp as the key (e.g. `IDictionary<DateTime, ChangedByUserReference>`). If you are confident that 2+ events will never occur at the exact same time, then go for it. Note, that the user ID could not be used as the key, as one user may make multiple changes and the list should reflect the whole history. Overall, I would rather have `ModifiedBy` as a list.

`CustomFields` on the other side is the perfect candidate for a dictionary if we decide not to use one custom field twice in the ticket.

<p>OK, it was a diversion. I hope, getting values from the <em>Custom Fields</em> along with the created/updated is straightforward (see <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">YABT source code</a> for examples). So, we resolved the first issue and can build the presentation layer for a known data structure. But what about filtering on those fields?</p>

### 2. Filtering on sub-attributes... and sub- sub- attributes
<hr style="border-color:rgba(34,37,43,.15);">

There are two interesting cases requiring filtering the *Backlog items* on `ModifiedBy` and `CustomFields` properties:

<ol>
    <li class="margin-top-xs">Getting "<em>My recent tickets</em>", tickets that were recently edited by the current user.</li>
    <li class="margin-top-xs">Filtering on certain values of various custom fields. E.g. for text fields where the "<em>Affected Persona</em>" contains word "<em>Burns</em>" or the "<em>Potential Loss</em>" is more than $1,000.</li>
</ol>

#### 2.1. Filter for "*My recent tickets*"

To filter a collection we need an index and the [Dynamic fields feature](https://ravendb.net/docs/article-page/latest/csharp/indexes/using-dynamic-fields) comes in handy. It creates key-value pairs in the index terms where the keys are resolved runtime when updating the index.

A generic example of *Dynamic fields* in index would look like:

<pre>
    <code class="language-csharp" style="background:transparent;">
    Map = tickets =>
        from ticket in tickets
        select new
        {
            _ = ticket.ModifiedBy.Select(x => CreateField(x.ActionedBy.Id, x.Timestamp))
        };
    </code>
</pre>

It will create index terms for the above example reflecting changes by each user:

<ul>
    <li class="margin-top-xs"><code>'users/1-A'</code>: <code>{ 2020-01-01T00:00:00 }</li>
    <li class="margin-top-xs"><code>'users/2-A'</code>: <code>{ 2020-01-02T00:00:00 }</li>
</ul>

Looks good, but not good enough as:

<ol>
    <li class="margin-top-xs">One user can modify one ticket multiple times, but to get the timestamp for the very last modification we need to group by <code>ActionedBy.Id</code>.</li>
    <li class="margin-top-xs">Filtering on the generated terms from a strongly-typed language requires a generic structure for using in the queries.</li>
</ol>

Battling the first problem is easy but figuring out a solution for the second one would require deep knowledge of underlining structures.

<p>If you need details, the <a href="https://groups.google.com/d/msg/ravendb/YvPZFIn5GVg/907Msqv4CQAJ" target="_blank" rel="nofollow">discussion is here</a>. The gist – forming the keys for <code>CreateField()</code> in a special format will allow querying on the terms in <code>C#</code> as it was a dictionary. Bear with me.</p>

For index created with syntax `CreateField("Bla_" + k.Key, k.Value)` and `k.Key='Key'` you would query:

<ul>
    <li class="margin-top-xs">in <code>RQL</code> as <code>from index 'Y' where Bla_Key = 4</code> (looks quite obvious),</li>
    <li class="margin-top-xs">in <code>C#</code> as <code>s.Query&lt;X,Y&gt;().Where(p => p.Bla["Key"].Equals(4))</code> (here is your dictionary).</li>
</ul>

RavenDB below v5 had two constraints on the *Dynamic field* key:

<ul>
    <li class="margin-top-xs">the key cannot start with a digit, e.g. <code>1-A</code> (<a href="https://issues.hibernatingrhinos.com/issue/RavenDB-15234" target="_blank" rel="nofollow">issue #15234</a>);</li>
    <li class="margin-top-xs">the key cannot contain the slash (<code>/</code>) symbol (<a href="https://issues.hibernatingrhinos.com/issue/RavenDB-15234" target="_blank" rel="nofollow">issue #15235</a>).</li>
</ul>

Fortunately, both have been fixed in [RavenDB v5.0](https://ravendb.net/articles/whats-new-in-ravendb-50).

If you are on an older version, then to work around those constraints, we set the key as `CreateField("Bla_" + k.Key!.Replace("/",""), k.Value)`. Then for `k.Key == 'users/1-A'` you would query `s.Query<X,Y>().Where(p => p.Bla["users1-A"].Equals(4))`.

Here is a full example:

<pre>
    <code class="language-csharp" style="background:transparent;">
    public class BacklogItems_ForList : AbstractIndexCreationTask<BacklogItem>
    {
        public class Result
        {
            public IDictionary<string, DateTime> ModifiedByUser { get; set; }
        }
        public BacklogItems_ForList()
        {
            Map = tickets =>
                from ticket in tickets
                select new
                {
                    _ = ticket.ModifiedBy.GroupBy(m => m.ActionedBy.Id)		// Grouping by user
                                        .Select(x => CreateField($"{nameof(Result.ModifiedByUser)}_{x.Key!.Replace("/","").ToLower()}",
                                                                x.Max(o => o.Timestamp)
                                                                )
                                                )
                };
        }
    }
    </code>
</pre>

Now we can get all the tickets modified by a user ID and sort in descending order by the timestamp of the last change by the current user via:

<pre>
    <code class="language-csharp" style="background:transparent;">
    var userKey = userId.Replace("/", "").ToLower();  // For 'users/1-A' get 'users1-A'
    s.Query<Result, BacklogItems_ForList>()
     .Where(t => t.ModifiedByUser[userKey] > DateTime.MinValue)
     .OrderByDescending(t => t.ModifiedByUser[userKey])
    </code>
</pre>

*Note:* here we take string concatenation for the key outside of the query, it'd fail otherwise.

#### 2.2. Filter by custom fields

We have already covered all the gotchas so it should be a smooth ride for filtering on custom fields. A simple case looks pretty much the same as for `ModifiedBy`:

<pre>
    <code class="language-csharp" style="background:transparent;">
    __ = ticket.CustomFields.Select(x => CreateField($"{nameof(Result.CustomFields)}_{x.Key.Replace("/","").ToLower()}", x.Value))
    </code>
</pre>

But here it all boils down to the supported data types of the custom fields. And we can knock ourselves out:

<ul>
    <li class="margin-top-xs">can search in text fields;</li>
    <li class="margin-top-xs">check for equality in numeric fields;</li>
    <li class="margin-top-xs">filter on sub-attributes for complex structures (e.g. on <code>ID</code> for <code>UserReference</code> when referencing to other users).</li>
</ul>

So the index would require resolving `FieldType` of the custom field and running many if/else conditions for each type:

<pre>
    <code class="language-csharp" style="background:transparent;">
    public class BacklogItems_ForList : AbstractIndexCreationTask<BacklogItem>
    {
        public class Result
        {
            public IDictionary<string, string> CustomFields { get; set; }
        }
        public BacklogItems_ForList()
        {
            Map = tickets =>
                from ticket in tickets
                select new
                {
                    __ = from x in ticket.CustomFields
                            let fieldType = LoadDocument<CustomField.CustomField>(x.Key).FieldType
                            let key = $"{nameof(Result.CustomFields)}_{x.Key.Replace("/", "").ToLower()}"
                            select 
                                (fieldType == CustomFieldType.Text)
                                    // search in text Custom Fields
                                    ? CreateField(key, x.Value, false, true)	
                                    : (fieldType == CustomFieldType.UserReference)
                                        // Exact match of User ID
                                        ? CreateField(key, x.Value.Id)
                                        // Other Custom Fields (e.g. numbers, dates) can use a '≥' comparison
                                        : CreateField(key, x.Value)
                };
        }
    }
    </code>
</pre>

And here how we can query against that index:

<ol>
    <li class="margin-top-xs">
    When the <em>Custom Field</em> in question has type <code>Text</code>
    <pre>
        <code class="language-json" style="background:transparent;">
    "CustomFields": {
        "CustomFields/1-A": "Mr. Burns"
    }</code>
    </pre>
    we can search text:
    <pre>
        <code class="language-csharp" style="background:transparent;">
    var fieldKey = customFieldId.Replace("/", "").ToLower();  // For 'CustomFields/1-A' get 'customfields1-A'
    s.Query<Result, BacklogItems_ForList>()
    .Search(t => t.CustomFields[fieldKey], "Burns")</code>
    </pre>
    </li>
    <li class="margin-top-xs">
    When the <em>Custom Field</em> in question is a <code>Number</code>:
    <pre>
        <code class="language-json" style="background:transparent;">
    "CustomFields": {
        "CustomFields/2-A": 10000000000
    }</code>
    </pre>
    we can use the '<code>></code>' operator:
    <pre>
        <code class="language-csharp" style="background:transparent;">
    var fieldKey = customFieldId.Replace("/", "").ToLower();  // For 'CustomFields/2-A' get 'customfields2-A'
    s.Query<Result, BacklogItems_ForList>()
    .Where(t => t.CustomFields[fieldKey] > 1_000_000)</code>
    </pre>
    A similar approach would be used for querying on user IDs.
    </li>
</ol>

### 3. RavenDB Studio Tools
<hr style="border-color:rgba(34,37,43,.15);">

*Dynamic fields* lack of transparency. To have a look under the hood (e.g. see the [Index Terms](https://ravendb.net/docs/article-page/latest/csharp/studio/database/indexes/create-map-index#index-fields-&-terms)) use the *RavenDB Studio*.

<p>If you open the <em>Index Terms</em> for an index when running one of the YABT tests covering the scenarios described above (e.g. <a href="https://github.com/ravendb/samples-yabt/blob/master/Domain.Tests/BacklogItemServices/BacklogItemListQueryByCustomFieldTests.cs" target="_blank" rel="nofollow">this one in the YABT repo</a>), you would see terms like:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt3/4.png" class="img-responsive m-0-auto" alt="RavenDB Studio Index Terms"/>
</div>

That screenshot shows values for *Custom Fields* and user's modifications you can query on.

That's it. Happy filtering.

<p>Check out the full source code at our repository on GitHub - <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">github.com/ravendb/samples-yabt</a> and let us know what you think. Stay tuned for the next articles in the <em>YABT</em> series.</p>

<h4 class="margin-top">Read more articles in this series</h4>
<hr style="border-color:rgba(34,37,43,.15);">
<div class="series-nav">
    <a href="https://ravendb.net/articles/hidden-side-of-document-ids-in-ravendb">
        <div class="nav-btn margin-bottom-xs">
            <small>‹ Previous in the series</small>
            <strong class="previous">Hidden side of document IDs in RavenDB</strong>
        </div>
    </a>
    <div class="nav-btn disabled margin-bottom-xs">
        <small>Next in the series ›</small>
        <strong class="next">Coming soon</strong>
    </div>
</div>