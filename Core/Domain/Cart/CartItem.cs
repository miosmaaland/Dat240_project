using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Cart
{
    public class CartItem : BaseEntity
    {
        public CartItem(Guid sku, string name, decimal price)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Count = 1;
        }

        public Guid Id { get; protected set; }
        public Guid Sku { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Count { get; private set; }
        public decimal TotalPrice => Price * Count;

        // Increment item count
        public void AddOne() => Count++;

        // Decrement item count
        public void RemoveOne() => Count--;
        
    }
}
