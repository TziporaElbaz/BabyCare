using AutoMapper;
using WEB_API.BL.Models;
using WEB_API.DAL.Models;


namespace WEB_API.BL.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AvailableAppointmentDTO, AvailableAppointment>().ReverseMap();
            CreateMap<BabyDTO, Baby>().ReverseMap();
            CreateMap<AppointmentDTO, Appointment>().ReverseMap();
            CreateMap<VaccineDTO, Vaccine>().ReverseMap();
            CreateMap<WorkerDTO, Worker>().ReverseMap();
            CreateMap<ShiftDTO, Shift>().ReverseMap();
        }
    }
}
