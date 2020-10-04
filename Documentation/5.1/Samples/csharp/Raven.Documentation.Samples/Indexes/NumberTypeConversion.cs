using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    class NumberTypeConversion
    {
        #region tryconvert_linq
        public class TryConvertIndexLINQ : AbstractIndexCreationTask<Item>
        {
            public TryConvertIndexLINQ()
            {
                Map = items => from item in items
                               select new
                               {
                                   DoubleValue = TryConvert<double>(item.DoubleValue) ?? -1,
                                   FloatValue = TryConvert<float>(item.FloatValue) ?? -1,
                                   LongValue = TryConvert<long>(item.LongValue) ?? -1,
                                   IntValue = TryConvert<int>(item.IntValue) ?? -1,
                                   StringValue = TryConvert<double>(item.StringValue) ?? -1,
                                   ObjectValue = TryConvert<long>(item.ObjectValue) ?? -1
                               };
            }
        }
        #endregion

        #region tryconvert_js
        public class TryConvertIndexJS : AbstractJavaScriptIndexCreationTask
        {
            public TryConvertIndexJS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Items', function (item) {
                        return {
                            DoubleValue: tryConvertToNumber(item.DoubleValue) || -1,
                            FloatValue: tryConvertToNumber(item.FloatValue) || -1,
                            LongValue: tryConvertToNumber(item.LongValue) || -1,
                            IntValue: tryConvertToNumber(item.IntValue) || -1,
                            StringValue: tryConvertToNumber(item.StringValue) || -1,
                            ObjectValue: tryConvertToNumber(item.ObjectValue) || -1
                        };
                    })"
                };
            }
        }
        #endregion

        #region tryconvert_class
        public class Item
        {
            public double DoubleValue { get; set; }
            public float FloatValue { get; set; }
            public long LongValue { get; set; }
            public int IntValue { get; set; }
            public string StringValue { get; set; }
            public Company ObjectValue { get; set; }
        }
        #endregion

        #region tryconvert_postal
        public class Employees_ByPostalCode : AbstractIndexCreationTask<Employee>
        {
            public class Result {
                public long PostalCode { get; set; }
            }

            public Employees_ByPostalCode()
            {
                Map = employees => from employee in employees
                                   select new Result
                                   {
                                       PostalCode = TryConvert<long>(employee.Address.PostalCode) ?? -1
                                   };
            }
        }
        #endregion

        public void Query() {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query
                    List<Employee> employeesWithoutPostalCode = session
                        .Query<Employees_ByPostalCode.Result, Employees_ByPostalCode>()
                        .Where(x => x.PostalCode == -1)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
