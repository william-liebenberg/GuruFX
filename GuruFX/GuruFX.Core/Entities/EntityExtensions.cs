using System;
using System.Collections.Generic;
using System.Linq;

namespace GuruFX.Core.Entities
{
	public static class EntityExtensions
	{
		// TODO: Benchmark IEnumerable<IEntity> vs IEntity[]
		// public static IEntity[] GetEntities(this IEntity parent) => parent.Entities?.Values?.ToArray();

		// TODO: Benchmark IEnumerable<IComponent> vs IComponent[]
		// public static IComponent[] GetComponents(this IEntity parent) => parent.Components?.Values?.ToArray();

		/// <summary>
		/// Return an array of all the Child Entities for this Entity.
		/// </summary>
		/// <returns>All the child entities.</returns>
		public static IEnumerable<IEntity> GetEntities(this IEntity parent) => parent.Entities?.Values ?? null;
		
		/// <summary>
		/// Return an array of all the Components for this Entity.
		/// </summary>
		/// <returns>All the Components.</returns>
		public static IEnumerable<IComponent> GetComponents(this IEntity parent) => parent.Components?.Values ?? null;
		
		#region ENTITY

		/// <summary>
		/// Add a child Entity to this Entity
		/// </summary>
		/// <param name="entity">The child Entity to add.</param>
		/// <returns>TRUE if the child Entity was added to this entity, otherwise an exception is thrown.</returns>
		/// <exception cref="ArgumentException">When the given entity is already a child of this entity.</exception>
		/// <exception cref="Exception">If the given entity could not be added to the child container of this entity.</exception>
		public static bool AddEntity(this IEntity parent, IEntity entity)
		{
			IEntity existingEntity = parent.FindEntityFromChildren(entity.InstanceID);

			if(existingEntity != null)
			{
				// the new entity is already part of this entities hierarchy

				// TODO: Add Logging
				throw new ArgumentException("The Entity (" + entity.InstanceID + ") has already been added to this Entity (" + parent.InstanceID + ")", nameof(parent));
			}

			if(!parent.Entities.TryAdd(entity.InstanceID, entity))
			{
				// TODO: Add Logging
				throw new Exception($"Could not add the Entity ({entity.InstanceID}) to this Entity ({parent.InstanceID})");
			}

			// Set this Entity as the Parent of the new Child Entity.
			entity.Parent = parent;

			return true;
		}

		/// <summary>
		/// TODO: WL@
		/// </summary>
		/// <param name="entities"></param>
		/// <returns></returns>
		public static bool AddEntities(this IEntity parent, params IEntity[] entities)
		{
			if(entities == null)
			{
				return false;
			}

			bool errors = false;
			foreach(IEntity entity in entities)
			{
				if(!parent.AddEntity(entity))
				{
					errors = true;
				}
			}

			return !errors;
		}

		/// <summary>
		/// Find a child entity via its InstanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID of the child entity to find.</param>
		/// <returns>If found the child entity, otherwise null.</returns>
		public static IEntity FindEntity(this IEntity parent, Guid instanceID)
		{
			if(parent.Entities.TryGetValue(instanceID, out IEntity entity))
			{
				return entity;
			}

			return null;
		}

