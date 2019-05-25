using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SoundMastery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "DateOfJoining"))
            {
                DateTime date = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "DateOfJoining").Value);
                Console.WriteLine(date); // claims are accessible via current user context.
            }

            return new[] { "value1", "value2", "value3", "value4", "value5" };
        }
    }
}
