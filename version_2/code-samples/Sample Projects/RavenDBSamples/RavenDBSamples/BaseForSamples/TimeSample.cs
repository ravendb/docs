using System;

namespace RavenDBSamples.BaseForSamples
{
	public class TimeSample
	{
		public string Title { get; set; }
		public DateTime Time { get; set; }
	}

	public class TimeOffsetSample
	{
		public string Title { get; set; }
		public DateTimeOffset Time { get; set; }
	}
}