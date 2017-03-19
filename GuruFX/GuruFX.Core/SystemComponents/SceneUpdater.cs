using System.Collections.Generic;
using GuruFX.Core.Entities;

namespace GuruFX.Core.SystemComponents
{
	public class SceneUpdater : SystemComponent
	{
		public override string Name { get; set; } = "Scene Updater";
		
		public override void Init()
		{

		}

		public override void Destroy()
		{

		}

		public int EntitiesUpdated { get; set; }

		/// <summary>
		/// Update all the Child Entities and Components.
		/// </summary>
		/// <param name="elapsedTime">Total Elapsed Time since the first update</param>
		/// <param name="deltaTime">Time passed since last Update</param>
		public override void Update(double elapsedTime, double deltaTime)
		{
			IEnumerable<IEntity> entities = this.Parent.GetEntities();
			UpdateEntities(entities, elapsedTime, deltaTime);
		}

		private void UpdateEntities(IEnumerable<IEntity> entities, double elapsedTime, double deltaTime)
		{
			if(entities == null)
			{
				return;
			}

			foreach(IEntity entity in entities)
			{
				UpdateEntity(entity, elapsedTime, deltaTime);
			}
		}

		private void UpdateEntity(IEntity entity, double elapsedTime, double deltaTime)
		{
			if(entity == null)
			{
				return;
			}

			IEnumerable<IComponent> components = entity.GetComponents();

			// update all the Components
			UpdateComponents(components, elapsedTime, deltaTime);

			// update all the child entities
			UpdateEntities(entity.GetEntities(), elapsedTime, deltaTime);
		}

		private void UpdateComponents(IEnumerable<IComponent> components, double elapsedTime, double deltaTime)
		{
			if(components == null)
			{
				return;
			}

			foreach(IComponent component in components)
			{
				if (!(component is IUpdateable updateableComponent))
				{
					continue;
				}

				updateableComponent.Update(elapsedTime, deltaTime);
				++EntitiesUpdated;
			}
		}
	}
}
