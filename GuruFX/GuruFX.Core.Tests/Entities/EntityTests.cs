using System;
using System.Linq;
using GuruFX.Core.Components;
using GuruFX.Core.Scenes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuruFX.Core.Entities.Tests
{
	[TestClass]
	public class EntityTests
	{
		IScene scene;
		IEntity entity;

		[TestInitialize]
		public void Init()
		{
			// how do we inject a Scene factory?
			scene = new Scene();
			entity = scene.CreateEntity();
		}

		[TestMethod]
		public void CreateComponentTest()
		{
			IComponent c = entity.CreateComponent();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.Parent);
			Assert.AreSame(c.Parent, entity);
			Assert.IsFalse(string.IsNullOrEmpty(c.InstanceID.ToString()));
		}

		[TestMethod]
		public void AddSingleComponent()
		{
			// how do we inject a Component? or Component factory?
			IComponent c = new FakeComponent();

			bool result = entity.AddComponent(c);

			Assert.IsTrue(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Should not be able to add the same Component to the same Entity more than once!")]
		public void AddSameComponentTwice()
		{
			// how do we inject a Component? or Component factory?
			IComponent c = new FakeComponent();

			bool result = entity.AddComponent(c);
			result = entity.AddComponent(c);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void FindComponent_using_Component()
		{
			// Create a new Component
			IComponent c = entity.CreateComponent();

			// Find the Component using the Component itself
			IComponent f = entity.FindComponent(c);

			// Assert
			Assert.IsNotNull(f);
			Assert.AreSame(f, c);
		}

		[TestMethod]
		public void FindComponent_using_InstanceID()
		{
			// Create a new Component
			IComponent c = entity.CreateComponent();

			// Find the Component using its InstanceID
			IComponent f = entity.FindComponent(c.InstanceID);

			// Assert
			Assert.IsNotNull(f);
			Assert.AreSame(f, c);
		}

		[TestMethod]
		public void GetComponentTest()
		{
			FakeComponentA fakeComponentA = new FakeComponentA();
			FakeComponentB fakeComponentB = new FakeComponentB();

			entity.AddComponent(fakeComponentB);
			entity.AddComponent(fakeComponentA);

			IComponent fa = entity.GetComponent<FakeComponentA>();
			IComponent fb = entity.GetComponent<FakeComponentB>();

			Assert.IsNotNull(fa);
			Assert.IsNotNull(fb);
			
			Assert.AreSame(fa, fakeComponentA);
			Assert.AreSame(fb, fakeComponentB);
			Assert.AreNotSame(fb, fa);
		}

		[TestMethod]
		public void GetComponent_Excluding_A_Single_Component()
		{
			FakeComponentA fakeComponentA_1 = new FakeComponentA();
			FakeComponentA fakeComponentA_2 = new FakeComponentA();
			FakeComponentA fakeComponentA_3 = new FakeComponentA();

			entity.AddComponent(fakeComponentA_1);
			entity.AddComponent(fakeComponentA_2);
			entity.AddComponent(fakeComponentA_3);

			IComponent fa = entity.GetComponent<FakeComponentA>(fakeComponentA_1);

			Assert.IsNotNull(fa);
			Assert.AreNotSame(fakeComponentA_1, fa);

			// 1. we cannot actually assert the following because the InstanceID's are GUIDs which are not guaranteed to be generated in an ascending sequence,
			// 2. the keys (the Component InstanceID GUID's) used to keep track of components inside the Entities will not be in an ascending order. 
			// 3. therefore any component returned from the GetComponent method may not be the one we expect if we base our assumption on the order they were added to the entity.
			//Assert.AreSame(fa, fakeComponentA_2);
			// fa could be fakeComponentA_2 or fakeComponentA_3 depending on the GUID value generated
		}

		[TestMethod]
		public void GetComponent_Excluding_Multiple_Components()
		{
			FakeComponentA fakeComponentA_1 = new FakeComponentA();
			FakeComponentA fakeComponentA_2 = new FakeComponentA();
			FakeComponentA fakeComponentA_3 = new FakeComponentA();

			entity.AddComponent(fakeComponentA_1);
			entity.AddComponent(fakeComponentA_2);
			entity.AddComponent(fakeComponentA_3);

			IComponent fa = entity.GetComponent<FakeComponentA>(new[] { fakeComponentA_1, fakeComponentA_3 });

			Assert.IsNotNull(fa);
			Assert.AreNotSame(fakeComponentA_1, fa);
			Assert.AreSame(fa, fakeComponentA_2);
		}

		[TestMethod]
		public void GetComponentsTest()
		{
			FakeComponentA fakeComponentA_1 = new FakeComponentA();
			FakeComponentA fakeComponentA_2 = new FakeComponentA();
			FakeComponentA fakeComponentA_3 = new FakeComponentA();

			FakeComponentB fakeComponentB_1 = new FakeComponentB();
			FakeComponentB fakeComponentB_2 = new FakeComponentB();
			
			entity.AddComponent(fakeComponentB_1);
			entity.AddComponent(fakeComponentA_1);
			entity.AddComponent(fakeComponentA_2);
			entity.AddComponent(fakeComponentA_3);
			entity.AddComponent(fakeComponentB_2);

			IComponent[] f = entity.GetComponents<FakeComponent>();
			IComponent[] fa = entity.GetComponents<FakeComponentA>();
			IComponent[] fb = entity.GetComponents<FakeComponentB>();

			Assert.IsNotNull(f);
			Assert.IsNotNull(fa);
			Assert.IsNotNull(fb);

			Assert.AreEqual(0, f.Length, $"There should be no Components of type {typeof(FakeComponent).Name} in this entity.");
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

			Assert.IsTrue(fa.Contains(fakeComponentA_1));
			Assert.IsTrue(fa.Contains(fakeComponentA_2));
			Assert.IsTrue(fa.Contains(fakeComponentA_3));
			Assert.IsFalse(fa.Contains(fakeComponentB_1));
			Assert.IsFalse(fa.Contains(fakeComponentB_2));

			Assert.IsFalse(fb.Contains(fakeComponentA_1));
			Assert.IsFalse(fb.Contains(fakeComponentA_2));
			Assert.IsFalse(fb.Contains(fakeComponentA_3));
			Assert.IsTrue(fb.Contains(fakeComponentB_1));
			Assert.IsTrue(fb.Contains(fakeComponentB_2));

			Assert.AreNotSame(fb, fa);
		}

		[TestMethod]
		public void GetComponents_Excluding_A_Single_Component()
		{
			FakeComponentA fakeComponentA_1 = new FakeComponentA();
			FakeComponentA fakeComponentA_2 = new FakeComponentA();
			FakeComponentA fakeComponentA_3 = new FakeComponentA();

			FakeComponentB fakeComponentB_1 = new FakeComponentB();
			FakeComponentB fakeComponentB_2 = new FakeComponentB();

			entity.AddComponent(fakeComponentA_1);
			entity.AddComponent(fakeComponentA_2);
			entity.AddComponent(fakeComponentA_3);
			entity.AddComponent(fakeComponentB_1);
			entity.AddComponent(fakeComponentB_2);

			IComponent[] fa = entity.GetComponents<FakeComponentB>(fakeComponentB_1);

			Assert.IsNotNull(fa);
			Assert.AreEqual(1, fa.Length, $"There should only be 1 Component of type {typeof(FakeComponentB).Name} in this entity.");
			Assert.IsNotNull(fa[0]);
			Assert.AreSame(fakeComponentB_2, fa[0]);
		}

		[TestMethod]
		public void GetComponents_Excluding_Multiple_Components()
		{
			FakeComponentA fakeComponentA_1 = new FakeComponentA();
			FakeComponentA fakeComponentA_2 = new FakeComponentA();
			FakeComponentA fakeComponentA_3 = new FakeComponentA();
			FakeComponentB fakeComponentB_1 = new FakeComponentB();
			FakeComponentB fakeComponentB_2 = new FakeComponentB();
			
			entity.AddComponent(fakeComponentA_1);
			entity.AddComponent(fakeComponentB_1);
			entity.AddComponent(fakeComponentA_2);
			entity.AddComponent(fakeComponentA_3);
			entity.AddComponent(fakeComponentB_2);

			IComponent[] fb = entity.GetComponents<FakeComponentB>(new[] { fakeComponentA_1, fakeComponentA_3 });

			Assert.IsNotNull(fb);
			Assert.AreEqual(2, fb.Length, $"There should only be 2 Components of type {typeof(FakeComponentB).Name} in this entity.");
			Assert.IsNotNull(fb[0]);
			Assert.IsNotNull(fb[1]);
			Assert.IsTrue(fb.Contains(fakeComponentB_1));
			Assert.IsTrue(fb.Contains(fakeComponentB_2));
		}
	}
	
	public class FakeComponent : Component
	{

	}

	public class FakeComponentA : Component
	{

	}

	public class FakeComponentB : Component
	{

	}
}