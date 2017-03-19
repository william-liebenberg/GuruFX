using System;

namespace GuruFX.Core
{
	public interface IBaseObject
	{
		Guid InstanceID { get; }
		string Name { get; set; }
	}
}