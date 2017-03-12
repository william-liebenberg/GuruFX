using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GuruFX.Core.Entities
{
	public abstract class Entity : IEntity
	{
		#region CONSTRUCTORS

		public Entity()
		{

		}

		#endregion CONSTRUCTORS

		#region PROPERTIES

		/// <summary>
		/// Dictionary of Components. Key = ComponentInstanceID, Value = Component
		/// </summary>
		public ConcurrentDictionary<Guid, IComponent> Components { get; set; } = new ConcurrentDictionary<Guid, IComponent>();

		/// <summary>
		/// Dictionary of Child Entities. Key = EntityInstanceID, Value = ChildEntity
		/// </summary>
		public ConcurrentDictionary<Guid, IEntity> Entities { get; set; } = new ConcurrentDictionary<Guid, IEntity>();

		/// <summary>
		/// Name of this Entity
		/// </summary>
		public abstract string Name { get; set; }

		/// <summary>
		/// Instance GUID of this Entity
		/// </summary>
		public Guid InstanceID { get; set; } = Guid.NewGuid();

		/// <summary>
		/// The Parent Entity. If Parent is NULL, then this entity is the root (or top-level) entity.
		/// </summary>
		public IEntity Parent { get; set; }

		/// <summary>
		/// Get the Root Entity (Root node of the Scene)
		/// </summary>
		public IEntity Root => Parent?.Root ?? Parent ?? this;

		/// <summary>
		/// Indicates if this Entity the Root Entity or not.
		/// </summary>
		public bool IsRoot => Parent == null;

		/// <summary>
		/// The Entity can be set to Active or Inactive.
		/// </summary>
		public bool IsActive { get; set; } = true;

		#endregion PROPERTIES

		#region ENTITY METHODS
		
		public IEntity[] GetEntities() => Entities?.Values?.ToArray();

		/// <summary>
		/// Add a child Entity to this Entity
		/// </summary>
		/// <param name="entity">The child Entity to add.</param>
		/// <returns>TRUE if the child Entity was added to this entity, otherwise an exception is thrown.</returns>
		/// <exception cref="ArgumentException">When the given entity is already a child of this entity.</exception>
		/// <exception cref="Exception">If the given entity could not be added to the child container of this entity.</exception>
		public bool AddEntity(IEntity entity)
		{
			IEntity existingEntity = FindEntityFromChildren(entity.InstanceID);

			if(existingEntity != null)
			{
				// the new entity is already part of this entities hierarchy

				// TODO: Add Logging
				throw new ArgumentException("The Entity (" + entity.InstanceID + ") has already been added to this Entity (" + InstanceID + ")", nameof(entity));
			}

			if(!Entities.TryAdd(entity.InstanceID, entity))
			{
				// TODO: Add Logging
				throw new Exception($"Could not add the Entity ({entity.InstanceID}) to this Entity ({InstanceID})");
			}

			// Set this Entity as the Parent of the new Child Entity.
			entity.Parent = this;

			return true;
		}

		public bool AddEntities(params IEntity[] entities)
		{
			if(entities == null)
			{
				return false;
			}

			bool errors = false;
			foreach(IEntity entity in entities)
			{
				if(!AddEntity(entity))
				{
					errors = true;
				}
			}

			return !errors;
		}
		
		public IEntity FindEntity(Guid instanceID)
		{
			if(Entities.TryGetValue(instanceID, out IEntity entity))
			{
				return entity;
			}

			return null;
		}
		
		public IEntity FindEntityFromChildren(Guid instanceID)
		{
			if(Entities.TryGetValue(instanceID, out IEntity entity))
			{
				return entity;
			}

			foreach(IEntity child in Entities.Values)
			{
				if((entity = child.FindEntityFromChildren(instanceID)) != null)
				{
					return entity;
				}
			}

			return null;
		}
		
		public IEntity RemoveEntity(IEntity entity)
		{
			if(entity == null)
			{
				throw new ArgumentNullException(nameof(entity), "Cannot remove invalid Entities");
			}

			return RemoveEntity(entity.InstanceID);
		}
		
		public IEntity RemoveEntity(Guid instanceID)
		{
			return Entities.TryRemove(instanceID, out IEntity removedEntity) ? removedEntity : null;
		}

		#endregion ENTITY METHODS

		#region COMPONENT METHODS
				
		public IComponent[] GetComponents() => Components?.Values?.ToArray();
		
		public bool AddComponent(IComponent component)
		{
			if(component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot add invalid Components to an Entity");
			}

			IComponent existingComponent = FindComponentFromChildren(component);

			if(existingComponent != null)
			{
				// the component is already part of this entity

				// TODO: Add Logging
				// Logger.Log("Could not add component: " + component.Name + "(" + component.InstanceID + ")");
				throw new ArgumentException("The Component (" + component.InstanceID + ") has already been added to this Entity (" + InstanceID + ")", nameof(component));
			}

			// add the component
			if(!Components.TryAdd(component.InstanceID, component))
			{
				// TODO: Add Logging
				// Logger.Log($"Could not add Entity '{entity.Name}' ({entity.InstanceID}) to the Scene");
				throw new Exception($"Could not add the Component ({component.InstanceID}) to this Entity ({InstanceID})");
			}

			// set its Parent to us!
			component.Parent = this;

			return true;
		}
		
		public bool AddComponents(params IComponent[] components)
		{
			if(components == null)
			{
				return false;
			}

			bool errors = false;
			foreach(IComponent component in components)
			{
				if(!AddComponent(component))
				{
					errors = true;
				}
			}

			return !errors;
		}
		
		public IComponent RemoveComponent(IComponent component)
		{
			if(component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot remove invalid Components");
			}

			return RemoveComponent(component.InstanceID);
		}
		
		public IComponent RemoveComponent(Guid instanceID)
		{
			return Components.TryRemove(instanceID, out IComponent removedComponent) ? removedComponent : null;
		}
		
		public IComponent FindComponent(Guid instanceID)
		{
			if(Components.TryGetValue(instanceID, out IComponent component))
			{
				return component;
			}

			return null;
		}
		
		public IComponent FindComponentFromChildren(IComponent component)
		{
			if(component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot find invalid Components");
			}

			return FindComponentFromChildren(component.InstanceID);
		}

		public IComponent FindComponentFromChildren(Guid instanceID)
		{
			if(Components.TryGetValue(instanceID, out IComponent component))
			{
				return component;
			}

			foreach(IEntity child in Entities.Values)
			{
				if((component = child.FindComponent(instanceID)) != null)
				{
					return component;
				}
			}

			return null;
		}
		
		public T GetComponent<T>()
			where T : class, IComponent => GetComponent<T>((T)null);
				
		public T GetComponent<T>(IComponent excludedComponent)
			where T : class, IComponent => GetComponent<T>(new[] { excludedComponent });

		public T GetComponent<T>(IComponent[] excludedComponents)
			where T : class, IComponent
		{
			Type searchType = typeof(T);

			foreach(Guid key in Components.Keys)
			{
				IComponent component = FindComponent(key);

				if(component != null && excludedComponents != null && !excludedComponents.Contains(component))
				{
					if(component is T castComponent)
					{
						return castComponent;
					}
				}
			}

			return default(T);
		}
		
		public T[] GetComponents<T>()
			where T : class, IComponent => GetComponents<T>((T)null);

		
		public T[] GetComponents<T>(IComponent excludedComponent)
			where T : class, IComponent => GetComponents<T>(new[] { excludedComponent });

		
		public T[] GetComponents<T>(IComponent[] excludedComponents)
			where T : class, IComponent
		{
			List<T> foundComponents = new List<T>();

			foreach(Guid instanceID in Components.Keys)
			{
				IComponent component = FindComponent(instanceID);
				if(component == null)
				{
					continue;
				}

				if(excludedComponents != null && excludedComponents != null && !excludedComponents.Contains(component))
				{
					if(component is T castComponent)
					{
						foundComponents.Add(castComponent);
					}
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : default(T[]);
		}

		
		public T GetComponentFromParents<T>()
			where T : class, IComponent => GetComponentFromParents<T>((T[])null);

		
		public T GetComponentFromParents<T>(IComponent excludedComponent)
			where T : class, IComponent => GetComponentFromParents<T>(new[] { excludedComponent });

		
		public T GetComponentFromParents<T>(IComponent[] excludedComponents)
			where T : class, IComponent
		{
			T component = GetComponent<T>(excludedComponents);

			if(component == null && Parent != null && Parent.IsActive)
			{
				component = Parent.GetComponentFromParents<T>(excludedComponents);
			}

			return component;
		}

		
		public T[] GetComponentsFromParents<T>()
			where T : class, IComponent => GetComponentsFromParents<T>((T[])null);

		
		public T[] GetComponentsFromParents<T>(IComponent excludedComponent)
			where T : class, IComponent => GetComponentsFromParents<T>(new[] { excludedComponent });

		
		public T[] GetComponentsFromParents<T>(IComponent[] excludedComponents)
			where T : class, IComponent
		{
			List<T> foundComponents = new List<T>();
			T[] myComponents;

			if((myComponents = GetComponents<T>(excludedComponents)) != null)
			{
				foundComponents.AddRange(myComponents);
			}

			if(Parent != null && Parent.IsActive)
			{
				T[] parentComponents;
				if((parentComponents = Parent.GetComponents<T>(excludedComponents)) != null)
				{
					foundComponents.AddRange(parentComponents);
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : null;
		}

		
		public T GetComponentFromChildren<T>()
			where T : class, IComponent => GetComponentFromChildren<T>((T)null);

		
		public T GetComponentFromChildren<T>(IComponent excludedComponent)
			where T : class, IComponent => GetComponentFromChildren<T>(new[] { excludedComponent });

		
		public T GetComponentFromChildren<T>(IComponent[] excludedComponents)
			where T : class, IComponent
		{
			T myComponent;

			if((myComponent = GetComponent<T>(excludedComponents)) != null)
			{
				return myComponent;
			}

			if(Entities != null)
			{
				foreach(Guid childInstanceID in Entities.Keys)
				{
					Entities.TryGetValue(childInstanceID, out IEntity childEntity);
					if(childEntity == null || !childEntity.IsActive)
					{
						continue;
					}

					T childComponent;
					if((childComponent = childEntity.GetComponentFromChildren<T>(excludedComponents)) != null)
					{
						return childComponent;
					}
				}
			}

			return null;
		}
		




		public T[] GetComponentsFromChildren<T>()
			where T : class, IComponent => GetComponentsFromChildren<T>((T)null);
				
		public T[] GetComponentsFromChildren<T>(IComponent excludedComponent)
			where T : class, IComponent => GetComponentsFromChildren<T>(new[] { excludedComponent });
				
		public T[] GetComponentsFromChildren<T>(IComponent[] excludedComponents)
			where T : class, IComponent
		{
			List<T> foundComponents = new List<T>();
			T[] myComponents;

			if((myComponents = GetComponents<T>(excludedComponents)) != null)
			{
				foundComponents.AddRange(myComponents);
			}

			if(Entities != null)
			{
				foreach(Guid childInstanceID in Entities.Keys)
				{
					Entities.TryGetValue(childInstanceID, out IEntity childEntity);
					if(childEntity == null || !childEntity.IsActive)
					{
						continue;
					}

					T[] childComponents;
					if((childComponents = childEntity.GetComponentsFromChildren<T>(excludedComponents)) != null)
					{
						foundComponents.AddRange(childComponents);
					}
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : default(T[]);
		}

		#endregion COMPONENT METHODS
	}
}
