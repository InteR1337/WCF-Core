using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GeoLib.Contracts;
using GeoLib.Data;

namespace GeoLib.Services
{
    public static class MyMapperConfiguration
    {
        private static readonly IMapper _mapper;

        static MyMapperConfiguration()
        {
            var config = new MapperConfiguration(x => 
                x.CreateMap<ZipCode, ZipCodeData>()
                .ForMember(y => y.ZipCode, z => z.MapFrom(src => src.Zip))
                .ForMember(y => y.City, z => z.MapFrom(src => src.City))
                .ForMember(y => y.State, z => z.MapFrom(src => src.State.Abbreviation)));
            _mapper = config.CreateMapper();
        }

        public static IMapper GetMapper()
        {
            return _mapper;
        }
    }
}
