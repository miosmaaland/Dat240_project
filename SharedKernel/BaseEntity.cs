using System.Collections.Generic;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.SharedKernel;

public abstract class BaseEntity
{
	public List<BaseDomainEvent> Events = new();
}