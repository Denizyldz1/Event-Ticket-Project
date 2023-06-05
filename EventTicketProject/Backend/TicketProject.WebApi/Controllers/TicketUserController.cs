using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketProject.BusinessLayer.Category.Business;
using TicketProject.BusinessLayer.TicketUserBusiness;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.EntityFramework.TicketFiles;
using TicketProject.DataLayer.EntityFramework.TicketUserFiles;
using TicketProject.DtoLayer.Dtos.CategoryDto;
using TicketProject.DtoLayer.Dtos.TicketUser;

namespace TicketProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketUserController : ControllerBase
    {
        private readonly UserManager<TicketUser> _userManager;
        private readonly RoleManager<TicketUserRole> _roleManager;
        public TicketUserController(UserManager<TicketUser> userManager, RoleManager<TicketUserRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddTicketUserDto addTicketUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (IsUserMailExist(addTicketUserDto.Email))
            {
                return BadRequest("Bu email kullanılmaktadır");
            }
            if (!_userManager.Users.Any())
            {
                await AdminUserCreate();
            }
            TicketUser ticketUser = new()
            {
                Name = addTicketUserDto.Name,
                Surname = addTicketUserDto.Surname,
                Email = addTicketUserDto.Email,
                Address = addTicketUserDto.Address,
                UserName = addTicketUserDto.UserName
            };

            var result = await _userManager.CreateAsync(ticketUser, addTicketUserDto.Password);
            if (result.Succeeded)
            {

                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Kullanıcı silinirken bir hata oluştu.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateTicketUserDto updateTicketUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stringId = updateTicketUserDto.Id.ToString();
            var existingTicketUser = await _userManager.FindByIdAsync(stringId);

            if (IsUserMailExist(updateTicketUserDto.Email, Convert.ToInt32(existingTicketUser.Id)))
            {
                return BadRequest("Bu email kullanılmaktadır");
            }
            var passwordHasher = new PasswordHasher<TicketUser>();

            if (passwordHasher.VerifyHashedPassword(existingTicketUser, existingTicketUser.PasswordHash, updateTicketUserDto.OldPassword) != PasswordVerificationResult.Success)
            {
                return BadRequest("Şifreniz uyuşmuyor.");
            }
            existingTicketUser.Name = updateTicketUserDto.Name;
            existingTicketUser.Surname = updateTicketUserDto.Surname;
            existingTicketUser.Address = updateTicketUserDto.Address;
            existingTicketUser.UserName = existingTicketUser.Email;
            existingTicketUser.Email = existingTicketUser.Email;
            existingTicketUser.PasswordHash = _userManager.PasswordHasher.HashPassword(existingTicketUser, updateTicketUserDto.Password);
            var result = await _userManager.UpdateAsync(existingTicketUser);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest("Güncelleme hatası");

            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }
        private bool IsUserMailExist(string email)
        {
            return _userManager.Users.Any(c => c.Email.ToLower() == email.ToLower());
        }
        private bool IsUserMailExist(string email, int Id)
        {
            return _userManager.Users.Any(c => c.Email.ToLower() == email.ToLower() && c.Id != Id);

        }
        private async Task AdminUserCreate()
        {
            TicketUser ticketUser = new()
            {
                Name = "Admin",
                Surname = "Admin",
                Email = "admin@admin.com",
                Address = "Admin",
                UserName = "admin@admin.com"
            };
            var password = "Admin1234.";

            await _userManager.CreateAsync(ticketUser, password);

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new TicketUserRole("Admin");
                await _roleManager.CreateAsync(adminRole);
            }

            // Admin kullanıcısına "Admin" rolünü ata
            await _userManager.AddToRoleAsync(ticketUser, "Admin");
        }       

    }
}
