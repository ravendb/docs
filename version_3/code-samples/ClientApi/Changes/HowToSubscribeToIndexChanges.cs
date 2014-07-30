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
					.Subscribe(
						change =>
						{
							switch (change.Type)
							{
								case IndexChangeTypes.IndexAdded:
									// do something
									break;
								case IndexChangeTypes.IndexDemotedToAbandoned:
									// do something
									break;
								case IndexChangeTypes.IndexDemotedToDisabled:
									// do something
									break;
								case IndexChangeTypes.IndexDemotedToIdle:
									// do something
									break;
								case IndexChangeTypes.IndexMarkedAsErrored:
									// do something
									break;
								case IndexChangeTypes.IndexPromotedFromIdle:
									// do something
									break;
								case IndexChangeTypes.IndexRemoved:
									// do something
									break;
								case IndexChangeTypes.MapCompleted:
									// do something
									break;
								case IndexChangeTypes.ReduceCompleted:
									// do something
									break;
								case IndexChangeTypes.RemoveFromIndex:
									// do something
									break;
							}
						});
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