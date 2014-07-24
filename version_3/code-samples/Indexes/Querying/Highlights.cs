using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes.Querying
{
	public class Highlights
	{
		private interface IHighlights<T>
		{
			#region highlights_3
			IDocumentQuery<T> Highlight(
				string fieldName,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);

			IDocumentQuery<T> Highlight<TValue>(
				Expression<Func<T, TValue>> propertySelector,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);

			#endregion

			#region highlights_4
			IDocumentQuery<T> SetHighlighterTags(string preTag, string postTag);

			IDocumentQuery<T> SetHighlighterTags(string[] preTags, string[] postTags);
			#endregion
		}

		#region highlights_1
		public class SearchItem
		{
			public string Id { get; set; }

			public string Text { get; set; }
		}

		public class ContentSearchIndex : AbstractIndexCreationTask<SearchItem>
		{
			public ContentSearchIndex()
			{
				Map = (docs => from doc in docs
							   select new { doc.Text });

				Index(x => x.Text, FieldIndexing.Analyzed);
				Store(x => x.Text, FieldStorage.Yes);
				TermVector(x => x.Text, FieldTermVector.WithPositionsAndOffsets);
			}
		}
		#endregion

		public Highlights()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region highlights_2
					FieldHighlightings highlightings;

					var results = session.Advanced.LuceneQuery<SearchItem>("ContentSearchIndex")
									 .Highlight("Text", 128, 1, out highlightings)
									 .Search("Text", "raven")
									 .ToArray();

					var builder = new StringBuilder()
						.AppendLine("<ul>");

					foreach (var result in results)
					{
						var fragments = highlightings.GetFragments(result.Id);
						builder.AppendLine(string.Format("<li>{0}</li>", fragments.First()));
					}

					var ul = builder
						.AppendLine("</ul>")
						.ToString();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region highlights_5
					FieldHighlightings highlightings;

					var results = session.Advanced.LuceneQuery<SearchItem>("ContentSearchIndex")
									 .Highlight("Text", 128, 1, out highlightings)
									 .SetHighlighterTags("**", "**")
									 .Search("Text", "raven")
									 .ToArray();
					#endregion
				}
			}
		}
	}
}