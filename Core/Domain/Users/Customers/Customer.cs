namespace SmaHauJenHoaVij.Core.Domain.Users.Customers;

    public class Customer : User
    {
        public string Phone { get; set; }

        public bool IsApplyingForCourier { get; set; } = false;
    }
