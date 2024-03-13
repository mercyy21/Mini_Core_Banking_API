using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_Core_Banking_Project.Controllers;
using Domain.Entity;
using Domain.Enums;
using Domain.DTO;
using Moq;
using Application.Customers.CustomerCommand;
using Application.Customers.CustomerQuery;

namespace Mini_Core_Banking_Project.Test
{
    public class CustomerControllerTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly CustomerController _customerController;
        public CustomerControllerTest()
        {
            _mockMediator = new Mock<IMediator>();
            _customerController = new CustomerController(_mockMediator.Object);

        }
        //Mock Database
        readonly List<Customer> customers = new List<Customer>
            {
                new Customer
                {
                    Id=Guid.NewGuid(),
                    FirstName = "Mercy",
                    LastName = "Awopetu",
                    Email = "rober@gmail.com",
                    Address = "dolphin estate",
                    PhoneNumber = "091283298336"
                }
            };
        //Success
        [Fact]
        public async Task CreateCustomerSuccessAsync()
        {
            //Arrange
            CreateCustomerDTO customerRequest = new CreateCustomerDTO
            {
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName,
                AccountType = AccountType.Savings
            };
            CustomerResponseDTO customerResponse = new CustomerResponseDTO
            {
                Id = customers[0].Id,
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName
            };
            AccountResponseDTO accountResponseDTO = new AccountResponseDTO
            {
                CustomerId = customerResponse.Id,
                AccountType = customerRequest.AccountType,
                Id = Guid.NewGuid(),
                Status = "Active"
            };
            MultipleDataResponseModel response = new MultipleDataResponseModel
            {
                Data = new List<object> { customerResponse, accountResponseDTO },
                Message = "Customer created successfully",
                Success = true
            };

            //Act
            CreateCustomerCommand request = new CreateCustomerCommand(customerRequest);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.CreateCustomer(customerRequest);
            OkObjectResult? resultType = result as OkObjectResult;
            MultipleDataResponseModel? actualResponse = resultType.Value as MultipleDataResponseModel;
            //Assert
            //Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.Message, actualResponse.Message);
            Assert.Equal(response.ToString(), resultType.Value.ToString());

        }

        [Fact]
        public async void ViewAllCustomersSuccess()
        {
            //Arrange
            CustomerResponseDTO customer = new CustomerResponseDTO
            {
                Id = customers[0].Id,
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName
            };
            ResponseModel response = new ResponseModel
            {
                Data = customer,
                Message = "Customers created successfully",
                Success = true
            };
            //Act
            ViewCustomersQuery request = new ViewCustomersQuery();
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(new ResponseModel { Data = customer, Message = "Customers created successfully", Success = true });
            IActionResult result = await _customerController.ViewCustomers();
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());

        }
        [Fact]
        public async void UpdateCustomerSuccess()
        {
            //Arrange 
            Guid id = Guid.NewGuid();
            CustomerDTO customerDTO = new CustomerDTO
            {
                FirstName = "Tiwa",
                PhoneNumber = "098765432"
            };
            ResponseModel response = new ResponseModel
            {
                Message = "Customer Updated Successfully",
                Success = true
            };

            //Act
            UpdateCustomerCommand request = new UpdateCustomerCommand(id, customerDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.UpdateCustomer(id, customerDTO);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }


        [Fact]
        public async void ViewCusomterByIdSuccess()
        {
            //Arrange
            Guid customerId = Guid.NewGuid();
            CustomerResponseDTO customer = new CustomerResponseDTO
            {
                Id = customers[0].Id,
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName
            };
            ResponseModel response = new ResponseModel
            {
                Data = customer,
                Message = "Customer returned successfully",
                Success = true
            };
            //Act 
            ViewCustomerByIdQuery request = new ViewCustomerByIdQuery(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(new ResponseModel { Data = customer, Message = "Customer returned successfully", Success = true });
            IActionResult result = await _customerController.ViewCustomerById(customerId);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        [Fact]
        public async void DeleteCustomer()
        {
            //Arrange
            Guid customerId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Deleted Successfully",
                Success = true
            };

            //Act
            DeleteCustomerCommand request = new DeleteCustomerCommand(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.DeleteCustomer(customerId);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());


        }


        //Failure
        [Fact]
        public async void CreateCustomer_CustomerAlreadyExists()
        {
            //Arrange
            CreateCustomerDTO customerRequest = new CreateCustomerDTO
            {
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName,
                AccountType = AccountType.Savings
            };
            MultipleDataResponseModel response = new MultipleDataResponseModel
            {
                Message = "Customer already exists",
                Success = false
            };

            //Act
            CreateCustomerCommand request = new CreateCustomerCommand(customerRequest);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.CreateCustomer(customerRequest);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        [Fact]
        public async void UpdateCustomer_CustomerDoesNotExist()
        {
            //Arrange 
            Guid id = Guid.NewGuid();
            CustomerDTO customerDTO = new CustomerDTO
            {
                FirstName = "Tiwa",
                PhoneNumber = "098765432"
            };
            ResponseModel response = new ResponseModel
            {
                Message = "Customer Does Not Exist",
                Success = false
            };

            //Act
            UpdateCustomerCommand request = new UpdateCustomerCommand(id, customerDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.UpdateCustomer(id, customerDTO);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        [Fact]
        public async void CheckIfViewAllCustomersIsEmpty()
        {
            //Arrange
            List<CustomerResponseDTO> customer = new List<CustomerResponseDTO> { };
            ResponseModel response = new ResponseModel
            {
                Message = "No customer has been created",
                Success = true
            };
            //Act
            ViewCustomersQuery request = new ViewCustomersQuery();
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(new ResponseModel { Message = "No customer has been created", Success = true });
            IActionResult result = await _customerController.ViewCustomers();
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        [Fact]
        public async void ViewCustomerById_CustomerDoesNotExist()
        {
            //Arrange
            Guid customerId = Guid.NewGuid();
            CustomerResponseDTO customer = new CustomerResponseDTO
            {
                Id = customerId,
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName
            };

            ResponseModel response = new ResponseModel
            {
                Message = "Customer Does Not Exist",
                Success = false
            };
            //Act
            ViewCustomerByIdQuery request = new ViewCustomerByIdQuery(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.ViewCustomerById(customerId);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteCustomer_CustomerDoesNotExist()
        {
            //Arrange
            Guid customerId = Guid.NewGuid();
            CustomerResponseDTO customer = new CustomerResponseDTO
            {
                Id = customers[0].Id,
                FirstName = customers[0].FirstName,
                LastName = customers[0].LastName,
                Email = customers[0].Email,
                Address = customers[0].Address,
                PhoneNumber = customers[0].FirstName
            };

            ResponseModel response = new ResponseModel
            {
                Message = "Customer Does Not Exist",
                Success = false
            };
            //Act
            DeleteCustomerCommand request = new DeleteCustomerCommand(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await  _customerController.DeleteCustomer(customerId);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
            Assert.IsType<BadRequestObjectResult>(result);

        }


    }
}