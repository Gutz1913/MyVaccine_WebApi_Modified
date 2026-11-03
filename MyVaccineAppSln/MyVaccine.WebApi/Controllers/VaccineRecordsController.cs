using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VaccineRecordsController : ControllerBase
{
    private readonly IVaccineRecordService _vaccineRecordService;
    private readonly IValidator<VaccineRecordRequestDTO> _validator;

    public VaccineRecordsController(IVaccineRecordService vaccineRecordService, IValidator<VaccineRecordRequestDTO> validator)
    {
        _vaccineRecordService = vaccineRecordService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _vaccineRecordService.GetAll();

        return Ok(records);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var record = await _vaccineRecordService.GetById(id);
            return Ok(record);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("get-records-by-userid/{userId}")]
    public async Task<IActionResult> GetRecordsByUserId(int userId)
    {
        try
        {
            var records = await _vaccineRecordService.GetRecordsByUserId(userId);
            return Ok(records);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineRecordRequestDTO recordDto)
    {
        var validationResult = await _validator.ValidateAsync(recordDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var record = await _vaccineRecordService.Add(recordDto);

        return Ok(record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineRecordRequestDTO recordDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(recordDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var record = await _vaccineRecordService.Update(id, recordDto);
            return Ok(record);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var record = await _vaccineRecordService.Delete(id);
            return Ok(record);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
