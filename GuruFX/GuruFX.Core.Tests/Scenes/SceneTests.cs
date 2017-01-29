using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuruFX.Core.Scenes.Tests
{
	[TestClass]
	public class SceneTests
	{
		Scene mScene;

		[TestInitialize]
		public void Init()
		{
			mScene = new Scene();
		}

		[TestMethod]
		public void CreateEntityTest()
		{
			IEntity e = mScene.CreateEntity();

			Assert.IsNotNull(e);
			Assert.IsNotNull(e.Entities);
			Assert.IsNotNull(e.Components);
			Assert.IsNotNull(e.InstanceID);
			Assert.IsFalse(string.IsNullOrEmpty(e.InstanceID.ToString()));
		}

		[TestMethod]
		public void FindEntity_using_Entity()
		{
			// create a new entity
			IEntity e = mScene.CreateEntity();

			// find the entity using the original entity object
			IEntity f = mScene.FindEntity(e);

			// the entities must be same
			Assert.AreSame(f, e);
		}

		[TestMethod]
		public void FindEntity_using_InstanceID()
		{
			// create a new entity
			IEntity e = mScene.CreateEntity();
			
			// find the entity using the instanceID
			IEntity f = mScene.FindEntity(e.InstanceID);

			// the entities must be same
			Assert.AreSame(f, e);
		}
	}
}
