using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.DAL.Models;

namespace WEB_API.BL.API
{
    public interface IBabyManagementBL
    {
        Task<Baby?> GetBabyById(string id);
        Task AddBaby(Baby baby);
        Task DeleteBaby(string id);
        Task<List<Baby>> GetAllBabies();
        Task UpdateBabyDetails(Baby updatedBaby);
        int BabysCurrentAge(string BabyId);
        Task<double> GetHeightPercentile(bool gender, int ageMonths, double height);
        Task<double> GetWeightPercentile(bool gender, int ageMonths, double weight);
    }
}