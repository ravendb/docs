using System;
using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Exceptions;
using Raven.Imports.Newtonsoft.Json;

namespace Raven.Documentation.CodeSamples.Server.ScalingOut.Replication
{
	public class HandlingConflicts
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region replicationconflicts1
				using (var session = store.OpenSession())
				{
					try
					{
						var user = session.Load<User>("users/ayende");
						Console.WriteLine(user.Name);
					}
					catch (ConflictException e)
					{
						Console.WriteLine("Choose which document you want to preserve:");
						var list = new List<JsonDocument>();
						for (int i = 0; i < e.ConflictedVersionIds.Length; i++)
						{
							var doc = store.DatabaseCommands.Get(e.ConflictedVersionIds[i]);
							list.Add(doc);
							Console.WriteLine("{0}. {1}", i, doc.DataAsJson.ToString(Formatting.None));
						}

						var select = int.Parse(Console.ReadLine());
						var resolved = list[select];
						store.DatabaseCommands.Put("users/ayende", null, resolved.DataAsJson, resolved.Metadata);
					}
				}
				#endregion
			}
		}
	}
}