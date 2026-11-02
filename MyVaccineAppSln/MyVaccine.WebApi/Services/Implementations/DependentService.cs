using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class DependentService : IDependentService
{
    private readonly IBaseRepository<Dependent> _dependentRepository;
    private readonly IBaseRepository<Allergy> _allergyRepository;
    private readonly IBaseRepository<DependentAllergy> _dependentAllergyRepository;
    private readonly IMapper _mapper;

    public DependentService(IBaseRepository<Dependent> dependentRepository, IBaseRepository<Allergy> allergyRepository, IBaseRepository<DependentAllergy> dependentAllergyRepository, IMapper mapper)
    {
        _dependentRepository = dependentRepository;
        _allergyRepository = allergyRepository;
        _dependentAllergyRepository = dependentAllergyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DependentResponseDTO>> GetAll()
    {
        var dependents = await _dependentRepository
            .GetAll()
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .Include(d => d.DependentAllergies)
            .ThenInclude(da => da.Allergy)
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<DependentResponseDTO>>(dependents);

        return response;
    }

    public async Task<DependentResponseDTO> GetById(int id)
    {
        var dependent = await _dependentRepository
            .FindByAsNoTracking(d => d.Id == id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .Include(d => d.DependentAllergies)
            .ThenInclude(da => da.Allergy)
            .FirstOrDefaultAsync();

        if (dependent == null)
            throw new KeyNotFoundException($"Dependiente con ID: {id} no encontrado.");

        var response = _mapper.Map<DependentResponseDTO>(dependent);

        return response;
    }

    public async Task<IEnumerable<DependentResponseDTO>> GetDependentsByUserId(int userId)
    {
        var dependents = await _dependentRepository
            .FindByAsNoTracking(d => d.UserId == userId)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .Include(d => d.DependentAllergies)
            .ThenInclude(da => da.Allergy)
            .ToListAsync();

        if (!dependents.Any())
            throw new KeyNotFoundException($"No se encontraron dependientes para el usuario con ID: {userId}.");

        var response = _mapper.Map<IEnumerable<DependentResponseDTO>>(dependents);

        return response;
    }

    public async Task<DependentResponseDTO> Add(DependentRequestDTO request)
    {
        var dependent = new Dependent
        {
            Name = request.Name,
            BirthDate = request.BirthDate,
            UserId = request.UserId,
            FamilyGroupId = request.FamilyGroupId
        };

        await _dependentRepository.Add(dependent);
        var dependentWithUserAndFamilyGroup = await _dependentRepository
            .FindBy(d => d.Id == dependent.Id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<DependentResponseDTO>(dependentWithUserAndFamilyGroup);

        return response;
    }

    public async Task<DependentResponseDTO> Update(int id, DependentRequestDTO request)
    {
        var dependent = await _dependentRepository
            .FindBy(d => d.Id == id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .Include(d => d.DependentAllergies)
            .ThenInclude(da => da.Allergy)
            .FirstOrDefaultAsync();

        if (dependent == null)
            throw new KeyNotFoundException($"Dependiente con ID: {id} no encontrado.");

        dependent.Name = request.Name;
        dependent.BirthDate = request.BirthDate;
        dependent.UserId = request.UserId;
        dependent.FamilyGroupId = request.FamilyGroupId;

        await _dependentRepository.Update(dependent);
        var dependentWithUserAndFamilyGroup = await _dependentRepository
            .FindBy(d => d.Id == dependent.Id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<DependentResponseDTO>(dependent);

        return response;
    }

    public async Task<DependentResponseDTO> Delete(int id)
    {
        var dependent = await _dependentRepository
            .FindBy(d => d.Id == id)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .Include(d => d.DependentAllergies)
            .ThenInclude(da => da.Allergy)
            .FirstOrDefaultAsync();

        if (dependent == null)
            throw new KeyNotFoundException($"Dependiente con ID: {id} no encontrado.");

        await _dependentRepository.Delete(dependent);
        var response = _mapper.Map<DependentResponseDTO>(dependent);

        return response;
    }

    public async Task RegisterAllergyAsync(DependentAllergyRequestDTO request)
    {
        var dependent = await _dependentRepository
            .FindBy(d => d.Id == request.DependentId)
            .Include(d => d.User)
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync();

        if (dependent == null)
            throw new KeyNotFoundException($"Dependiente con ID: {request.DependentId} no encontrado.");

        var allergy = await _allergyRepository
            .FindBy(a => a.Id == request.AllergyId)
            .FirstOrDefaultAsync();

        if (allergy == null)
            throw new KeyNotFoundException($"Alergia con ID: {request.AllergyId} no encontrada.");

        var dependentAllergy = new DependentAllergy()
        {
            DependentId = request.DependentId,
            AllergyId = request.AllergyId,
            RegisteredAt = DateTime.UtcNow
        };

        await _dependentAllergyRepository.Add(dependentAllergy);
        _mapper.Map<DependentAllergyResponseDTO>(dependentAllergy);
    }
}