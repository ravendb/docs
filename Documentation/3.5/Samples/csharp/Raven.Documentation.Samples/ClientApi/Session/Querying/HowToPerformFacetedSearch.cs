using System;
using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToPerformFacetedSearch
	{
		private interface IFoo
		{
			/*
			#region facet_1
			FacetResults ToFacets<T>(
				this IQueryable<T> queryable,
				IEnumerable<Facet> facets,
				int start = 0,
				int? pageSize = null) { ... }

			FacetResults ToFacets<T>(
				this IQueryable<T> queryable,
				string facetSetupDoc,
				int start = 0,
				int? pageSize = null) { ... }
			#endregion
			*/

			/*
			#region facet_4
			FacetQuery ToFacetQuery<T>(
				this IQueryable<T> queryable,
				IEnumerable<Facet> facets,
				int start = 0,
				int? pageSize = null) { ... }

			FacetQuery ToFacetQuery<T>(
				this IQueryable<T> queryable,
				string facetSetupDoc,
				int start = 0,
				int? pageSize = null) { ... }
			#endregion
			*/
		}

		public HowToPerformFacetedSearch()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region facet_2
					// passing facets directly
					FacetResults facets = session.Query<Camera>()
						.ToFacets(
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

					TimeSpan duration = facets.Duration;
					Dictionary<string, FacetResult> results = facets.Results;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region facet_3
					// using predefined facet setup
					session.Store(
						new FacetSetup
							{
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
							}, "facets/CameraFacets");

					session.SaveChanges();

					FacetResults facets = session
						.Query<Camera>("Camera/Costs")
						.ToFacets("facets/CameraFacets");

					TimeSpan duration = facets.Duration;
					Dictionary<string, FacetResult> results = facets.Results;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region facet_5
					FacetQuery facetQuery1 = session.Query<Camera>()
						.ToFacetQuery("facets/CameraFacets1");

					FacetQuery facetQuery2 = session.Query<Camera>()
						.ToFacetQuery("facets/CameraFacets2");

					FacetResults[] results = session
						.Advanced
						.MultiFacetedSearch(facetQuery1, facetQuery2);

					FacetResults facetResults1 = results[0];
					FacetResults facetResults2 = results[1];
					#endregion
				}
			}
		}
	}
}