using System;

using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes.HowTo
{
	public class ChangeIndexPriority
	{
		private interface IFoo
		{
			#region change_index_priority_1
			void SetIndexPriority(string name, IndexingPriority priority);
			#endregion
		}

		private class Foo
		{
			#region change_index_priority_2
			[Flags]
			private enum IndexingPriority
			{
				None = 0,
				Normal = 1,
				Disabled = 2,
				Idle = 4,
				Abandoned = 8,
				Error = 16,
				Forced = 512,
			}
			#endregion
		}

		public ChangeIndexPriority()
		{
			using (var store = new DocumentStore())
			{
				#region change_index_priority_3
				store.DatabaseCommands.SetIndexPriority("Orders/Totals", IndexingPriority.Disabled);
				#endregion
			}
		}
	}
}