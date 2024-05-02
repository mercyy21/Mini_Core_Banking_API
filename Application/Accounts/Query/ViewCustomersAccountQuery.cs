using Application.Accounts.Helper;
using AutoMapper;
using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountQuery
{
    public sealed record ViewCustomersAccountQuery(Guid CustomerId) : IRequest<ResponseModel>;
    public sealed class ViewCustomersAccountQueryHandler : IRequestHandler<ViewCustomersAccountQuery, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public ViewCustomersAccountQueryHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseModel> Handle(ViewCustomersAccountQuery query, CancellationToken cancellationToken)
        {
            //View customers account
            AccountHelper accountHelper = new AccountHelper();
            Account account = await _context.Accounts.FirstOrDefaultAsync(x => x.CustomerId == query.CustomerId);
            if (account == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }
            AccountResponseDTO accountDTO = _mapper.Map<AccountResponseDTO>(accountHelper.AccountResponse(account));

            return new ResponseModel { Data = accountDTO, Message = "Customers Account returned successfully", Success = true };
        }
    }
}

