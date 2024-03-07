using AutoMapper;
using Domain.Models;
using WebAPI.Models;

namespace WebAPI.AutoMapper
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<RawAddressData, AddressDto>().ReverseMap();
        }
    }
}
