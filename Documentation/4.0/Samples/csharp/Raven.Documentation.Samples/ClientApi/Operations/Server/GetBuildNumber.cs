namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class GetBuildNumber
    {
        private class Syntax
        {
            public class GetBuildNumberOperation
            {
                #region buildNumber_1
                public GetBuildNumberOperation()
                #endregion
                {                    
                }
            }

            #region buildNumber_2
            public class BuildNumber
            {
                public string ProductVersion { get; set; }

                public int BuildVersion { get; set; }

                public string CommitHash { get; set; }

                public string FullVersion { get; set; }
            }
            #endregion
        }
    }
}
