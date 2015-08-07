using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Transformers.HowTo
{
	public class ChangeTransformerLockMode
	{
		private interface IFoo
		{
			#region change_transformer_lock_1
			void SetTransformerLock(string name, TransformerLockMode lockMode);
			#endregion
		}

		public ChangeTransformerLockMode()
		{
			using (var store = new DocumentStore())
			{
				#region change_transformer_lock_2
				store.DatabaseCommands.SetTransformerLock("Orders/Company", TransformerLockMode.LockedIgnore);
				#endregion
			}
		}
	}
}