using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session
{
	public class LoadingEntities
	{
		private class Employees_NoLastName : AbstractTransformerCreationTask<Employee>
		{
		}

		private interface IFoo
		{
			#region loading_entities_1_0
			T Load<T>(string id);

			T Load<T>(ValueType id);

			TResult Load<TResult>(
				string id,
				string transformer,
				Action<ILoadConfiguration> configure);

			TResult Load<TResult>(
				string id,
				Type transformerType,
				Action<ILoadConfiguration> configure = null);

			TResult Load<TTransformer, TResult>(
				string id,
				Action<ILoadConfiguration> configure = null)
				where TTransformer : AbstractTransformerCreationTask, new();
			#endregion

			#region loading_entities_2_0
			ILoaderWithInclude<object> Include(string path);

			ILoaderWithInclude<T> Include<T>(Expression<Func<T, object>> path);

			ILoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, object>> path);
			#endregion

			#region loading_entities_3_0
			TResult[] Load<TResult>(IEnumerable<string> ids);

			TResult[] Load<TResult>(IEnumerable<ValueType> ids);

			TResult[] Load<TResult>(params ValueType[] ids);

			TResult[] Load<TResult>(
				IEnumerable<string> ids,
				string transformer,
				Action<ILoadConfiguration> configure = null);

			TResult[] Load<TResult>(
				IEnumerable<string> ids,
				Type transformerType,
				Action<ILoadConfiguration> configure = null);

			TResult[] Load<TTransformer, TResult>(
				IEnumerable<string> ids,
				Action<ILoadConfiguration> configure = null)
				where TTransformer : AbstractTransformerCreationTask, new();
			#endregion

			#region loading_entities_4_0
			TResult[] LoadStartingWith<TResult>(
				string keyPrefix,
				string matches = null,
				int start = 0,
				int pageSize = 25,
				string exclude = null,
				RavenPagingInformation pagingInformation = null);

			TResult[] LoadStartingWith<TTransformer, TResult>(
				string keyPrefix,
				string matches = null,
				int start = 0,
				int pageSize = 25,
				string exclude = null,
				RavenPagingInformation pagingInformation = null,
				Action<ILoadConfiguration> configure = null);
			#endregion

			#region loading_entities_5_0
			IEnumerator<StreamResult<T>> Stream<T>(
				Etag fromEtag,
				int start = 0,
				int pageSize = int.MaxValue,
				RavenPagingInformation pagingInformation = null);

			IEnumerator<StreamResult<T>> Stream<T>(
				string startsWith,
				string matches = null,
				int start = 0,
				int pageSize = int.MaxValue,
				RavenPagingInformation pagingInformation = null);
			#endregion

			#region loading_entities_6_0
			bool IsLoaded(string id);
			#endregion
		}

		public LoadingEntities()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region loading_entities_1_1
					var employee = session.Load<Employee>("employees/1");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_1_2
					// loading 'employees/1'
					// and transforming result using 'Employees_NoLastName' transformer
					// which returns 'LastName' as 'null'
					var employee = session.Load<Employees_NoLastName, Employee>("employees/1");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_1
					// loading 'products/1'
					// including document found in 'Supplier' property
					var product = session
						.Include<Product>(x => x.Supplier)
						.Load<Product>("products/1");

					var supplier = session.Load<Address>(product.Supplier); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_2
					// loading 'products/1'
					// including document found in 'Supplier' property
					var product = session
						.Include("Supplier")
						.Load<Product>("products/1");

					var supplier = session.Load<Address>(product.Supplier); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_3_1
					var employees = session.Load<Employee>(new List<string> { "employees/1", "employees/2" });
					var employee1 = employees[0];
					var employee2 = employees[1];
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_3_2
					// loading 'employees/1' and 'employees/2'
					// and transforming results using 'Employees_NoLastName' transformer
					// which returns 'LastName' as 'null'
					var employees = session
						.Load<Employees_NoLastName, Employee>(new List<string> { "employees/1", "employees/2" });
					var employee1 = employees[0];
					var employee2 = employees[1];
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_1
					// return up to 128 entities with Id that starts with 'employees'
					var result = session
						.Advanced
						.LoadStartingWith<Employee>("employees", null, 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_2
					// return up to 128 entities with Id that starts with 'employees/' 
					// and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
					var result = session
						.Advanced
						.LoadStartingWith<Employee>("employees/", "1*|2*", 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_3
					// return up to 128 entities with Id that starts with 'employees/' 
					// and rest of the Id have length of 3, begins and ends with "1" 
					// and contains any character at 2nd position e.g. employees/101, employees/1B1
					// and transform results using 'Employees_NoLastName' transformer
					var result = session
						.Advanced
						.LoadStartingWith<Employees_NoLastName, Employee>("employees/", "1?1", 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_5_1
					var enumerator = session.Advanced.Stream<Employee>(null, "employees/");
					while (enumerator.MoveNext())
					{
						var employee = enumerator.Current;
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_6_1
					var isLoaded = session.Advanced.IsLoaded("employees/1"); // false
					var employee = session.Load<Employee>("employees/1");
					isLoaded = session.Advanced.IsLoaded("employees/1"); // true
					#endregion
				}
			}
		}
	}
}