using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VaccineCategoriesController : ControllerBase
{
    private readonly IVaccineCategoryService _vaccineCategoryService;
    private readonly IValidator<VaccineCategoryRequestDTO> _validator;

    public VaccineCategoriesController(IVaccineCategoryService vaccineCategoryService, IValidator<VaccineCategoryRequestDTO> validator)
    {
        _vaccineCategoryService = vaccineCategoryService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _vaccineCategoryService.GetAll();
        return Ok(categories);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var category = await _vaccineCategoryService.GetById(id);
            return Ok(category);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("get-vaccines-by-categoryid/{vaccineCategoryId}")]
    public async Task<IActionResult> GetVaccinesByCategoryId(int vaccineCategoryId)
    {
        try
        {
            var categories = await _vaccineCategoryService.GetVaccinesByCategoryId(vaccineCategoryId);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineCategoryRequestDTO categoryDto)
    {
        var validationResult = await _validator.ValidateAsync(categoryDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var category = await _vaccineCategoryService.Add(categoryDto);

        return Ok(category);
    }

    [HttpPost("register-vaccines")]
    public async Task<IActionResult> RegisterVaccine(VaccineCategory_VaccineRequestDTO request)
    {
        try
        {
            await _vaccineCategoryService.RegisterVaccineAsync(request);
            return Ok(new { message = "Vacuna registrada exitosamente" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineCategoryRequestDTO vaccineCategoryDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(vaccineCategoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var category = await _vaccineCategoryService.Update(id, vaccineCategoryDto);
            return Ok(category);
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
            var category = await _vaccineCategoryService.Delete(id);
            return Ok(category);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
