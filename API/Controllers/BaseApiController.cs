// Every controller inheriting this controller will have their routes prefixed with api/
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")] //api/ ([controller] is replaced with the first part of our Controller class name so Users)
public class BaseApiController : ControllerBase
{

}
