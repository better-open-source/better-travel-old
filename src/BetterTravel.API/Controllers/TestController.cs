using BetterTravel.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetterTravel.API.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    public class TestController : ApiController
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService) => 
            _testService = testService;

        /// <summary>
        /// Just test endpoint
        /// </summary>
        /// <response code="200">Hello world text.</response>
        [HttpGet("test", Name = nameof(GetTest))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetTest()
        {
            _testService.RunTestAsync();
            return new OkObjectResult("Hello World!");
        }
    }
}