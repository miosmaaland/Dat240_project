using SmaHauJenHoaVij.Core.Domain.Users;
using SmaHauJenHoaVij.Core.Domain.Users.Admins;
using SmaHauJenHoaVij.Core.Domain.Users.Couriers;
using SmaHauJenHoaVij.Core.Domain.Users.Customers;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Users.Services;

namespace SmaHauJenHoaVij.Pages.AdminPages
{
    public class AllUsersModel : PageModel
    {
        private readonly ShopContext _context;
        private readonly IUserService _userService;

        public AllUsersModel(ShopContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public List<UserViewModel> Users { get; set; }
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            // Load all users into memory
            var users = await _context.Users.ToListAsync();

            // Map to UserViewModel and determine roles in-memory
            Users = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = GetRole(u) // Determine role in-memory
            }).ToList();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync(Guid userId, string newRole)
        {
            var result = await _userService.ChangeUserRole(userId, newRole);
            Message = result.Message;
            await OnGetAsync(); // Refresh the user list
            return Page();
        }

        private string GetRole(User user)
        {
            if (user is Admin) return "Administrator";
            if (user is Courier) return "Courier";
            if (user is Customer) return "Customer";
            return "Unknown";
        }
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
