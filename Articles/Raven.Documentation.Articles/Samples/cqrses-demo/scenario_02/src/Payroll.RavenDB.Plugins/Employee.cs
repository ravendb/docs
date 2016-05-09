using Raven.Database.Plugins;

namespace Payroll.RavenDB.Plugins
{
    #region article_cqrs_12
    public static class Employee
    {
        public static string FullName(dynamic source)
        {
            var e = source.Events[0];
            return $"{e.Name.GivenName} {e.Name.Surname}";
        }

        public static decimal Salary(dynamic source, int atVersion = 0)
        {
            var result = 0M;

            foreach (var evt in source.Events)
            {
                if (IsEmployeeRegistered(evt))
                    result += (decimal) evt.InitialSalary;
                else if (IsSalaryRaised(evt))
                    result += (decimal) evt.Amount;

                if (atVersion != 0 && evt.Version == atVersion)
                    break;
            }
            return result;
        }

        private static bool IsEmployeeRegistered(dynamic evt)
        {
            return ((string) evt["$type"])
                .StartsWith("Payroll.Domain.Events.EmployeeRegisteredEvent");
        }

        private static bool IsSalaryRaised(dynamic evt)
        {
            return ((string) evt["$type"])
                .StartsWith("Payroll.Domain.Events.EmployeeSalaryRaisedEvent");
        }
    }
    
    public class EmployeeDynamicCompilationExtension : 
        AbstractDynamicCompilationExtension
    {
        public override string[] GetNamespacesToImport()
        {
            return new[] {typeof (Employee).Namespace};
        }

        public override string[] GetAssembliesToReference()
        {
            return new[] {typeof (Employee).Assembly.Location};
        }
    }
    #endregion

}
