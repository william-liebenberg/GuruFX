using System;
using System.Collections.Generic;
using GuruFX.Core.Entities;

namespace GuruFX.Core.Components
{
	/// <summary>
	/// TODO: DOCUMENT ALL THE THINGS
	/// </summary>
	public static class ComponentExtensions
	{
		public static IEnumerable<IEntity> GetEntities(this IComponent parent) => parent.Parent?.GetEntities();

		public static IEnumerable<IComponent> GetComponents(this IComponent parent) => parent.Parent.GetComponents();

		#region ENTITY

		public static bool AddEntity(this IComponent parent, IEntity entity) => parent.Parent?.AddEntity(entity) ?? false;
		public static bool AddEntities(this IComponent parent, params IEntity[] entities) => parent.Parent?.AddEntities(entities) ?? false;
		public static IEntity FindEntity(this IComponent parent, Guid instanceID) => parent.Parent?.FindEntity(instanceID);
		public static IEntity FindEntityFromChildren(this IComponent parent, Guid instanceID) => parent.Parent?.FindEntityFromChildren(instanceID);
		public static IEntity RemoveEntity(this IComponent parent, IEntity entity) => parent.Parent?.RemoveEntity(entity);
		public static IEntity RemoveEntity(this IComponent parent, Guid instanceID) => parent.Parent?.RemoveEntity(instanceID);

		#endregion ENTITY

		#region COMPONENT

		public static bool AddComponent(this IComponent parent, IComponent component) => parent.Parent?.AddComponent(component) ?? false;
		public static bool AddComponents(this IComponent parent, params IComponent[] components) => parent.Parent?.AddComponents(components) ?? false;
		public static IComponent FindComponent(this IComponent parent, Guid instanceID) => parent.Parent?.FindComponent(instanceID);
		public static IComponent FindComponentFromChildren(this IComponent parent, Guid instanceID) => parent.Parent?.FindComponentFromChildren(instanceID);
		public static IComponent RemoveComponent(this IComponent parent, IComponent component) => parent.Parent?.RemoveComponent(component);
		public static IComponent RemoveComponent(this IComponent parent, Guid instanceID) => parent.Parent?.RemoveComponent(instanceID);

		#endregion COMPONENT

		#region Search Methods

		public static T GetComponent<T>(this IComponent parent) where T : class, IComponent => parent.Parent.GetComponent<T>((T)null);
		public static T GetComponent<T>(this IComponent parent, IComponent excludedComponent) where T : class, IComponent => parent.Parent?.GetComponent<T>(new[] { excludedComponent });
		public static T GetComponent<T>(this IComponent parent, IComponent[] excludedComponents) where T : class, IComponent => parent.Parent?.GetComponent<T>(excludedComponents);

		public static T[] GetComponents<T>(this IComponent parent) where T : class, IComponent => parent.Parent?.GetComponents<T>((T)null);
		public static T[] GetComponents<T>(this IComponent parent, IComponent excludedComponent) where T : class, IComponent => parent.Parent?.GetComponents<T>(new[] { excludedComponent });
		public static T[] GetComponents<T>(this IComponent parent, IComponent[] excludedComponents) where T : class, IComponent => parent.Parent?.GetComponents<T>(excludedComponents);

		public static T GetComponentFromParents<T>(this IComponent parent) where T : class, IComponent => parent.Parent?.GetComponentFromParents<T>((T[])null);
		public static T GetComponentFromParents<T>(this IComponent parent, IComponent excludedComponent) where T : class, IComponent => parent.Parent?.GetComponentFromParents<T>(new[] { excludedComponent });
		public static T GetComponentFromParents<T>(this IComponent parent, IComponent[] excludedComponents) where T : class, IComponent => parent.Parent?.GetComponentFromParents<T>(excludedComponents);

		public static T[] GetComponentsFromParents<T>(this IComponent parent) where T : class, IComponent => parent.Parent?.GetComponentsFromParents<T>((T[])null);
		public static T[] GetComponentsFromParents<T>(this IComponent parent, IComponent excludedComponent) where T : class, IComponent => parent.Parent?.GetComponentsFromParents<T>(new[] { excludedComponent });
		public static T[] GetComponentsFromParents<T>(this IComponent parent, IComponent[] excludedComponents) where T : class, IComponent => parent.Parent?.GetComponentsFromParents<T>(excludedComponents);

		public static T GetComponentFromChildren<T>(this IComponent parent) where T : class, IComponent => parent.Parent?.GetComponentFromChildren<T>((T)null);
		public static T GetComponentFromChildren<T>(this IComponent parent, IComponent excludedComponent) where T : class, IComponent => parent.Parent?.GetComponentFromChildren<T>(new[] { excludedComponent });
		public static T GetComponentFromChildren<T>(this IComponent parent, IComponent[] excludedComponents) where T : class, IComponent => parent.Parent?.GetComponentFromChildren<T>(excludedComponents);

		public static T[] GetComponentsFromChildren<T>(this IComponent parent) where T : class, IComponent => parent.Parent?.GetComponentsFromChildren<T>((T)null);
		public static T[] GetComponentsFromChildren<T>(this IComponent parent, IComponent excludedComponent) where T : class, IComponent => parent.Parent?.GetComponentsFromParents<T>(new[] { excludedComponent });
		public static T[] GetComponentsFromChildren<T>(this IComponent parent, IComponent[] excludedComponents) where T : class, IComponent => parent.Parent?.GetComponentsFromChildren<T>(excludedComponents);

		#endregion Search Methods
	}
}
