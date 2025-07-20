using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace WEB_API.BL.API
{
    public interface IBabyManagementBL
    {
        Task<BabyDTO?> GetBabyById(string id);
        Task AddBaby(BabyDTO baby);
        Task DeleteBaby(string id);
        Task<List<BabyDTO>> GetAllBabies();
        Task UpdateBabyDetails(Baby updatedBaby);
        int GetBabysAge(string BabyId);
        //Task<double> GetHeightPercentile(bool gender, int ageMonths, double height);
        //Task<double> GetWeightPercentile(bool gender, int ageMonths, double weight);
    }
}