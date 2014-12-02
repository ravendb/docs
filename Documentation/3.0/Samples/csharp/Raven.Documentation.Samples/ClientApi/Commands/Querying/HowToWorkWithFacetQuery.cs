using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Querying
{
	public class HowToWorkWithFacetQuery
	{
		private interface IFoo
		{
			#region get_facets_2
			FacetResults GetFacets(
				string index,
				IndexQuery query,
				string facetSetupDoc,
				int start = 0,
				int? pageSize = null);
			#endregion

			#region get_facets_1
			FacetResults GetFacets(
				string index,
				IndexQuery query,
				List<Facet> facets,
				int start = 0,
				int? pageSize = null);
			#endregion

			#region get_facets_5
			FacetResults[] GetMultiFacets(FacetQuery[] facetedQueries);
			#endregion
		}

		public HowToWorkWithFacetQuery()
		{
			using (var store = new DocumentStore())
			{
				#region get_facets_3
				// For the Manufacturer field look at the documents and return a count for each unique Term found
				// For the Cost field, return the count of the following ranges:
				//		Cost <= 200.0
				//		200.0 <= Cost <= 400.0
				//		400.0 <= Cost <= 600.0
				//		600.0 <= Cost <= 800.0
				//		Cost >= 800.0
				// For the Megapixels field, return the count of the following ranges:
				//		Megapixels <= 3.0
				//		3.0 <= Megapixels <= 7.0
				//		7.0 <= Megapixels <= 10.0
				//		Megapixels >= 10.0
				FacetResults facetResults = store
					.DatabaseCommands
					.GetFacets(
						"Camera/Costs",
						new IndexQuery(),
						new List<Facet>
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
										"[Dx300.0 TO Dx400.0]",
										"[Dx500.0 TO Dx600.0]",
										"[Dx700.0 TO Dx800.0]",
										"[Dx900.0 TO NULL]"
									}
								},
								new Facet
								{
									Name = "Megapixels_Range",
									Mode = FacetMode.Ranges,
									Ranges =
									{
										"[NULL TO Dx3.0]",
										"[Dx4.0 TO Dx7.0]",
										"[Dx8.0 TO Dx10.0]",
										"[Dx11.0 TO NULL]"
									}
								}
							});

				FacetResult manufacturerResults = facetResults.Results["Manufacturer"];
				FacetResult costResults = facetResults.Results["Cost_Range"];
				FacetResult megapixelResults = facetResults.Results["Megapixels_Range"];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_facets_4
				// For the Manufacturer field look at the documents and return a count for each unique Term found
				// For the Cost field, return the count of the following ranges:
				//		Cost <= 200.0
				//		200.0 <= Cost <= 400.0
				//		400.0 <= Cost <= 600.0
				//		600.0 <= Cost <= 800.0
				//		Cost >= 800.0
				// For the Megapixels field, return the count of the following ranges:
				//		Megapixels <= 3.0
				//		3.0 <= Megapixels <= 7.0
				//		7.0 <= Megapixels <= 10.0
				//		Megapixels >= 10.0
				store.DatabaseCommands.Put(
					"facets/CameraFacets",
					null,
					RavenJObject.FromObject(
						new FacetSetup
						{
							Id = "facets/CameraFacets",
							Facets =
								new List<Facet>
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
												"[Dx300.0 TO Dx400.0]",
												"[Dx500.0 TO Dx600.0]",
												"[Dx700.0 TO Dx800.0]",
												"[Dx900.0 TO NULL]"
											}
										},
										new Facet
										{
											Name = "Megapixels_Range",
											Mode = FacetMode.Ranges,
											Ranges =
											{
												"[NULL TO Dx3.0]", 
												"[Dx4.0 TO Dx7.0]", 
												"[Dx8.0 TO Dx10.0]", 
												"[Dx11.0 TO NULL]"
											}
										}
									}
						}),
					new RavenJObject());

				FacetResults facetResults = store
					.DatabaseCommands
					.GetFacets("Camera/Costs", new IndexQuery(), "facets/CameraFacets");

				FacetResult manufacturerResults = facetResults.Results["Manufacturer"];
				FacetResult costResults = facetResults.Results["Cost_Range"];
				FacetResult megapixelResults = facetResults.Results["Megapixels_Range"];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_facets_6
				FacetResults[] facetResults = store
					.DatabaseCommands
					.GetMultiFacets(
						new[]
						{
							new FacetQuery
							{
								IndexName = "Camera/Costs",
								FacetSetupDoc = "facets/CameraFacets1", 
								Query = new IndexQuery()
							},
							new FacetQuery
							{
								IndexName = "Camera/Costs",
								FacetSetupDoc = "facets/CameraFacets2", 
								Query = new IndexQuery()
							},
							new FacetQuery
							{
								IndexName = "Camera/Costs",
								FacetSetupDoc = "facets/CameraFacets3", 
								Query = new IndexQuery()
							}
						});

				Dictionary<string, FacetResult> facetResults1 = facetResults[0].Results;
				Dictionary<string, FacetResult> facetResults2 = facetResults[1].Results;
				Dictionary<string, FacetResult> facetResults3 = facetResults[2].Results;
				#endregion
			}
		}
	}
}