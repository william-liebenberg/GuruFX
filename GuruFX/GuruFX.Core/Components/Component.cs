using System;

namespace GuruFX.Core.Components
{
	public abstract class Component : IComponent
	{
		/// <summary>
		/// The Parent Entity. Components are always attached to an Entity.
		/// </summary>
		public IEntity Parent { get; set; }

		/// <summary>
		/// Name of this Component
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Instance GUID of this Component
		/// </summary>
		public Guid InstanceID { get; set; } = Guid.NewGuid();

		/// <summary>
		/// The Component can be set to Active or Inactive.
		/// </summary>
		public bool IsActive { get; set; } = true;
	}
}
