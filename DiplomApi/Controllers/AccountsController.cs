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
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DiplombdContext _context;

        public AccountsController(DiplombdContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> CheckAccount([FromBody] EmailInput input)
        {
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == input.Email);

            if (existingAccount != null)
            {
                return Ok(new
                {
                    code = 200,
                    message = "Аккаунт существует"
                });
            }

            return StatusCode(201, new
            {
                code = 201,
                message = "Аккаунта не существует"
            });
        }

        [HttpPost("getToken")]
        public async Task<ActionResult<object>> GetToken([FromBody] AccountInput input)
        {
            // Проверяем, существует ли аккаунт с указанным номером телефона
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == input.Email);

            if (existingAccount.Password != input.Password) 
            {
                // Если пароль неверный, возвращаем 401 с сообщением
                return Unauthorized(new
                {
                    code = 401,
                    message = "Неверный пароль"
                });
            }

            // Если аккаунт существует и пароль верный, возвращаем токен
            return Ok(new
            {
                token = existingAccount.Token
            });
        }
        [HttpGet("userProfile")]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            var userProfiles = await _context.UserProfiles
                .Select(up => new UserProfile
                {
                    Name = up.Name
                })
                .ToListAsync();

            return Ok(userProfiles);
        }
    }

}
