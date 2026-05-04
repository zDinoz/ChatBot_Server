using Microsoft.AspNetCore.Mvc;
using ChatBotServer.DTOs;
using ChatBotServer.Services;

namespace ChatBotServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalHistoriesController : ControllerBase
{
    private readonly IMedicalHistoryService _medicalHistoryService;

    public MedicalHistoriesController(IMedicalHistoryService medicalHistoryService)
    {
        _medicalHistoryService = medicalHistoryService;
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatientId(int patientId)
    {
        var histories = await _medicalHistoryService.GetByPatientIdAsync(patientId);
        return Ok(histories);
    }

    [HttpGet("query")]
    public async Task<IActionResult> Query([FromQuery] string? patientName, [FromQuery] string? phone)
    {
        var histories = await _medicalHistoryService.QueryAsync(patientName, phone);
        return Ok(histories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var history = await _medicalHistoryService.GetByIdAsync(id);
        if (history == null) return NotFound();
        return Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMedicalHistoryDto createDto)
    {
        var created = await _medicalHistoryService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalHistoryDto updateDto)
    {
        var updated = await _medicalHistoryService.UpdateAsync(id, updateDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _medicalHistoryService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
