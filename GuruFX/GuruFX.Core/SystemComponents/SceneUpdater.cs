namespace GuruFX.Core.SystemComponents
{
	public class SceneUpdater : SystemComponent
	{
		public override string Name => "Scene Updater";

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
			IEntity[] entities = this.Parent.GetEntities();
			UpdateEntities(entities, elapsedTime, deltaTime);
		}

		protected void UpdateEntities(IEntity[] entities, double elapsedTime, double deltaTime)
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

		protected void UpdateEntity(IEntity entity, double elapsedTime, double deltaTime)
		{
			if(entity == null)
			{
				return;
			}
			
			IComponent[] components = entity.GetComponents();

			// update all the Components
			UpdateComponents(components, elapsedTime, deltaTime);

			// update all the child entities
			UpdateEntities(entity.GetEntities(), elapsedTime, deltaTime);
		}

		protected void UpdateComponents(IComponent[] components, double elapsedTime, double deltaTime)
		{
			if(components == null)
			{
				return;
			}

			foreach(IComponent component in components)
			{
				IUpdateable updateableComponent = component as IUpdateable;
				if(updateableComponent != null)
				{
					updateableComponent.Update(elapsedTime, deltaTime);
					++EntitiesUpdated;
				}
			}
		}
	}
}
