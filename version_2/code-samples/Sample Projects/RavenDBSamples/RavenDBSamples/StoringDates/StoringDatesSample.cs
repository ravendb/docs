using System;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.StoringDates
{
	public class StoringDatesSample : SampleBase
	{
		private DateTime Local { get; set; }
		private DateTime Utc { get; set; }
		public StoringDatesSample()
		{
			Local = DateTime.Now;
			Utc = DateTime.UtcNow;
		}

		public void StoreLocalTime()
		{
			using (var session = DocumentStore.OpenSession())
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
			using (var session = DocumentStore.OpenSession())
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
			using (var session = DocumentStore.OpenSession())
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