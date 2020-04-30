using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project01.Helpers;
using Project01.Services;

namespace Project01.Controllers
{
    [Route("api/team-member/3/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public readonly IDbService _iService;

        public TaskController(IDbService iService)
        {
            _iService = iService;
        }

        [HttpGet]
        public IActionResult GetTasks(string member) 
        {
            var result = _iService.GetTasks(member);
            if (result == null) 
            {
                return BadRequest("Error occurred");
            }
            return Ok(result);
        }
    }
}
