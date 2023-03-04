using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{
    
}