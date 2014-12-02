using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes.HowTo
{
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