using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Indexes.Querying
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
		public class BlogPosts_ByContent : AbstractIndexCreationTask<BlogPost>
		{
			public BlogPosts_ByContent()
			{
				Map = posts => from post in posts
							   select new
								{
									post.Content
								};

				Index(x => x.Content, FieldIndexing.Analyzed);
				Store(x => x.Content, FieldStorage.Yes);
				TermVector(x => x.Content, FieldTermVector.WithPositionsAndOffsets);
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

					BlogPost[] results = session
						.Advanced
						.DocumentQuery<BlogPost, BlogPosts_ByContent>()
						.Highlight("Content", 128, 1, out highlightings)
						.Search("Content", "raven")
						.ToArray();

					StringBuilder builder = new StringBuilder()
						.AppendLine("<ul>");

					foreach (BlogPost result in results)
					{
						string[] fragments = highlightings.GetFragments(result.Id);
						builder.AppendLine(string.Format("<li>{0}</li>", fragments.First()));
					}

					string ul = builder
						.AppendLine("</ul>")
						.ToString();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region highlights_5
					FieldHighlightings highlightings;

					BlogPost[] results = session
						.Advanced
						.DocumentQuery<BlogPost, BlogPosts_ByContent>()
						.Highlight("Content", 128, 1, out highlightings)
						.SetHighlighterTags("**", "**")
						.Search("Content", "raven")
						.ToArray();
					#endregion
				}
			}
		}
	}
}