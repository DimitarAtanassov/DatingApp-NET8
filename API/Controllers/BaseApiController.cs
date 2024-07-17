using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] //api/users ([controller] is replaced with the first part of our Controller class name so Users)
public class BaseApiController : ControllerBase
{

}
