using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAzureSQL.Models;

namespace WebApiAzureSQL.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        // GET: api/Driver
        [HttpGet]
        public List<DriverModel> Get()
        {
            return new DriverModel().GetAll();
        }

        // GET: api/Driver/5
        [HttpGet("{id}", Name = "Get")]
        public DriverModel Get(int id)
        {
            return new DriverModel().Get(id);
        }

        // POST: api/Driver
        [HttpPost]
        public ApiResponse Post([FromBody] DriverModel driver)
        {
            return driver.Insert();
        }

        // PUT: api/Driver/5
        [HttpPut("{id}")]
        public ApiResponse Put(int id, [FromBody] DriverModel driver)
        {
            return driver.Update(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            return new DriverModel().Delete(id);
        }
    }
}
