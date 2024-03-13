using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_Core_Banking_Project.Controllers;
using Domain.Entity;
using Domain.Enums;
using Domain.DTO;
using Moq;
using Application.Accounts.AccountCommand;
using Application.Accounts.AccountQuery;

namespace Mini_Core_Banking_Project.Test
{
    public class AccountControllerTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly AccountController _accountController;
        public AccountControllerTest()
        {
            _mockMediator = new Mock<IMediator>();
            _accountController = new AccountController(_mockMediator.Object);

        }
        List<Account> accounts = new List<Account>
        {
            new Account
            {
                CustomerId= Guid.NewGuid(),
                Id= Guid.NewGuid(),
                AccountType= "1",
                Balance= 0,
                CreatedAt = DateTime.Now,
                Status = "Active"
            }
        };

        [Fact]
        public async Task CreateAccountSuccess()
        {
            //Arrange
            AccountDTO accountDTO = new AccountDTO
            {
                CustomerId= accounts[0].CustomerId,
                AccountType= AccountType.Savings,
            };
            AccountResponseDTO accountResponseDTO = new AccountResponseDTO
            {
                CustomerId = accountDTO.CustomerId,
                AccountType = accountDTO.AccountType,
                Id = accounts[0].Id,
                Status = accounts[0].Status
            };

            ResponseModel response = new ResponseModel
            {
                Data = accountResponseDTO,
                Message = "Account created successfully",
                Success = true
            };
            //Act
            CreateAccountCommand request = new CreateAccountCommand(accountDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.CreateAccount(accountDTO);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert 
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        //View Customers Account
        [Fact]
        public async Task ViewCustomerAccountSuccess()
        {
            //Arrange
            Guid customerId = Guid.NewGuid();

            AccountResponseDTO accountResponseDTO = new AccountResponseDTO
            {
                CustomerId = accounts[0].CustomerId,
                AccountType = AccountType.Savings,
                Id = accounts[0].Id,
                Status= accounts[0].Status
            };
            ResponseModel response = new ResponseModel
            {
                Data = accountResponseDTO,
                Message = "Customers Account returned successfully",
                Success = true
            };
            //Act
            ViewCustomersAccountQuery request = new ViewCustomersAccountQuery(customerId);
            _mockMediator.Setup(x=> x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.ViewCustomersAccount(customerId);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull (result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        //Activate account 
        [Fact]
        public async Task ActivateAccountSuccess()
        {
            //Arrange
            Guid accountId = Guid.NewGuid();

            ResponseModel response = new ResponseModel
            {
                Message= "Account Activated",
                Success = true
            };

            //Act
            ActivateAccountCommand request = new ActivateAccountCommand(accountId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Activate(accountId);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        //Deactivate account
        [Fact]
        public async Task DeactivateAccountSuccessAsync()
        {
            //Arrange
            Guid accountId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Account Deactivated",
                Success = true
            };

            //Act
            DeactivateAccountCommand request = new DeactivateAccountCommand(accountId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deactivate(accountId);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        //Deposit
        [Fact]
        public async void DepositSuccess()
        {
            //Arrange 
            Guid accountId = Guid.NewGuid();
            double amount = 1000;
            ResponseModel response = new ResponseModel
            {
                Message = "Amount successfully deposited",
                Success = true
            };
            //Act
            DepositCommand request = new DepositCommand(accountId,amount);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deposit(accountId, amount);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        //Withdraw 
        [Fact]
        public async Task WithdrawSuccessAsync()
        {
            //Arrange 
            Guid accountId = Guid.NewGuid();
            double amount = 50;
            ResponseModel response = new ResponseModel
            {
                Message = "Amount successfully withdrawn",
                Success = true
            };
            //Act
            WithdrawCommand request = new WithdrawCommand(accountId,amount);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Withdraw(accountId, amount);
            OkObjectResult? resultType = result as OkObjectResult;
            //Assert
            Assert.NotNull (result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        [Fact]
        public async Task DeleteAccountSuccessAsync()
        {
            //Arrange 
            Guid customerId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Account Deleted Successfully",
                Success = true
            };

            //Act
            DeleteAccountCommand request = new DeleteAccountCommand(customerId);
            _mockMediator.Setup(x=> x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.DeleteAccount(customerId);
            OkObjectResult? resultType = result as OkObjectResult;
            ResponseModel actualResult = resultType.Value as ResponseModel;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.Message, actualResult.Message);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        //Failure 
        [Fact]
        public async Task CreateAccount_CustomerAlreadyHasAnAccountAsync()
        {
            //Arrange
            AccountDTO accountDTO = new AccountDTO
            {
                CustomerId = Guid.NewGuid(),
                AccountType = AccountType.Savings,
            };
            ResponseModel response = new ResponseModel
            {
                Message = "Customer already has an account",
                Success = false
            };
            //Act
            CreateAccountCommand request = new CreateAccountCommand(accountDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.CreateAccount(accountDTO);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull (result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        [Fact]
        public async void CreateAccount_CustomerDoesNotExist()
        {
            //Arrange
            AccountDTO accountDTO = new AccountDTO
            {
                CustomerId = Guid.NewGuid(),
                AccountType = AccountType.Savings,
            };
            ResponseModel response = new ResponseModel
            {
                Message = "Customer Does Not Exist",
                Success = false
            };
            //Act
            CreateAccountCommand request = new CreateAccountCommand(accountDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.CreateAccount(accountDTO);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        [Fact]
        public async void ViewCustomerAccount_CustomerAccountDoesNotExist()
        {
            //Arrange
            Guid customerId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Account does not exist",
                Success = false
            };
            //Act
            ViewCustomersAccountQuery request = new ViewCustomersAccountQuery(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.ViewCustomersAccount(customerId);
            NotFoundObjectResult? resultType = result as NotFoundObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        [Fact]
        public async void ActivateAccount_AccountDoesNotExist()
        {
            //Arrange
            Guid accountId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Account Does Not Exist",
                Success = false
            };

            //Act
            ActivateAccountCommand request = new ActivateAccountCommand(accountId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.Activate(accountId);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        
        public async void DeactivateAccount_AccountDoesNotExist()
        {
            //Arrange
            Guid accountId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Account Does Not Exist",
                Success = false
            };
            //Act
            DeactivateAccountCommand request = new DeactivateAccountCommand(accountId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deactivate(accountId);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        [Fact]
        public async void Deposit_AccountDoesNotExist()
        {
            //Arrange 
            Guid accountId = Guid.NewGuid();
            double amount = 1000;
            ResponseModel response = new ResponseModel
            {
                Message = "Account does not exist",
                Success = false
            };
            //Act
            DepositCommand request = new DepositCommand(accountId, amount);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deposit(accountId, amount);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        [Fact]
        public async void Deposit_AmountLessThanZero()
        {
            //Arrange 
            Guid accountId = Guid.NewGuid();
            double amount = -10;
            ResponseModel response = new ResponseModel
            {
                Message = "Amount must be greater than zero",
                Success = false
            };
            //Act
            DepositCommand request = new DepositCommand(accountId, amount);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deposit(accountId, amount);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

        [Fact]
        public async void Withdraw_AccountDoesNotExist()
        {
            //Arrange 
            Guid accountId = Guid.NewGuid();
            double amount = 50;
            ResponseModel response = new ResponseModel
            {
                Message = "Account does not exist",
                Success = false
            };
            //Act
            WithdrawCommand request = new WithdrawCommand(accountId, amount);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Withdraw(accountId, amount);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        [Fact]
        public async void Withdraw_AmountExceedsBalance()
        {
            //Arrange 
            Guid accountId = Guid.NewGuid();
            double amount = 10000;
            ResponseModel response = new ResponseModel
            {
                Message = "Amount exceeds balance",
                Success = false
            };
            //Act
            WithdrawCommand request = new WithdrawCommand(accountId, amount);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.Withdraw(accountId, amount);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }
        //Delete Account Failure
        [Fact]
        public async void DeleteAccount_AccountDoesNotExist()
        {
            //Arrange 
            Guid customerId = Guid.NewGuid();
            ResponseModel response = new ResponseModel
            {
                Message = "Account does not exist",
                Success = false
            };
            //Act
            DeleteAccountCommand request = new DeleteAccountCommand(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.DeleteAccount(customerId);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            ResponseModel actualResult = resultType.Value as ResponseModel;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.Message, actualResult.Message);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

    }
}
