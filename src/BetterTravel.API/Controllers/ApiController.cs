using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace BetterTravel.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    public abstract class ApiController : ControllerBase
    {
        
    }
}