using System;
using GuruFX.Core.Components;
using GuruFX.Core.Entities;
using GuruFX.Core.Scenes;
using GuruFX.Core.SystemComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuruFX.Core.Tests.Scenes
{
	[TestClass]
	public class SceneTests
	{
		private Scene m_scene;

		[TestInitialize]
		public void Init()
		{
			m_scene = new Scene();
		}

		[TestMethod]
		public void NewScene_ValidRoot_DoesNotThrow()
		{
			IEntity root = new GameObject();
			Scene s = new Scene(root);

			Assert.IsNotNull(root);
			Assert.IsNotNull(s);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NewScene_NullRoot_ThenThrows()
		{
			IEntity root = null;

			Assert.IsNull(root);

			Scene s = new Scene(root);
		}

		[TestMethod]
		public void AddComponents_to_Scene_using_SystemComponents()
		{
			m_scene.AddComponents(new SceneUpdater(), new SceneRenderer());

			IEntity e = new GameObject();
			m_scene.Root.AddEntity(e);

			Behaviour behaviour = new Behaviour();

			bool addComponentResult = e.AddComponent(behaviour);
			Assert.IsTrue(addComponentResult);

			double elapsedTime = 2.123;
			const double deltaTime = 0.016;

			for(int j = 0; j < 10; j++)
			{
				m_scene.Update(elapsedTime, deltaTime);
				Assert.AreEqual(elapsedTime, m_scene.LastElapsedTime);
				Assert.AreEqual(elapsedTime, behaviour.LastElapsedTime);
				elapsedTime += deltaTime;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddComponent_to_Scene_using_Component_ThenThrows()
		{
			bool result = m_scene.AddComponent(new Behaviour());
			Assert.IsFalse(result);
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddComponents_to_Scene_using_Component_ThenThrows()
		{
			bool result = m_scene.AddComponents(new Behaviour());
			Assert.IsFalse(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddComponents_to_Scene_using_Components_ThenThrows()
		{
			bool result = m_scene.AddComponents(new Behaviour(), new Behaviour());
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void RootTest()
		{
			IEntity e = m_scene.Root;

			Assert.IsNotNull(e);
			Assert.IsNotNull(e.Entities);
			Assert.IsNotNull(e.Components);
			Assert.IsNotNull(e.InstanceID);
			Assert.IsNull(e.Parent);
			Assert.IsFalse(string.IsNullOrEmpty(e.InstanceID.ToString()));
			Assert.IsTrue(m_scene.Name.Equals(e.Name));
		}
	}
}
