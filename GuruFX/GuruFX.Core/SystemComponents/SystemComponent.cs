using GuruFX.Core.Components;

namespace GuruFX.Core.SystemComponents
{
	public abstract class SystemComponent : Component, ISystem, IUpdateable
	{
		public double LastElapsedTime { get; set; }
		
		public abstract void Init();

		public abstract void Destroy();

		public virtual void Update(double elapsedTime, double deltaTime)
		{
			this.LastElapsedTime = elapsedTime;
		}
	}
}
