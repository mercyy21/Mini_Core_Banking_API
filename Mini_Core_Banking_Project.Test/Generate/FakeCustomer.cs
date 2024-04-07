using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Core_Banking_Project.Test.Generate
{
    public static class FakeCustomer
    {
        public static Customer GenerateCustomer()
        {
            return new Customer
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Address= "Test",
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                PhoneNumber = "Test",
                Status = "Test",
                UpdatedAt= DateTime.Now
            };
        }
    }
}
