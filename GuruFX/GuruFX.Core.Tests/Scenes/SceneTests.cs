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
			//m_scene.AddComponents(new SceneUpdater(), new SceneRenderer());
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
		}

		[TestMethod]
		public void FindEntity_using_InstanceID()
		{
			// create a new entity
			IEntity e = new GameObject();
			m_scene.Root.AddEntity(e);

			// find the entity using the instanceID
			IEntity f = m_scene.FindEntity(e.InstanceID);

			// the entities must be same
			Assert.IsNotNull(f);
			Assert.AreSame(f, e);
			Assert.AreSame(f.Parent, m_scene.Root);
		}

		[TestMethod]
		public void RemoveEntity_using_Entity()
		{
			GameObject e = new GameObject();
			m_scene.Root.AddEntity(e);

			IEntity r = m_scene.RemoveEntity(e);
			IEntity f = m_scene.FindEntity(r.InstanceID);

			Assert.IsNotNull(e);
			Assert.IsNotNull(r);
			Assert.AreSame(r, e);
			Assert.IsNull(f);
		}

		[TestMethod]
		public void RemoveEntity_using_InstanceID()
		{
			GameObject e = new GameObject();
			m_scene.Root.AddEntity(e);

			IEntity r = m_scene.RemoveEntity(e.InstanceID);
			IEntity f = m_scene.FindEntity(r.InstanceID);

			Assert.IsNotNull(e);
			Assert.IsNotNull(r);
			Assert.AreSame(r, e);
			Assert.IsNull(f);
		}
	}
}
