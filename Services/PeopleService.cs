using Microsoft.EntityFrameworkCore;
using RecordWebMVC.Data;
using RecordWebMVC.Models;
using RecordWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecordWebMVC.Services
{
    public class PeopleService
    {
        private readonly RecordWebMVCContext _context;

        public PeopleService(RecordWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> FindAllAsync()
        {
            return await _context.Person.ToListAsync();

        }
        
        public async Task InsertAsync(Person obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Person> FindByIdAsync(int id)
        {
            return  await _context.Person.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Person.FindAsync(id);
            _context.Person.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person obj)
        {
            bool hasAny = await _context.Person.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
          
        }
    }
}
