using System.Collections.Generic;
using GuruFX.Core.Entities;

namespace GuruFX.Core.SystemComponents
{
	public class SceneRenderer : SystemComponent
	{
		public override string Name => "Scene Renderer";

		public override void Destroy()
		{
		}

		public override void Init()
		{
		}

		public int EntitiesRendered { get; set; }

		/// <summary>
		/// Render all the Child Entities and Components.
		/// </summary>
		/// <param name="elapsedTime">Total Elapsed Time since the first update</param>
		/// <param name="deltaTime">Time passed since last Update</param>
		public override void Update(double elapsedTime, double deltaTime)
		{
			base.Update(elapsedTime, deltaTime);

			IEnumerable<IEntity> entities = this.Parent.GetEntities();

			RenderEntities(entities, elapsedTime, deltaTime);
		}

		protected void RenderEntities(IEnumerable<IEntity> entities, double elapsedTime, double deltaTime)
		{
			if(entities == null)
			{
				return;
			}

			foreach(IEntity entity in entities)
			{
				RenderEntity(entity, elapsedTime, deltaTime);
			}
		}

		protected void RenderEntity(IEntity entity, double elapsedTime, double deltaTime)
		{
			if(entity == null)
			{
				return;
			}

			IEnumerable<IComponent> components = entity.GetComponents();

			// render all the Components
			RenderComponents(components, elapsedTime, deltaTime);

			// render all the child entities
			RenderEntities(entity.GetEntities(), elapsedTime, deltaTime);
		}

		protected void RenderComponents(IEnumerable<IComponent> components, double elapsedTime, double deltaTime)
		{
			if(components == null)
			{
				return;
			}

			foreach(IComponent component in components)
			{
				if(component is IRenderable updateableComponent)
				{
					updateableComponent.Render(elapsedTime, deltaTime);
					++EntitiesRendered;
				}
			}
		}
	}
}
