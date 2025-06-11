using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.BL.API;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.DAL.Services;


namespace WEB_API.Services
{
 
        public class VaccineManagementBL : IVaccineManagementBL
    {
        IBabyManagementBL BabyManagementBL;
        IVaccineManagementDAL vaccineManagementDAL;
        IBabyVaccineManagementDAL babyVaccineManagementDAL;

        public VaccineManagementBL(IVaccineManagementDAL _vaccineManagementDAL, IBabyVaccineManagementDAL _babyVaccineManagementDAL, IBabyManagementBL _BabyManagementBL)
        {
            vaccineManagementDAL = _vaccineManagementDAL;
            babyVaccineManagementDAL = _babyVaccineManagementDAL;
            BabyManagementBL = _BabyManagementBL;
        }
        public Dictionary<Vaccine, bool> ListOfBabysVaccines(string id)
        {
            Dictionary<Vaccine, bool> vaccines = new Dictionary<Vaccine, bool>();
            List<Vaccine> allVaccines = ListOfBabysUnvaccinatedVaccines(id);
            List<Vaccine> allBabysVaccines = babyVaccineManagementDAL.GetVaccinesAsync(id).Result;
            foreach (var vaccine in allBabysVaccines)
            {
                vaccines[vaccine] = true;
            }
            foreach (var vaccine in allVaccines)
            {

                vaccines[vaccine] = false;
            }
            vaccines = vaccines
               .OrderBy(v => v.Key.MinAgeMonths) // Sort by MinAgeMonths
               .ThenBy(v => v.Key.MaxAgeMonths) // Then by MaxAgeMonths
               .ToDictionary(v => v.Key, v => v.Value);
            return vaccines;
        }
        public List<Vaccine> ListOfBabysUnvaccinatedVaccines(string babyId)
        {

            List<Vaccine> allVaccines = vaccineManagementDAL.GetAllVaccinesAsync().Result;
            List<Vaccine> allBabysVaccines = babyVaccineManagementDAL.GetVaccinesAsync(babyId).Result;
            foreach (var vaccine in allVaccines)
            {
                if (allBabysVaccines.FirstOrDefault(v => v.Name == vaccine.Name) == null)
                    allVaccines.Remove(vaccine);
            }
            return allVaccines;
        }
        public List<Vaccine> ShowUpcomingVaccines(string babyId)
        {
            int babyAge = BabyManagementBL.BabysCurrentAge(babyId);
            List<Vaccine> babysUnvaccinatedVaccines = ListOfBabysUnvaccinatedVaccines(babyId);
            foreach (Vaccine vaccine in babysUnvaccinatedVaccines)
            {
                if (vaccine.MinAgeMonths > (babyAge + 3))

                    babysUnvaccinatedVaccines.Remove(vaccine);
            }

            return babysUnvaccinatedVaccines;
        }
    }

}



