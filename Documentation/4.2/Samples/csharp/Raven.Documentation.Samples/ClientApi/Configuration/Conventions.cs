using System;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Sparrow;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{
    public class Conventions
    {
        public Conventions()
        {
            #region conventions_1
            using (var store = new DocumentStore()
            {
                Conventions =
                {
                    // customizations go here
                }
            }.Initialize())
            {

            }
            #endregion
        }
        
        #region FirstChar
        private string FirstCharToLower(string str) => $"{Char.ToLower(str[0])}{str.Substring(1)}";
        #endregion

        public void Examples()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
	                #region MaxHttpCacheSize
	                MaxHttpCacheSize = new Size(256, SizeUnit.Megabytes)
	                #endregion
	                ,
	                #region MaxNumberOfRequestsPerSession
	                MaxNumberOfRequestsPerSession = 10
	                #endregion
	                ,
	                #region UseOptimisticConcurrency
	                UseOptimisticConcurrency = true
	                #endregion
	                ,
	                #region DisableTopologyUpdates
                    DisableTopologyUpdates = false
	                #endregion
                    ,
	                #region SaveEnumsAsIntegers
                    SaveEnumsAsIntegers = true
	                #endregion
                    ,
                    #region RequestTimeout
                    RequestTimeout = TimeSpan.FromSeconds(90)
                    #endregion
                    ,
                    #region UseCompression
                    UseCompression = true
                    #endregion
                    ,
                    #region OperationStatusFetchMode
                    OperationStatusFetchMode = OperationStatusFetchMode.ChangesApi
                    #endregion
                    ,
                    #region TopologyCacheLocation
                    TopologyCacheLocation = @"C:\RavenDB\TopologyCache"
                    #endregion
                    ,
                    #region PropertyCasing
                    CustomizeJsonSerializer = s => s.ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    PropertyNameConverter = mi => FirstCharToLower(mi.Name)
                    #endregion
                    ,
                    #region AddIdFieldToDynamicObjects
                    AddIdFieldToDynamicObjects = false
                    #endregion
                }
            };
            
            var stor2e = new DocumentStore()
            {
                Conventions =
                {
                    #region disable_cache
                    MaxHttpCacheSize = new Size(0, SizeUnit.Megabytes)
                    #endregion
                }
            };
        }
    }
}
