using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
					List<Facet> facets = new List<Facet>
					{
						new Facet
						{
							Name = "Manufacturer"
						},
						new Facet<Camera>
						{
							Name = x => x.Cost,
							Ranges =
							{
								x => x.Cost < 200m,
								x => x.Cost > 200m && x.Cost < 400m,
								x => x.Cost > 400m && x.Cost < 600m,
								x => x.Cost > 600m && x.Cost < 800m,
								x => x.Cost > 800m
							}
						},
						new Facet<Camera>
						{
							Name = x => x.Megapixels,
							Ranges =
							{
								x => x.Megapixels < 3.0,
								x => x.Megapixels > 3.0 && x.Megapixels < 7.0,
								x => x.Megapixels > 7.0 && x.Megapixels < 10.0,
								x => x.Megapixels > 10.0
							}
						}
					};
					#endregion

					#region step_4_0
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
					List<Facet> facets = new List<Facet>();

					#region step_3_0
					FacetResults facetResults = session
						.Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
						.Where(x => x.Cost >= 100 && x.Cost <= 300)
						.ToFacets(facets);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					List<Facet> facets = new List<Facet>();

					#region step_3_1
					FacetResults facetResults = session
						.Advanced
						.DocumentQuery<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
						.WhereBetweenOrEqual(x => x.Cost, 100, 300)
						.ToFacets(facets);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				List<Facet> facets = new List<Facet>();

				#region step_3_2
				FacetResults facetResults = store
					.DatabaseCommands
					.GetFacets(
						"Cameras/ByManufacturerModelCostDateOfListingAndMegapixels",
						new IndexQuery
						{
							Query = "Cost_Range:[Dx100 TO Dx300]"
						},
						facets);
				#endregion
			}
		}

		public void Step4()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					List<Facet> facets = new List<Facet>();

					#region step_4_1
					FacetResults facetResults = session
						.Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
						.Where(x => x.Cost >= 100 && x.Cost <= 300)
						.ToFacets("facets/CameraFacets");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					List<Facet> facets = new List<Facet>();

					#region step_4_2
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
				List<Facet> facets = new List<Facet>();

				#region step_4_3
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