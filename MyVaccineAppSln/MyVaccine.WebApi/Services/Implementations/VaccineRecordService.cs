using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class VaccineRecordService : IVaccineRecordService
{
    private readonly IBaseRepository<VaccineRecord> _vaccineRecordRepository;
    private readonly IMapper _mapper;

    public VaccineRecordService(IBaseRepository<VaccineRecord> vaccineRecordRepository, IMapper mapper)
    {
        _vaccineRecordRepository = vaccineRecordRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineRecordResponseDTO>> GetAll()
    {
        var records = await _vaccineRecordRepository
            .GetAll()
            .Include(vr => vr.User)
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<IEnumerable<VaccineRecordResponseDTO>>(records);

        return response;
    }

    public async Task<VaccineRecordResponseDTO> GetById(int id)
    {
        var record = await _vaccineRecordRepository
            .FindByAsNoTracking(vr => vr.Id == id)
            .Include(vr => vr.User)
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .FirstOrDefaultAsync();

        if (record == null)
            throw new KeyNotFoundException($"Registro de vacuna con ID {id} no encontrado.");

        var response = _mapper.Map<VaccineRecordResponseDTO>(record);

        return response;
    }

    public async Task<IEnumerable<VaccineRecordResponseDTO>> GetRecordsByUserId(int userId)
    {
        var recordsByUserId = await _vaccineRecordRepository
            .FindByAsNoTracking(vr => vr.UserId == userId)
            .Include(d => d.User)
            .Include(d => d.Dependent)
            .Include(d => d.Vaccine)
            .ToListAsync();

        if (!recordsByUserId.Any())
            throw new KeyNotFoundException($"No se encontraron registros de vacunas para el usuario con ID {userId}.");

        var response = _mapper.Map<IEnumerable<VaccineRecordResponseDTO>>(recordsByUserId);

        return response;
    }

    public async Task<VaccineRecordResponseDTO> Add(VaccineRecordRequestDTO request)
    {
        var record = new VaccineRecord
        {
            UserId = request.UserId,
            DependentId = request.DependentId,
            VaccineId = request.VaccineId,
            AdministeredDate = request.AdministeredDate,
            AdministeredLocation = request.AdministeredLocation,
            AdministeredBy = request.AdministeredBy
        };

        await _vaccineRecordRepository.Add(record);
        var recordDetailed = await _vaccineRecordRepository
            .FindBy(vr => vr.Id == record.Id)
            .Include(vr => vr.User)
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<VaccineRecordResponseDTO>(recordDetailed);

        return response;
    }

    public async Task<VaccineRecordResponseDTO> Update(int id, VaccineRecordRequestDTO request)
    {
        var record = await _vaccineRecordRepository
            .FindBy(vr => vr.Id == id)
            .Include(vr => vr.User)
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .FirstOrDefaultAsync();

        if (record == null)
            throw new KeyNotFoundException($"Registro de vacuna con ID {id} no encontrado.");

        record.UserId = request.UserId;
        record.DependentId = request.DependentId;
        record.VaccineId = request.VaccineId;
        record.AdministeredDate = request.AdministeredDate;
        record.AdministeredLocation = request.AdministeredLocation;
        record.AdministeredBy = request.AdministeredBy;

        await _vaccineRecordRepository.Update(record);
        var recordDetailed = await _vaccineRecordRepository
            .FindBy(vr => vr.Id == record.Id)
            .Include(vr => vr.User)
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<VaccineRecordResponseDTO>(record);

        return response;
    }

    public async Task<VaccineRecordResponseDTO> Delete(int id)
    {
        var record = await _vaccineRecordRepository
            .FindBy(vr => vr.Id == id)
            .Include(vr => vr.User)
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .FirstOrDefaultAsync();

        if (record == null)
            throw new KeyNotFoundException($"Registro de vacuna con ID {id} no encontrado.");

        await _vaccineRecordRepository.Delete(record);
        var response = _mapper.Map<VaccineRecordResponseDTO>(record);

        return response;
    }
}