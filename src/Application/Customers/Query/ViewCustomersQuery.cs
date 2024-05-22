using AutoMapper;
using Application.Domain.Entity;
using Application.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.Domain.Enums;
using Application.ResultType;

namespace Application.Customers.CustomerQuery
{
    public sealed record ViewCustomersQuery() : IRequest<Result>;

    public sealed class ViewCustomerQueryHandler : IRequestHandler<ViewCustomersQuery, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public ViewCustomerQueryHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(ViewCustomersQuery query, CancellationToken cancellationToken)
        {
            //View Customer

            List<Customer> customerList = await _context.Customers.ToListAsync();
            if (customerList == null || !customerList.Any())
            {
                return Result.Failure<ViewCustomersQuery>("No customer has been created");
            }
            var customerTransactions =customerList
                .Where(o => o.Status == Status.Active || o.Status==Status.Inactive)
                .ToList();
            List<CustomerResponseDTO> customerDTO = _mapper.Map<List<CustomerResponseDTO>>(customerList);
            return Result.Success<ViewCustomersQuery>("Customers returned successfully", customerDTO);

        }
    }
}

