using System;

namespace SmaHauJenHoaVij.Core.Domain.Ordering
{
    public class DeliveryFee : IEquatable<DeliveryFee>
    {
        public decimal Value { get; }

        public DeliveryFee(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("Delivery fee cannot be negative.");
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is DeliveryFee fee && Equals(fee);
        }

        public bool Equals(DeliveryFee? other)
        {
            return other != null && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString("C"); // Formats as currency
        }
    }
}
