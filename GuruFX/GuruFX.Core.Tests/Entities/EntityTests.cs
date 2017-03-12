using System;
using System.Linq;
using GuruFX.Core.Entities;
using GuruFX.Core.Scenes;
using GuruFX.Core.Tests.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuruFX.Core.Tests.Entities
{
	[TestClass]
	public class EntityTests
	{
		private Scene m_scene;
		private IEntity m_root;

		[TestInitialize]
		public void Init()
		{
			m_scene = new Scene();
			m_root = m_scene.Root;
		}

		[TestMethod]
		public void AddComponentTest()
		{
			IComponent c = new FakeComponent();
			bool result = m_root.AddComponent(c);

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.Parent);
			Assert.AreSame(c.Parent, m_root);
			Assert.IsTrue(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Should not be able to add the same Component to the same Entity more than once!")]
		public void AddSameComponentTwice()
		{
			IComponent c = new FakeComponent();

			bool result1 = m_root.AddComponent(c);
			bool result2 = m_root.AddComponent(c);

			Assert.IsTrue(result1);
			Assert.IsFalse(result2);
		}

		[TestMethod]
		public void FindComponent_using_InstanceID_NoRecurse()
		{
			// Create a new Component
			IComponent c = new FakeComponent();
			m_root.AddComponent(c);

			// Find the Component using the Component itself
			IComponent f = m_root.FindComponent(c.InstanceID);

			// Assert
			Assert.IsNotNull(f);
			Assert.AreSame(f, c);
			Assert.AreEqual(f.InstanceID, c.InstanceID);
		}

		[TestMethod]
		public void FindComponent_using_InstanceID_Recurse()
		{
			// Create a new Entity
			IEntity e = new GameObject();
			m_root.AddEntity(e);

			// Create a new Component
			IComponent c = new FakeComponent();
			e.AddComponent(c);

			// Find the Component from Root using the Component itself
			IComponent f = m_root.FindComponentFromChildren(c.InstanceID);

			// Assert
			Assert.IsNotNull(f);
			Assert.AreSame(f, c);
			Assert.AreSame(e, c.Parent);
			Assert.AreEqual(f.InstanceID, c.InstanceID);
		}

		[TestMethod]
		public void RemoveEntity_using_Entity()
		{
			IEntity e = new GameObject();
			m_root.AddEntity(e);

			IEntity r = m_root.RemoveEntity(e);
			IEntity f = m_root.FindEntity(r.InstanceID);

			Assert.IsNotNull(e);
			Assert.IsNotNull(r);
			Assert.AreSame(r, e);
			Assert.IsNull(f);
		}

		[TestMethod]
		public void RemoveEntity_using_InstanceID()
		{
			IEntity e = new GameObject();
			m_root.AddEntity(e);

			IEntity r = m_root.RemoveEntity(e.InstanceID);
			IEntity f = m_root.FindEntity(r.InstanceID);

			Assert.IsNotNull(e);
			Assert.IsNotNull(r);
			Assert.AreSame(r, e);
			Assert.IsNull(f);
		}

		[TestMethod]
		public void RemoveComponent_using_Component()
		{
			IComponent c = new FakeComponent();
			m_root.AddComponent(c);

			IComponent r = m_root.RemoveComponent(c);
			IComponent f = m_root.FindComponent(r.InstanceID);

			Assert.IsNotNull(c);
			Assert.IsNotNull(r);
			Assert.AreSame(r, c);
			Assert.IsNull(f);
		}

		[TestMethod]
		public void RemoveComponent_using_InstanceID()
		{
			IComponent c = new FakeComponent();
			m_root.AddComponent(c);

			IComponent r = m_root.RemoveComponent(c.InstanceID);
			IComponent f = m_root.FindComponent(r.InstanceID);

			Assert.IsNotNull(c);
			Assert.IsNotNull(r);
			Assert.AreSame(r, c);
			Assert.IsNull(f);
		}

		[TestMethod]
		public void GetComponentTest()
		{
			FakeComponentA fakeComponentA = new FakeComponentA();
			FakeComponentB fakeComponentB = new FakeComponentB();

			m_root.AddComponent(fakeComponentB);
			m_root.AddComponent(fakeComponentA);

			IComponent fa = m_root.GetComponent<FakeComponentA>();
			IComponent fb = m_root.GetComponent<FakeComponentB>();

			Assert.IsNotNull(fa);
			Assert.IsNotNull(fb);

			Assert.AreSame(fa, fakeComponentA);
			Assert.AreSame(fb, fakeComponentB);
			Assert.AreNotSame(fb, fa);
		}

		[TestMethod]
		public void GetComponent_Excluding_A_Single_Component()
		{
			FakeComponentA fakeComponentA1 = new FakeComponentA();
			FakeComponentA fakeComponentA2 = new FakeComponentA();
			FakeComponentA fakeComponentA3 = new FakeComponentA();

			m_root.AddComponent(fakeComponentA1);
			m_root.AddComponent(fakeComponentA2);
			m_root.AddComponent(fakeComponentA3);

			IComponent fa = m_root.GetComponent<FakeComponentA>(fakeComponentA1);

			Assert.IsNotNull(fa);
			Assert.AreNotSame(fakeComponentA1, fa);

			// 1. we cannot actually assert the following because the InstanceID's are GUIDs which are not guaranteed to be generated in an ascending sequence,
			// 2. the keys (the Component InstanceID GUID's) used to keep track of components inside the Entities will not be in an ascending order. 
			// 3. therefore any component returned from the GetComponent method may not be the one we expect if we base our assumption on the order they were added to the entity.
			//Assert.AreSame(fa, fakeComponentA_2);
			// fa could be fakeComponentA_2 or fakeComponentA_3 depending on the GUID value generated
		}

		[TestMethod]
		public void GetComponent_Excluding_Multiple_Components()
		{
			FakeComponentA fakeComponentA1 = new FakeComponentA();
			FakeComponentA fakeComponentA2 = new FakeComponentA();
			FakeComponentA fakeComponentA3 = new FakeComponentA();

			m_root.AddComponent(fakeComponentA1);
			m_root.AddComponent(fakeComponentA2);
			m_root.AddComponent(fakeComponentA3);

			IComponent fa = m_root.GetComponent<FakeComponentA>(new IComponent[] { fakeComponentA1, fakeComponentA3 });

			Assert.IsNotNull(fa);
			Assert.AreNotSame(fakeComponentA1, fa);
			Assert.AreSame(fa, fakeComponentA2);
		}

		[TestMethod]
		public void GetComponentsTest()
		{
			FakeComponentA fakeComponentA1 = new FakeComponentA();
			FakeComponentA fakeComponentA2 = new FakeComponentA();
			FakeComponentA fakeComponentA3 = new FakeComponentA();

			FakeComponentB fakeComponentB1 = new FakeComponentB();
			FakeComponentB fakeComponentB2 = new FakeComponentB();

			m_root.AddComponent(fakeComponentB1);
			m_root.AddComponent(fakeComponentA1);
			m_root.AddComponent(fakeComponentA2);
			m_root.AddComponent(fakeComponentA3);
			m_root.AddComponent(fakeComponentB2);

			IComponent[] f = m_root.GetComponents<FakeComponent>();
			IComponent[] fa = m_root.GetComponents<FakeComponentA>();
			IComponent[] fb = m_root.GetComponents<FakeComponentB>();

			Assert.IsNull(f);
			Assert.IsNotNull(fa);
			Assert.IsNotNull(fb);
			
			Assert.AreEqual(3, fa.Length, $"There should only be 3 Components of type {typeof(FakeComponentA).Name} in this entity.");
			Assert.AreEqual(2, fb.Length, $"There should only be 2 Components of type {typeof(FakeComponentB).Name} in this entity.");

			// 1. we cannot actually assert the following because the InstanceID's are GUIDs which are not guaranteed to be generated in an ascending sequence,
			// 2. the keys (the Component InstanceID GUID's) used to keep track of components inside the Entities will not be in an ascending order. 
			// 3. therefore any component returned from the GetComponents method may not be the one we expect if we base our assumption on the order they were added to the entity.

			//Assert.AreSame(fa[0], fakeComponentA_1);
			//Assert.AreSame(fa[1], fakeComponentA_2);
			//Assert.AreSame(fa[2], fakeComponentA_3);
			//Assert.AreSame(fb[0], fakeComponentB_1);
			//Assert.AreSame(fb[1], fakeComponentB_2);

			Assert.IsTrue(fa.Contains(fakeComponentA1));
			Assert.IsTrue(fa.Contains(fakeComponentA2));
			Assert.IsTrue(fa.Contains(fakeComponentA3));
			Assert.IsFalse(fa.Contains(fakeComponentB1));
			Assert.IsFalse(fa.Contains(fakeComponentB2));

			Assert.IsFalse(fb.Contains(fakeComponentA1));
			Assert.IsFalse(fb.Contains(fakeComponentA2));
			Assert.IsFalse(fb.Contains(fakeComponentA3));
			Assert.IsTrue(fb.Contains(fakeComponentB1));
			Assert.IsTrue(fb.Contains(fakeComponentB2));

			Assert.AreNotSame(fb, fa);
		}

		[TestMethod]
		public void GetComponents_Excluding_A_Single_Component()
		{
			FakeComponentA fakeComponentA1 = new FakeComponentA();
			FakeComponentA fakeComponentA2 = new FakeComponentA();
			FakeComponentA fakeComponentA3 = new FakeComponentA();

			FakeComponentB fakeComponentB1 = new FakeComponentB();
			FakeComponentB fakeComponentB2 = new FakeComponentB();

			m_root.AddComponent(fakeComponentA1);
			m_root.AddComponent(fakeComponentA2);
			m_root.AddComponent(fakeComponentA3);
			m_root.AddComponent(fakeComponentB1);
			m_root.AddComponent(fakeComponentB2);

			IComponent[] fa = m_root.GetComponents<FakeComponentB>(fakeComponentB1);

			Assert.IsNotNull(fa);
			Assert.AreEqual(1, fa.Length, $"There should only be 1 Component of type {typeof(FakeComponentB).Name} in this entity.");
			Assert.IsNotNull(fa[0]);
			Assert.AreSame(fakeComponentB2, fa[0]);
		}

		[TestMethod]
		public void GetComponents_Excluding_Multiple_Components()
		{
			FakeComponentA fakeComponentA1 = new FakeComponentA();
			FakeComponentA fakeComponentA2 = new FakeComponentA();
			FakeComponentA fakeComponentA3 = new FakeComponentA();
			FakeComponentB fakeComponentB1 = new FakeComponentB();
			FakeComponentB fakeComponentB2 = new FakeComponentB();

			m_root.AddComponent(fakeComponentA1);
			m_root.AddComponent(fakeComponentB1);
			m_root.AddComponent(fakeComponentA2);
			m_root.AddComponent(fakeComponentA3);
			m_root.AddComponent(fakeComponentB2);

			IComponent[] fb = m_root.GetComponents<FakeComponentB>(new IComponent[] { fakeComponentA1, fakeComponentA3 });

			Assert.IsNotNull(fb);
			Assert.AreEqual(2, fb.Length, $"There should only be 2 Components of type {typeof(FakeComponentB).Name} in this entity.");
			Assert.IsNotNull(fb[0]);
			Assert.IsNotNull(fb[1]);
			Assert.IsTrue(fb.Contains(fakeComponentB1));
			Assert.IsTrue(fb.Contains(fakeComponentB2));
		}

		[TestMethod]
		public void RootTest_1()
		{
			IEntity e1 = new GameObject();
			m_root.AddEntity(e1);
			
			Assert.AreSame(m_root, e1.Root);
		}

		[TestMethod]
		public void RootTest_2()
		{
			IEntity e1 = new GameObject();
			m_root.AddEntity(e1);

			IEntity e2 = new GameObject();
			e1.AddEntity(e2);

			Assert.AreSame(m_root, e1.Root);
			Assert.AreSame(m_root, e2.Root);
		}

		[TestMethod]
		public void RootTest_3()
		{
			IEntity e1 = new GameObject();
			m_root.AddEntity(e1);

			IEntity e2 = new GameObject();
			e1.AddEntity(e2);

			IEntity e3 = new GameObject();
			e2.AddEntity(e3);

			Assert.AreSame(m_root, e1.Root);
			Assert.AreSame(m_root, e2.Root);
			Assert.AreSame(m_root, e3.Root);
		}

		[TestMethod]
		public void RootTest_4()
		{
			IEntity e1 = new GameObject();
			m_root.AddEntity(e1);

			IEntity e2 = new GameObject();
			e1.AddEntity(e2);

			IEntity e3 = new GameObject();
			e2.AddEntity(e3);

			IEntity e4 = new GameObject();
			e3.AddEntity(e4);

			Assert.AreSame(m_root, e1.Root);
			Assert.AreSame(m_root, e2.Root);
			Assert.AreSame(m_root, e3.Root);
			Assert.AreSame(m_root, e4.Root);
		}

		[TestMethod]
		public void RootTest_5()
		{
			IEntity e1 = new GameObject();
			m_root.AddEntity(e1);

			IEntity e2 = new GameObject();
			e1.AddEntity(e2);

			IEntity e3 = new GameObject();
			e2.AddEntity(e3);

			IEntity e4 = new GameObject();
			e3.AddEntity(e4);

			IEntity e5 = new GameObject();
			e4.AddEntity(e5);

			Assert.AreSame(m_root, e1.Root);
			Assert.AreSame(m_root, e2.Root);
			Assert.AreSame(m_root, e3.Root);
			Assert.AreSame(m_root, e4.Root);
			Assert.AreSame(m_root, e5.Root);
		}
	}
}
