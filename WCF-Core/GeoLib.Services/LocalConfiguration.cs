using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace GeoLib.Services
{
    public static class LocalConfiguration
    {
        public static IMapper Mapper
        {
            get { return MyMapperConfiguration.GetMapper(); }
        }
    }
}