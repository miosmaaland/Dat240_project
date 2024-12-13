using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Infrastructure.Services;
using System;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Users.Customers;
using SmaHauJenHoaVij.Core.Domain.Users;

namespace SmaHauJenHoaVij.Core.Domain.Users.Pipelines
{
    public class UpdateCustomerProfilePipeline
    {
        private readonly ShopContext _context;

        public UpdateCustomerProfilePipeline(ShopContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(Guid customerId, string name, string phone)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return (false, "Customer not found.");
            }

            customer.Name = name;
            customer.Phone = phone;

            await _context.SaveChangesAsync();

            return (true, "Customer profile updated successfully.");
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }
    }
}
