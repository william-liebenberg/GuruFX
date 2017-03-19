using System;
using System.Collections.Concurrent;

namespace GuruFX.Core.Entities
{
	public abstract class Entity : IEntity
	{
		protected Entity(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Dictionary of Components. Key = ComponentInstanceID, Value = Component
		/// </summary>
		public ConcurrentDictionary<Guid, IComponent> Components { get; set; } = new ConcurrentDictionary<Guid, IComponent>();

		/// <summary>
		/// Dictionary of Child Entities. Key = EntityInstanceID, Value = ChildEntity
		/// </summary>
		public ConcurrentDictionary<Guid, IEntity> Entities { get; set; } = new ConcurrentDictionary<Guid, IEntity>();

		/// <summary>
		/// Name of this Entity
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Instance GUID of this Entity
		/// </summary>
		public Guid InstanceID { get; set; } = Guid.NewGuid();

		/// <summary>
		/// The Parent Entity. If Parent is NULL, then this entity is the root (or top-level) entity.
		/// </summary>
		public IEntity Parent { get; set; }

		/// <summary>
		/// Get the Root Entity (Root node of the Scene)
		/// </summary>
		public IEntity Root => Parent?.Root ?? Parent ?? this;

		/// <summary>
		/// Indicates if this Entity the Root Entity or not.
		/// </summary>
		public bool IsRoot => Parent == null;

		/// <summary>
		/// The Entity can be set to Active or Inactive.
		/// </summary>
		public bool IsActive { get; set; } = true;
	}
}
