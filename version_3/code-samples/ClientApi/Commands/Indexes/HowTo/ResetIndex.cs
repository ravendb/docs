namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes.HowTo
{
	using Raven.Client.Document;

	public class ResetIndex
	{
		private interface IFoo
		{
			#region reset_index_1
			void ResetIndex(string name);
			#endregion
		}

		public ResetIndex()
		{
			using (var store = new DocumentStore())
			{
				#region reset_index_2
				store.DatabaseCommands.ResetIndex("Orders/Totals");
				#endregion
			}
		}
	}
}