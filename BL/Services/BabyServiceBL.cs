using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.API;
using WEB_API.DAL.API;
using WEB_API.Models;

namespace BL.Services
{
    public class BabyServiceBL : IBabyServiceBL
    {
        private IBabyManagementDAL babyManagementDAL;
        public BabyServiceBL(IBabyManagementDAL _babyManagement)
        {
            babyManagementDAL = _babyManagement;
        }
        public int BabysCurrentAge(int BabyId)
        {
            Baby baby = babyManagementDAL.GetBabyByIdAsync(BabyId).Result;
            return System.DateTime.Now.Month >= baby.Birthdate.Month ?
            ((System.DateTime.Now.Year - baby.Birthdate.Year) * 12 + System.DateTime.Now.Month - baby.Birthdate.Month) :
            ((System.DateTime.Now.Year - baby.Birthdate.Year) * 12 + 12 - baby.Birthdate.Month + System.DateTime.Now.Month);
        }


    }
}
