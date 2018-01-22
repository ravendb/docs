using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{
	public class WhatAreConventions
	{
		public WhatAreConventions()
		{
			#region conventions_1
		    using (var store = new DocumentStore()
		    {
		        Conventions =
		        {
                // customizations go here
		        }
		    }.Initialize())
		    {

		    }
			#endregion
		}
	}
}
