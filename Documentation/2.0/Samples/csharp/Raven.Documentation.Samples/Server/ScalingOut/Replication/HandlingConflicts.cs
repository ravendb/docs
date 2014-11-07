namespace RavenCodeSamples.Server.ScalingOut.Replication
{
	using System;
	using System.Collections.Generic;

	using Raven.Abstractions.Data;
	using Raven.Client.Exceptions;
	using Raven.Imports.Newtonsoft.Json;

	public class HandlingConflicts : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region replicationconflicts1
				using (var session = documentStore.OpenSession())
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
							var doc = documentStore.DatabaseCommands.Get(e.ConflictedVersionIds[i]);
							list.Add(doc);
							Console.WriteLine("{0}. {1}", i, doc.DataAsJson.ToString(Formatting.None));
						}

						var select = int.Parse(Console.ReadLine());
						var resolved = list[select];
						documentStore.DatabaseCommands.Put("users/ayende", null, resolved.DataAsJson, resolved.Metadata);
					}
				}

				#endregion
			}
		}
	}
}