using GuruFX.Core.Components;

namespace GuruFX.Core.SystemComponents
{
	public abstract class SystemComponent : Component, IUpdateable, ISystem
	{
		public double LastElapsedTime { get; set; }

		public SystemComponent()
		{

		}

		public SystemComponent(IEntity parent) : base(parent)
		{
		}

		public abstract void Init();

		public abstract void Destroy();

		public virtual void Update(double elapsedTime, double deltaTime)
		{
			this.LastElapsedTime = elapsedTime;
		}
	}
}
