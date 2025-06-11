using BabyCare.DAL.Models;

namespace BabyCare.DAL.API

{
    public interface IBabyManagementDAL
    {
        Task<Baby?> GetBabyByIdAsync(string id);
        Task AddBabyAsync(Baby baby);
        Task DeleteBabyAsync(Baby baby);
        Task<List<Baby>> GetAllBabiesAsync();
        Task UpdateBabyDetailsAsync(Baby updatedBaby);
    }
}
