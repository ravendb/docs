using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.Indexes.Querying
{
	public class FacetedSearch
	{
		public void Step1()
		{
			List<Facet> _facets;

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region step_1
					_facets = new List<Facet>
						          {
							          new Facet
								          {
									          Name = "Manufacturer"
								          },
							          new Facet
								          {
									          Name = "Cost_Range",
									          Mode = FacetMode.Ranges,
									          Ranges =
										          {
											          "[NULL TO Dx200.0]",
													  "[Dx200.0 TO Dx400.0]",
													  "[Dx400.0 TO Dx600.0]",
													  "[Dx600.0 TO Dx800.0]",
													  "[Dx800.0 TO NULL]",
										          }
								          },
							          new Facet
								          {
									          Name = "Megapixels_Range", 
											  Mode = FacetMode.Ranges, 
											  Ranges =
												  {
													  "[NULL TO Dx3.0]",
													  "[Dx3.0 TO Dx7.0]",
													  "[Dx7.0 TO Dx10.0]",
													  "[Dx10.0 TO NULL]",
												  }
								          }
						          };

					session.Store(new FacetSetup { Id = "facets/CameraFacets", Facets = _facets });

					#endregion
				}
			}
		}

		public void Step2()
		{
			using (var store = new DocumentStore())
			{
				#region step_2
				store.DatabaseCommands.PutIndex("CameraCost",
							new IndexDefinition
							{
								Map = @"from camera in docs 
                                    select new 
                                    { 
                                        camera.Manufacturer, 
                                        camera.Model, 
                                        camera.Cost,
                                        camera.DateOfListing,
                                        camera.Megapixels
                                    }"
							});

				#endregion
			}
		}

		public void Step3()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region step_3
					var facetResults = session.Query<Camera>("CameraCost")
						.Where(x => x.Cost >= 100 && x.Cost <= 300)
						.ToFacets("facets/CameraFacets");

					#endregion
				}
			}
		}

		private class Camera
		{
			public int Cost { get; set; }
		}
	}
}