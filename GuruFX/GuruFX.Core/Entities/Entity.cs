using System;
using System.Collections.Generic;
using System.Linq;
using GuruFX.Core.Components;

namespace GuruFX.Core.Entities
{
	public class Entity : IEntity
	{
		/// <summary>
		/// Dictionary of Components. Key = ComponentInstanceID, Value = Component
		/// </summary>
		Dictionary<Guid, IComponent> mComponents = new Dictionary<Guid, IComponent>();

		/// <summary>
		/// Dictionary of Child Entities. Key = EntityInstanceID, Value = ChildEntity
		/// </summary>
		Dictionary<Guid, IEntity> mEntities = new Dictionary<Guid, IEntity>();


		public string Name { get; set; }

		public Guid InstanceID { get; set; } = Guid.NewGuid();

		public Dictionary<Guid, IComponent> Components
		{
			get { return mComponents; }
			set { mComponents = value; }
		}

		public Dictionary<Guid, IEntity> Entities
		{
			get { return mEntities; }
			set { mEntities = value; }
		}

		public IComponent CreateComponent()
		{
			// TODO: Need a Component factory? or Dependency Injection?
			var comp = new Component(this);

			if (!AddComponent(comp))
			{
				throw new Exception("Could not add newly created Component to Component collection.");
			}
			return comp;
		}

		public IComponent FindComponent(IComponent component)
		{
			if (component == null)
			{
				return null;
			}
			return FindComponent(component.InstanceID);
		}

		public IComponent FindComponent(Guid instanceID)
		{
			IComponent component;
			Components.TryGetValue(instanceID, out component);
			return component;
		}

		public bool AddComponent(IComponent component)
		{
			var existingComponent = FindComponent(component.InstanceID);
			if (existingComponent != null)
			{
				// the component is already part of this entity

				// TODO: Add Logging
				// Logger.Log("Could not add component: " + component.Name + "(" + component.InstanceID + ")");
				throw new ArgumentException("The Component (" + component.InstanceID + ") has already been added to this Entity (" + InstanceID + ")", nameof(component));
			}

			// add the component
			Components.Add(component.InstanceID, component);

			return true;
		}

		public bool AddComponents(IComponent[] components)
		{
			bool errors = false;
			foreach (IComponent component in components)
			{
				errors |= !AddComponent(component);
			}
			return !errors;
		}

		/// <summary>
		/// Get the first component of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <returns>The first component matching the type <typeparamref name="T"/>, otherwise NULL.</returns>
		public IComponent GetComponent<T>() where T : IComponent => GetComponent<T>((IComponent)null);

		/// <summary>
		/// Get the first component of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search</param>
		/// <returns>The first component matching the type <typeparamref name="T"/>, otherwise NULL.</returns>
		public IComponent GetComponent<T>(IComponent excludedComponent) where T : IComponent
		{
			Type searchType = typeof(T);

			foreach (var key in Components.Keys)
			{
				IComponent component = FindComponent(key);
				if (component != null && component != excludedComponent && component.GetType().Equals(searchType))
				{
					return component;
				}
			}

			return null;
		}

		/// <summary>
		/// Get the first component of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search</param>
		/// <returns>The first component matching the type <typeparamref name="T"/>, otherwise NULL.</returns>
		public IComponent GetComponent<T>(IComponent[] excludedComponents) where T : IComponent
		{
			Type searchType = typeof(T);

			foreach (var key in Components.Keys)
			{
				IComponent component = FindComponent(key);
				if (component != null && !excludedComponents.Contains(component) && component.GetType().Equals(searchType))
				{
					return component;
				}
			}

			return null;
		}


		/// <summary>
		/// Get all the components of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <returns>All the components matching the type <typeparamref name="T"/>, otherwise an empty array.</returns>
		public IComponent[] GetComponents<T>() where T : IComponent => GetComponents<T>((IComponent)null);

		/// <summary>
		/// Get all the components of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <returns>All the components matching the type <typeparamref name="T"/>, otherwise an empty array.</returns>
		public IComponent[] GetComponents<T>(IComponent excludedComponent) where T : IComponent
		{
			Type searchType = typeof(T);
			List<IComponent> foundComponents = new List<IComponent>();

			foreach (var key in Components.Keys)
			{
				IComponent component = FindComponent(key);
				Type checkType = component.GetType();

				if (component != null && component != excludedComponent && checkType.Equals(searchType))
				{
					foundComponents.Add(component);
				}
			}

			return foundComponents.ToArray();
		}

		/// <summary>
		/// Get all the components of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <returns>All the components matching the type <typeparamref name="T"/>, otherwise an empty array.</returns>
		public IComponent[] GetComponents<T>(IComponent[] excludedComponents) where T : IComponent
		{
			Type searchType = typeof(T);
			List<IComponent> foundComponents = new List<IComponent>();

			foreach (var key in Components.Keys)
			{
				IComponent component = FindComponent(key);
				Type checkType = component.GetType();

				if (component != null && !excludedComponents.Contains(component) && checkType.Equals(searchType))
				{
					foundComponents.Add(component);
				}
			}

			return foundComponents.ToArray();
		}
	}
}
