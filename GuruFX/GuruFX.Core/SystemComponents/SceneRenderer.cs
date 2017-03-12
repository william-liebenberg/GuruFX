﻿namespace GuruFX.Core.SystemComponents
{
	public class SceneRenderer : SystemComponent
	{
		public override string Name => "Scene Renderer";

		public SceneRenderer() : base()
		{

		}

		public SceneRenderer(IEntity parent) : base(parent)
		{

		}		

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
		/// <param name="elapsedTime">TODO: WL@</param>
		/// <param name="deltaTime">TODO: WL@</param>
		public override void Update(double elapsedTime, double deltaTime)
		{
			base.Update(elapsedTime, deltaTime);

			IEntity[] entities = this.Parent.GetEntities();
			RenderEntities(entities, elapsedTime, deltaTime);
		}

		protected void RenderEntities(IEntity[] entities, double elapsedTime, double deltaTime)
		{
			if (entities == null)
			{
				return;
			}

			foreach (IEntity entity in entities)
			{
				RenderEntity(entity, elapsedTime, deltaTime);
			}
		}

		protected void RenderEntity(IEntity entity, double elapsedTime, double deltaTime)
		{
			if (entity == null)
			{
				return;
			}

			IComponent[] components = entity.GetComponents();

			// render all the Components
			RenderComponents(components, elapsedTime, deltaTime);

			// render all the child entities
			RenderEntities(entity.GetEntities(), elapsedTime, deltaTime);
		}

		protected void RenderComponents(IComponent[] components, double elapsedTime, double deltaTime)
		{
			if (components == null)
			{
				return;
			}

			foreach (IComponent component in components)
			{
				IRenderable updateableComponent = component as IRenderable;
				if (updateableComponent != null)
				{
					updateableComponent.Render(elapsedTime, deltaTime);
					++EntitiesRendered;
				}
			}
		}
	}
}
