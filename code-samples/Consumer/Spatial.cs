namespace RavenCodeSamples.Consumer
{
	public class Spatial : CodeSampleBase
	{
		#region spatial0
		class Restaurant
		{
			public string Name { get; set; }
			public double Longitude { get; set; }
			public double Latitude { get; set; }
			public short Rating { get; set; }
		}
		#endregion

		public void BasicSpatialSearch()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					
				}
			}
		}
	}
}
