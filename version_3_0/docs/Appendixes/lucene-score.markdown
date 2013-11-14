#Lucene Score

Every indexed RavenDB document has an associated score value that has been calculated by Lucene. This value is really important part of the Lucene engine
because it has the influence on the search results relevancy. The basic idea is that the greater score value is the more relevant document is to a query. The entire mechanism is managed
by Lucene so a user does not need to care and even know about. However if you want to get more info about Lucene scoring, please follow [this link](http://lucene.apache.org/core/3_6_0/scoring.html).

As the RavenDB user you are able to retrieve the calculated score by asking about document metadata and using this value to order by. Let's define an index and store some items:

{CODE index_def_and_storing_items@Appendixes\LuceneScore.cs /}

In order to sort by Lucene score value use `OrderByScore` extension method (import `Raven.Client` namespace). To see the score calculated by Lucene 
get the document metadata *in the same session scope* as the executed query and take *Temp-Index-Score* key. 

{CODE order_by_and_get_score@Appendixes\LuceneScore.cs /}

The articles in the sample code above will be returned in the following order:

{CODE-START:json /}
	{ 
		Id = "articles/2", 
		Text = "Ipsum lorem ipsum is simply text. Lorem Ipsum.", 
		Score = 1.2337708473205566 
	}

	{ 
		Id = "articles/3",
		Text = "Lorem ipsum. Ipsum is simply text.", 
		Score = 1.0073696374893188 
	}

	{ 
		Id = "articles/1", 
		Text = "Lorem ipsum is simply text.", 
		Score = 0.712317943572998 
	}
{CODE-END /}

Note that the more times the word *ipsum* appears in the article the higher *Score* factor the document has.