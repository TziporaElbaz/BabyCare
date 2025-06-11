using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BabyCare.BL.Models;
using BabyCare.DAL.Models;

namespace BabyCare.BL.Services
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
