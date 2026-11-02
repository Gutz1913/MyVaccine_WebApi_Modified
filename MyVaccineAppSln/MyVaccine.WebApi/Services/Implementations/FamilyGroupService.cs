using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class FamilyGroupService : IFamilyGroupService
{
    private readonly IBaseRepository<FamilyGroup> _familyGroupRepository;
    private readonly IMapper _mapper;

    public FamilyGroupService(IBaseRepository<FamilyGroup> familyGroupRepository, IMapper mapper)
    {
        _familyGroupRepository = familyGroupRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FamilyGroupResponseDTO>> GetAll()
    {
        var familyGroups = await _familyGroupRepository
            .GetAll()
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<FamilyGroupResponseDTO>>(familyGroups);

        return response;
    }

    public async Task<FamilyGroupResponseDTO> GetById(int id)
    {
        var familyGroup = await _familyGroupRepository
            .FindByAsNoTracking(fg => fg.Id == id)
            .Include(fg => fg.Users)
            .Include(fg => fg.Dependents)
            .FirstOrDefaultAsync();

        if (familyGroup == null)
            throw new KeyNotFoundException($"Grupo familiar con ID {id} no encontrado.");

        var response = _mapper.Map<FamilyGroupResponseDTO>(familyGroup);

        return response;
    }

    public async Task<FamilyGroupResponseDTO> Add(FamilyGroupRequestDTO request)
    {
        var familyGroup = new FamilyGroup
        {
            Name = request.Name
        };

        await _familyGroupRepository.Add(familyGroup);
        var familyGroupWithUsersAndDependents = await _familyGroupRepository
            .FindBy(fg => fg.Id == familyGroup.Id)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<FamilyGroupResponseDTO>(familyGroupWithUsersAndDependents);

        return response;
    }

    public async Task<FamilyGroupResponseDTO> Update(int id, FamilyGroupRequestDTO request)
    {
        var familyGroup = await _familyGroupRepository
            .FindBy(fg => fg.Id == id)
            .Include(fg => fg.Users)
            .Include(fg => fg.Dependents)
            .FirstOrDefaultAsync();

        if (familyGroup == null)
            throw new KeyNotFoundException($"Grupo familiar con ID {id} no encontrado.");

        familyGroup.Name = request.Name;

        await _familyGroupRepository.Update(familyGroup);
        var response = _mapper.Map<FamilyGroupResponseDTO>(familyGroup);

        return response;
    }

    public async Task<FamilyGroupResponseDTO> Delete(int id)
    {
        var familyGroup = await _familyGroupRepository
            .FindBy(fg => fg.Id == id)
            .Include(fg => fg.Users)
            .Include(fg => fg.Dependents)
            .FirstOrDefaultAsync();

        if (familyGroup == null)
            throw new KeyNotFoundException($"Grupo familiar con ID {id} no encontrado.");

        await _familyGroupRepository.Delete(familyGroup);
        var response = _mapper.Map<FamilyGroupResponseDTO>(familyGroup);

        return response;
    }
}