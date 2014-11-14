using System;
using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Exceptions;
using Raven.Imports.Newtonsoft.Json;

namespace Raven.Documentation.Samples.Server.ScalingOut.Replication
{
	public class HandlingConflicts
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (DocumentStore store = new DocumentStore())
			{
				#region replicationconflicts1
				using (IDocumentSession session = store.OpenSession())
				{
					try
					{
						User user = session.Load<User>("users/ayende");
						Console.WriteLine(user.Name);
					}
					catch (ConflictException e)
					{
						Console.WriteLine("Choose which document you want to preserve:");
						List<JsonDocument> list = new List<JsonDocument>();
						for (int i = 0; i < e.ConflictedVersionIds.Length; i++)
						{
							var doc = store.DatabaseCommands.Get(e.ConflictedVersionIds[i]);
							list.Add(doc);
							Console.WriteLine("{0}. {1}", i, doc.DataAsJson.ToString(Formatting.None));
						}

						int select = int.Parse(Console.ReadLine());

						JsonDocument resolved = list[select];
						resolved.Metadata.Remove("Raven-Replication-Conflict-Document");
						resolved.Metadata.Remove("Raven-Replication-Conflict");
						resolved.Metadata.Remove("@id");
						resolved.Metadata.Remove("@etag");

						store.DatabaseCommands.Put("users/ayende", null, resolved.DataAsJson, resolved.Metadata);
					}
				}
				#endregion
			}
		}
	}
}