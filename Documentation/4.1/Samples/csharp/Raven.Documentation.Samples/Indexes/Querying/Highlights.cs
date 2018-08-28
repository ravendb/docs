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

			IDocumentQuery<T> Highlight(
				string fieldName,
				string fieldKeyName,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);

			IDocumentQuery<T> Highlight<TValue>(
				Expression<Func<T, TValue>> propertySelector,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);

			IDocumentQuery<T> Highlight<TValue>(
				Expression<Func<T, TValue>> propertySelector,
				Expression<Func<T, TValue>> keyPropertySelector,
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
			public class Result
			{
				public string Content { get; set; }
			}

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

		#region highlights_7
		public class BlogPosts_ByCategory_Content : AbstractIndexCreationTask<BlogPost, BlogPosts_ByCategory_Content.Result>
		{
			public class Result
			{
				public string Category { get; set; }

				public string Content { get; set; }
			}

			public BlogPosts_ByCategory_Content()
			{
				Map = posts => from post in posts
							   select new
							   {
								   post.Category,
								   post.Content
							   };

				Reduce = results => from result in results
									group result by result.Category into g
									select new
									{
										Category = g.Key,
										Content = string.Join(" ", g.Select(r => r.Content))
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

				using (var session = store.OpenSession())
				{
					#region highlights_6
					FieldHighlightings highlightings;

					BlogPosts_ByContent.Result[] results = session
						.Advanced
						.DocumentQuery<BlogPost, BlogPosts_ByContent>()
						.Highlight("Content", 128, 1, out highlightings)
						.SetHighlighterTags("**", "**")
						.Search("Content", "raven")
						.SelectFields<BlogPosts_ByContent.Result>()		// projecting
						.ToArray();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region highlights_8
					FieldHighlightings highlightings;

					BlogPosts_ByCategory_Content.Result[] results = session
						.Advanced
						.DocumentQuery<BlogPosts_ByCategory_Content.Result, BlogPosts_ByCategory_Content>()
						.Highlight("Content", "Category", 128, 1, out highlightings) // highlighting 'Content', but marking 'Category' as key
						.SetHighlighterTags("**", "**")
						.Search("Content", "raven")
						.ToArray();

					var newsHighlightings = highlightings.GetFragments("News"); // get fragments for 'News' category
					#endregion
				}
			}
		}
	}
}