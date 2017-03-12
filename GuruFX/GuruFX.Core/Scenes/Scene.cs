using GuruFX.Core.Entities;
using GuruFX.Core.SystemComponents;

namespace GuruFX.Core.Scenes
{
	public class Scene : Entity, IUpdateable, ISystem
	{
		public double LastElapsedTime { get; set; }

		public override string Name { get; set; } = "Scene";

		public void Init()
		{

		}

		public void Destroy()
		{

		}

		/// <summary>
		/// Update all the System Components
		/// </summary>
		/// <param name="elapsedTime">Total Elapsed Time since the first update</param>
		/// <param name="deltaTime">Time passed since last Update</param>
		public void Update(double elapsedTime, double deltaTime)
		{
			this.LastElapsedTime = elapsedTime;

			// ok.. so each scene has to go through its "System Components" and Update them
			// the Scene itself does not update/process the entities that it owns.
			// It is up to this Scene's "System Components" to deal with all the entities and their components
			
			SystemComponent[] systemComponents = this.GetComponents<SystemComponent>();

			if (systemComponents != null)
			{
				foreach (var systemComponent in systemComponents)
				{
					systemComponent.Update(elapsedTime, deltaTime);
				}
			}
		}
	}
}
