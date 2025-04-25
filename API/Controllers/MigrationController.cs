using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/migrations")]
[ApiController]
public class MigrationController : ControllerBase
{
    private readonly MigrationUpdateService _migrationService;

    public MigrationController(MigrationUpdateService migrationService)
    {
        _migrationService = migrationService;
    }

    [HttpPost("apply-to-all")]
    public async Task<IActionResult> ApplyAll()
    {
        var updated = await _migrationService.ApplyMigrationsToAllTenantsAsync();
        return Ok(new { updated });
    }
}