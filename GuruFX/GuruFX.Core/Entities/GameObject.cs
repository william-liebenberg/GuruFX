using GuruFX.Core.Components;

namespace GuruFX.Core.Entities
{
	public class GameObject : Entity
	{
		public GameObject() : this(nameof(GameObject))
		{
		}

		public GameObject(string name) : base(name)
		{
			// every game object will have a transform so that it can be placed, orientated, and scaled.
			this.AddComponent(new Transform());

			// from here on we can ensure that every game object has access to things like:
			//	* The Camera System?
			//	* The Audio System?
			//	* The Input System?
			//	* The Graphics/Lighting System?
			//	* etc.
		}
	}
}
