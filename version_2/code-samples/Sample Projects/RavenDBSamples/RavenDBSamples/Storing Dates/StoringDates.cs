using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;

namespace RavenDBSamples.Storing_Dates
{
	public class StoringDates
	{
		private DateTime Local { get; set; }
		private DateTime Utc { get; set; }
		private IDocumentStore Store { get; set; }
		public StoringDates()
		{
			Store = new DocumentStore
				{
					Url = "http://localhost:8080",
					DefaultDatabase = "SampleDatabase"
				}.Initialize();

			Local = DateTime.Now;
			Utc = DateTime.UtcNow;
		}

		public void StoreLocalTime()
		{
			using (var session = Store.OpenSession())
			{
				session.Store(new TimeSample
					{
						Title = "Local",
						Time = Local
					});
			}
		}

		public void StoreUtcTime()
		{
			using (var session = Store.OpenSession())
			{
				session.Store(new TimeSample
				{
					Title = "Utc",
					Time = Utc
				});
			}
		}

		public void StoreWithOffset()
		{
			using (var session = Store.OpenSession())
			{
				session.Store(new TimeOffsetSample
				{
					Title = "Offset",
					Time = new DateTimeOffset(Local)
				});
			}
		}
	}
}