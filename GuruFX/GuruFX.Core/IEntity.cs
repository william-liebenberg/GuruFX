using System;
using System.Collections.Generic;

namespace GuruFX.Core
{
	public interface IEntity
	{
		Guid InstanceID { get; set; }
		string Name { get; set; }
		Dictionary<Guid, IComponent> Components { get; set; }
		Dictionary<Guid, IEntity> Entities { get; set; }
	}
}