		/// <summary>
		/// Find the child entity with matching instanceID of this entity, and if none are found on this entity then search through Children.
		/// TODO: should we only search through ACTIVE children? or all children??
		/// </summary>
		/// <param name="instanceID"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		public static IEntity FindEntityFromChildren(this IEntity parent, Guid instanceID)
		{
			if(parent.Entities.TryGetValue(instanceID, out IEntity entity))
			{
				return entity;
			}

			foreach(IEntity child in parent.Entities.Values)
			{
				if((entity = child.FindEntityFromChildren(instanceID)) != null)
				{
					return entity;
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
		public static IEntity RemoveEntity(this IEntity parent, IEntity entity)
		{
			if(entity == null)
			{
				throw new ArgumentNullException(nameof(entity), "Cannot remove invalid Entities");
			}

			return parent.RemoveEntity(entity.InstanceID);
		}

		/// <summary>
		/// Remove a the child entity with the given instanceID.
		/// </summary>
		/// <param name="instanceID">The child entity to remove' instanceID.</param>
		/// <returns></returns>
		public static IEntity RemoveEntity(this IEntity parent, Guid instanceID)
		{
			IEntity removedEntity = null;
			if(parent.Entities.TryRemove(instanceID, out removedEntity))
			{
				return removedEntity;
			}
			return null;
		}

		#endregion ENTITY

		#region COMPONENT

		/// <summary>
		/// Add one or more components to this Entity
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public static bool AddComponent(this IEntity parent, IComponent component)
		{
			if(component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot add invalid Components to an Entity");
			}

			IComponent existingComponent = parent.FindComponentFromChildren(component.InstanceID);

			if(existingComponent != null)
			{
				// the component is already part of this entity

				// TODO: Add Logging
				// Logger.Log("Could not add component: " + component.Name + "(" + component.InstanceID + ")");
				throw new ArgumentException("The Component (" + component.InstanceID + ") has already been added to this Entity (" + parent.InstanceID + ")", nameof(component));
			}

			// add the component
			if(!parent.Components.TryAdd(component.InstanceID, component))
			{
				// TODO: Add Logging
				// Logger.Log($"Could not add Entity '{entity.Name}' ({entity.InstanceID}) to the Scene");
				throw new Exception($"Could not add the Component ({component.InstanceID}) to this Entity ({parent.InstanceID})");
			}

			// set its Parent to us!
			component.Parent = parent;

			return true;
		}

		/// <summary>
		/// Add one or more components to this Entity
		/// </summary>
		/// <param name="components"></param>
		/// <returns>If no errors occurred, then returns TRUE, otherwise if one or more errors occurred then returns FALSE.</returns>
		public static bool AddComponents(this IEntity parent, params IComponent[] components)
		{
			if(components == null)
			{
				return false;
			}

			bool errors = false;
			foreach(IComponent component in components)
			{
				if(!parent.AddComponent(component))
				{
					errors = true;
				}
			}

			return !errors;
		}

		/// <summary>
		/// Find a component that matches the given instanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID to search for</param>
		/// <returns>If found, the original component is returned, otherwise NULL</returns>
		public static IComponent FindComponent(this IEntity parent, Guid instanceID)
		{
			if(parent.Components.TryGetValue(instanceID, out IComponent component))
			{
				return component;
			}

			return null;
		}

		/// <summary>
		/// TODO: WL@
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="instanceID"></param>
		/// <returns></returns>
		public static IComponent FindComponentFromChildren(this IEntity parent, Guid instanceID)
		{
			if(parent.Components.TryGetValue(instanceID, out IComponent component))
			{
				return component;
			}

			foreach(IEntity child in parent.Entities.Values)
			{
				if((component = child.FindComponent(instanceID)) != null)
				{
					return component;
				}
			}

			return null;
		}

		/// <summary>
		/// Remove a Component from this Entity
		/// </summary>
		/// <param name="component">The Component to remove</param>
		/// <returns>If removed, the original component is returned, otherwise NULL</returns>
		public static IComponent RemoveComponent(this IEntity parent, IComponent component)
		{
			if(component == null)
			{
				throw new ArgumentNullException(nameof(component), "Cannot remove invalid Components");
			}

			return parent.RemoveComponent(component.InstanceID);
		}

		/// <summary>
		/// Remove the component that matches the given instanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID of the component to remove</param>
		/// <returns>If removed, the original component is returned, otherwise NULL</returns>
		public static IComponent RemoveComponent(this IEntity parent, Guid instanceID)
		{
			return parent.Components.TryRemove(instanceID, out IComponent removedComponent) ? removedComponent : null;
		}

		#endregion COMPONENT

		#region Search Methods

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponent<T>(this IEntity parent) where T : class, IComponent => parent.GetComponent<T>((T)null);

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponent<T>(this IEntity parent, IComponent excludedComponent) where T : class, IComponent => parent.GetComponent<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponent<T>(this IEntity parent, IComponent[] excludedComponents) where T : class, IComponent
		{
			Type searchType = typeof(T);

			foreach(Guid key in parent.Components.Keys)
			{
				IComponent component = parent.FindComponent(key);

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

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponentFromChildren<T>(this IEntity parent) where T : class, IComponent => parent.GetComponentFromChildren<T>((T)null);

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponentFromChildren<T>(this IEntity parent, IComponent excludedComponent) where T : class, IComponent => parent.GetComponentFromChildren<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponentFromChildren<T>(this IEntity parent, IComponent[] excludedComponents) where T : class, IComponent
		{
			T myComponent;

			if((myComponent = parent.GetComponent<T>(excludedComponents)) != null)
			{
				return myComponent;
			}

			if(parent.Entities != null)
			{
				foreach(Guid childInstanceID in parent.Entities.Keys)
				{
					parent.Entities.TryGetValue(childInstanceID, out IEntity childEntity);
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

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponentFromParents<T>(this IEntity parent) where T : class, IComponent => parent.GetComponentFromParents<T>((T[])null);

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponentFromParents<T>(this IEntity parent, IComponent excludedComponent) where T : class, IComponent => parent.GetComponentFromParents<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		public static T GetComponentFromParents<T>(this IEntity parent, IComponent[] excludedComponents) where T : class, IComponent
		{
			T component = parent.GetComponent<T>(excludedComponents);

			if(component == null && parent.Parent != null && parent.Parent.IsActive)
			{
				component = parent.Parent.GetComponentFromParents<T>(excludedComponents);
			}

			return component;
		}

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponents<T>(this IEntity parent) where T : class, IComponent => parent.GetComponents<T>((T)null);

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponents<T>(this IEntity parent, IComponent excludedComponent) where T : class, IComponent => parent.GetComponents<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponents<T>(this IEntity parent, IComponent[] excludedComponents) where T : class, IComponent
		{
			List<T> foundComponents = new List<T>();

			foreach(Guid instanceID in parent.Components.Keys)
			{
				IComponent component = parent.FindComponent(instanceID);
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


		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponentsFromParents<T>(this IEntity parent) where T : class, IComponent => parent.GetComponentsFromParents<T>((T[])null);

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponentsFromParents<T>(this IEntity parent, IComponent excludedComponent) where T : class, IComponent => parent.GetComponentsFromParents<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponentsFromParents<T>(this IEntity parent, IComponent[] excludedComponents) where T : class, IComponent
		{
			List<T> foundComponents = new List<T>();
			T[] myComponents;

			if((myComponents = parent.GetComponents<T>(excludedComponents)) != null)
			{
				foundComponents.AddRange(myComponents);
			}

			if(parent.Parent != null && parent.Parent.IsActive)
			{
				T[] parentComponents;
				if((parentComponents = parent.Parent.GetComponents<T>(excludedComponents)) != null)
				{
					foundComponents.AddRange(parentComponents);
				}
			}

			return foundComponents.Count > 0 ? foundComponents.ToArray() : null;
		}

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponentsFromChildren<T>(this IEntity parent) where T : class, IComponent => parent.GetComponentsFromChildren<T>((T)null);

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponentsFromChildren<T>(this IEntity parent, IComponent excludedComponent) where T : class, IComponent => parent.GetComponentsFromChildren<T>(new[] { excludedComponent });

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		public static T[] GetComponentsFromChildren<T>(this IEntity parent, IComponent[] excludedComponents) where T : class, IComponent
		{
			List<T> foundComponents = new List<T>();
			T[] myComponents;

			if((myComponents = parent.GetComponents<T>(excludedComponents)) != null)
			{
				foundComponents.AddRange(myComponents);
			}

			if(parent.Entities != null)
			{
				foreach(Guid childInstanceID in parent.Entities.Keys)
				{
					parent.Entities.TryGetValue(childInstanceID, out IEntity childEntity);
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

		#endregion
	}
}