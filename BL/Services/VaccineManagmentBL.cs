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

      

        public async Task<Dictionary<string, bool>> ListOfBabysVaccines(string id)
        {
            Dictionary<string, bool> vaccines = new Dictionary<string, bool>();
            List<Vaccine> allVaccines = await ListOfBabysUnvaccinatedVaccines(id);
            List<Vaccine> allBabysVaccines = await babyVaccineManagementDAL.GetVaccinesAsync(id);
            foreach (var vaccine in allBabysVaccines)
            {
                vaccines[vaccine.Name] = true;
            }
            foreach (var vaccine in allVaccines)
            {

                vaccines[vaccine.Name] = false;
            }
            var sortedVaccines = allVaccines.Concat(allBabysVaccines)
           .GroupBy(v => v.Name)
           .Select(g => g.First()) // Get the first instance to avoid duplicates
           .OrderBy(v => v.MinAgeMonths) // Sort by StartDate
           .ThenBy(v => v.MaxAgeMonths) // Then by EndDate
           .ToList();

            // Create a new dictionary for sorted results
            var sortedVaccineDictionary = sortedVaccines.ToDictionary(v => v.Name, v => vaccines[v.Name]);

            return sortedVaccineDictionary;
        
        }
        public async Task<List<Vaccine>?> ListOfBabysUnvaccinatedVaccines(string babyId)
        {
            List<Vaccine> allVaccines = await vaccineManagementDAL.GetAllVaccinesAsync();
            List<Vaccine> allBabysVaccines = await babyVaccineManagementDAL.GetVaccinesAsync(babyId);

            if (allBabysVaccines != null)
            {
                // יצירת רשימה חדשה לחיסונים שלא ניתנים
                List<Vaccine> unvaccinatedVaccines = new List<Vaccine>();

                foreach (var vaccine in allVaccines)
                {
                    if (allBabysVaccines.FirstOrDefault(v => v.Name.Equals(vaccine.Name)) == null)
                    {
                        unvaccinatedVaccines.Add(vaccine);
                    }
                }
                return unvaccinatedVaccines;
            }
            return null;
        }



        public async Task<List<Vaccine>> ShowUpcomingVaccines(string babyId)
        {
            int babyAge = BabyManagementBL.BabysCurrentAge(babyId);
            List<Vaccine> babysUnvaccinatedVaccines = await ListOfBabysUnvaccinatedVaccines(babyId);
            foreach (Vaccine vaccine in babysUnvaccinatedVaccines)
            {
                if (vaccine.MinAgeMonths > (babyAge + 3))

                    babysUnvaccinatedVaccines.Remove(vaccine);
            }

            return babysUnvaccinatedVaccines;
        }
    }

}



