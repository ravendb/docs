namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes.HowTo
{
	using System.Collections.Generic;

	using Raven.Client.Document;

	public class GetTerms
	{
		private interface IFoo
		{
			#region get_terms_1
			IEnumerable<string> GetTerms(
				string index,
				string field,
				string fromValue,
				int pageSize);
			#endregion
		}

		public GetTerms()
		{
			using (var store = new DocumentStore())
			{
				#region get_terms_2
				var terms = store
					.DatabaseCommands
					.GetTerms("Orders/Totals", "Company", null, 128);
				#endregion
			}
		}
	}
}