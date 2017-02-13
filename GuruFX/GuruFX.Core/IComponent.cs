using System;

namespace GuruFX.Core
{
	public interface IComponent : IEntityComponentBase
	{
		Guid InstanceID { get; }

		string Name { get; }

		IEntity Parent { get; set; }
		
		// TODO: Write tests for "Inactive" Components
		bool IsActive { get; set; }
	}
}
