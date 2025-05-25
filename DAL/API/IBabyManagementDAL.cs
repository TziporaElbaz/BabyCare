

using WEB_API.Models;

namespace WEB_API.DAL.API
    {
        public interface IBabyManagementDAL
        {
            Task AddBabyAsync(Baby baby);
            Task DeleteBabyAsync(int id);
            Task<Baby?> GetBabyByIdAsync(int id);
            Task<List<Baby>> GetAllBabiesAsync();
            Task UpdateBabyDetailsAsync(Baby updatedBaby);
        }
    }
