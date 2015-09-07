using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
	public class HowToSubscribeToDataSubscriptionChanges
	{
		private interface IFoo
		{
			#region data_subscription_changes_1
			IObservableWithTask<DataSubscriptionChangeNotification>
				ForAllDataSubscriptions();
			#endregion

			#region data_subscription_changes_3
			IObservableWithTask<DataSubscriptionChangeNotification> 
				ForDataSubscription(long id);
			#endregion
		}

		public HowToSubscribeToDataSubscriptionChanges()
		{
			using (var store = new DocumentStore())
			{
				{
					#region data_subscription_changes_2

					IDisposable subscription = store
						.Changes()
						.ForAllDataSubscriptions()
						.Subscribe(
							change =>
							{
								var subscriptionId = change.Id;

								switch (change.Type)
								{
									case DataSubscriptionChangeTypes.SubscriptionOpened:
										// do something
										break;
									case DataSubscriptionChangeTypes.SubscriptionReleased:
										// do something
										break;
								}
							});

					#endregion
				}
				{
					#region data_subscription_changes_4

					var subscriptionId = 3;

					IDisposable subscription = store
						.Changes()
						.ForDataSubscription(subscriptionId)
						.Subscribe(
							change =>
							{
								switch (change.Type)
								{
									case DataSubscriptionChangeTypes.SubscriptionOpened:
										// do something
										break;
									case DataSubscriptionChangeTypes.SubscriptionReleased:
										// do something
										break;
								}
							});

					#endregion
				}
			}
		}
	}
}