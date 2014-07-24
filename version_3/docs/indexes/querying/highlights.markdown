#Highlights

Another feature called `Highlights` has been added to RavenDB to enhance the search UX.

##Usage

Lets consider a class and index as follows:   

{CODE highlights_1@Indexes\Querying\Highlights.cs /}

Now to use Highlights we just need to use one of the `Highlight` query extension methods. The basic usage can be as simple as:   

{CODE highlights_2@Indexes\Querying\Highlights.cs /}

This will return the list of results and for each result we will be displaying first found fragment with the length up to 128 characters.

##Customization

{CODE highlights_3@Indexes\Querying\Highlights.cs /}

where:   
* **fieldName** or **propertySelector** is used to mark a field/property for highlight.   
* **fragmentLength** this is the maximum length of text fragments that will be returned.   
* **fragmentCount** this is the maximum number of fragments that will be returned.   
* **highlightings** this will return an instance of a `FieldHighlightings` that contains the highlight fragments for each returned result.       

By default, the highlighted text is wrapped with `<b></b>` tags, to change this behavior the `SetHighlighterTags` method was introduced.

{CODE highlights_4@Indexes\Querying\Highlights.cs /}

Example. To wrap highlighted text with `**` we just need to execute following query:   

{CODE highlights_5@Indexes\Querying\Highlights.cs /}

{NOTE Default `<b></b>` tags are coloured and colours are returned in following order: `yellow`, `lawngreen`, `aquamarine`, `magenta`, `palegreen`, `coral`, `wheat`, `khaki`, `lime`, `deepskyblue`, `deeppink`, `salmon`, `peachpuff`, `violet`, `mediumpurple`, `palegoldenrod`, `darkkhaki`, `springgreen`, `turquoise` and `powderblue` /}

#### Related articles

TODO