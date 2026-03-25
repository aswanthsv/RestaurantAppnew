using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using RestaurantApp.Models;

namespace RestaurantApp.Services
{
    public class PersonalService: IPersonalService
    {
        private readonly AppDbContext _context;
        public PersonalService(AppDbContext dbContext)
        {
          _context = dbContext;
        }
        public async Task<IEnumerable<PersonalDto>> GetAllPersonalAsync()
        {
            var personal = await _context.Personal.ToListAsync();
            return personal.Select(c=> new PersonalDto
            {
                Id=c.Id,
                Name=c.Name,
                Number=c.Number,
                Place=c.Place,
            }
            );
        }

        public async Task<IEnumerable<PersonalDto>> GetAllPersonalAsync(string? search=null)
        {
            var query = _context.Personal.AsQueryable();

            if(!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name.Contains(search) || x.Place.Contains(search));
            }
            query = query.OrderBy(x => x.Name);
            var result = await query.ToListAsync();

            return result.Select(d => new PersonalDto
            {
                Id=d.Id,
                Name=d.Name,
                Number=d.Number,
                Place=d.Place
            });
        }
        public async Task<PersonalDto> GetByIdAsync(int id)
        {
            var person = await _context.Personal.FindAsync(id);

            if (person == null) return null;

            return new PersonalDto
            {
                Id = person.Id,
                Name = person.Name,
                Number = person.Number,
                Place = person.Place
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = await _context.Personal.FindAsync(id);
            if (person == null) return false;

            _context.Personal.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PersonalDto> CreatePersonalAsync(PersonalDto dto)
        {
            try
            {
                var entity = new Personal
                {
                    Name = dto.Name,
                    Number = dto.Number,
                    Place = dto.Place,
                };

                _context.Personal.Add(entity);
                await _context.SaveChangesAsync();

                dto.Id = entity.Id;
                return dto;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
