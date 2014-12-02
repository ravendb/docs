using System.IO;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Attachments
{
	public class Put
	{
		private interface IFoo
		{
			#region put_1
			void PutAttachment(string key, Etag etag, Stream data, RavenJObject metadata);
			#endregion
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_2
				using (var file = File.Open("sea.png", FileMode.Open))
				{
					store
						.DatabaseCommands
						.PutAttachment("albums/holidays/sea.jpg", null, file, new RavenJObject
							                                           {
								                                           { "Description", "Holidays 2014" }
							                                           });
				}
				#endregion
			}
		}
	}
}