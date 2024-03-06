using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;

namespace Domain.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<RawAddressData, AddressDto>()
                .ForMember(wrap => wrap.Address,
                    opt => opt.MapFrom(cont => cont.Address))
                .ReverseMap();
        }
    }
}
