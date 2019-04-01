# Querying: Highlighting

Another feature called `Highlighting` has been added to RavenDB to enhance the search UX.

## Setup

{CODE-TABS}
{CODE-TAB:csharp:BlogPost blog_post@Blog.cs /}
{CODE-TAB:csharp:BlogComment blog_comment@Blog.cs /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Index highlights_1@Indexes\Querying\Highlights.cs /}
{CODE-TABS/}

{INFO:Important}

Each of the fields on which we want to use **Highlighting** needs to have:

- **FieldIndexing** set to `Search`
- **FieldStorage** set to `Yes`
- **FieldTermVector** set to `WithPositionsAndOffsets`

{INFO/}

## Usage

To use Highlighting we just need to use one of the `Highlight` query methods. The basic usage can be as simple as:   

{CODE highlights_2@Indexes\Querying\Highlights.cs /}

This will return the list of results and for each result we will be displaying first found fragment with the length up to 128 characters.

### Highlighting + Projections

Highlighting can also be done when projections are performed.

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery highlights_6_1@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:DocumentQuery highlights_6_2@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:Index highlights_1@Indexes\Querying\Highlights.cs /}
{CODE-TABS/}

### Highlighting + Map-Reduce

Highlighting can be performed when executing queries on map-reduce indexes.

{CODE-TABS}
{CODE-TAB:csharp:Query highlights_8_1@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:DocumentQuery highlights_8_2@Indexes\Querying\Highlights.cs /}
{CODE-TAB:csharp:Index highlights_7@Indexes\Querying\Highlights.cs /}
{CODE-TABS/}

## Remarks

{NOTE:Note}
Default `<b></b>` tags are coloured and colours are returned in following order:

- <span style="border-left: 10px solid yellow">&nbsp;</span>yellow,
- <span style="border-left: 10px solid lawngreen">&nbsp;</span>lawngreen,
- <span style="border-left: 10px solid aquamarine">&nbsp;</span>aquamarine,
- <span style="border-left: 10px solid magenta">&nbsp;</span>magenta,
- <span style="border-left: 10px solid palegreen">&nbsp;</span>palegreen,
- <span style="border-left: 10px solid coral">&nbsp;</span>coral,
- <span style="border-left: 10px solid wheat">&nbsp;</span>wheat,
- <span style="border-left: 10px solid khaki">&nbsp;</span>khaki,
- <span style="border-left: 10px solid lime">&nbsp;</span>lime,
- <span style="border-left: 10px solid deepskyblue">&nbsp;</span>deepskyblue,
- <span style="border-left: 10px solid deeppink">&nbsp;</span>deeppink,
- <span style="border-left: 10px solid salmon">&nbsp;</span>salmon,
- <span style="border-left: 10px solid peachpuff">&nbsp;</span>peachpuff,
- <span style="border-left: 10px solid violet">&nbsp;</span>violet,
- <span style="border-left: 10px solid mediumpurple">&nbsp;</span>mediumpurple,
- <span style="border-left: 10px solid palegoldenrod">&nbsp;</span>palegoldenrod,
- <span style="border-left: 10px solid darkkhaki">&nbsp;</span>darkkhaki,
- <span style="border-left: 10px solid springgreen">&nbsp;</span>springgreen,
- <span style="border-left: 10px solid turquoise">&nbsp;</span>turquoise,
- <span style="border-left: 10px solid powderblue">&nbsp;</span>powderblue
{NOTE/}

## Related articles

### Client API

- [How to Use Highlighting](../../client-api/session/querying/how-to-use-highlighting)
