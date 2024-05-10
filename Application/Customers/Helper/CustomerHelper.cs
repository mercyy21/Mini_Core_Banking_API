using Application.Domain.Entity;
using Application.DTO;

namespace Application.Customers.Helper
{
    internal class CustomerHelper
    {

        public Customer GetCustomerResponse(CreateCustomerDTO customerDto, Guid id)
        {
            return new Customer
            {
                Id = id,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                Address = customerDto.Address,
                PhoneNumber = customerDto.PhoneNumber,
            };
        }
    }
}
