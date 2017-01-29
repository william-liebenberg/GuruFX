using System;

namespace GuruFX.Core
{
	public interface IComponent
	{
		IEntity Parent { get; }
		Guid InstanceID { get; }
		string Name { get; set; }
	}
}