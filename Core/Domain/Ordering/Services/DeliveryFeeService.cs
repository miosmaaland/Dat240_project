using SmaHauJenHoaVij.Core.Domain.Ordering;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Services
{
    public class DeliveryFeeService
    {
        private DeliveryFee? _currentFee;

        public DeliveryFee GetCurrentFee()
        {
            if (_currentFee == null)
                _currentFee = new DeliveryFee(50);
            return _currentFee;
        }

        public void SetFee(decimal fee)
        {
            _currentFee = new DeliveryFee(fee);
        }

    }
}
