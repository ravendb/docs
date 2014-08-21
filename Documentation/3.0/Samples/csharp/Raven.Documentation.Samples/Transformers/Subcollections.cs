using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	public class Subcollections
	{
		#region transformers_1
		public class Orders_ByOrderIdCompanyAndProductName : AbstractIndexCreationTask<Order>
		{
			public class Result
			{
				public string OrderId { get; set; }

				public string Company { get; set; }

				public string ProductName { get; set; }
			}

			public Orders_ByOrderIdCompanyAndProductName()
			{
				Map = orders => from order in orders
						from line in order.Lines
						select new
						{
							OrderId = order.Id,
							order.Company,
							line.ProductName
						};

				StoreAllFields(FieldStorage.Yes);
			}
		}
		#endregion

		#region transformers_2
		public class OrderIdCompanyAndProductName_ToOrderIdAndProductName : AbstractTransformerCreationTask<Orders_ByOrderIdCompanyAndProductName.Result>
		{
			public class Result
			{
				public string OrderId { get; set; }

				public string ProductName { get; set; }
			}

			public OrderIdCompanyAndProductName_ToOrderIdAndProductName()
			{
				TransformResults = results => from result in results
									select new
									{
										OrderId = result.OrderId,
										ProductName = result.ProductName
									};
			}
		}
		#endregion

		public Subcollections()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_3
					// this query will return only 6 results
					// because only 1 entry per document will be processed by transformer
					IList<OrderIdCompanyAndProductName_ToOrderIdAndProductName.Result> results = session
						.Query<Orders_ByOrderIdCompanyAndProductName.Result, Orders_ByOrderIdCompanyAndProductName>()
						.Where(x => x.Company == "companies/1")
						.TransformWith<OrderIdCompanyAndProductName_ToOrderIdAndProductName, OrderIdCompanyAndProductName_ToOrderIdAndProductName.Result>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_4
					// this query will return only all 12 results
					IList<OrderIdCompanyAndProductName_ToOrderIdAndProductName.Result> results = session
						.Query<Orders_ByOrderIdCompanyAndProductName.Result, Orders_ByOrderIdCompanyAndProductName>()
						.Customize(x => x.SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(true))
						.Where(x => x.Company == "companies/1")
						.TransformWith<OrderIdCompanyAndProductName_ToOrderIdAndProductName, OrderIdCompanyAndProductName_ToOrderIdAndProductName.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}