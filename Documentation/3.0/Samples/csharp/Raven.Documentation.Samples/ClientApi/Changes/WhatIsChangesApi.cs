using System;

using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Changes
{
	public class WhatIsChangesApi
	{
		 private interface IFoo
		 {
			 #region changes_1
			 IDatabaseChanges Changes(string database = null);
			 #endregion
		 }

		public WhatIsChangesApi()
		{
			using (var store = new DocumentStore())
			{
				#region changes_2
				var subscribtion = store
					.Changes()
					.ForAllDocuments()
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));

				try
				{
					// application code here
				}
				finally
				{
					if (subscribtion != null)
						subscribtion.Dispose();
				}
				#endregion
			}
		}
	}
}