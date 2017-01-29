using System;
using System.Collections.Generic;

namespace GuruFX.Core
{
	public interface IEntity : IComponentFactory, IComponentSearch
	{
		Dictionary<Guid, IComponent> Components { get; set; }
		Dictionary<Guid, IEntity> Entities { get; set; }
		Guid InstanceID { get; set; }
		string Name { get; set; }

		bool AddComponent(IComponent component);
		bool AddComponents(IComponent[] components);

		IComponent FindComponent(IComponent component);
		IComponent FindComponent(Guid instanceID);
	}
}