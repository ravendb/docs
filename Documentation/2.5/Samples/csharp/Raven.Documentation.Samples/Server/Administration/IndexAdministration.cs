// -----------------------------------------------------------------------
//  <copyright file="IndexAdministration.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace RavenCodeSamples.Server.Administration
{
	public class IndexAdministration : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region reset_index
				store.DatabaseCommands.ResetIndex("IndexName");
				#endregion

				#region delete_index
				store.DatabaseCommands.DeleteIndex("IndexName");
				#endregion
			}
		}
	}
}