using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.Models;
using WEB_API.DAL.Models;
using WEB_API.Models;

namespace WEB_API.BL.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AvailableAppointmentBL, AvailableAppointment>().ReverseMap();
            CreateMap<BabyBL, Baby>().ReverseMap();
        }
    }
}
