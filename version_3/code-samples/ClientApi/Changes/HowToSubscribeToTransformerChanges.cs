using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Changes
{
	public class HowToSubscribeToTransformerChanges
	{
		private interface IFoo
		{
			#region transformer_changes_1
			IObservableWithTask<TransformerChangeNotification>
				ForAllTransformers();
			#endregion
		}

		public HowToSubscribeToTransformerChanges()
		{
			using (var store = new DocumentStore())
			{
				#region transformer_changes_2
				var subscribtion = store
					.Changes()
					.ForAllTransformers()
					.Subscribe(change => Console.WriteLine("{0} on transformer {1}", change.Type, change.Name));
				#endregion
			}
		}
	}
}