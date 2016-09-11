using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Documentation.Samples.ClientApi.Commands.Documents;
using Raven.Json.Linq;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session
{
	using System.Linq;

	public class LoadingEntities
	{
		private class Employees_NoLastName : AbstractTransformerCreationTask<Employee>
		{
		}

		private class Products_Transformer: AbstractTransformerCreationTask<Product>
		{
		}

        #region transformer
        public class SimpleTransformer : AbstractTransformerCreationTask
        {
            public class Result
            {
                public string Name { get; set; }
            }

            public override TransformerDefinition CreateTransformerDefinition(bool prettify = true)
            {
                return new TransformerDefinition
                {
                    Name = "SimpleTransformer",
                    TransformResults = "from r in results select new { Name = Parameter(\"Name\") }"
                };
            }
        }
        #endregion

        #region class_with_interger_id
        public class EntityWithIntegerId
		{
			public int Id { get; set; }
			/*
			...
			*/
		}
		#endregion

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
				RavenPagingInformation pagingInformation = null,
				string skipAfter = null);

			TResult[] LoadStartingWith<TTransformer, TResult>(
				string keyPrefix,
				string matches = null,
				int start = 0,
				int pageSize = 25,
				string exclude = null,
				RavenPagingInformation pagingInformation = null,
				Action<ILoadConfiguration> configure = null,
				string skipAfter = null);
			#endregion

			#region loading_entities_5_0
            IEnumerator<StreamResult<T>> Stream<T>(
                Etag fromEtag, 
                int start = 0, 
                int pageSize = int.MaxValue, 
                RavenPagingInformation pagingInformation = null, 
                string transformer = null, 
                Dictionary<string, RavenJToken> transformerParameters = null);
            
            IEnumerator<StreamResult<T>> Stream<T>(
                string startsWith, 
                string matches = null, 
                int start = 0, 
                int pageSize = int.MaxValue, 
                RavenPagingInformation pagingInformation = null, 
                string skipAfter = null, 
                string transformer = null, 
                Dictionary<string, RavenJToken> transformerParameters = null);
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
					Employee employee = session.Load<Employee>("employees/1");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_1_2
					// loading 'employees/1'
					// and transforming result using 'Employees_NoLastName' transformer
					// which returns 'LastName' as 'null'
					Employee employee = session.Load<Employees_NoLastName, Employee>("employees/1");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_1_3
					EntityWithIntegerId entity = session.Load<EntityWithIntegerId>(1);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_1_4
					Employee employee = session.Load<Employee>(1);
					#endregion
				}
				

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_1
					// loading 'products/1'
					// including document found in 'Supplier' property
					Product product = session
						.Include<Product>(x => x.Supplier)
						.Load<Product>("products/1");

					Supplier supplier = session.Load<Supplier>(product.Supplier); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_2
					// loading 'products/1'
					// including document found in 'Supplier' property
					Product product = session
						.Include("Supplier")
						.Load<Product>("products/1");

					Supplier supplier = session.Load<Supplier>(product.Supplier); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_3
					// loading 'products/1'
					// including document found in 'Supplier' property
					// transforming loaded product according to Products_Transformer
					Product product = session
						.Include<Product>(x => x.Supplier)
						.Load<Products_Transformer, Product>("products/1");

					Supplier supplier = session.Load<Supplier>(product.Supplier); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_3_1
					Employee[] employees = session.Load<Employee>(new List<string> { "employees/1", "employees/2" });
					Employee employee1 = employees[0];
					Employee employee2 = employees[1];
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_3_2
					// loading 'employees/1' and 'employees/2'
					// and transforming results using 'Employees_NoLastName' transformer
					// which returns 'LastName' as 'null'
					Employee[] employees = session
						.Load<Employees_NoLastName, Employee>(new List<string> { "employees/1", "employees/2" });
					Employee employee1 = employees[0];
					Employee employee2 = employees[1];
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_1
					// return up to 128 entities with Id that starts with 'employees'
					Employee[] result = session
						.Advanced
						.LoadStartingWith<Employee>("employees", null, 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_2
					// return up to 128 entities with Id that starts with 'employees/' 
					// and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
					Employee[] result = session
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
					Employee[] result = session
						.Advanced
						.LoadStartingWith<Employees_NoLastName, Employee>("employees/", "1?1", 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_5_1
					IEnumerator<StreamResult<Employee>> enumerator = session.Advanced.Stream<Employee>("employees/");
					while (enumerator.MoveNext())
					{
						StreamResult<Employee> employee = enumerator.Current;
					}
					#endregion
				}

                using (var session = store.OpenSession())
                {
                    #region loading_entities_5_2
                    var transformer = new SimpleTransformer();
                    transformer.Execute(store);

                    using (IEnumerator<StreamResult<SimpleTransformer.Result>> enumerator = session.Advanced.Stream<SimpleTransformer.Result>("people/", transformer: transformer.TransformerName, transformerParameters: new Dictionary<string, RavenJToken> { { "Name", "Bill" } }))
                    {
                        while (enumerator.MoveNext())
                        {
                            StreamResult<SimpleTransformer.Result> result = enumerator.Current;
                            string name = result.Document.Name;
                            Assert.Equal("Bill", name); // Should be true
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
				{
					#region loading_entities_6_1
					bool isLoaded = session.Advanced.IsLoaded("employees/1"); // false
					Employee employee = session.Load<Employee>("employees/1");
					isLoaded = session.Advanced.IsLoaded("employees/1"); // true
					#endregion
				}
			}
		}
	}
}