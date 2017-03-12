using System;

namespace GuruFX.Core
{
	/// <summary>
	/// TODO: DOCUMENT ALL THE THINGS!
	/// </summary>
	public interface IEntityComponentBase
	{
		/// <summary>
		/// Return an array of all the Child Entities for this Entity.
		/// </summary>
		/// <returns>All the child entities.</returns>
		IEntity[] GetEntities();

		/// <summary>
		/// Return an array of all the Components for this Entity.
		/// </summary>
		/// <returns>All the Components.</returns>
		IComponent[] GetComponents();


		#region ENTITY

		/// <summary>
		/// Add a child Entity to this Entity
		/// </summary>
		/// <param name="entity">The child Entity to add.</param>
		/// <returns>TRUE if the child Entity was added to this entity, otherwise an exception is thrown.</returns>
		bool AddEntity(IEntity entity);


		bool AddEntities(params IEntity[] entities);

		/// <summary>
		/// Find a child entity via its InstanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID of the child entity to find.</param>
		/// <returns>If found the child entity, otherwise null.</returns>
		IEntity FindEntity(Guid instanceID);

		/// <summary>
		/// TODO: WL@
		/// </summary>
		/// <param name="instanceID"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IEntity FindEntityFromChildren(Guid instanceID);

		/// <summary>
		/// Remove the given child entity from the current entity.
		/// </summary>
		/// <param name="entity">The child entity to remove.</param>
		/// <returns>The removed child entity, otherwise if the entity could not be removed null is returned.</returns>
		/// <exception cref="ArgumentNullException">If the given child entity is null.</exception>
		IEntity RemoveEntity(IEntity entity);

		/// <summary>
		/// Remove a the child entity with the given instanceID.
		/// </summary>
		/// <param name="instanceID">The child entity to remove' instanceID.</param>
		/// <returns></returns>
		IEntity RemoveEntity(Guid instanceID);

		#endregion ENTITY


		#region COMPONENT

		/// <summary>
		/// Add one or more components to this Entity
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		bool AddComponent(IComponent component);

		/// <summary>
		/// Add one or more components to this Entity
		/// </summary>
		/// <param name="components"></param>
		/// <returns>If no errors occurred, then returns TRUE, otherwise if one or more errors occurred then returns FALSE.</returns>
		bool AddComponents(params IComponent[] components);

		/// <summary>
		/// Find a component that matches the given instanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID to search for</param>
		/// <returns>If found, the original component is returned, otherwise NULL</returns>
		IComponent FindComponent(Guid instanceID);


		IComponent FindComponentFromChildren(Guid instanceID);

		/// <summary>
		/// Remove a Component from this Entity
		/// </summary>
		/// <param name="component">The Component to remove</param>
		/// <returns>If removed, the original component is returned, otherwise NULL</returns>
		IComponent RemoveComponent(IComponent component);

		/// <summary>
		/// Remove the component that matches the given instanceID.
		/// </summary>
		/// <param name="instanceID">The instanceID of the component to remove</param>
		/// <returns>If removed, the original component is returned, otherwise NULL</returns>
		IComponent RemoveComponent(Guid instanceID);

		#endregion COMPONENT

		#region Search Methods

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponent<T>() where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponent<T>(IComponent excludedComponent) where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponent<T>(IComponent[] excludedComponents) where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponentFromChildren<T>() where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponentFromChildren<T>(IComponent excludedComponent) where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponentFromChildren<T>(IComponent[] excludedComponents) where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponentFromParents<T>() where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponentFromParents<T>(IComponent excludedComponent) where T : class, IComponent;

		/// <summary>
		/// Get the first component from this entity, and if none are found on this entity then search through active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>If found the first component of type <typeparamref name="T"/>, otherwise null.</returns>
		T GetComponentFromParents<T>(IComponent[] excludedComponents) where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponents<T>() where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponents<T>(IComponent excludedComponent) where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity that match the type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponents<T>(IComponent[] excludedComponents) where T : class, IComponent;


		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponentsFromParents<T>() where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponentsFromParents<T>(IComponent excludedComponent) where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity and active Parents.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponentsFromParents<T>(IComponent[] excludedComponents) where T : class, IComponent;


		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponentsFromChildren<T>() where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponent">The component to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponentsFromChildren<T>(IComponent excludedComponent) where T : class, IComponent;

		/// <summary>
		/// Get the list of components from this entity and active Children.
		/// </summary>
		/// <typeparam name="T">The type of Component to return.</typeparam>
		/// <param name="excludedComponents">The components to exclude from the search.</param>
		/// <returns>The list of components of type <typeparamref name="T"/>, otherwise if none are found then null is returned.</returns>
		T[] GetComponentsFromChildren<T>(IComponent[] excludedComponents) where T : class, IComponent;

		#endregion
	}
}