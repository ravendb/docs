using System;
using NUnit.Framework;
using SharpTestsEx;

namespace Payroll.Domain.Tests
{ 
    [TestFixture]
    public class FullnameTests
    {
        [TestCase("John", "Doe", "John Doe")]
        public void ToStringReturnsGivenNameFollowedBySurname(
            string givenName, 
            string surname, 
            string expected)
        {
            var name = new FullName(givenName, surname);
            name.ToString().Should().Be(expected);
        }

        [Test]
        public void GivenNameCouldNotBeNull()
        {
            Executing.This(() => new FullName(null, "Doe"))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
