using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GuruFX.Core.Entities
{
	public class Entity : IEntity
	{
		#region CONSTRUCTORS

		public Entity()
		{

		}

		public Entity(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Create a new Entity with list of Instantiated Components
		/// </summary>
		/// <param name="name"></param>
		/// <param name="componentTypes"></param>
		public Entity(string name, params Type[] componentTypes)
			: this(name)
		{
			foreach (Type componentType in componentTypes)
			{
				CreateAndAddComponentOfType(componentType);
			}
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
		public string Name { get; set; }

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
		/// The Entity can be set to Active or Inactive.
		/// </summary>
		public bool IsActive { get; set; } = true;

		#endregion PROPERTIES

		#region ENTITY METHODS

		/// <summary>
		/// Create and Add a new Child Entity of type <typeparamref name="T"/> to this Entity
		/// </summary>
		/// <typeparam name="T">The derived Entity Type</typeparam>
		/// <returns></returns>
		public T CreateAndAddEntity<T>() where T : class, IEntity, new() => (T)CreateAndAddEntityOfType(typeof(T));

		/// <summary>
		/// Create and 
		/// </summary>
		/// <param name="entityType"></param>
		/// <returns></returns>
		public IEntity CreateAndAddEntityOfType(Type entityType)
		{
			// TODO: Move to EntityFactory
			IEntity entity = Activator.CreateInstance(entityType) as IEntity;

			if (entity == null)
			{
				throw new NullReferenceException("Could not Create Entity Instance of type: " + entityType.FullName);
			}

			return !AddEntity(entity) ? null : entity;
		}

		/// <summary>
		/// Add a child Entity to this Entity
		/// </summary>
		/// <param name="entity">The child Entity to add.</param>
		/// <returns>True if the child Entity was added to this entity, otherwise an exception is thrown.</returns>
		/// <exception cref="ArgumentException">When the given entity is already a child of this entity.</exception>
		/// <exception cref="Exception">If the given entity could not be added to the child container of this entity.</exception>
		public bool AddEntity(IEntity entity)
		{
			IEntity existingEntity = FindEntity(entity.InstanceID, true);

			if (existingEntity != null)
			{
				// the new entity is already part of this entities hierarchy

				// TODO: Add Logging
				throw new ArgumentException("The Entity (" + entity.InstanceID + ") has already been added to this Entity (" + InstanceID + ")", nameof(entity));
			}

			if (!Entities.TryAdd(entity.InstanceID, entity))
			{
				// TODO: Add Logging
				throw new Exception($"Could not add the Entity ({entity.InstanceID}) to this Entity ({InstanceID})");
			}

			// Set this Entity as the Parent of the new Child Entity.
			entity.Parent = this;

			return true;
		}

		/// <summary>
		/// Find the child entity.
		/// </summary>
		/// <param name="entity">The child entity to find.</param>
		/// <param name="recurse">Recurse into child entities to find the specified entity.</param>
		/// <returns>The given entity, otherwise if the entity was not found to be a child of this entity then null is returned.</returns>
		public IEntity FindEntity(IEntity entity, bool recurse)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity), "Cannot find invalid Entities");
			}
			return FindEntity(entity.InstanceID, recurse);
		}

		/// <summary>
		/// Find a child entity via its InstanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID of the child entity to find.</param>
		/// <param name="recurse">Recurse into child entities to find the specified entity.</param>
		/// <returns>If found the child entity, otherwise null.</returns>
		public IEntity FindEntity(Guid instanceID, bool recurse)
		{
			IEntity entity;
			if (Entities.TryGetValue(instanceID, out entity))
			{
				return entity;
			}

			if (recurse)
			{
				foreach (IEntity child in Entities.Values)
				{
					if ((entity = child.FindEntity(instanceID, true)) != null)
					{
						return entity;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Remove the given child entity from the current entity.
		/// </summary>
		/// <param name="entity">The child entity to remove.</param>
		/// <returns>The removed child entity, otherwise if the entity could not be removed null is returned.</returns>
		/// <exception cref="ArgumentNullException">If the given child entity is null.</exception>
		public IEntity RemoveEntity(IEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity), "Cannot remove invalid Entities");
			}

			return RemoveEntity(entity.InstanceID);
		}

		/// <summary>
		/// Remove a the child entity with the given instanceID.
		/// </summary>
		/// <param name="instanceID">The child entity to remove' instanceID.</param>
		/// <returns></returns>
		public IEntity RemoveEntity(Guid instanceID)
		{
			IEntity removedEntity;

			return Entities.TryRemove(instanceID, out removedEntity) ? removedEntity : null;
		}

		#endregion ENTITY METHODS


		#region COMPONENT METHODS

		public T CreateAndAddComponent<T>() where T : class, IComponent, new() => (T)CreateAndAddComponentOfType(typeof(T));

		public IComponent CreateAndAddComponentOfType(Type componentType)
		{
			// TODO: Check for RequiredComponentAttribute/RequiredComponentsAttribute then create and it to this entity
			// TODO: Use FastFactory or something better than CreateInstance
			// TODO: Components may need a .Initialize() method

			IComponent component = Activator.CreateInstance(componentType) as IComponent;

			return component == null ? null : (AddComponent(component) == false ? null : component);
		}

		public bool AddComponent(IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot add invalid Components to an Entity");
			}

			IComponent existingComponent = FindComponent(component, true);

			if (existingComponent != null)
			{
				// the component is already part of this entity

				// TODO: Add Logging
				// Logger.Log("Could not add component: " + component.Name + "(" + component.InstanceID + ")");
				throw new ArgumentException("The Component (" + component.InstanceID + ") has already been added to this Entity (" + InstanceID + ")", nameof(component));
			}

			// add the component
			if (!Components.TryAdd(component.InstanceID, component))
			{
				// TODO: Add Logging
				// Logger.Log($"Could not add Entity '{entity.Name}' ({entity.InstanceID}) to the Scene");
				throw new Exception($"Could not add the Component ({component.InstanceID}) to this Entity ({InstanceID})");
			}

			// set its Parent to us!
			component.Parent = this;

			return true;
		}

		public IComponent RemoveComponent(IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot remove invalid Components");
			}

			return RemoveComponent(component.InstanceID);
		}

		public IComponent RemoveComponent(Guid instanceID)
		{
			IComponent removedComponent;

			return Components.TryRemove(instanceID, out removedComponent) ? removedComponent : null;
		}

		public IComponent FindComponent(IComponent component, bool recurse)
		{
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot find invalid Components");
			}

			return FindComponent(component.InstanceID, recurse);
		}

		public IComponent FindComponent(Guid instanceID, bool recurse)
		{
			IComponent component;

			if (Components.TryGetValue(instanceID, out component))
			{
				return component;
			}

			if (recurse)
			{
				foreach (IEntity child in Entities.Values)
				{
					if ((component = child.FindComponent(instanceID, true)) != null)
					{
						return component;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponent<T>()
			where T : IComponent => GetComponent<T>((IComponent)null);

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponent<T>(IComponent excludedComponent)
			where T : IComponent => GetComponent<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponent<T>(IComponent[] excludedComponents)
			where T : IComponent
		{
			Type searchType = typeof(T);

			foreach (Guid key in Components.Keys)
			{
				IComponent component = FindComponent(key, false);
				if (component != null && !excludedComponents.Contains(component) && component.GetType() == searchType)
				{
					return component;
				}
			}

			return null;
		}

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponents<T>()
			where T : IComponent => GetComponents<T>((IComponent)null);

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponents<T>(IComponent excludedComponent)
			where T : IComponent => GetComponents<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponents<T>(IComponent[] excludedComponents)
			where T : IComponent
		{
			Type searchType = typeof(T);
			List<IComponent> foundComponents = new List<IComponent>();

			foreach (Guid instanceID in Components.Keys)
			{
				IComponent component = FindComponent(instanceID, false);
				if (component == null)
				{
					continue;
				}

				if (!excludedComponents.Contains(component) && component.GetType() == searchType)
				{
					foundComponents.Add(component);
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : null;
		}

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponentFromParents<T>()
			where T : IComponent => GetComponentFromParents<T>((IComponent[])null);

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponentFromParents<T>(IComponent excludedComponent)
			where T : IComponent => GetComponentFromParents<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponentFromParents<T>(IComponent[] excludedComponents)
			where T : IComponent
		{
			IComponent component = GetComponent<T>(excludedComponents);

			if (component == null && Parent != null && Parent.IsActive)
			{
				component = Parent.GetComponentFromParents<T>(excludedComponents);
			}

			return component;
		}

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponentsFromParents<T>()
			where T : IComponent => GetComponentsFromParents<T>((IComponent[])null);

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponentsFromParents<T>(IComponent excludedComponent)
			where T : IComponent => GetComponentsFromParents<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponentsFromParents<T>(IComponent[] excludedComponents)
			where T : IComponent
		{
			List<IComponent> foundComponents = new List<IComponent>();
			IComponent[] myComponents;

			if ((myComponents = GetComponents<T>(excludedComponents)) != null)
			{
				foundComponents.AddRange(myComponents);
			}

			if (Parent != null && Parent.IsActive)
			{
				IComponent[] parentComponents;
				if ((parentComponents = Parent.GetComponents<T>(excludedComponents)) != null)
				{
					foundComponents.AddRange(parentComponents);
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : null;
		}

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponentFromChildren<T>() where T : IComponent => GetComponentFromChildren<T>((IComponent)null);

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponentFromChildren<T>(IComponent excludedComponent) where T : IComponent => GetComponentFromChildren<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public IComponent GetComponentFromChildren<T>(IComponent[] excludedComponents) where T : IComponent
		{
			IComponent myComponent;

			if ((myComponent = GetComponent<T>(excludedComponents)) != null)
			{
				return myComponent;
			}

			if (Entities != null)
			{
				foreach (Guid childInstanceID in Entities.Keys)
				{
					IEntity childEntity;
					Entities.TryGetValue(childInstanceID, out childEntity);
					if (childEntity == null || !childEntity.IsActive)
					{
						continue;
					}

					IComponent childComponent;
					if ((childComponent = childEntity.GetComponentFromChildren<T>(excludedComponents)) != null)
					{
						return childComponent;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponentsFromChildren<T>() where T : IComponent => GetComponentsFromChildren<T>((IComponent)null);

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponentsFromChildren<T>(IComponent excludedComponent)
			where T : IComponent => GetComponentsFromChildren<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public IComponent[] GetComponentsFromChildren<T>(IComponent[] excludedComponents)
			where T : IComponent
		{
			List<IComponent> foundComponents = new List<IComponent>();
			IComponent[] myComponents;

			if ((myComponents = GetComponents<T>(excludedComponents)) != null)
			{
				foundComponents.AddRange(myComponents);
			}

			if (Entities != null)
			{
				foreach (Guid childInstanceID in Entities.Keys)
				{
					IEntity childEntity;
					Entities.TryGetValue(childInstanceID, out childEntity);
					if (childEntity == null || !childEntity.IsActive)
					{
						continue;
					}

					IComponent[] childComponents;
					if ((childComponents = childEntity.GetComponentsFromChildren<T>(excludedComponents)) != null)
					{
						foundComponents.AddRange(childComponents);
					}
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : null;
		}

		#endregion COMPONENT METHODS
	}
}
