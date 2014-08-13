namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Transformers
{
	using Raven.Client.Document;

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