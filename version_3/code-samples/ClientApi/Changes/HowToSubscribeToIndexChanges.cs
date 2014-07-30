using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Changes
{
	public class HowToSubscribeToIndexChanges
	{
		private interface IFoo
		{
			#region index_changes_1
			IObservableWithTask<IndexChangeNotification> 
				ForIndex(string indexName);
			#endregion

			#region index_changes_3
			IObservableWithTask<IndexChangeNotification>
				ForAllIndexes();
			#endregion
		}

		public HowToSubscribeToIndexChanges()
		{
			using (var store = new DocumentStore())
			{
				#region index_changes_2
				var subscribtion = store
					.Changes()
					.ForIndex("Orders/All")
					.Subscribe(change => Console.WriteLine("{0} on index {1}", change.Type, change.Name));
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region index_changes_4
				var subscribtion = store
					.Changes()
					.ForAllIndexes()
					.Subscribe(change => Console.WriteLine("{0} on index {1}", change.Type, change.Name));
				#endregion
			}
		}
	}
}