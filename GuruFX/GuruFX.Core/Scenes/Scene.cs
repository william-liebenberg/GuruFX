using GuruFX.Core.Entities;
using GuruFX.Core.SystemComponents;

namespace GuruFX.Core.Scenes
{
	public class Scene : Entity, ISystem, IUpdateable
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
		/// Update all the Scene System Components
		/// </summary>
		/// <param name="elapsedTime">TODO: WL@</param>
		/// <param name="deltaTime">TODO: WL@</param>
		public void Update(double elapsedTime, double deltaTime)
		{
			this.LastElapsedTime = elapsedTime;

			// ok.. so each scene has to go through its "System Components" and Update them
			// the Scene itself does not update/process the entities that it owns.
			// It is up to this Scene's "System Components" to deal with all the entities and their components

			SystemComponent[] systemComponents = GetComponentsFromChildren<SystemComponent>();

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
