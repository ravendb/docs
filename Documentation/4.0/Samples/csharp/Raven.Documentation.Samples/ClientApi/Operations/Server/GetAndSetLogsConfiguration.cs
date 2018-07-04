using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations.Logs;
using Sparrow.Logging;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class GetAndSetLogsConfiguration
    {
        private class Syntax
        {
            public class GetLogsConfigurationOperation
            {
                #region get_logs_1
                public GetLogsConfigurationOperation()
                #endregion
                {
                }
            }

            #region get_logs_2
            public class GetLogsConfigurationResult
            {
                public LogMode CurrentMode { get; set; }

                public LogMode Mode { get; set; }

                public string Path { get; set; }

                public bool UseUtcTime { get; set; }
            }
            #endregion

            public class SetLogsConfigurationOperation
            {
                #region set_logs_2
                public class Parameters
                {
                    public LogMode Mode { get; set; }
                }
                #endregion

                #region set_logs_1
                public SetLogsConfigurationOperation(Parameters parameters)
                #endregion
                {
                }
            }
        }

        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                #region get_logs_3
                GetLogsConfigurationResult logsConfiguration = store
                    .Maintenance
                    .Server
                    .Send(new GetLogsConfigurationOperation());
                #endregion

                #region set_logs_3
                store
                    .Maintenance
                    .Server
                    .Send(new SetLogsConfigurationOperation(
                        new SetLogsConfigurationOperation.Parameters
                        {
                            Mode = LogMode.Information
                        }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region get_logs_4
                GetLogsConfigurationResult logsConfiguration = await store
                    .Maintenance
                    .Server
                    .SendAsync(new GetLogsConfigurationOperation());
                #endregion

                #region set_logs_4
                await store
                    .Maintenance
                    .Server
                    .SendAsync(new SetLogsConfigurationOperation(
                        new SetLogsConfigurationOperation.Parameters
                        {
                            Mode = LogMode.Information
                        }));
                #endregion
            }
        }
    }
}
