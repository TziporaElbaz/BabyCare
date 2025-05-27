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
using WEB_API.DAL.API;
using WEB_API.Models;
using static System.Net.WebRequestMethods;

namespace BL.Services


{
    public class BabyManagementBL : IBabyServiceBL
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
 
            public double GetHeightPercentile(bool gender, int ageMounths, double height)
        {
            string genderString = gender ? "male" : "female";

            string url = $"https://growthcalculator.org/api/height_for_age_percentile?sex={genderString}&age={ageMounths}&age_unit=months&height={height}";
            return ;
        }
        public double GetPercentile(bool gender, int ageMounths, double weight)
        { string genderString = gender ? "male" : "female";
          string url = $"https://growthcalculator.org/api/weight_for_age_percentile?sex={genderString}&age={ageMounths}&age_unit=months&weight={weight}";
            return;

        }
    }
}