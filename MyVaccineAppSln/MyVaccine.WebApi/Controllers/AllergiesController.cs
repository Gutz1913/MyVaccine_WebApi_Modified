using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AllergiesController : ControllerBase
{
    private readonly IAllergyService _allergyService;
    private readonly IValidator<AllergyRequestDTO> _validator;

    public AllergiesController(IAllergyService allergyService, IValidator<AllergyRequestDTO> validator)
    {
        _allergyService = allergyService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allergies = await _allergyService.GetAll();

        return Ok(allergies);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var allergy = await _allergyService.GetById(id);
            return Ok(allergy);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(AllergyRequestDTO allergyDto)
    {
        var validationResult = await _validator.ValidateAsync(allergyDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var allergy = await _allergyService.Add(allergyDto);

        return Ok(allergy);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AllergyRequestDTO allergyDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(allergyDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var allergy = await _allergyService.Update(id, allergyDto);
            return Ok(allergy);
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
            var allergy = await _allergyService.Delete(id);
            return Ok(allergy);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}