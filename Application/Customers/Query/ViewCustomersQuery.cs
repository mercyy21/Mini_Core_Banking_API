using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.CustomerQuery
{
    public sealed record ViewCustomersQuery() : IRequest<ResponseModel>;

    public sealed class ViewCustomerQueryHandler : IRequestHandler<ViewCustomersQuery, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public ViewCustomerQueryHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel> Handle(ViewCustomersQuery query, CancellationToken cancellationToken)
        {
            //View Customer

            List<Customer> customerList = await _context.Customers.ToListAsync();
            if (customerList == null || !customerList.Any())
            {
                return new ResponseModel { Message = "No customer has been created", Success = false };
            }
            List<CustomerResponseDTO> customerDTO = _mapper.Map<List<CustomerResponseDTO>>(customerList);
            return new ResponseModel { Data = customerDTO, Message = "Customers returned successfully", Success = true };

        }
    }
}

