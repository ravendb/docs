using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Transformers
{
	public class Delete
	{
		private interface IFoo
		{
			#region delete_1
			void DeleteTransformer(string name);
			#endregion
		}

		public Delete()
		{
			using (var store = new DocumentStore())
			{
				#region delete_2
				store.DatabaseCommands.DeleteTransformer("Order/Statistics");
				#endregion
			}
		}
	}
}