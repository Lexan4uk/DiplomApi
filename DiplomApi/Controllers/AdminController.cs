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
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DiplombdContext _context;

        public AdminController(DiplombdContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] AdminAcc input)
        {
            // Проверяем, существует ли администратор с таким логином и паролем
            var adminAccount = await _context.AdminAccs
                .FirstOrDefaultAsync(a => a.Login == input.Login && a.Password == input.Password);

            if (adminAccount != null)
            {
                // Возвращаем 200 OK, если данные корректны
                return Ok(new
                {
                    code = 200,
                    message = "Администратор найден"
                });
            }

            // Если не найдено, возвращаем 401 Unauthorized
            return Unauthorized(new
            {
                code = 401,
                message = "Неверный логин или пароль"
            });
        }
    }


}
