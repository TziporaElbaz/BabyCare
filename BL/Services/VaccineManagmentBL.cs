using AutoMapper;
using AutoMapper.Internal;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.Services
{
    public class VaccineManagementBL : IVaccineManagementBL
    {
        IBabyManagementBL BabyManagementBL;
        IVaccineManagementDAL vaccineManagementDAL;
        IBabyVaccineManagementDAL babyVaccineManagementDAL;
        IMapper mapper;
        public VaccineManagementBL(IVaccineManagementDAL _vaccineManagementDAL, IBabyVaccineManagementDAL _babyVaccineManagementDAL, IBabyManagementBL _BabyManagementBL, IMapper _mapper)
        {
            vaccineManagementDAL = _vaccineManagementDAL;
            babyVaccineManagementDAL = _babyVaccineManagementDAL;
            BabyManagementBL = _BabyManagementBL;
            mapper = _mapper;
        }

        public async Task<Dictionary<string, string>> ListOfBabysVaccines(string babyId)
        {
            Dictionary<string, string> vaccines = new Dictionary<string, string>();
            List<Vaccine> allVaccines = await vaccineManagementDAL.GetAllVaccinesAsync();
            List<Vaccine> allBabysVaccines = await babyVaccineManagementDAL.GetVaccinesAsync(babyId);

            foreach (var vaccine in allVaccines)
            {
                var babyVaccine = allBabysVaccines.FirstOrDefault(bv => bv.Id == vaccine.Id);
                var specificBabyVaccine = await babyVaccineManagementDAL.GetBabyVaccineAsync(babyId, vaccine.Id);
                if (babyVaccine != null && specificBabyVaccine.DateGiven <= DateOnly.FromDateTime(DateTime.Now))
                {
                    vaccines[vaccine.Name] = "given";
                }
                else if (babyVaccine != null && specificBabyVaccine.DateGiven > DateOnly.FromDateTime(DateTime.Now))
                {
                    vaccines[vaccine.Name] = "upcoming";
                }
                else
                {
                    vaccines[vaccine.Name] = "not given";
                }
            }

            var sortedVaccines = allVaccines.Concat(allBabysVaccines)
                .GroupBy(v => v.Name)
                .Select(g => g.First())
                .OrderBy(v => v.MinAgeMonths)
                .ThenBy(v => v.MaxAgeMonths)
                .ToList();

            var sortedVaccineDictionary = sortedVaccines.ToDictionary(v => v.Name, v => vaccines[v.Name]);

            return sortedVaccineDictionary;
        }

        public async Task<List<VaccineDTO>?> ListOfBabysUnvaccinatedVaccines(string babyId)
        {
            List<Vaccine> allVaccines = await vaccineManagementDAL.GetAllVaccinesAsync();
            List<Vaccine> allBabysVaccines = await babyVaccineManagementDAL.GetVaccinesAsync(babyId);

            if (allBabysVaccines != null)
            {
                List<VaccineDTO> unvaccinatedVaccines = new List<VaccineDTO>();

                foreach (var vaccine in allVaccines)
                {
                    if (allBabysVaccines.FirstOrDefault(v => v.Name.Equals(vaccine.Name)) == null)
                    {
                        var vaccineDTO = mapper.Map<VaccineDTO>(vaccine);
                        unvaccinatedVaccines.Add(vaccineDTO);
                    }
                }

                return unvaccinatedVaccines;
            }
            return null;
        }

        public async Task<List<VaccineDTO>> ShowUpcomingVaccines(string babyId)
        {
            int babyAge = BabyManagementBL.GetBabysAge(babyId);
            List<VaccineDTO> babysUnvaccinatedVaccines = await ListOfBabysUnvaccinatedVaccines(babyId);
            foreach (VaccineDTO vaccine in babysUnvaccinatedVaccines)
            {
                if (vaccine.MinAgeMonths > (babyAge + 3) || vaccine.MinAgeMonths < babyAge)
                    babysUnvaccinatedVaccines.Remove(vaccine);
            }

            return babysUnvaccinatedVaccines;
        }
    }

}



