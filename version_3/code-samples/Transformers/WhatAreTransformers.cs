using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Transformers
{
	public class WhatAreTransformers
	{
		#region transformers_1
		public class Orders_Prices : AbstractTransformerCreationTask<Order>
		{
			public class Result
			{
				public string CustomerId { get; set; }

				public double TotalPrice { get; set; }
			}

			public Orders_Prices()
			{
				TransformResults = orders => from order in orders 
											 select new
												        {
															CustomerId = order.CustomerId,
													        TotalPrice = order.TotalPrice
												        };
			}
		}
		#endregion

		public WhatAreTransformers()
		{
			using (var store = new DocumentStore())
			{
				#region transformers_2
				// save transformer on server
				new Orders_Prices().Execute(store);
				#endregion

				using (var session = store.OpenSession())
				{
					#region transformers_3
					Orders_Prices.Result result = session
						.Load<Orders_Prices, Orders_Prices.Result>("orders/1"); 
					#endregion

					#region transformers_4
					IList<Orders_Prices.Result> results = session
						.Query<Order>()
						.TransformWith<Orders_Prices, Orders_Prices.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}