namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
	using System;
	using Client.Document;
	using CodeSamples;
	using Imports.Newtonsoft.Json.Serialization;

	public class RequestHandling
	{
		public RequestHandling()
		{
			var store = new DocumentStore();

			DocumentConvention Conventions = store.Conventions;

			#region use_paraller_multi_get
			Conventions.UseParallelMultiGet = true;
			#endregion

			#region allow_multiple_async_ops
			Conventions.AllowMultipuleAsyncOperations = false;
			#endregion

			#region handle_forbidden_response_async
			Conventions.HandleForbiddenResponseAsync = (forbiddenResponse, credentials) => 
			#endregion
				null;

			#region handle_unauthorized_response_async
			Conventions.HandleUnauthorizedResponseAsync = (unauthorizedResponse, credentials) =>
			#endregion
			null;

			#region customize_json_serializer
			Conventions.CustomizeJsonSerializer = serializer => { };
			#endregion

			#region json_contract_resolver
			Conventions.JsonContractResolver = new CustomJsonContractResolver();
			#endregion
		} 
	}

	#region custom_json_contract_resolver
	public class CustomJsonContractResolver : IContractResolver
	{
		public JsonContract ResolveContract(Type type)
		{
			throw new CodeOmitted();
		}
	}
	#endregion
}