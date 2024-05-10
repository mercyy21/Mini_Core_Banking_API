using AutoMapper;
using Application.DTO;
using Application.Domain.Entity;

namespace Application.AutoMapperConfig
{
    /* public interface IAutoMapperConfig<T>
     {
         void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
     }*/
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerResponseDTO>();
            CreateMap<Account, AccountResponseDTO>();
        }
    }

}
