using System.Linq;
using System.Text;

using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToUseHighlighting
	{
		private class SearchItem
		{
			public string Id { get; set; }

			public string Text { get; set; }
		}

		private interface IFoo
		{
			#region highlight_1
			IDocumentQueryCustomization Highlight(
				string fieldName,
				int fragmentLength,
				int fragmentCount,
				string fragmentsField);

			IDocumentQueryCustomization Highlight(
				string fieldName,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);
			#endregion
		}

		public HowToUseHighlighting()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region highlight_2
					FieldHighlightings highlightings = null;
					var results = session
						.Query<SearchItem>("ContentSearchIndex")
						.Customize(x => x.Highlight("Text", 128, 1, out highlightings))
						.Search(x => x.Text, "raven")
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
			}
		}
	}
}