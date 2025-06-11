using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyCare.DAL.API;
using BabyCare.DAL.Services;
using BabyCare.DAL.Models;
using BabyCare.BL.API;

namespace BabyCare.BL.Services
{

    public class BabyManagementBL : IBabyManagementBL
    {
        private readonly IBabyManagementDAL _dal;

        public BabyManagementBL(IBabyManagementDAL dal)
        {
            _dal = dal;
        }

        public async Task<Baby?> GetBabyById(string id)
        {
            return await _dal.GetBabyByIdAsync(id);
        }

        public async Task AddBaby(Baby baby)
        {
            await _dal.AddBabyAsync(baby);
        }

        public async Task DeleteBaby(string id)
        {
            var baby = await _dal.GetBabyByIdAsync(id);
            if (baby == null)
                throw new KeyNotFoundException("Baby not found");

            await _dal.DeleteBabyAsync(baby);
        }

        public async Task<List<Baby>> GetAllBabies()
        {
            return await _dal.GetAllBabiesAsync();
        }

        public async Task UpdateBabyDetails(Baby updatedBaby)
        {
                Baby baby = await _dal.GetBabyByIdAsync(updatedBaby.BabyId);
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

                await _dal.UpdateBabyDetailsAsync(baby);
            }       
    }
}

