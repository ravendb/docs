namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Transformers
{
	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;

	public class Get
	{
		private interface IFoo
		{
			#region get_1_0
			TransformerDefinition GetTransformer(string name);
			#endregion

			#region get_2_0
			TransformerDefinition[] GetTransformers(int start, int pageSize);
			#endregion
		}

		public Get()
		{
			using (var store = new DocumentStore())
			{
				#region get_1_1
				var transformer = store
					.DatabaseCommands
					.GetTransformer("Order/Statistics"); // returns null if does not exist
				#endregion

				#region get_2_1
				var transformers = store
					.DatabaseCommands
					.GetTransformers(0, 128);
				#endregion
			}
		}
	}
}