namespace Payroll.Domain.Model
{
    public class Employee
    {
        public EmployeeId Id { get; private set; }
        public FullName Name { get; private set; }
        public Address HomeAddress { get; private set; }
        public decimal Salary { get; private set; }

        public Employee(EmployeeId id, FullName name, Address homeAddress, decimal salary)
        {
            Id = id;
            Name = name;
            HomeAddress = homeAddress;
            Salary = salary;
        }
    }
}

 
