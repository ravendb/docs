using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.ClientApi.Session
{
	public class LoadingEntities
	{
		private class PersonNoLastName : AbstractTransformerCreationTask<Person>
		{
			public PersonNoLastName()
			{
				TransformResults = people => from person in people
											 select new
														{
															Id = person.Id,
															FirstName = person.FirstName
														};
			}
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
					var person = session.Load<Person>("people/1");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_1_2
					// loading 'people/1'
					// and transforming result using 'PersonNoLastName' transformer
					// which returns 'LastName' as 'null'
					var person = session.Load<PersonNoLastName, Person>("people/1");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_1
					// loading 'people/1'
					// including document found in 'AddressId' property
					var person = session
						.Include<Person>(x => x.AddressId)
						.Load<Person>("people/1");

					var address = session.Load<Address>(person.AddressId); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_2_2
					// loading 'people/1'
					// including document found in 'AddressId' property
					var person = session
						.Include("AddressId")
						.Load<Person>("people/1");

					var address = session.Load<Address>(person.AddressId); // this will not make server call
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_3_1
					var people = session.Load<Person>(new List<string> { "people/1", "people/2" });
					var person1 = people[0];
					var person2 = people[1];
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_3_2
					// loading 'people/1' and 'people/2'
					// and transforming results using 'PersonNoLastName' transformer
					// which returns 'LastName' as 'null'
					var people = session
						.Load<PersonNoLastName, Person>(new List<string> { "people/1", "people/2" });
					var person1 = people[0];
					var person2 = people[1];
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_1
					// return up to 128 entities with Id that starts with 'people'
					var result = session
						.Advanced
						.LoadStartingWith<Person>("people", null, 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_2
					// return up to 128 entities with Id that starts with 'people/' 
					// and rest of the key begins with "1" or "2" e.g. people/10, people/25
					var result = session
						.Advanced
						.LoadStartingWith<Person>("people/", "1*|2*", 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_4_3
					// return up to 128 entities with Id that starts with 'people/' 
					// and rest of the Id have length of 3, begins and ends with "1" 
					// and contains any character at 2nd position e.g. people/101, people/1B1
					// and transform results using 'PersonNoLastName' transformer
					var result = session
						.Advanced
						.LoadStartingWith<PersonNoLastName, Person>("people/", "1?1", 0, 128);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_5_1
					var enumerator = session.Advanced.Stream<Person>(null, "people/");
					while (enumerator.MoveNext())
					{
						var person = enumerator.Current;
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region loading_entities_6_1
					var isLoaded = session.Advanced.IsLoaded("people/1"); // false
					var person = session.Load<Person>("people/1");
					isLoaded = session.Advanced.IsLoaded("people/1"); // true
					#endregion
				}
			}
		}
	}
}