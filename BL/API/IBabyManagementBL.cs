using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyCare.DAL.Models;

namespace BabyCare.BL.API
{
    public interface IBabyManagementBL
    {
        Task<Baby?> GetBabyById(string id);
        Task AddBaby(Baby baby);
        Task DeleteBaby(string id);
        Task<List<Baby>> GetAllBabies();
        Task UpdateBabyDetails(Baby updatedBaby);
    }
}
