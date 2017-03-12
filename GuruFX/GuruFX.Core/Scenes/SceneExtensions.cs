using System;
using GuruFX.Core.Components;
using GuruFX.Core.SystemComponents;
using GuruFX.Core.Entities;

namespace GuruFX.Core.Scenes
{
	public static class SceneExtensions
	{
		/// <summary>
		/// This extension method intercepts the Component and checks if it implements an ISystem or IUpdateable interface (or both) before adding it to the root entity of the scene.
		/// This is useful to avoid having to search for System and Updateable components on scene initialization or on every frame update. 
		/// </summary>
		/// <param name="scene">The scene that the component is being added to</param>
		/// <param name="component">The component to add</param>
		/// <returns>If the component implements an ISystem or IUpdateable interface and is successfully added to the scene root, then returns TRUE, otherwise FALSE.</returns>
		public static bool AddComponent(this Scene scene, Component component)
		{
			bool addAttempt = false;
			
			if(component is ISystem)
			{
				if(scene.AddComponent(component as ISystem))
				{
					if(component is IUpdateable)
					{
						if(scene.AddComponent(component as IUpdateable))
						{
							addAttempt = true;
						}
					}
				}
			}
			
			if(!addAttempt)
			{
				throw new ArgumentException($"Only {nameof(SystemComponent)}s can be added to the Scene");
			}

			// add the component to the Scene Root Entity
			return scene.Root.AddComponent(component);
		}

		public static bool AddComponents(this Scene scene, params Component[] components)
		{
			bool errors = false;
			foreach(Component component in components)
			{
				if(!scene.AddComponent(component))
				{
					errors = true;
				}
			}
			return !errors;
		}

		/// <summary>
		/// This extension method intercepts the Component and checks if it implements an ISystem or IUpdateable interface (or both) before adding it to the root entity of the scene.
		/// This is useful to avoid having to search for System and Updateable components on scene initialization or on every frame update. 
		/// </summary>
		/// <param name="scene">The scene that the component is being added to</param>
		/// <param name="systemComponent">The system component to add</param>
		/// <returns>
		/// TRUE if the component implements an ISystem or IUpdateable interface and is successfully added to the scene root.
		/// FALSE if the component could not be added to the scene root.
		/// Otherwise throws an Exception if the component could not be added as an ISystem or IUpdateable object.
		/// </returns>

		public static bool AddComponent(this Scene scene, SystemComponent systemComponent)
		{
			if(!scene.AddComponent(systemComponent as ISystem))
			{
				throw new Exception("Could not add System Component!");
			}
			
			if(!scene.AddComponent(systemComponent as IUpdateable))
			{
				throw new Exception("Could not add Updateable Component!");
			}

			// add the component to the Root Entity
			return scene.Root.AddComponent(systemComponent);
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="components"></param>
		/// <returns>
		/// TRUE if all the components implements an ISystem or IUpdateable interface and is successfully added to the scene root.
		/// FALSE if one or more of the components could not be added to the scene root.
		/// Otherwise throws an Exception if one or more of the components could not be added as an ISystem or IUpdateable object.
		/// </returns>
		public static bool AddComponents(this Scene scene, params SystemComponent[] components)
		{
			bool errors = false;
			foreach(SystemComponent component in components)
			{
				if(!scene.AddComponent(component))
				{
					errors = true;
				}
			}
			return !errors;
		}

		/// <summary>
		/// Adds the ISystem object to the Systems collection of the Scene
		/// </summary>
		/// <param name="scene">The Scene</param>
		/// <param name="systemComponent">The <see cref="ISystem"/> object</param>
		/// <returns>TRUE if the object is not null, doesn't already exist in the Systems collection, and is then successfully added to the Systems collection of the Scene.
		/// Otherwise throws an Exception.
		/// </returns>
		private static bool AddComponent(this IScene scene, ISystem systemComponent)
		{
			if(systemComponent == null)
			{
				throw new ArgumentNullException(nameof(systemComponent), "Cannot add invalid System Components to a Scene");
			}

			if(scene.Systems.ContainsKey(systemComponent.InstanceID))
			{
				throw new ArgumentException("The System Component (" + systemComponent.InstanceID + ") has already been added to this Scene (" + scene.InstanceID + ")", nameof(systemComponent));
			}

			return scene.Systems.TryAdd(systemComponent.InstanceID, systemComponent);
		}


		/// <summary>
		/// Adds the IUpdateable object to the Updateables collection of the Scene
		/// </summary>
		/// <param name="scene">The Scene</param>
		/// <param name="updateableComponent">The <see cref="IUpdateable"/> object</param>
		/// <returns>TRUE if the object is not null, doesn't already exist in the Updateables collection, and is then successfully added to the Updateables collection of the Scene.
		/// Otherwise throws an Exception.
		/// </returns>
		private static bool AddComponent(this IScene scene, IUpdateable updateableComponent)
		{
			if(updateableComponent == null)
			{
				throw new ArgumentNullException(nameof(updateableComponent), "Cannot add invalid Updateable Components to a Scene");
			}

			if(scene.Updateables.ContainsKey(updateableComponent.InstanceID))
			{
				throw new ArgumentException("The Updateable Component (" + updateableComponent.InstanceID + ") has already been added to this Scene (" + scene.InstanceID + ")", nameof(updateableComponent));
			}

			return scene.Updateables.TryAdd(updateableComponent.InstanceID, updateableComponent);
		}
	}
}
