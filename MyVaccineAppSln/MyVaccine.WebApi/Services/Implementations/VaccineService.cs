using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class VaccineService : IVaccineService
{
    private readonly IBaseRepository<Vaccine> _vaccineRepository;
    private readonly IMapper _mapper;

    public VaccineService(IBaseRepository<Vaccine> vaccineRepository, IMapper mapper)
    {
        _vaccineRepository = vaccineRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineResponseDTO>> GetAll()
    {
        var vaccines = await _vaccineRepository
            .GetAll()
            .Include(v => v.VaccineCategory_Vaccines)
                .ThenInclude(vcv => vcv.VaccineCategory)
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<VaccineResponseDTO>>(vaccines);

        return response;
    }

    public async Task<VaccineResponseDTO> GetById(int id)
    {
        var vaccine = await _vaccineRepository
            .FindByAsNoTracking(v => v.Id == id)
            .Include(v => v.VaccineCategory_Vaccines)
            .ThenInclude(vcv => vcv.VaccineCategory)
            .FirstOrDefaultAsync();

        if (vaccine == null)
            throw new KeyNotFoundException($"Vacuna con ID {id} no encontrada.");

        var response = _mapper.Map<VaccineResponseDTO>(vaccine);

        return response;
    }

    public async Task<VaccineResponseDTO> Add(VaccineRequestDTO request)
    {
        var vaccine = new Vaccine()
        {
            Name = request.Name,
            RequiresBooster = request.RequiresBooster
        };

        await _vaccineRepository.Add(vaccine);
        var response = _mapper.Map<VaccineResponseDTO>(vaccine);

        return response;
    }

    public async Task<VaccineResponseDTO> Update(int id, VaccineRequestDTO request)
    {
        var vaccine = await _vaccineRepository
            .FindBy(v => v.Id == id)
            .FirstOrDefaultAsync();

        if (vaccine == null)
            throw new KeyNotFoundException($"Vacuna con ID {id} no encontrada.");

        vaccine.Name = request.Name;
        vaccine.RequiresBooster = request.RequiresBooster;

        await _vaccineRepository.Update(vaccine);
        var response = _mapper.Map<VaccineResponseDTO>(vaccine);

        return response;
    }

    public async Task<VaccineResponseDTO> Delete(int id)
    {
        var vaccine = await _vaccineRepository
            .FindBy(v => v.Id == id)
            .FirstOrDefaultAsync();

        if (vaccine == null)
            throw new KeyNotFoundException($"Vacuna con ID {id} no encontrada.");

        await _vaccineRepository.Delete(vaccine);
        var response = _mapper.Map<VaccineResponseDTO>(vaccine);

        return response;
    }
}
