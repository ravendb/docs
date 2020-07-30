using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Changes
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
				IDisposable subscription = store
					.Changes()
					.ForAllTransformers()
					.Subscribe(
						change =>
						{
							switch (change.Type)
							{
								case TransformerChangeTypes.TransformerAdded:
									// do something
									break;
								case TransformerChangeTypes.TransformerRemoved:
									// do something
									break;
							}
						});
				#endregion
			}
		}
	}
}
