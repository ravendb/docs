using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes
{
	public class Delete
	{
		private interface IFoo
		{
			#region delete_1
			void DeleteIndex(string name);
			#endregion
		}

		public Delete()
		{
			using (var store = new DocumentStore())
			{
				#region delete_2
				store.DatabaseCommands.DeleteIndex("Orders/Totals");
				#endregion
			}
		}
	}
}