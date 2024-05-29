using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Application.Enums;
using Application.DTO;
using Moq;
using Application.Accounts.AccountCommand;
using Application.Accounts.AccountQuery;
using Application.Domain.Entity;
using Application.Domain.Enums;

namespace API.Test
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
                AccountType= AccountType.Savings,
                Balance= 0,
                CreatedAt = DateTime.Now,
                Status = Status.Active
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

            Application.ResultType.Result response = Application.ResultType.Result.Success<CreateAccountCommand>("Account created successfully", accountResponseDTO);
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
            Application.ResultType.Result response = Application.ResultType.Result.Success<ViewCustomersAccountQuery>("Customers Account returned successfully", accountResponseDTO);
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

            Application.ResultType.Result response = Application.ResultType.Result.Success<ActivateAccountCommand>("Account Activated");
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
            Application.ResultType.Result response = Application.ResultType.Result.Success<DeactivateAccountCommand>("Account Deactivated");

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
            TransactDTO transactDTO = new TransactDTO
            {
                TransactionDetails = "Ughfol193ejak=",
                Amount = 200
            };
            Application.ResultType.Result response = Application.ResultType.Result.Success<DepositCommand>("Amount successfully deposited");
            //Act
            DepositCommand request = new DepositCommand(transactDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deposit(transactDTO);
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
            TransactDTO transactDTO = new TransactDTO
            {
                TransactionDetails = "Ughfol193ejak=",
                Amount = 50
            };
            Application.ResultType.Result response = Application.ResultType.Result.Success<WithdrawCommand>("Amount successfully withdrawn");
            //Act
            WithdrawCommand request = new WithdrawCommand(transactDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Withdraw(transactDTO);
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
            Application.ResultType.Result response = Application.ResultType.Result.Success<DeleteAccountCommand>("Account Deleted Successfully");

            //Act
            DeleteAccountCommand request = new DeleteAccountCommand(customerId);
            _mockMediator.Setup(x=> x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.DeleteAccount(customerId);
            OkObjectResult? resultType = result as OkObjectResult;
            Application.ResultType.Result actualResult = resultType.Value as Application.ResultType.Result;

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
            Application.ResultType.Result response = Application.ResultType.Result.Failure<CreateAccountCommand>("Customer already has an account");
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
            Application.ResultType.Result response = Application.ResultType.Result.Failure<CreateAccountCommand>("Customer Does Not Exist");
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
            Application.ResultType.Result response = Application.ResultType.Result.Failure<ViewCustomersAccountQuery>("Account does not exist");
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
            Application.ResultType.Result response = Application.ResultType.Result.Failure<ActivateAccountCommand>("Account Does Not Exist");

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
            Application.ResultType.Result response = Application.ResultType.Result.Failure<DeactivateAccountCommand>("Account Does Not Exist");
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
            TransactDTO transactDTO = new TransactDTO
            {
                TransactionDetails = "Pghfsl193ejaw=",
                Amount = 200
            };
            Application.ResultType.Result response = Application.ResultType.Result.Failure<DepositCommand>("Account does not exist");
            //Act
            DepositCommand request = new DepositCommand(transactDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deposit(transactDTO);
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
            TransactDTO transactDTO = new TransactDTO
            {
                TransactionDetails = "Ughfol193ejak=",
                Amount = 0
            };
            Application.ResultType.Result response = Application.ResultType.Result.Failure<DepositCommand>("Amount must be greater than zero");
            //Act
            DepositCommand request = new DepositCommand(transactDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Deposit(transactDTO);
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
            TransactDTO transactDTO = new TransactDTO
            {
                TransactionDetails = "Pghfsl193ejaw=",
                Amount = 200
            };
            Application.ResultType.Result response = Application.ResultType.Result.Failure<WithdrawCommand>("Account does not exist");
            //Act
            WithdrawCommand request = new WithdrawCommand(transactDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result = await _accountController.Withdraw(transactDTO);
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
            TransactDTO transactDTO = new TransactDTO
            {
                TransactionDetails = "Ughfol193ejak=",
                Amount = 200000
            };
            Application.ResultType.Result response = Application.ResultType.Result.Failure<WithdrawCommand>("Amount exceeds balance");
            //Act
            WithdrawCommand request = new WithdrawCommand(transactDTO);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.Withdraw(transactDTO);
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
            Application.ResultType.Result response = Application.ResultType.Result.Failure<DeleteAccountCommand>("Account does not exist");
            //Act
            DeleteAccountCommand request = new DeleteAccountCommand(customerId);
            _mockMediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);
            IActionResult result =await _accountController.DeleteAccount(customerId);
            BadRequestObjectResult? resultType = result as BadRequestObjectResult;
            Application.ResultType.Result actualResult = resultType.Value as Application.ResultType.Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.Message, actualResult.Message);
            Assert.Equal(response.ToString(), resultType.Value.ToString());
        }

    }
}
