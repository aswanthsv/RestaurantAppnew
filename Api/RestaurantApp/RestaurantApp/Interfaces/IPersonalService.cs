using RestaurantApp.DTOs;

namespace RestaurantApp.Interfaces
{
    public interface IPersonalService
    {
        Task<IEnumerable<PersonalDto>> GetAllPersonalAsync();
        Task<IEnumerable<PersonalDto>> GetAllPersonalAsync(string? search=null);
        Task<PersonalDto>  GetByIdAsync(int id);

        Task<PersonalDto> CreatePersonalAsync(PersonalDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
