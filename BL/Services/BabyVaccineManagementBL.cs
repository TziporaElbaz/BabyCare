using AutoMapper;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.DAL.Services;

namespace WEB_API.BL.Services
{
    public class BabyVaccineManagementBL : IBabyVaccineManagementBL
    {

        private readonly IBabyVaccineManagementDAL _babyVaccineManagementDAL;
        private readonly IBabyManagementDAL _babyManagementDAL;
        private readonly IVaccineManagementDAL _vaccineManagementDAL;
        private readonly IMapper _mapper;

        public BabyVaccineManagementBL(
            IBabyVaccineManagementDAL babyVaccineManagementDAL,
            IBabyManagementDAL babyManagementDAL,
            IVaccineManagementDAL vaccineManagementDAL,
            IMapper mapper)
        {
            _babyVaccineManagementDAL = babyVaccineManagementDAL;
            _babyManagementDAL = babyManagementDAL;
            _vaccineManagementDAL = vaccineManagementDAL;
            _mapper = mapper;
        }

        public async Task<BabyVaccine> CreateBabyVaccineAsync(string babyId, int vaccineId)
        {
            var baby = await _babyManagementDAL.GetBabyByIdAsync(babyId);
            var vaccine = await _vaccineManagementDAL.GetVaccineByIdAsync(vaccineId);
            if (baby == null || vaccine == null)
            {
                throw new ArgumentException("תינוק או חיסון לא נמצא");
            }
            return await _babyVaccineManagementDAL.AddBabyVaccineAsync(baby, vaccine, DateOnly.FromDateTime(DateTime.Now));
        }

        public async Task<List<VaccineDTO>> GetVaccinesByBabyIdAsync(string babyId)
        {
            var vaccines = await _babyVaccineManagementDAL.GetVaccinesAsync(babyId);
            return vaccines.Select(a => _mapper.Map<VaccineDTO>(a)).ToList();
        }

        public async Task<IEnumerable<BabyVaccine>> GetAllBabyVaccinesAsync()
        {
            return await _babyVaccineManagementDAL.GetAllVaccinesAsync();
        }

        public async Task<BabyVaccine> UpdateBabyVaccineAsync(BabyVaccine babyVaccine)
        {
            return await _babyVaccineManagementDAL.UpdateVaccineAsync(babyVaccine);
        }

        public async Task DeleteBabyVaccineAsync(string babyId, string vaccine)
        {
            await _babyVaccineManagementDAL.DeleteVaccineAsync(babyId, vaccine);
        }
    }
}
