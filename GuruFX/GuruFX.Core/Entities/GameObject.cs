using GuruFX.Core.Components;

namespace GuruFX.Core.Entities
{
	public class GameObject : Entity
	{
		public override string Name { get; set; } = "Game Object";

		public double LastElapsedTime { get; set; }

		public GameObject()
		{
			// every game object will have a transform so that it can be placed, orientated, and scaled.
			AddComponent(new Transform());

			// from here on we can ensure that every game object has access to things like:
			//	* The Camera System?
			//	* The Audio System?
			//	* The Input System?
			//	* The Graphics/Lighting System?
			//	* etc.
		}

		public GameObject(string name) : this()
		{
			Name = name;
		}
	}
}
