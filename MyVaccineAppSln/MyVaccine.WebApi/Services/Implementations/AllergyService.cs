using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class AllergyService : IAllergyService
{
    private readonly IBaseRepository<Allergy> _allergyRepository;
    private readonly IMapper _mapper;
    public AllergyService(IBaseRepository<Allergy> allergyRepository, IMapper mapper)
    {
        _allergyRepository = allergyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AllergyResponseDTO>> GetAll()
    {
        var allergies = await _allergyRepository
            .GetAll()
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<AllergyResponseDTO>>(allergies);

        return response;
    }

    public async Task<AllergyResponseDTO> GetById(int id)
    {
        var allergy = await _allergyRepository
            .FindByAsNoTracking(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (allergy == null)
            throw new KeyNotFoundException($"Allergia con ID {id} no encontrada.");

        var response = _mapper.Map<AllergyResponseDTO>(allergy);

        return response;
    }

    public async Task<AllergyResponseDTO> Add(AllergyRequestDTO request)
    {
        var allergy = new Allergy()
        {
            Name = request.Name
        };

        await _allergyRepository.Add(allergy);
        var response = _mapper.Map<AllergyResponseDTO>(allergy);

        return response;
    }

    public async Task<AllergyResponseDTO> Update(int id, AllergyRequestDTO request)
    {
        var allergy = await _allergyRepository
            .FindBy(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (allergy == null)
            throw new KeyNotFoundException($"Allergia con ID {id} no encontrada.");

        allergy.Name = request.Name;

        await _allergyRepository.Update(allergy);
        var response = _mapper.Map<AllergyResponseDTO>(allergy);

        return response;
    }

    public async Task<AllergyResponseDTO> Delete(int id)
    {
        var allergy = await _allergyRepository
            .FindBy(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (allergy == null)
            throw new KeyNotFoundException($"Allergia con ID {id} no encontrada.");

        await _allergyRepository.Delete(allergy);
        var response = _mapper.Map<AllergyResponseDTO>(allergy);

        return response;
    }
}