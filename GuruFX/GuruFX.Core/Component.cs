using System;

namespace GuruFX.Core
{
	public class Component : IComponent
	{
		IEntity mParentEntity;

		public Component(IEntity parent)
		{
			if (this.Parent != null)
			{
				this.Parent = parent;
			}
		}

		public IEntity Parent
		{
			get
			{
				return this.mParentEntity;
			}
			private set
			{
				this.mParentEntity = value;
			}
		}

		public string Name { get; set; }
		public Guid InstanceID { get; set; } = Guid.NewGuid();
	}
}
