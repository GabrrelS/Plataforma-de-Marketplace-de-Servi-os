using Microsoft.AspNetCore.Mvc;

namespace PlataformaServicos.Controllers;

[ApiController]
[Route("api/v1/version")]
public class VersionController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public VersionController(
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            version = _configuration["AppVersion"] ?? "0.1.0-dev",
            environment = _environment.EnvironmentName,
            buildDate = DateTime.UtcNow
        });
    }
}