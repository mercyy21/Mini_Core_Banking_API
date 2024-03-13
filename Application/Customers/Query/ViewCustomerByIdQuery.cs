using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.CustomerQuery
{
    public sealed record ViewCustomerByIdQuery(Guid CustomerId) : IRequest<ResponseModel>;

    public sealed class ViewCustomerByIdQueryHandler : IRequestHandler<ViewCustomerByIdQuery, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public ViewCustomerByIdQueryHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseModel> Handle(ViewCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            //View Customer by ID
            Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id==query.CustomerId);
            if (customer == null)
            {
                return new ResponseModel { Message = "Customer does not exist", Success = false };
            }
            CustomerResponseDTO customerDTO = _mapper.Map<CustomerResponseDTO>(customer);
            return new ResponseModel { Data = customerDTO, Message = "Customer returned successfully", Success = true };
        }
    }
}

