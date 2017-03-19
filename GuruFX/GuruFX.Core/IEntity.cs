using System;
using System.Collections.Concurrent;

namespace GuruFX.Core
{
	public interface IEntity : IBaseObject
	{
		ConcurrentDictionary<Guid, IComponent> Components { get; set; }
		ConcurrentDictionary<Guid, IEntity> Entities { get; set; }

		IEntity Parent { get; set; }

		// TODO: Write tests for "Inactive" Entities
		bool IsActive { get; set; }

		IEntity Root { get; }

		bool IsRoot { get; }
	}
}
