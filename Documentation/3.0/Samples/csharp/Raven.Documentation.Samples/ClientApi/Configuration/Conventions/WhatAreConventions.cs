namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
	using Client.Document;

	public class WhatAreConventions
	{
		public WhatAreConventions()
		{
			var store = new DocumentStore();

			#region conventions_1
			DocumentConvention conventions = store.Conventions;
			#endregion
		}
	}
}