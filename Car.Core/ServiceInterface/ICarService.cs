using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Car.Core.Dto;

namespace Car.Core.ServiceInterface;

public interface ICarService
{
    Task<CarDto> AddAsync(CarCreateUpdateDto dto);
    Task<CarDto> UpdateAsync(Guid id, CarCreateUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);

    Task<CarDto?> GetAsync(Guid id);
    Task<List<CarDto>> GetAllAsync();
}

