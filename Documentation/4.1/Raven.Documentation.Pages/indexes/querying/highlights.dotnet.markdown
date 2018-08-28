# Highlights

Another feature called `Highlights` has been added to RavenDB to enhance the search UX.

## Usage

Lets consider a class and index as follows:   

{CODE blog_post@Blog.cs /}

{CODE blog_comment@Blog.cs /}

{CODE highlights_1@Indexes\Querying\Highlights.cs /}

Now to use Highlights we just need to use one of the `Highlight` query extension methods. The basic usage can be as simple as:   

{CODE highlights_2@Indexes\Querying\Highlights.cs /}

This will return the list of results and for each result we will be displaying first found fragment with the length up to 128 characters.

### Highlights + Projections

Highlights can also be accessed when projections are performed.

{CODE highlights_6@Indexes\Querying\Highlights.cs /}

### Highlights + Map-Reduce

Highlights can be accessed when performing queries on map-reduce indexes.

{CODE highlights_7@Indexes\Querying\Highlights.cs /}

{CODE highlights_8@Indexes\Querying\Highlights.cs /}

## Customization

{CODE highlights_3@Indexes\Querying\Highlights.cs /}

where:   
* **fieldName** or **propertySelector** is used to mark a field/property for highlight.   
* **fieldKeyName** or **keyPropertySelector** is used to mark a field/property key for highlight.   
* **fragmentLength** this is the maximum length of text fragments that will be returned.   
* **fragmentCount** this is the maximum number of fragments that will be returned.   
* **highlightings** this will return an instance of a `FieldHighlightings` that contains the highlight fragments for each returned result.       

By default, the highlighted text is wrapped with `<b></b>` tags, to change this behavior the `SetHighlighterTags` method was introduced.

{CODE highlights_4@Indexes\Querying\Highlights.cs /}

Example. To wrap highlighted text with `**` we just need to execute following query:   

{CODE highlights_5@Indexes\Querying\Highlights.cs /}

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

- [Client API : Session : How to use highlighting?](../../client-api/session/querying/how-to-use-highlighting)