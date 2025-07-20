using AutoMapper;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;


namespace WEB_API.BL.Services
{

    public class BabyManagementBL : IBabyManagementBL
    {
        private readonly IBabyManagementDAL babyManagementDAL;
        private readonly IMapper mapper;
        public BabyManagementBL(IBabyManagementDAL dal, IMapper _mapper)
        {
            babyManagementDAL = dal;
            mapper = _mapper;
        }

        public async Task<BabyDTO?> GetBabyById(string id)
        {
            var baby = await babyManagementDAL.GetBabyByIdAsync(id);
            return mapper.Map<BabyDTO>(baby);
        }

        public async Task AddBaby(BabyDTO babyDto)
        {
            var baby = mapper.Map<Baby>(babyDto);
            if (baby == null)
                throw new ArgumentNullException(nameof(baby), "Baby cannot be null");
            await babyManagementDAL.AddBabyAsync(baby);
        }

        public async Task DeleteBaby(string id)
        {
            var baby = await babyManagementDAL.GetBabyByIdAsync(id);
            if (baby == null)
                throw new KeyNotFoundException("Baby not found");

            await babyManagementDAL.DeleteBabyAsync(baby);
        }

        public async Task<List<BabyDTO>> GetAllBabies()
        {
            var babies = await babyManagementDAL.GetAllBabiesAsync();
            return babies.Select(b => mapper.Map<BabyDTO>(b)).ToList();
        }

        public async Task UpdateBabyDetails(Baby updatedBaby)
        {
            Baby baby = await babyManagementDAL.GetBabyByIdAsync(updatedBaby.BabyId);
            if (baby == null)
                throw new KeyNotFoundException("Baby not found");

            baby.Name = updatedBaby.Name ?? baby.Name;
            baby.Birthdate = updatedBaby.Birthdate != default ? updatedBaby.Birthdate : baby.Birthdate;
            baby.Gender = updatedBaby.Gender;
            baby.Weight = updatedBaby.Weight != default ? updatedBaby.Weight : baby.Weight;
            baby.Height = updatedBaby.Height != default ? updatedBaby.Height : baby.Height;
            baby.HeadCircumference = updatedBaby.HeadCircumference != default ? updatedBaby.HeadCircumference : baby.HeadCircumference;
            baby.BirthWeight = updatedBaby.BirthWeight != default ? updatedBaby.BirthWeight : baby.BirthWeight;
            baby.IsInGrowthCurve = updatedBaby.IsInGrowthCurve;
            baby.MotherName = updatedBaby.MotherName ?? baby.MotherName;
            baby.FatherName = updatedBaby.FatherName ?? baby.FatherName;
            baby.ParentPhone = updatedBaby.ParentPhone ?? baby.ParentPhone;
            baby.ParentEmail = updatedBaby.ParentEmail ?? baby.ParentEmail;
            baby.Address = updatedBaby.Address ?? baby.Address;

            await babyManagementDAL.UpdateBabyDetailsAsync(baby);
        }

        //public async Task<double> GetHeightPercentile(bool gender, int ageMonths, double height)
        //{
        //    string genderString = gender ? "male" : "female";
        //    string url = $"https://growthcalculator.org/api/height_for_age_percentile?sex={genderString}&age={ageMonths}&age_unit=months&height={height}";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.GetAsync(url);
        //        var json = await response.Content.ReadAsStringAsync();
        //        var jObject = JObject.Parse(json);
        //        double percentile = (double)jObject["percentile"];
        //        return percentile;
        //    }
        //}
        //public async Task<double> GetWeightPercentile(bool gender, int ageMonths, double weight)
        //{
        //    string genderString = gender ? "male" : "female";
        //    string url = $"https://growthcalculator.org/api/weight_for_age_percentile?sex={genderString}&age={ageMonths}&age_unit=months&weight={weight}";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.GetAsync(url);
        //        var json = await response.Content.ReadAsStringAsync();
        //        var jObject = JObject.Parse(json);
        //        double percentile = (double)jObject["percentile"];
        //        return percentile;
        //    }
        //}
        public int GetBabysAge(string BabyId)
        {
            Baby? baby = babyManagementDAL.GetBabyByIdAsync(BabyId).Result;
            return DateTime.Now.Month >= baby.Birthdate.Month ?
            ((DateTime.Now.Year - baby.Birthdate.Year) * 12 + DateTime.Now.Month - baby.Birthdate.Month) :
            ((DateTime.Now.Year - baby.Birthdate.Year) * 12 + 12 - baby.Birthdate.Month + DateTime.Now.Month);
        }
    }
}



