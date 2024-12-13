using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MediatR;
using SmaHauJenHoaVij.Infrastructure.Data; // For ShopContext
using SmaHauJenHoaVij.Core.Domain.Users;
using SmaHauJenHoaVij.Core.Domain.Users.Couriers; // For IUserService, Customer, Courier, etc.
using SmaHauJenHoaVij.Core.Domain.Users.Events;
using SmaHauJenHoaVij.Infrastructure.Services;
using SmaHauJenHoaVij.Core.Domain.Users.Admins;
using SmaHauJenHoaVij.Core.Domain.Users.Customers; 


namespace SmaHauJenHoaVij.Core.Domain.Users.Services
{
    public class UserService : IUserService
    {
        private readonly ShopContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public UserService(ShopContext context, IHttpContextAccessor httpContextAccessor, IMediator mediator, IEmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
                return false;

            // Establish session
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return false;

            session.SetString("UserId", user.Id.ToString());
            session.SetString("UserType", user.GetType().Name); // e.g., Customer, Courier, Administrator
            session.SetString("UserName", user.Name);

            return true;
        }


        public async Task<User?> FindByPasswordResetToken(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token);
        }
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> FindByIdAsync(Guid userId) // New method
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token);
            if (user == null)
                return false;

            // Update the user's password securely
            user.PasswordHash = CreatePasswordHash(newPassword); // Replace with your password hashing method
            user.PasswordResetToken = null; // Invalidate the token
            user.PasswordResetTokenExpiry = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
public async Task<User> FindOrCreateExternalUserAsync(string email, string name, string providerName, string providerKey)
{
    // Check if a user already exists with this email, regardless of provider
    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    if (existingUser != null)
    {
        // Return the existing user if found
        return existingUser;
    }

    // If no existing user is found, create a new Customer (treating external user as Customer)
    var customer = new Customer
    {
        Name = name,
        Email = email,
        PasswordHash = null, // No password needed for external users
        Phone = null, // Phone is optional, can be set if required
        Provider = providerName,
        ProviderKey = providerKey
    };

    _context.Add(customer);
    await _context.SaveChangesAsync();

    // Store the newly created Customer's UserId in the session
    var session = _httpContextAccessor.HttpContext?.Session;
    if (session != null)
    {
        session.SetString("UserId", customer.Id.ToString());  // Store UserId in session
        session.SetString("UserType", "Customer");  // Set UserType to Customer (not ExternalUser)
        session.SetString("UserName", customer.Name);  // Store User Name
    }

    // Return the Customer (treated as a User)
    return customer;
}

        public async Task<(bool Success, string Message)> ApproveCourier(Guid courierId)
        {
            var courier = await _context.Couriers.FirstOrDefaultAsync(c => c.Id == courierId);
            if (courier == null)
                return (false, "Courier not found");

            courier.IsApproved = true;
            _context.Couriers.Update(courier);
            await _context.SaveChangesAsync();

            // Publish the event to notify about approval (email will be handled here)
            await _mediator.Publish(new CourierApplicationApproved(courier.Email));

            return (true, "Courier approved.");
        }

        public async Task<(bool Success, string Message)> DenyCourier(Guid courierId)
        {
            var courier = await _context.Couriers.FirstOrDefaultAsync(c => c.Id == courierId);
            if (courier == null)
                return (false, "Courier not found");

            _context.Couriers.Remove(courier);
            await _context.SaveChangesAsync();

            // Publish the event to notify about the denial (email will be handled here)
            await _mediator.Publish(new CourierApplicationDeclined(courier.Email));

            return (true, "Courier denied.");
        }

        public Task Logout()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Clear();
            return Task.CompletedTask;
        }

        public async Task<(bool Success, string Message)> Register(string name, string email, string password, string phone)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return (false, "Email already exists");

            var customer = new Customer
            {
                Name = name,
                Email = email,
                PasswordHash = CreatePasswordHash(password),
                Phone = phone
            };

            _context.Add(customer);
            await _context.SaveChangesAsync();

            var userRegisteredEvent = new UserRegistered(name, email);
            await _mediator.Publish(userRegisteredEvent);

            return (true, "User registered successfully as a Customer");
        }


        public async Task SaveAsync(User user)
        {
            // Check if the user already exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
            {
                // If user does not exist, add the new user
                _context.Users.Add(user);
            }
            else
            {
                // If user exists, update their details
                _context.Users.Update(user);
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message)> ApplyToBecomeCourier(Guid customerId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer == null)
                return (false, "Customer not found");

            if (await _context.Couriers.AnyAsync(c => c.Id == customerId))
                return (false, "Customer has already applied to become a Courier");

            var courier = new Courier
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PasswordHash = customer.PasswordHash, // Same login
                IsApproved = false // Pending approval
            };

            _context.Customers.Remove(customer);
            _context.Couriers.Add(courier);
            await _context.SaveChangesAsync();
            return (true, "Applied to become a Courier. Awaiting admin approval.");
        }

        private string CreatePasswordHash(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hash == storedHash;
        }
        public async Task<(bool Success, string Message)> ChangeUserRole(Guid userId, string newRole)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return (false, "User not found");

        if (newRole == "Admin" && !(user is Admin))
        {
            var admin = new Admin
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                PasswordNeedsReset = true
            };

            _context.Users.Remove(user);
            _context.Admins.Add(admin);
        }
        else if (newRole == "Customer" && !(user is Customer))
        {
            var customer = new Customer
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };

            _context.Users.Remove(user);
            _context.Customers.Add(customer);
        }
        else if (newRole == "Courier" && !(user is Courier))
        {
            var courier = new Courier
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                IsApproved = false
            };

            _context.Users.Remove(user);
            _context.Couriers.Add(courier);
        }
        else
        {
            return (false, "Invalid role or user already has this role");
        }

        await _context.SaveChangesAsync();
        return (true, "User role changed successfully");
    }
    }


    



}
