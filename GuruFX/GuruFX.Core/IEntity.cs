using System;
using System.Collections.Concurrent;

namespace GuruFX.Core
{
	public interface IEntity : IEntityComponentBase
	{
		ConcurrentDictionary<Guid, IComponent> Components { get; set; }
		ConcurrentDictionary<Guid, IEntity> Entities { get; set; }
		Guid InstanceID { get; set; }
		string Name { get; set; }
		IEntity Parent { get; set; }

		// TODO: Write tests for "Inactive" Entities
		bool IsActive { get; set; }

		IEntity Root { get; }
	}
}
