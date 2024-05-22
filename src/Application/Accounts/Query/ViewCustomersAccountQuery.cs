using Application.Accounts.Helper;
using AutoMapper;
using Application.Domain.Entity;
using Application.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.ResultType;

namespace Application.Accounts.AccountQuery
{
    public sealed record ViewCustomersAccountQuery(Guid CustomerId) : IRequest<ResultType.Result>;
    public sealed class ViewCustomersAccountQueryHandler : IRequestHandler<ViewCustomersAccountQuery, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public ViewCustomersAccountQueryHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResultType.Result> Handle(ViewCustomersAccountQuery query, CancellationToken cancellationToken)
        {
            //View customers account
            AccountHelper accountHelper = new AccountHelper();
            Account account = await _context.Accounts.FirstOrDefaultAsync(x => x.CustomerId == query.CustomerId);
            if (account == null)
            {
                return ResultType.Result.Failure<ViewCustomersAccountQuery>("Account does not exist");
            }
            AccountResponseDTO accountDTO = _mapper.Map<AccountResponseDTO>(accountHelper.AccountResponse(account));

            return ResultType.Result.Success<ViewCustomersAccountQuery>("Customers Account returned successfully", accountDTO);
        }
    }
}

