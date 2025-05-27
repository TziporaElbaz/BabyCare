using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using BL.API;
using DAL.API;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WEB_API.DAL.API;
using WEB_API.Models;
using static System.Net.WebRequestMethods;

namespace BL.Services


{
    public class BabyManagementBL :IBabyManagementBL
    {
      
        private IBabyManagementDAL babyManagementDAL;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
       
        public BabyManagementBL(IBabyManagementDAL _babyManagement, IConfiguration _configuration, IMapper _mapper)
        {
            babyManagementDAL = _babyManagement;
            this.configuration = _configuration;
            mapper = _mapper;
       
        }
        public int BabysCurrentAge(string BabyId)
        {
            Baby baby = babyManagementDAL.GetBabyByIdAsync(BabyId).Result;
            return System.DateTime.Now.Month >= baby.Birthdate.Month ?
            ((System.DateTime.Now.Year - baby.Birthdate.Year) * 12 + System.DateTime.Now.Month - baby.Birthdate.Month) :
            ((System.DateTime.Now.Year - baby.Birthdate.Year) * 12 + 12 - baby.Birthdate.Month + System.DateTime.Now.Month);
        }

        public async Task<double> GetWeightPercentile(bool gender, int ageMonths, double weight)
        {
            string genderString = gender ? "male" : "female";
            string url = $"https://growthcalculator.org/api/weight_for_age_percentile?sex={genderString}&age={ageMonths}&age_unit=months&weight={weight}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);
                double percentile = (double)jObject["percentile"];
                return percentile;
            }
        }
        public async Task<double> GetHeightPercentile(bool gender, int ageMonths, double height)
        {
            string genderString = gender ? "male" : "female";
            string url = $"https://growthcalculator.org/api/height_for_age_percentile?sex={genderString}&age={ageMonths}&age_unit=months&height={height}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);
                double percentile = (double)jObject["percentile"];
                return percentile;
            }
        }
    }
}