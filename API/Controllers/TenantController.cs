﻿using API.Data.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/tenants")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly TenantService _tenantService;

    public TenantController(TenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterTenant([FromBody] TenantRegistrationRequest request)
    {
        Data.Models.Central.Tenant tenant = await _tenantService.RegisterTenantAsync(request);
        return Ok(new { tenant.Id, tenant.Slug });
    }
}
