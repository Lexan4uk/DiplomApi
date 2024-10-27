using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiplomApi.Models;

namespace DiplomApi.Controllers
{
    [Route("api/boquets")]
    [ApiController]
    public class BoquetsController : ControllerBase
    {
        private readonly DiplombdContext _context;

        public BoquetsController(DiplombdContext context)
        {
            _context = context;
        }

        [HttpGet("getBoquetCompleted")]
        public async Task<ActionResult<IEnumerable<BoquetCompleted>>> GetBoquetCompleted()
        {
            if (_context.BoquetCompleteds == null)
            {
                return NotFound();
            }

            var boquetCompleted = await _context.BoquetCompleteds.ToListAsync();
            return Ok(boquetCompleted);
        }

        // Получение данных из таблицы BoquetConstructor
        [HttpGet("getBoquetConstructor")]
        public async Task<ActionResult<IEnumerable<BoquetConstructor>>> GetBoquetConstructor()
        {
            if (_context.BoquetConstructors == null)
            {
                return NotFound();
            }

            var boquetConstructor = await _context.BoquetConstructors.ToListAsync();
            return Ok(boquetConstructor);
        }
        [HttpGet("getBoquetByLink/{link}")]
        public async Task<ActionResult<BoquetCompleted>> GetBoquetByLink(string link)
        {
            if (_context.BoquetCompleteds == null)
            {
                return NotFound();
            }

            var boquet = await _context.BoquetCompleteds
                                        .FirstOrDefaultAsync(b => b.Link == link);

            if (boquet == null)
            {
                return NotFound();
            }

            return Ok(boquet);
        }
    }
}
