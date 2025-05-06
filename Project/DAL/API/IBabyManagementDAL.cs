 using global::Project.models;
 using Project.models;

    namespace Project.DAL.API
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
