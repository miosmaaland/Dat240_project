using System.Collections.Generic;

namespace SmaHauJenHoaVij.SharedKernel
{
    public interface IValidator<T>
    {
    	(bool IsValid, string Error) IsValid(T item);
    }
}