using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetYouApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetYouApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")] // Rota da api http://localhost:5000/api/values
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
            // return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
