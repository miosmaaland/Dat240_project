using SmaHauJenHoaVij.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace SmaHauJenHoaVij.Core.Domain.Cart
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }

        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }

        private readonly List<CartItem> _items = new();
        public IEnumerable<CartItem> Items => _items.AsReadOnly();

        public void AddItem(Guid sku, string name, decimal price)
        {
            var item = _items.SingleOrDefault(i => i.Sku == sku);
            if (item == null)
            {
                item = new CartItem(sku, name, price);
                _items.Add(item);
            }
            else
            {
                item.AddOne();
            }
        }

        public void RemoveItem(Guid sku)
        {
            var item = _items.SingleOrDefault(i => i.Sku == sku);
            if (item != null)
            {
                _items.Remove(item); // Remove the item completely
            }
        }

        public void ClearCart()
        {
            _items.Clear();
        }
    }
}
