using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes.HowTo
{
	public class ChangeIndexLockMode
	{
		private interface IFoo
		{
			#region change_index_lock_1
			void SetIndexLock(string name, IndexLockMode lockMode);
			#endregion
		}

		public ChangeIndexLockMode()
		{
			using (var store = new DocumentStore())
			{
				#region change_index_lock_2
				store.DatabaseCommands.SetIndexLock("Orders/Totals", IndexLockMode.LockedIgnore);
				#endregion
			}
		}
	}
}