using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
	public class Stream
	{
		private interface IFoo
		{
			#region stream_1
			IEnumerator<RavenJObject> StreamDocs(
				Etag fromEtag = null,
				string startsWith = null,
				string matches = null,
				int start = 0,
				int pageSize = int.MaxValue,
				string exclude = null,
				RavenPagingInformation pagingInformation = null,
				string skipAfter = null);
			#endregion
		}

		public Stream()
		{
			using (var store = new DocumentStore())
			{
				#region stream_2
				IEnumerator<RavenJObject> enumerator = store.DatabaseCommands.StreamDocs(null, "products/");
				while (enumerator.MoveNext())
				{
					RavenJObject document = enumerator.Current;
				}
				#endregion
			}
		}
	}
}