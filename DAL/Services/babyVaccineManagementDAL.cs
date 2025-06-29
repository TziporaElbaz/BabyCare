﻿using WEB_API.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WEB_API.DAL.Services
{
    public class BabyVaccineManagementDAL : IBabyVaccineManagementDAL
    {
        private readonly myDatabase _context;

        public BabyVaccineManagementDAL(myDatabase context)
        {
            _context = context;
        }

        public async Task<BabyVaccine> CreateAsync(Baby baby, Vaccine vaccine)
        {
            BabyVaccine babyVaccine = new BabyVaccine(baby, vaccine);
            _context.Set<BabyVaccine>().Add(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;

        }
        //שליפת כל החיסונים של תינוק מסוים
        public async Task<List<Vaccine>> GetVaccinesAsync(string babyId)
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby) 
                                 .Include(bv => bv.Vaccine) 
                                 .Where(bv => bv.Baby.BabyId == babyId) 
                                 .Select(bv => bv.Vaccine)
                                 .ToListAsync(); 
        }
        //שליפת כל החיסונים של התינוקות
        public async Task<IEnumerable<BabyVaccine>> GetAllAsync()
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby)
                                 .Include(bv => bv.Vaccine)
                                 .ToListAsync();
        }
        public async Task<BabyVaccine> UpdateAsync(BabyVaccine babyVaccine)
        {
            _context.Set<BabyVaccine>().Update(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;
        }

        public async Task DeleteAsync(string babyId, string vaccine)
        {
            var babyVaccine = await _context.Set<BabyVaccine>().FirstOrDefaultAsync(bv => bv.Baby.BabyId == babyId && bv.Vaccine.Name == vaccine);
            if (babyVaccine != null)
            {
                _context.Set<BabyVaccine>().Remove(babyVaccine);
                await _context.SaveChangesAsync();
            }
        }
    }
}


