using AutoMapper;
using MediatR;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Castle.Core.Logging;
using Application.Interfaces;
using Application.DTO;
using Application.Enums;
using Application.Domain.Enums;
using Application.Customers.CustomerCommand;
using Application.Accounts.AccountCommand;

namespace API.Test.Customers.Command
{
    public class CreateCustomerCommandTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
        private readonly Mock<IHasher> _mockHasher;
        private readonly Mock<ILogger> _loggerMock;


        public CreateCustomerCommandTest()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _contextMock = new Mock<IMiniCoreBankingDbContext>();
            _mockHasher = new Mock<IHasher>();
            _loggerMock = new Mock<ILogger>();
        }
        [Fact]
        public async Task CreateCustomerSuccessAsync()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            CreateCustomerDTO createCustomerDTO = new CreateCustomerDTO { 
            FirstName="Mercy",
            LastName="Johnson",
            Email="cynthia2019@gmail.com",
            PhoneNumber="0912786534",
            Address="21A, foster street",
            Password="mercy12345",
            AccountType=AccountType.Savings};

            //AccountDTO
            AccountDTO newAccount = new AccountDTO
            {
                CustomerId = Guid.NewGuid(),
                AccountType = createCustomerDTO.AccountType
            };
            AccountResponseDTO accountResponseDTO = new AccountResponseDTO
            {
                CustomerId = newAccount.CustomerId,
                AccountType = newAccount.AccountType,
                Id = Guid.NewGuid(),
                Status =Status.Active
            };

            Application.ResultType.Result response = Application.ResultType.Result.Success<CreateAccountCommand>("Account created successfully", accountResponseDTO);
            //Act
            var request = new CreateCustomerCommand(createCustomerDTO);
            _mediator.Setup(x => x.Send(It.IsAny<CreateAccountCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var handler = new CreateCustomerCommandHandler(_contextMock.Object, _mapper.Object, _mediator.Object,_mockHasher.Object);
            var result = await handler.Handle(request, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Customer created successfully",result.Message.ToString());
        }

        [Fact]
        public async Task CreateCustomer_CustomerAlreadyExists()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            CreateCustomerDTO createCustomer = new CreateCustomerDTO
            {
                FirstName = "Mercy",
                LastName = "Johnson",
                Email = "cyxsa20@gmail.com",
                PhoneNumber = "0912786534",
                Address = "21A, foster street",
                Password = "mercy12345",
                AccountType = AccountType.Savings
            };
           
            //Act
            var request = new CreateCustomerCommand(createCustomer);
            var handler = new CreateCustomerCommandHandler(_contextMock.Object, _mapper.Object, _mediator.Object,_mockHasher.Object);
            var result = await handler.Handle(request, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Customer already exists", result.Message.ToString());

        }
    }
}
