namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes
{
	using Raven.Client.Document;

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
				store.DatabaseCommands.DeleteIndex("BlogPosts/ByTitles");
				#endregion
			}
		}
	}
}