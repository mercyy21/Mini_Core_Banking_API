using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Application.Enums;
using Application.DTO;
using Moq;
using Application.Customers.CustomerCommand;
using Application.Customers.CustomerQuery;
using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.ResultType;

namespace API.Test
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
                Status = Status.Active
            };
            var responses = new
            {
                CustomerResponse = customerResponse,
                AccountResponseDTO = accountResponseDTO
            };
            Result response = Result.Success("Customer created successfully",responses);

            //Act
            CreateCustomerCommand request = new CreateCustomerCommand(customerRequest);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _customerController.CreateCustomer(customerRequest);
            OkObjectResult? resultType = result as OkObjectResult;
            Result? actualResponse = resultType.Value as Result;
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
            Result response = Application.ResultType.Result.Success<ViewCustomersQuery>("Customers returned successfully");
            //Act
            ViewCustomersQuery request = new ViewCustomersQuery();
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
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
            Result response = Result.Success<UpdateCustomerCommand>("Customer Updated Successfully");
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
            Result response = Result.Success<ViewCustomerByIdQuery>("Customer returned successfully", customer);
            //Act 
            ViewCustomerByIdQuery request = new ViewCustomerByIdQuery(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
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
            Result response = Result.Success<DeleteCustomerCommand>("Deleted Successfully");

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
            Result response = Result.Failure<CreateCustomerCommand>("Customer already exists");
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
            Result response = Result.Failure<UpdateCustomerCommand>("Customer Does Not Exist");

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
            Result response = Result.Failure<ViewCustomersQuery>("No customer has been created");
            //Act
            ViewCustomersQuery request = new ViewCustomersQuery();
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
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

            Result response = Result.Failure<ViewCustomerByIdQuery>("Customer Does Not Exist");
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

            Result response = Application.ResultType.Result.Failure<DeleteCustomerCommand>("Customer Does Not Exist");
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