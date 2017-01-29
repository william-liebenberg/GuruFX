using System;
using System.Collections.Generic;

namespace GuruFX.Core
{
	public class Entity : IEntity
	{
		/// <summary>
		/// Dictionary of Components. Key = ComponentInstanceID, Value = Component
		/// </summary>
		Dictionary<Guid, IComponent> m_components = new Dictionary<Guid, IComponent>();

		/// <summary>
		/// Dictionary of Child Entities. Key = EntityInstanceID, Value = ChildEntity
		/// </summary>
		Dictionary<Guid, IEntity> m_entities = new Dictionary<Guid, IEntity>();

		public string Name { get; set; }
		public Guid InstanceID { get; set; } = Guid.NewGuid();

		public Dictionary<Guid, IComponent> Components
		{
			get { return m_components; }
			set { m_components = value; }
		}

		public Dictionary<Guid, IEntity> Entities
		{
			get { return m_entities; }
			set { m_entities = value; }
		}


		public IComponent FindComponent(Guid instanceID)
		{
			IComponent component;
			Components.TryGetValue(instanceID, out component);
			return component;
		}

		public bool AddComponent<T>(T component) 
			where T : IComponent, new()
		{
			var existingComponent = FindComponent(component.InstanceID);
			if (existingComponent != null)
			{
				// the component is already part of this entity
				
				// TODO: Add Logging
				// Logger.Log("Could not add component: " + component.Name + "(" + component.InstanceID + ")");
				return false;
			}

			// add the component
			Components.Add(component.InstanceID, component);

			return true;
		}

		public bool AddComponents<T>(T[] components) 
			where T : IComponent, new()
		{
			bool errors = false;
			foreach(T component in components)
			{
				errors |= !AddComponent(component);
			}
			return !errors;
		}
	}
}
