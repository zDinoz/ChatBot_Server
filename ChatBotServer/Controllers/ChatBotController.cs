using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using ChatBotServer.DTOs;
using ChatBotServer.Services;

namespace ChatBotServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatBotController : ControllerBase
{
    private readonly IPatientService _patientService;

    public ChatBotController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("message")]
    public async Task<IActionResult> ReceiveMessage([FromBody] ChatMessageRequest request)
    {
        var (name, phone) = ParseMessage(request.Message);
        
        if (string.IsNullOrWhiteSpace(phone))
        {
            return BadRequest(new { error = "Phone number not found in message" });
        }

        var createDto = new CreatePatientDto
        {
            Name = name ?? "Unknown",
            Phone = phone,
            Email = ""
        };

        var created = await _patientService.CreateAsync(createDto);
        
        return Ok(new 
        { 
            id = created.Id,
            name = created.Name,
            phone = created.Phone,
            message = "Patient registered successfully" 
        });
    }

    private (string? name, string? phone) ParseMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return (null, null);

        var phonePattern = @"(0\d{9,10})";
        var phoneMatch = Regex.Match(message, phonePattern);
        var phone = phoneMatch.Success ? phoneMatch.Value : null;

        var name = message.Replace(phoneMatch.Value, "").Trim();
        name = Regex.Replace(name, @"^(name|hi|hello|register)\s*", "", RegexOptions.IgnoreCase).Trim();
        name = Regex.Replace(name, @"\s+", " ");

        return string.IsNullOrWhiteSpace(name) ? (null, phone) : (name, phone);
    }
}

public class ChatMessageRequest
{
    public string Message { get; set; } = string.Empty;
}