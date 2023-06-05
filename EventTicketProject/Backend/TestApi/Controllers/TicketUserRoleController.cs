using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketProject.DataLayer.Concrete;

namespace TicketProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketUserRoleController : ControllerBase
    {
        private readonly RoleManager<TicketUserRole> _roleManager;
        private readonly UserManager<TicketUser> _userManager;

        public TicketUserRoleController(RoleManager<TicketUserRole> roleManager, UserManager<TicketUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(string roleName) 
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest("Yetkili bulunamadı");
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            return Ok(usersInRole);
        }
        [HttpPost]

        public async Task<IActionResult> AssignRoleToUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Kullanıcı bulunamadı
                return BadRequest("Kullanıcı bulunamadı.");
            }
            var roleName = "Yetkili";
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                // Yetkili rolü bulunamadı, rolü oluştur
                var newRole = new TicketUserRole { Name = roleName };
                var createRoleResult = await _roleManager.CreateAsync(newRole);

                if (!createRoleResult.Succeeded)
                {
                    // Rol oluşturma başarısız oldu
                    return BadRequest("Rol oluşturulamadı.");
                }

                role = newRole;
            }
            // Kullanıcıya rol ataması yap
            var assignRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!assignRoleResult.Succeeded)
            {
                // Rol ataması başarısız oldu
                return BadRequest("Rol ataması yapılamadı.");
            }

            return Ok("Yetkili rolü başarıyla kullanıcıya atandı.");
        }
        [HttpGet("GetUsersWithRole")]
        public async Task<IActionResult> GetUsersWithRole()
        {
            // Yetkili rolünü bul
            var roleName = "Yetkili";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                // Yetkili rolü bulunamadı
                return NotFound("Yetkili rolü bulunamadı.");
            }

            // Roldeki kullanıcıları getir
            var usersWithRole = await _userManager.GetUsersInRoleAsync(role.Name);

            return Ok(usersWithRole);
        }
    }
}
