using System;
using SmaHauJenHoaVij.Core.Domain.Users;

namespace SmaHauJenHoaVij.Core.Domain.Users.Couriers;

    public class Courier : User
    {
        public bool IsApproved { get; set; } = false;
        public decimal Earnings { get; set; } = 0;
    }
