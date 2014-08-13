using System;
using System.Linq;
using System.Linq.Expressions;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Client.Spatial;
using Raven.Documentation.CodeSamples.Indexes;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToQuerySpatialIndex
	{
		private interface IFoo<TResult>
		{
			#region spatial_1
			IRavenQueryable<TResult> Spatial(
				Expression<Func<TResult, object>> path,
				Func<SpatialCriteriaFactory, SpatialCriteria> clause);
			#endregion

			#region spatial_3
			IDocumentQueryCustomization RelatesToShape(
				string fieldName,
				string shapeWKT,
				SpatialRelation rel);
			#endregion

			#region spatial_5
			IDocumentQueryCustomization SortByDistance();
			#endregion

			#region spatial_7
			IDocumentQueryCustomization WithinRadiusOf(
				double radius,
				double latitude,
				double longitude);

			IDocumentQueryCustomization WithinRadiusOf(
				double radius,
				double latitude,
				double longitude,
				SpatialUnits radiusUnits);

			IDocumentQueryCustomization WithinRadiusOf(
				string fieldName,
				double radius,
				double latitude,
				double longitude);

			IDocumentQueryCustomization WithinRadiusOf(
				string fieldName,
				double radius,
				double latitude,
				double longitude,
				SpatialUnits radiusUnits);
			#endregion
		}

		public HowToQuerySpatialIndex()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region spatial_2
					// return all matching entities
					// where 'Shape' (spatial field) is within 10 kilometers radius
					// from 32.1234 latitude and 23.4321 longitude coordinates
					var results = session
						.Query<SpatialDoc, SpatialDoc_Index>()
						.Spatial(x => x.Shape, criteria => criteria.WithinRadiusOf(10, 32.1234, 23.4321))
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region spatial_4
					// return all matching entities
					// where 'Shape' (spatial field) is within 10 kilometers radius
					// from 32.1234 latitude and 23.4321 longitude coordinates
					// this equals to WithinRadiusOf(10, 32.1234, 23.4321)
					var results = session
						.Query<SpatialDoc, SpatialDoc_Index>()
						.Customize(x => x.RelatesToShape("Shape", "Circle(32.1234 23.4321 d=10.0000)", SpatialRelation.Within))
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region spatial_6
					// return all matching entities
					// where 'Shape' (spatial field) is within 10 kilometers radius
					// from 32.1234 latitude and 23.4321 longitude coordinates
					// sort results by distance from origin point
					var results = session
						.Query<SpatialDoc, SpatialDoc_Index>()
						.Customize(x => x.SortByDistance())
						.Spatial(x => x.Shape, criteria => criteria.WithinRadiusOf(10, 32.1234, 23.4321))
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region spatial_8
					// return all matching entities
					// where 'Shape' (spatial field) is within 10 kilometers radius
					// from 32.1234 latitude and 23.4321 longitude coordinates
					var results = session
						.Query<SpatialDoc, SpatialDoc_Index>()
						.Customize(x => x.WithinRadiusOf("Shape", 10, 32.1234, 23.4321))
						.ToList();
					#endregion
				}
			}
		}
	}
}