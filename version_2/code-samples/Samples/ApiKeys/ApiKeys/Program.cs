using System;
using Raven.Abstractions;
using Raven.Client.Document;

namespace ApiKeys
{
	class Program
	{
		static void Main(string[] args)
		{
			var port = 0;

			Console.Out.WriteLine("Run a local server, make sure to set \"AnonymousAccess\" to \"None\"  ");
			Console.Out.WriteLine("Enter server's port here (leave empty to use 8080): ");

			while (port == 0)
			{
				var line = Console.In.ReadLine();
				if (string.IsNullOrEmpty(line))
					port = 8080;
				else if (int.TryParse(line, out port) == false)
					Console.Out.WriteLine("Port must be a number, enter port (leave empty to use 8080): ");
			}

			Console.Out.WriteLine("Follow these instructions:");
			Console.Out.WriteLine("1) Open the browser");
			Console.Out.WriteLine("2) Navigate to: http://localhost:" + port);
			Console.Out.WriteLine("3) Go to the Databases section and select \"System Database\" (on the top right)");
			Console.Out.WriteLine("4) Go to the settings (press the cog wheel next to the database name)");
			Console.Out.WriteLine("5) In the Api Keys section Add a new key, select a name, generate a secret and add a database settings for \"ApiKeySample\"");
			Console.Out.WriteLine("6) Don't forget to click the \"Save Changes\" button");
			Console.Out.WriteLine("7) Right-click on the full api key and copy it");

			Console.Out.WriteLine("Paste here the full api key: ");
			var apiKey = Console.In.ReadLine();

			Console.Out.WriteLine("");
			Console.Out.WriteLine("Press Enter to try and save with correct api key");
			Console.In.ReadLine();
			Console.Out.WriteLine("Trying to save a document with the correct api key");

			#region apikeys1
			var store = new DocumentStore
				                       {
					                       Url = "http://localhost:" + port,
					                       DefaultDatabase = "ApiKeySample",
					                       ApiKey = apiKey
				                       }.Initialize();
			#endregion

			using (var session = store.OpenSession())
			{
				session.Store(new SampleData
				{
					Id = "SampleData/1",
					Name = "Rhinos"
				});
				session.SaveChanges();
				Console.Out.WriteLine("Document saved");
			}

			store.Dispose();

			Console.Out.WriteLine("");
			Console.Out.WriteLine("Press Enter to try and load with correct api key");
			Console.In.ReadLine();
			Console.Out.WriteLine("Trying to load the document with the correct api key");

			store = new DocumentStore
			{
				Url = "http://localhost:" + port,
				DefaultDatabase = "ApiKeySample",
				ApiKey = apiKey
			}.Initialize();

			using (var session = store.OpenSession())
			{
				var data = session.Load<SampleData>("SampleData/1");

				Console.Out.WriteLine("Name: " + data.Name + ", Created At: " + data.StoredAt);
			}

			store.Dispose();

			Console.Out.WriteLine("");
			Console.Out.WriteLine("Press Enter to try and load with wrong api key");
			Console.In.ReadLine();
			Console.Out.WriteLine("Trying to load the document with the wrong api key");
			try
			{
				store = new DocumentStore
					        {
						        Url = "http://localhost:" + port,
						        DefaultDatabase = "ApiKeySample",
						        ApiKey = "NotApiKey"
					        }.Initialize();

				using (var session = store.OpenSession())
				{
					var data = session.Load<SampleData>("SampleData/1");

					Console.Out.WriteLine("Name: " + data.Name + ", Created At: " + data.StoredAt);
				}
			}
			catch (Exception e)
			{
				Console.Out.WriteLine("Process failed, error message: " + e.Message);
			}

			store.Dispose();
		}
	}

	public class SampleData
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public DateTime StoredAt { get; set; }

		public SampleData()
		{
			StoredAt = SystemTime.UtcNow.ToLocalTime();
		}
	}
}