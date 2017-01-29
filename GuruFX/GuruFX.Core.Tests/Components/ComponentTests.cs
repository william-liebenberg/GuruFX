using GuruFX.Core.Scenes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuruFX.Core.Components.Tests
{
	[TestClass]
	public class ComponentTests
	{
		IScene scene;
		IEntity entity;
		IComponent component;

		[TestInitialize]
		public void Init()
		{
			// how do we inject a Scene factory?
			scene = new Scene();
			entity = scene.CreateEntity();
			component = entity.CreateComponent();
		}
	}
}
