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
        [HttpPost("addAddress")]
        public async Task<ActionResult<object>> AddAddress([FromBody] Address newAddress)
        {
            if (_context.Addresses == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Таблица адресов не найдена"
                });
            }

            if (newAddress == null || string.IsNullOrEmpty(newAddress.Address1) || string.IsNullOrEmpty(newAddress.Name) || string.IsNullOrEmpty(newAddress.Phone))
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Некорректные данные. Убедитесь, что все поля заполнены"
                });
            }

            try
            {
                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    code = 500,
                    message = "Ошибка при добавлении адреса",
                });
            }

            return Ok(new
            {
                code = 200,
                message = "Адрес успешно добавлен",
            });
        }
        [HttpDelete("deleteAddress/{id}")]
        public async Task<ActionResult<object>> DeleteAddress(Guid id)
        {
            if (_context.Addresses == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Таблица адресов не найдена"
                });
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Адрес с указанным ID не найден"
                });
            }

            try
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    code = 500,
                    message = "Ошибка при удалении адреса",
                    error = ex.Message
                });
            }

            return Ok(new
            {
                code = 200,
                message = "Адрес успешно удален"
            });
        }





    }

}
