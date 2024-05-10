using AutoMapper;
using Application.Domain.Entity;
using Application.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.ResultType;

namespace Application.Customers.CustomerQuery
{
    public sealed record ViewCustomerByIdQuery(Guid CustomerId) : IRequest<ResultType.Result>;

    public sealed class ViewCustomerByIdQueryHandler : IRequestHandler<ViewCustomerByIdQuery, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public ViewCustomerByIdQueryHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ViewCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            //View Customer by ID
            Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id==query.CustomerId);
            if (customer == null)
            {
                return Result.Failure<ViewCustomerByIdQuery>( "Customer does not exist");
            }
            CustomerResponseDTO customerDTO = _mapper.Map<CustomerResponseDTO>(customer);
            return Result.Success<ViewCustomerByIdQuery>( "Customer returned successfully",customerDTO);
        }
    }
}

