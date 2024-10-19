using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiplomApi.Models;
using static DiplomApi.Models.DiplombdContext;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DiplomApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly DiplombdContext _context;

        public AddressesController(DiplombdContext context)
        {
            _context = context;
        }
        [HttpGet("getAddresses")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }

            var addresses = await _context.Addresses.ToListAsync();
            return Ok(addresses);
        }



    }

}
