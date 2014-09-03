using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class FacetedSearch
	{
		#region step_2
		public class Cameras_ByManufacturerModelCostDateOfListingAndMegapixels : AbstractIndexCreationTask<Camera>
		{
			public Cameras_ByManufacturerModelCostDateOfListingAndMegapixels()
			{
				Map = cameras => from camera in cameras
							select new
							{
								camera.Manufacturer,
								camera.Model,
								camera.Cost,
								camera.DateOfListing,
								camera.Megapixels
							};
			}
		}
		#endregion

		public void Step1()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region step_1

					var facets = new List<Facet>
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
												"[Dx800.0 TO NULL]"
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
												"[Dx10.0 TO NULL]"
											}
								        }
						             };

					session.Store(new FacetSetup { Id = "facets/CameraFacets", Facets = facets });
					#endregion
				}
			}
		}

		public void Step3()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region step_3_0
					FacetResults facetResults = session
						.Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
						.Where(x => x.Cost >= 100 && x.Cost <= 300)
						.ToFacets("facets/CameraFacets");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region step_3_1
					FacetResults facetResults = session
						.Advanced
						.DocumentQuery<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
						.WhereBetweenOrEqual(x => x.Cost, 100, 300)
						.ToFacets("facets/CameraFacets");
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region step_3_2
				FacetResults facetResults = store
					.DatabaseCommands
					.GetFacets(
						"Cameras/ByManufacturerModelCostDateOfListingAndMegapixels",
						new IndexQuery
						{
							Query = "Cost_Range:[Dx100 TO Dx300]"
						},
						"facets/CameraFacets");
				#endregion
			}
		}
	}
}