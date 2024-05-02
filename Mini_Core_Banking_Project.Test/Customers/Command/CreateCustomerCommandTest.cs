using Application.Accounts.AccountCommand;
using Application.Customers.CustomerCommand;
using AutoMapper;
using Domain.DTO;
using Domain.Enums;
using Infrastructure.DBContext;
using MediatR;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;

namespace Mini_Core_Banking_Project.Test.Customers.Command
{
    public class CreateCustomerCommandTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IMiniCoreBankingDbContext> _contextMock;

        public CreateCustomerCommandTest()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _contextMock = new Mock<IMiniCoreBankingDbContext>();
        }
        [Fact]
        public async Task CreateCustomerSuccessAsync()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            CreateCustomerDTO CreateCustomerDTO = new CreateCustomerDTO { 
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
                AccountType = CreateCustomerDTO.AccountType
            };
            AccountResponseDTO accountResponseDTO = new AccountResponseDTO
            {
                CustomerId = newAccount.CustomerId,
                AccountType = newAccount.AccountType,
                Id = Guid.NewGuid(),
                Status = "Active"
            };

            ResponseModel response = new ResponseModel
            {
                Data = accountResponseDTO,
                Message = "Account created successfully",
                Success = true
            };
            //Act
            var request = new CreateCustomerCommand(CreateCustomerDTO);
            _mediator.Setup(x => x.Send(It.IsAny<CreateAccountCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var handler = new CreateCustomerCommandHandler(_contextMock.Object, _mapper.Object, _mediator.Object);
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
            var handler = new CreateCustomerCommandHandler(_contextMock.Object, _mapper.Object, _mediator.Object);
            var result = await handler.Handle(request, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Customer already exists", result.Message.ToString());

        }
    }
}
