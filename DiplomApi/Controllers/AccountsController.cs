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

        [HttpPost("checkEmail")]
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
        [HttpPost("createAccount")]
        public async Task<ActionResult<object>> CreateAccount([FromBody] AccountInput input)
        {
            // Проверяем, существует ли аккаунт с указанным email
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == input.Email);

            if (existingAccount != null)
            {
                // Если аккаунт уже существует, возвращаем 409 с сообщением
                return Conflict(new
                {
                    code = 409,
                    message = "Аккаунт с таким email уже существует"
                });
            }

            // Создаем новый аккаунт
            var newAccount = new Account
            {
                Email = input.Email,
                Password = input.Password // Здесь можно добавить хеширование пароля для безопасности
            };

            // Сохраняем новый аккаунт в базе данных
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            // Возвращаем успешный ответ
            return Ok(new
            {
                code = 200,
                message = "Аккаунт успешно создан"
            });
        }

        [HttpGet("login")]
        public async Task<ActionResult<object>> Login()
        {
            var tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!Guid.TryParse(tokenString, out Guid token))
            {
                return StatusCode(404, new
                {
                    code = 404,
                    message = "Неверный формат токена"
                });
            }

            // Ищем профиль пользователя по токену
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Token == token);

            if (userProfile == null)
            {
                return Ok(new
                {
                    code = 201,
                    message = "Пользовательский профиль не найден"
                });
            }

            // Ищем аккаунт по тому же токену
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Token == token);

            if (account == null)
            {
                return StatusCode(404, new
                {
                    code = 404,
                    message = "Пользовательский профиль не найден"
                });
            }

            return Ok(new
            {
                Name = userProfile.Name,
                PhoneNumber = userProfile.PhoneNumber,
                Email = account.Email
            });
        }

        [HttpPost("editName")]
        public async Task<ActionResult<object>> EditName([FromBody] EditName input)
        {
            // Извлекаем токен из заголовка запроса
            var tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!Guid.TryParse(tokenString, out Guid token))
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Неверный формат токена"
                });
            }

            // Ищем профиль пользователя по токену
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Token == token);

            if (userProfile == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Пользовательский профиль не найден"
                });
            }

            // Обновляем имя пользователя
            userProfile.Name = input.Name;

            // Сохраняем изменения
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Имя успешно обновлено"
            });
        }
        [HttpPost("editPhone")]
        public async Task<ActionResult<object>> EditPhone([FromBody] EditPhone input)
        {
            // Извлекаем токен из заголовка запроса
            var tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!Guid.TryParse(tokenString, out Guid token))
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Неверный формат токена"
                });
            }

            // Ищем профиль пользователя по токену
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Token == token);

            if (userProfile == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Пользовательский профиль не найден"
                });
            }

            // Обновляем номер телефона пользователя
            userProfile.PhoneNumber = input.PhoneNumber;

            // Сохраняем изменения
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Номер телефона успешно обновлен"
            });
        }
        [HttpPost("editPassword")]
        public async Task<ActionResult<object>> EditPassword([FromBody] EditPassword input)
        {
            // Извлекаем токен из заголовка запроса
            var tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!Guid.TryParse(tokenString, out Guid token))
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Неверный формат токена"
                });
            }

            // Ищем аккаунт по токену
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Token == token);

            if (account == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Аккаунт не найден"
                });
            }

            // Проверяем, совпадает ли старый пароль
            if (account.Password != input.OldPassword)
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Неверный текущий пароль"
                });
            }

            // Обновляем пароль аккаунта
            account.Password = input.NewPassword;

            // Сохраняем изменения
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 200,
                message = "Пароль успешно обновлен"
            });
        }







    }

}
