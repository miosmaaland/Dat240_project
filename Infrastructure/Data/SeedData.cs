using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Cart;
using SmaHauJenHoaVij.Core.Domain.Fulfillment;
using SmaHauJenHoaVij.Core.Domain.Invoicing;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using SmaHauJenHoaVij.Core.Domain.Products;
using SmaHauJenHoaVij.Core.Domain.Users.Customers;
using SmaHauJenHoaVij.Core.Domain.Users.Couriers;

namespace SmaHauJenHoaVij.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();

            // Seed Customers
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer
                    {
                        Id = Guid.NewGuid(),
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        PasswordHash = CreatePasswordHash("Customer123")
                    },
                    new Customer
                    {
                        Id = Guid.NewGuid(),
                        Name = "Jane Smith",
                        Email = "jane.smith@example.com",
                        PasswordHash = CreatePasswordHash("Customer123")
                    }
                };

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            // Seed Couriers
            if (!context.Couriers.Any())
            {
                var couriers = new List<Courier>
                {
                    new Courier
                    {
                        Id = Guid.NewGuid(),
                        Name = "Courier A",
                        Email = "courier1@example.com",
                        PasswordHash = CreatePasswordHash("Courier123")
                    },
                    new Courier
                    {
                        Id = Guid.NewGuid(),
                        Name = "Courier B",
                        Email = "courier2@example.com",
                        PasswordHash = CreatePasswordHash("Courier123")
                    }
                };

                context.Couriers.AddRange(couriers);
                context.SaveChanges();
            }

            // Seed Products
            if (!context.FoodItems.Any())
            {
                var foodItems = new List<FoodItem>
                {
                    new FoodItem("Burger", "A delicious burger", 130m) { Picture = Picture.FromPath("SmaHauJenHoaVij/wwwroot/images/fooditems/burger.jpg") },
                    new FoodItem("Pizza", "A cheesy pizza", 220m) { Picture = Picture.FromPath("SmaHauJenHoaVij/wwwroot/images/fooditems/Pizza.jpg") },
                    new FoodItem("Kebab", "A tasty kebab", 120m) { Picture = Picture.FromPath("SmaHauJenHoaVij/wwwroot/images/fooditems/Kebab.jpg") },
                    new FoodItem("Ramen", "Yummy Time", 100m) { Picture = Picture.FromPath("SmaHauJenHoaVij/wwwroot/images/fooditems/Ramen.jpg") },
                    new FoodItem("Sushi", "Sushi Time", 150m) { Picture = Picture.FromPath("SmaHauJenHoaVij/wwwroot/images/fooditems/Sushi.jpg") }
                };

                context.FoodItems.AddRange(foodItems);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var customers = context.Customers.ToList();
                var couriers = context.Couriers.ToList();
                var foodItems = context.FoodItems.ToList();

                foreach (var customer in customers)
                {
                    var orders = new List<Order>
                    {
                        // For Placed orders, no courier should be assigned
                        CreateOrderWithDependencies(customer, foodItems[0], null, Status.Placed, context),
                        
                        // For Accepted, Picked_Up, and Delivered orders, assign a courier
                        CreateOrderWithDependencies(customer, foodItems[1], couriers.First(), Status.Accepted, context),
                        CreateOrderWithDependencies(customer, foodItems[2], couriers.First(), Status.Picked_Up, context),
                        CreateOrderWithDependencies(customer, foodItems[0], couriers.First(), Status.Delivered, context),
                        
                        // For Canceled orders, no courier is needed
                        CreateOrderWithDependencies(customer, foodItems[1], couriers.First(), Status.Canceled, context)
                    };

                    context.Orders.AddRange(orders);
                }

                context.SaveChanges();
            }



            // Seed Invoices for all orders with dependencies
            if (!context.Invoices.Any())
            {
                var orders = context.Orders.ToList();

                foreach (var order in orders)
                {
                    var invoiceStatus = InvoiceStatus.New; // Default

                    if (order.Status == Status.Delivered)
                    {
                        invoiceStatus = InvoiceStatus.Paid;
                    }
                    else if (order.Status == Status.Canceled)
                    {
                        invoiceStatus = InvoiceStatus.Cancelled;
                    }

                    var invoice = new Invoice(
                        customerId: order.CustomerId,
                        orderId: order.Id,
                        amount: order.GetTotalPrice(),
                        tip: 0m
                    );

                    invoice.MarkAsSent();


                    context.Invoices.Add(invoice);
                }

                context.SaveChanges();
            }
        }

        private static Order CreateOrderWithDependencies(Customer customer, FoodItem foodItem, Courier? courier, Status status, ShopContext context)
        {
            var deliveryFee = new DeliveryFee(30m);
            var order = new Order(
                orderId: Guid.NewGuid(),
                location: new Location("Building A", "101", "Left corner"),
                customerId: customer.Id,
                customerName: customer.Name,
                deliveryFee: deliveryFee
            );

            order.AddOrderLine(foodItem.Id, foodItem.Name, 1, foodItem.Price);

            switch (status)
            {
                case Status.Placed:
                    break;

                case Status.Accepted:
                    order.MarkAsAccepted();
                    break;

                case Status.Picked_Up:
                    order.MarkAsAccepted();
                    order.MarkAsPicked_Up();
                    break;

                case Status.Delivered:
                    order.MarkAsAccepted();
                    order.MarkAsPicked_Up();
                    order.MarkAsDelivered();
                    break;

                case Status.Canceled:
                    order.MarkAsCancelled();
                    break;

                default:
                    throw new InvalidOperationException("Unsupported order status.");
            }

            // Create a reimbursement
            var reimbursement = new Reimbursement(100m);
            if (courier != null)
            {
                reimbursement.AssignCourier(courier.Id);
            }

            context.Reimbursements.Add(reimbursement);

            // Create an offer
            var offer = new Offer(order.Id, reimbursement.Id);
            if (status == Status.Placed)
            {
                // For placed orders, the offer must be open (courier is not assigned)
                if (courier != null)
                {
                    throw new InvalidOperationException("Placed orders must have an open offer (no courier assigned).");
                }
            }
            else if (status >= Status.Accepted)
            {
                // For accepted and beyond orders, a courier must be assigned
                if (courier == null)
                {
                    throw new InvalidOperationException("Accepted and beyond orders must have a courier.");
                }
                offer.AssignCourier(courier.Id);
            }

            context.Offers.Add(offer);

            return order;
        }



        private static string CreatePasswordHash(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }
}