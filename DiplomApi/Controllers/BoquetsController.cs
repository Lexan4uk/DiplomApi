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
        [HttpPost("addBoquet")]
        public async Task<ActionResult<object>> AddBoquet([FromBody] BoquetCompleted newBoquet)
        {
            if (newBoquet == null)
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Данные букета не предоставлены"
                });
            }

            newBoquet.Id = Guid.NewGuid(); // Генерируем новый ID для букета
            _context.BoquetCompleteds.Add(newBoquet);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Букет успешно добавлен",
            });
        }

        [HttpDelete("deleteBoquet/{id}")]
        public async Task<ActionResult<object>> DeleteBoquet(Guid id)
        {
            if (_context.BoquetCompleteds == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Таблица букетов не найдена"
                });
            }

            var boquet = await _context.BoquetCompleteds.FindAsync(id);
            if (boquet == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Букет с указанным ID не найден"
                });
            }

            _context.BoquetCompleteds.Remove(boquet);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Букет успешно удален"
            });
        }
        [HttpPut("updateBoquet/{id}")]
        public async Task<ActionResult<object>> UpdateBoquet(Guid id, [FromBody] BoquetCompleted updatedBoquet)
        {
            if (_context.BoquetCompleteds == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Таблица букетов не найдена"
                });
            }

            var existingBoquet = await _context.BoquetCompleteds.FindAsync(id);
            if (existingBoquet == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Букет с указанным ID не найден"
                });
            }

            // Обновляем данные букета
            existingBoquet.Name = updatedBoquet.Name ?? existingBoquet.Name;
            existingBoquet.Link = updatedBoquet.Link ?? existingBoquet.Link;
            existingBoquet.Price = updatedBoquet.Price ?? existingBoquet.Price;
            existingBoquet.OldPrice = updatedBoquet.OldPrice ?? existingBoquet.OldPrice;
            existingBoquet.Promo = updatedBoquet.Promo ?? existingBoquet.Promo;
            existingBoquet.Cover = updatedBoquet.Cover ?? existingBoquet.Cover;
            existingBoquet.Description = updatedBoquet.Description ?? existingBoquet.Description;
            existingBoquet.Composition = updatedBoquet.Composition ?? existingBoquet.Composition;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    code = 500,
                    message = "Ошибка обновления данных букета"
                });
            }

            return Ok(new
            {
                code = 200,
                message = "Данные букета успешно обновлены",
            });
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
        [HttpGet("getBoquetAdditions")]
        public async Task<ActionResult<IEnumerable<BoquetConstructor>>> GetBoquetAdditions()
        {
            if (_context.BoquetConstructors == null)
            {
                return NotFound();
            }

            var additions = await _context.BoquetConstructors
                                          .Where(b => b.Type == "ribbon" || b.Type == "wrappapper")
                                          .ToListAsync();

            if (!additions.Any())
            {
                return NotFound("No additions with types 'ribbon' or 'wrappapper' found.");
            }

            return Ok(additions);
        }

    }
}
