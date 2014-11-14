namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
	using Client.Document;
	using CodeSamples.Orders;

	public class CustomizeCollectionAssignmentForEntities
	{
		public CustomizeCollectionAssignmentForEntities()
		{
			var store = new DocumentStore();

			#region custom_collection_name
			store.Conventions.FindTypeTagName = type =>
			{
				if (typeof(Category).IsAssignableFrom(type))
					return "ProductGroups";
				
				return DocumentConvention.DefaultTypeTagName(type);
			};
			#endregion
		} 
	}
}